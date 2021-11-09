﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Authentication;
using AuthMicroservice.Core.Exceptions;
using AuthMicroservice.Core.Fluent;
using AuthMicroservice.Core.Fluent.Entities;
using AuthMicroservice.Core.Interfaces.Services;
using AuthMicroservice.Core.Models.Dto;
using AuthMicroservice.Core.Models.Dto.Person;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AuthMicroservice.Core.Services
{
    public class MicroserviceService : IMicroserviceService
    {
        private readonly ILogger<MicroserviceService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<UserDomain> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IHeaderContextService _headerContextService;

        public MicroserviceService(
            ILogger<MicroserviceService> logger,
            MicroserviceContext context,
            IMapper mapper,
            IPasswordHasher<UserDomain> passwordHasher, 
            AuthenticationSettings authenticationSettings,
            IHeaderContextService headerContextService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _headerContextService = headerContextService;
        }

        public void ChangePassword(ChangePassword dto)
        {
            if (dto.NewPassword != dto.ConfirmNewPassword)
            {
                throw new RegisterException("Passwords not matching");
            }

            var userDomain = _context.UsersDomains
                .Include(u => u.UserCredentials)
                .Where(u => u.IsEnabled == true && u.IsExpired == false && u.IsLocked == false)
                .Where(u => u.UserCredentials.Where(uc => uc.IsExpired == false).Count() > 0)
                .FirstOrDefault(u => u.Id == _headerContextService.GetUserDomainId());

            if (userDomain is null)
            {
                throw new AuthException("JWT");
            }

            var hashPassword = userDomain.UserCredentials.Where(uc => uc.IsExpired == false).Select(uc => uc.Password).First();
            var result = _passwordHasher.VerifyHashedPassword(userDomain, hashPassword, dto.CurrentPassword);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new AuthException("Invalid current password");
            }

            userDomain.UserCredentials.ToList().ForEach(uc => {
                uc.ExpiredDate = DateTime.Now;
                uc.IsExpired = true;
            });

            userDomain.UserCredentials.Add(new UserCredential()
            {
                Password = _passwordHasher.HashPassword(userDomain, dto.NewPassword)
            });

            _context.UsersDomains.Update(userDomain);
            _context.SaveChanges();
        }

        public void CloseAccount()
        {
            var userDomain = _context.UsersDomains
               .Include(u => u.UserCredentials)
               .Where(u => u.IsEnabled == true)
               .FirstOrDefault(u => u.Id == _headerContextService.GetUserDomainId());

            if (userDomain is null)
            {
                throw new AuthException("JWT");
            }

            userDomain.UserCredentials.ToList().ForEach(uc => {
                uc.ExpiredDate = DateTime.Now;
                uc.IsExpired = true;
            });

            userDomain.IsEnabled = false;
            userDomain.IsExpired = true;
            userDomain.IsLocked = true;

            _context.UsersDomains.Update(userDomain);
            _context.SaveChanges();
        }

        public string Login(LoginDto dto)
        {
            var userDomain = _context.UsersDomains
                .AsNoTracking()
                .Include(u => u.UserCredentials)
                .Include(u => u.Person)
                .Include(u => u.EnterprisesToUsersDomains)
                .Where(u => u.IsEnabled == true && u.IsExpired == false && u.IsLocked == false)
                .Where(u => u.UserCredentials.Where(uc => uc.IsExpired == false).Count() > 0)
                .FirstOrDefault(u => u.Username == dto.Username);

            if(userDomain is null)
            {
                throw new AuthException("Invalid username or password");
            }

            var hashPassword = userDomain.UserCredentials.Where(uc => uc.IsExpired == false).Select(uc => uc.Password).First();
            var result = _passwordHasher.VerifyHashedPassword(userDomain, hashPassword, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new AuthException("Invalid username or password");
            }

            var jwtToken =  generateJwtToken(userDomain);
            return jwtToken;
        }

        private string generateJwtToken(UserDomain userDomain)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authenticationSettings.JwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("claim_nameid", userDomain.Id.ToString()),
                    new Claim("claim_unique_name", userDomain.Person.FullName),
                    new Claim("claim_role", "user"),
                    new Claim("claim_enterprises", userDomain.EnterprisesToUsersDomains.Count > 0 ?
                        string.Join(",", userDomain.EnterprisesToUsersDomains.Select(e2u => e2u.EnterpriseId)) : ""
                    )
                }),
                Audience = _authenticationSettings.JwtIssuer,
                Issuer = _authenticationSettings.JwtIssuer,
                Expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public void Register(RegisterDto dto)
        {
            if (dto.Password != dto.Password)
            {
                throw new RegisterException("Passwords not matching");
            }

            var userDomain = new UserDomain()
            {
                Person = _mapper.Map<PersonCoreDto, Person>(dto.Person),
                Username = dto.Username,
                UserCredentials = new HashSet<UserCredential>()
            };

            var userCredential = new UserCredential()
            {
                Password = _passwordHasher.HashPassword(userDomain, dto.Password)
            };

            userDomain.UserCredentials.Add(userCredential);

            _context.UsersDomains.Add(userDomain);
            _context.SaveChanges();
        }
    }
}
