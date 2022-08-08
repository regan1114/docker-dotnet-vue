using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VueNet.Helpers;
using VueNet.Models;

namespace VueNet.Authorization
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(UserModel user);
        public int ValidateJwtToken(string token);
        public int DecodeJwtToken(string token);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly AppSettings _appSettings;

        public JwtUtils(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string GenerateJwtToken(UserModel user)
        {
            // 如果appsetting的TTL設定大於0，則產生token的驗證時間會有設定數字的天數，反之只有15分鐘。
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var expiresDate = _appSettings.RefreshTokenTTL == 0 ? DateTime.UtcNow.AddMinutes(15) : DateTime.UtcNow.AddDays(_appSettings.RefreshTokenTTL);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = expiresDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int ValidateJwtToken(string token)
        {
            if (token == null)
                return 0;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                
                // return user id from JWT token if validation successful
                return int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
            }
            catch
            {
                // return null if validation fails
                return 0;
            }
        }

        public int DecodeJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            return int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
        }
    }
}

