using System;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using VueNet.DB;
using VueNet.Models;
using VueNet.Authorization;
using VueNet.Helpers;

namespace VueNet.Services
{
    public interface IUserService
    {
        AuthenticateResponse Login(AuthenticateRequest model, string ipAddress);
        AuthenticateResponse RefreshToken(string token);
        List<UserModel> GetAll();
        UserModel GetById(int id);
    }

    public class UserService : IUserService
    {
        private IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;
        public UserService(IJwtUtils jwtUtils, IOptions<AppSettings> appSettings)
        {
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Login(AuthenticateRequest model, string ipAddress)
        {
            var userInfo = QueryUserInfo(model);

            if (userInfo == null)
                throw new AppException("帳號或密碼錯誤");

            // authentication successful so generate jwt and refresh tokens
            userInfo.Token = _jwtUtils.GenerateJwtToken(userInfo);
            
            UpdateUserToken(userInfo);
            return new AuthenticateResponse(userInfo);
        }

        public AuthenticateResponse RefreshToken(string token)
        {
            var userID = _jwtUtils.DecodeJwtToken(token);
            if (userID == 0)
                throw new AppException("驗證失敗");

            var userInfo = GetById(userID);

            userInfo.Token = _jwtUtils.GenerateJwtToken(userInfo);
            UpdateUserToken(userInfo);
            return new AuthenticateResponse(userInfo);
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

            return connection.Query<UserModel>(sqlStr, new
            {
                id = id
            }).FirstOrDefault();
        }

        private UserModel QueryUserInfo(AuthenticateRequest model)
        {
            using var connection = DBContext.MySqlConnection();
            var sqlStr = "SELECT * FROM users";
            sqlStr += " WHERE account = @account AND password = @password";

            return connection.Query<UserModel>(sqlStr, new
            {
                account = model.Account,
                password = model.Password.ToMD5()
            }).FirstOrDefault();
        }

        private void UpdateUserToken(UserModel user)
        {
            using var connection = DBContext.MySqlConnection();
            var sqlStr = "UPDATE users SET token=@token";
            sqlStr += " WHERE id = @id";

            var pas = new DynamicParameters();
            pas.Add("token", user.Token);
            pas.Add("id", user.Id);

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