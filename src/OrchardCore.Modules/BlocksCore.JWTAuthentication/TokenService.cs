using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blocks.Framework.Utility.TypeHelper;
using BlocksCore.Abstractions.Exception;
using BlocksCore.Abstractions.Security;
using BlocksCore.Security;
using BlocksCore.Users.Abstractions;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BlocksCore.JWTAuthentication
{
    public class TokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<IUser> _userManager;

        public TokenService(IOptions<JwtSettings> jwtSettings,UserManager<IUser> userManager)
        {
            this._jwtSettings = jwtSettings.Value;
            this._userManager = userManager;
        }
        public object Token(IUserIdentifier userIdentifier)
        {
            //测试自己创建的对象
            //var user = new app_mobile_user
            //{
            //    id = 1,
            //    phone = "138000000",
            //    password = "e10adc3949ba59abbe56e057f20f883e"
            //};
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var authTime = DateTime.Now;//授权时间
            var expiresAt = authTime.AddMinutes(SafeConvert.ToDouble(_jwtSettings.ExpiresMinute));//过期时间
            var tokenDescripor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(JwtClaimTypes.Audience,_jwtSettings.Audience),
                    new Claim(JwtClaimTypes.Issuer,_jwtSettings.Issuer),
                    new Claim(JwtClaimTypes.Name, userIdentifier.UserAccount),
                    new Claim(JwtClaimTypes.Id, userIdentifier.UserId),
                }),
                Expires = expiresAt,
                //对称秘钥SymmetricSecurityKey
                //签名证书(秘钥，加密算法)SecurityAlgorithms
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescripor);
            var tokenString = tokenHandler.WriteToken(token);
            var result = new
            {
                access_token = tokenString,
                token_type = "Bearer",
                auth_time = authTime,
                expires_at = expiresAt
            };
            return result;
        }

        public object GetToken(IUser user)
        {
            return Token(new DefaultUserIdentifier(null, user.Id, user.UserName, null));
        }
    }
}
