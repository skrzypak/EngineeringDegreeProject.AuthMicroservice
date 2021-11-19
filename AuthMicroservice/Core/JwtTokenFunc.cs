using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication;
using AuthMicroservice.Core.Fluent.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace AuthMicroservice.Core
{
    internal static class JwtTokenFunc
    {
        public static CookieOptions GenerateJwtEmptyCookie(AuthenticationSettings authenticationSettings) 
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);
            option.Secure = true;
            option.HttpOnly = true;
            option.SameSite = SameSiteMode.Unspecified;
            return option;
        }

        public static string GenerateJwtToken(AuthenticationSettings authenticationSettings, UserDomain userDomain)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(authenticationSettings.JwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("claim_id", userDomain.Id.ToString()),
                    new Claim("claim_username", userDomain.Username)
                }),
                Audience = authenticationSettings.JwtIssuer,
                Issuer = authenticationSettings.JwtIssuer,
                Expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
