using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using AuthMicroservice.Core.Fluent.Entities.Confirmation;
using Microsoft.Extensions.Configuration;

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
        private readonly IConfiguration _configuration;

        public MicroserviceService(
            ILogger<MicroserviceService> logger,
            MicroserviceContext context,
            IMapper mapper,
            IPasswordHasher<UserDomain> passwordHasher, 
            AuthenticationSettings authenticationSettings,
            IHeaderContextService headerContextService,
            IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _headerContextService = headerContextService;
            _configuration = configuration;
        }

        public void ChangePassword(ChangePassword dto)
        {
            if (dto.New != dto.Confirm)
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
            var result = _passwordHasher.VerifyHashedPassword(userDomain, hashPassword, dto.Current);

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
                Password = _passwordHasher.HashPassword(userDomain, dto.New)
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
                .Include(u => u.EnterprisesToUsersDomains)
                .Where(u => u.Username == dto.Username)
                .Where(u => u.IsEnabled == true)
                .Where(u => u.UserCredentials.Where(uc => uc.IsExpired == false).Count() > 0)
                .FirstOrDefault();

            if(userDomain is null)
            {
                throw new AuthException("Invalid username or password");
            }

            if (userDomain.IsExpired == true)
            {
                throw new AuthException("Account is expired");
            }

            if (userDomain.IsLocked == true)
            {
                throw new AuthException("Account is locked");
            }

            if (userDomain.IsConfirmed == false)
            {
                throw new AuthException("Account is not confirmed");
            }

            var hashPassword = userDomain.UserCredentials.Where(uc => uc.IsExpired == false).Select(uc => uc.Password).First();
            var result = _passwordHasher.VerifyHashedPassword(userDomain, hashPassword, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new AuthException("Invalid username or password");
            }

            var jwtToken = JwtTokenFunc.GenerateJwtToken(_authenticationSettings, userDomain);
            return jwtToken;
        }

        public string RefreshToken()
        {
            var userDomain = _context.UsersDomains
               .Include(u => u.EnterprisesToUsersDomains)
               .Where(u => u.Id == _headerContextService.GetUserDomainId())
               .FirstOrDefault(u => u.IsEnabled == true && u.IsConfirmed == true);

            if (userDomain is null)
            {
                throw new AuthException("Invalid cookie");
            }

            return JwtTokenFunc.GenerateJwtToken(_authenticationSettings, userDomain);
        }

        public async Task<string> Register(RegisterDto dto)
        {
            if (dto.Password != dto.ConfirmedPassword)
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

            string code = "";

            // CONFIRMATION
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () => 
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.UsersDomains.Add(userDomain);
                        _context.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new RegisterException("Username or email exists in database");
                    }

                    try
                    {
                        code = generateRegisterConfirmation(userDomain);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new RegisterException("Can't send email confirmation");
                    }
                }
            });

            return code;
        }


        public string RequestPasswordReset(string username, Password dto)
        {
            if (dto.New != dto.Confirm)
            {
                throw new AuthException("Passwords not matching");
            }

            var userDomain = _context.UsersDomains
                .AsNoTracking()
                .Include(u => u.Person)
                .Include(u => u.UserCredentials)
                .Include(u => u.EnterprisesToUsersDomains)
                .Where(u => u.Username == username)
                .Where(u => u.IsEnabled == true)
                .FirstOrDefault();

            if (userDomain is not null)
            {
                string hashUserCredential = _passwordHasher.HashPassword(userDomain, dto.New);

                // CONFIRMATION
                return generatePasswordResetConfirmation(userDomain, hashUserCredential);
            } else
            {
                throw new AuthException("Account problem. Contact with support");
            }
        }

        public void RegisterConfirmation(Guid id)
        {
            var cfg = _context.RegisterConfirmations
                .FirstOrDefault(rc => rc.Id == id && rc.ProcessedDate == null);

            if (cfg is null)
            {
                throw new ConfirmationException("Invalid confirmation url link");
            }

            var userDomainModel = _context.UsersDomains
                 .FirstOrDefault(ud => ud.Id == cfg.UserDomainId && ud.IsConfirmed == false);

            if (userDomainModel is null)
            {
                throw new ConfirmationException("Invalid confirmation url link");
            }

            cfg.ProcessedDate = DateTime.Now;
            userDomainModel.IsConfirmed = true;

            _context.SaveChanges();
        }

        public void PasswordResetConfirmation(Guid id)
        {
            var cfg = _context.PasswordConfirmations
                .FirstOrDefault(pc => pc.Id == id && pc.ProcessedDate == null);

            if (cfg is null)
            {
                throw new ConfirmationException("Invalid confirmation url link");
            }

            var userDomainModel = _context.UsersDomains
                 .Include(ud => ud.UserCredentials)
                 .FirstOrDefault(ud => ud.Id == cfg.UserDomainId && ud.IsConfirmed == true);

            if (userDomainModel is null)
            {
                throw new ConfirmationException("Invalid confirmation url link");
            }

            cfg.ProcessedDate = DateTime.Now;

            // Revoke all passwords - TODO REVOKE SESSIONS
            userDomainModel.UserCredentials.ToList().ForEach(uc =>
            {
                if (uc.IsExpired == false)
                {
                    uc.IsExpired = true;
                    uc.ExpiredDate = DateTime.Now;
                }
            });

            userDomainModel.UserCredentials.Add(new UserCredential()
            {
                Password = cfg.HashUserCredential,
            });

            _context.SaveChanges();
        }

        private string generateRegisterConfirmation(UserDomain userDomain)
        {
            var cfg = new RegisterConfirmation()
            {
                UserDomainId = userDomain.Id,
                UserDomain = userDomain
            };

            _context.RegisterConfirmations.Add(cfg);
            _context.SaveChanges();

            sendConfirmationRegisterEmail (userDomain.Person.FullName, userDomain.Person.Email, cfg.Id);

            return $"{cfg.Id}";
        }

        private string generatePasswordResetConfirmation(UserDomain userDomain, string hashUserCredential)
        {
            var cfg = new PasswordConfirmation()
            {
                UserDomainId = userDomain.Id,
                HashUserCredential = hashUserCredential
            };

            _context.PasswordConfirmations.Add(cfg);
            _context.SaveChanges();

            sendConfirmationResetPassowrdEmail(userDomain.Person.FullName, userDomain.Person.Email, cfg.Id);
            return $"{cfg.Id}";
        }

        private void sendConfirmationRegisterEmail(string receiverFullName, string receiverEmail, Guid random)
        {
            var upstream = _configuration.GetValue<string>("GatewayUrl");
            var url = $"{upstream}/auth/msv/no/register/confirmation/{random}";
            sendEmail(receiverFullName, receiverEmail, "Welcome in EDP", url);
        }

        private void  sendConfirmationResetPassowrdEmail(string receiverFullName, string receiverEmail, Guid random)
        {
            var upstream = _configuration.GetValue<string>("GatewayUrl");
            var url = $"{upstream}/auth/msv/no/request/password-reset/confirmation/{random}";
            sendEmail(receiverFullName, receiverEmail, $"EDP password reset confirmation", url);
        }

        private void sendEmail(string receiverFullName, string receiverEmail, string msgHeader, string msgHtml)
        {
            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("ebd6e458083d65", "766f877b2f8100"),
                EnableSsl = true
            };

            client.Send("noreplay@nuie.com", $"{receiverFullName}<{receiverEmail}>", msgHeader, msgHtml);
        }
    }
}
