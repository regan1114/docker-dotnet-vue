using System;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using VueNet5.DB;
using VueNet5.Models;
using VueNet5.Authorization;
using VueNet5.Helpers;
using Newtonsoft.Json;

namespace VueNet5.Services
{
    public interface IUserService
    {
        AuthenticateResponse login(AuthenticateRequest model, string ipAddress);
        // AuthenticateResponse RefreshToken(string token, string ipAddress);
        // void RevokeToken(string token, string ipAddress);
        List<UserModel> GetAll();
        UserModel GetById(int id);
    }

    public class UserService:IUserService
    {
        private IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;
        public UserService(IJwtUtils jwtUtils, IOptions<AppSettings> appSettings)
        {
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
        }
        
        public AuthenticateResponse login(AuthenticateRequest model, string ipAddress)
        {
            var userInfo = QueryUserInfo(model);
            
            if (userInfo == null)
                throw new AppException("帳號或密碼錯誤");

            // authentication successful so generate jwt and refresh tokens
            userInfo.token = _jwtUtils.GenerateJwtToken(userInfo);
            var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            UpdateUserToken(userInfo);
            return new AuthenticateResponse(userInfo, userInfo.token, refreshToken.Token);
        }

        public List<UserModel> GetAll()
        {
            using (var connection = DBContext.MySqlConnection())
            {
                var sqlStr = "SELECT * FROM users";

                return connection.Query<UserModel>(sqlStr).ToList();
            }
        }

        private RefreshToken rotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void revokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }

        public UserModel GetById(int id)
        {
            using (var connection = DBContext.MySqlConnection())
            {
                var sqlStr = "SELECT * FROM users";                
                sqlStr += " WHERE id = @id";

                return connection.Query<UserModel>(sqlStr, new
                {
                    id = id
                }).FirstOrDefault();
            }
        }

        private UserModel QueryUserInfo(AuthenticateRequest model)
        {
            using (var connection = DBContext.MySqlConnection())
            {
                var sqlStr = "SELECT * FROM users";                
                sqlStr += " WHERE username = @username AND password = @password";

                return connection.Query<UserModel>(sqlStr, new
                {
                    username = model.Username,
                    password = model.Password.ToMD5()
                }).FirstOrDefault();
            }
        }

        private bool UpdateUserToken(UserModel user) 
        {
            using (var connection = DBContext.MySqlConnection())
            {
                var sqlStr = "UPDATE users SET token=@token";                
                sqlStr += " WHERE id = @id";
                
                var pas = new DynamicParameters();
                pas.Add("token",user.token);
                pas.Add("id",user.Id);
                var tuple = ExecuteSql(sqlStr, pas);
                return tuple.Item1 == 1;
            }
        }

        private Tuple<int,string> ExecuteSql(string sqlStr, DynamicParameters pas)
        {
            using (var connection = DBContext.MySqlConnection())
            {
                try
                {
                    var result = connection.Execute(sqlStr, pas);                    
                    return Tuple.Create(result, "");
                }
                catch (Exception e)
                {
                    string message = e.Message;
                    return Tuple.Create(0, message);
                }
            }
        }
    }
}