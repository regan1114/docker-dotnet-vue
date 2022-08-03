using System;
using Dapper;
using Microsoft.Extensions.Options;
using VueNet.Authorization;
using VueNet.DB;
using VueNet.Helpers;
using VueNet.Models;

namespace VueNet.Services
{
    public interface IUserService
    {
        AuthenticateResponse login(AuthenticateRequest model, string ipAddress);
        List<UserModel> GetAll();
        UserModel GetById(int id);
    }

    public class UserService : IUserService
    {
        private readonly IJwtUtils _jwtUtils;
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
                return null;

            // authentication successful so generate jwt and refresh tokens
            userInfo.Token = _jwtUtils.GenerateJwtToken(userInfo);
            var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            UpdateUserToken(userInfo);
            return new AuthenticateResponse(userInfo, userInfo.Token, refreshToken.Token);
        }

        public List<UserModel> GetAll()
        {
            using var connection = DBContext.MySqlConnection();
            var sqlStr = "SELECT * FROM users";

            return connection.Query<UserModel>(sqlStr).ToList();
        }

        public UserModel GetById(int id)
        {
            using var connection = DBContext.MySqlConnection();
            var sqlStr = "SELECT * FROM users";
            sqlStr += " WHERE id = @id";

            var user = connection.Query<UserModel>(sqlStr, new
            {
                id = id
            }).FirstOrDefault();
            return user;
        }

        private UserModel QueryUserInfo(AuthenticateRequest model)
        {
            using var connection = DBContext.MySqlConnection();
            var sqlStr = "SELECT * FROM users";
            sqlStr += " WHERE account = @account AND password = @password";
            var password = model.Password.ToMD5();
            var info = connection.Query<UserModel>(sqlStr, new
            {
                account = model.Account,
                password = password
            }).FirstOrDefault();
            return info;
        }

        private void UpdateUserToken(UserModel user)
        {
            using var connection = DBContext.MySqlConnection();
            var sqlStr = "UPDATE users SET token=@token";
            sqlStr += " WHERE id = @id";

            var pas = new DynamicParameters();
            pas.Add("token", user.Token);
            pas.Add("id", user.ID);
            ExecuteSql(sqlStr, pas);
        }

        private void ExecuteSql(string sqlStr, DynamicParameters pas)
        {
            using var connection = DBContext.MySqlConnection();
            try
            {
                connection.Execute(sqlStr, pas);
            }
            catch (Exception e)
            {
                string message = string.Format("資料庫錯誤 {0}", e.Message);
                throw new AppException(message);
            }
        }
    }
}

