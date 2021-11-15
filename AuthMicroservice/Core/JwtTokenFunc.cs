using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Authentication;
using Authentication.Json;
using AuthMicroservice.Core.Fluent.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

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
                    new Claim("claim_nameid", userDomain.Id.ToString()),
                    new Claim("claim_unique_name", userDomain.Username),
                    new Claim("claim_role", "user"),
                    new Claim("claim_e2ud", userDomain.EnterprisesToUsersDomains.Count > 0 ?
                        JsonConvert.SerializeObject (
                            userDomain.EnterprisesToUsersDomains.Select(e2u => 
                                new Claim_e2ud_item {
                                    eudId = e2u.Id,
                                    epsId = e2u.EnterpriseId,
                                })
                            ) : "[]"
                    )
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
