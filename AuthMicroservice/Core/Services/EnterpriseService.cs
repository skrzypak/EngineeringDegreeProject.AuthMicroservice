using System;
using System.Collections.Generic;
using System.Linq;
using Authentication;
using AuthMicroservice.Core.Exceptions;
using AuthMicroservice.Core.Fluent;
using AuthMicroservice.Core.Fluent.Entities;
using AuthMicroservice.Core.Interfaces.Services;
using AuthMicroservice.Core.Models.Dto.Enterprise;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthMicroservice.Core.Services
{
    public class EnterpriseService : IEnterpriseService
    {
        private readonly ILogger<EnterpriseService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;
        private readonly IHeaderContextService _headerContextService;

        public EnterpriseService(
            ILogger<EnterpriseService> logger,
            MicroserviceContext context,
            IMapper mapper,
            IHeaderContextService headerContextService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _headerContextService = headerContextService;
        }

        public void AddEnterpriseUser(int enterpriseId, string username, string email)
        {
            if(string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Select username or email, not both");
            }

            int newMemberUserDomainId = _context.UsersDomains
                .AsNoTracking()
                .Include(mud => mud.Person)
                .Where(mud => string.IsNullOrEmpty(email) ? mud.Username == username : mud.Person.Email == email)
                .Select(mud => mud.Id)
                .FirstOrDefault();

            if (newMemberUserDomainId == 0)
            {
                throw new NotFoundException($"NOT FOUND user with username: {username} or email: {email}");
            }

            var model = _context.Enterprises
                .Include(e => e.EnterprisesToUsersDomains)
                .Where(e => 
                    e.Id == enterpriseId &&
                    e.EnterprisesToUsersDomains.Any(e2u => e2u.UserDomainId == _headerContextService.GetUserDomainId()))
                .FirstOrDefault();

            if (model is null)
            {
                throw new NotFoundException($"NOT FOUND enterprise with id {enterpriseId}");
            }

            var exists = model.EnterprisesToUsersDomains.Any(e2u => e2u.UserDomainId == newMemberUserDomainId);

            if (exists == true)
            {
                throw new NotFoundException($"User with username: {username} or email: {email} exists in enterprise with id {enterpriseId}");
            }

            model.EnterprisesToUsersDomains.Add(new EnterpriseToUserDomain {
                EnterpriseId = model.Id,
                UserDomainId = newMemberUserDomainId
            });

            _context.SaveChanges();
        }

        public int Create(EnterpriseCoreDto dto)
        {
            var model = _mapper.Map<EnterpriseCoreDto, Enterprise>(dto);

            model.EnterprisesToUsersDomains = new HashSet<EnterpriseToUserDomain>
            {
                new EnterpriseToUserDomain
                {
                    UserDomainId = (int) _headerContextService.GetUserDomainId()
                }
            };

            _context.Enterprises.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Delete(int enterpriseId)
        {
            var model = _context.Enterprises
                .FirstOrDefault(e => e.Id == enterpriseId &&
                    e.EnterprisesToUsersDomains.Any(e2u => e2u.UserDomainId == _headerContextService.GetUserDomainId())
                );

            if (model is null )
            {
                throw new NotFoundException($"NOT FOUND enterprise with id {enterpriseId}");
            }

            _context.Enterprises.Remove(model);
            _context.SaveChanges();
        }

        public object Get()
        {
            var dtos = _context.EnterprisesToUsersDomains
                .AsNoTracking()
                .Include(e2u => e2u.UserDomain)
                .Include(e2u => e2u.Enterprise)
                .Where(e2u => e2u.UserDomainId == _headerContextService.GetUserDomainId())
                .Select(e2u => new
                {
                    e2u.EnterpriseId,
                    e2u.Enterprise.CompanyName,
                    e2u.Enterprise.Address
                })
                .ToList();

            if (dtos is null || dtos.Count() == 0)
            {
                throw new NotFoundException($"NOT FOUND any enterprise for user {_headerContextService.GetUserDomainId()}");
            }

            return dtos;
        }

        public object GetById(int enterpriseId)
        {
            var dto = _context.EnterprisesToUsersDomains
                .AsNoTracking()
                .Include(e2u => e2u.UserDomain)
                .Include(e2u => e2u.Enterprise)
                .Where(e2u => 
                    e2u.UserDomainId == _headerContextService.GetUserDomainId() &&
                    e2u.EnterpriseId == enterpriseId
                )
                .Select(e2u => new
                {
                    e2u.EnterpriseId,
                    e2u.Enterprise.Nip,
                    e2u.Enterprise.CompanyName,
                    e2u.Enterprise.Email,
                    e2u.Enterprise.PhoneNumber,
                    e2u.Enterprise.StreetAddress,
                    e2u.Enterprise.PostalCode,
                    e2u.Enterprise.City,
                    e2u.Enterprise.State,
                    e2u.Enterprise.Address
                })
                .FirstOrDefault();

            if (dto is null)
            {
                throw new NotFoundException($"Enterprise with id {enterpriseId} NOT FOUND");
            }

            return dto;
        }

        public object GetEnterpriseUsers(int enterpriseId)
        {
            var dto = _context.Enterprises
                .AsNoTracking()
                .Include(e => e.EnterprisesToUsersDomains)
                    .ThenInclude(e2u => e2u.UserDomain)
                        .ThenInclude(u => u.Person)
                .Where(e =>
                    e.Id == enterpriseId &&
                    e.EnterprisesToUsersDomains.Any(e2u => e2u.UserDomainId == _headerContextService.GetUserDomainId())
                )
                .SelectMany(e => e.EnterprisesToUsersDomains.Select(ee => new
                {
                    EnterpriseUserId = ee.Id,
                    ee.UserDomain.Person.FirstName,
                    ee.UserDomain.Person.LastName,
                    ee.UserDomain.Person.Email
                }))
                .ToList();

            if (dto is null || dto.Count() == 0)
            {
                throw new NotFoundException($"NOT FOUND any user in enterprise with id {enterpriseId}");
            }

            return dto;
        }

        public void LeftFromEnterprise(int enterpriseId)
        {
            RemoveEnterpriseUser(enterpriseId, (int) _headerContextService.GetUserDomainId());
        }

        public void RemoveEnterpriseUser(int enterpriseId, int enterpriseUserId)
        {
            var model = _context.Enterprises
                 .AsNoTracking()
                 .Include(e => e.EnterprisesToUsersDomains)
                 .FirstOrDefault(e => e.Id == enterpriseId &&
                     e.EnterprisesToUsersDomains.Any(e2u => e2u.UserDomainId == _headerContextService.GetUserDomainId())
                 );

            if (model is null)
            {
                throw new NotFoundException($"NOT FOUND enterprise with id {enterpriseId}");
            }

            var itemToDelete = model.EnterprisesToUsersDomains
                .Where(e2u => e2u.UserDomainId == _headerContextService.GetUserDomainId()).First();

            model.EnterprisesToUsersDomains.Remove(itemToDelete);

            _context.SaveChanges();
        }

        public void Update(int enterpriseId, EnterpriseCoreDto dto)
        {
            var oldModel = _context.Enterprises
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == enterpriseId &&
                    e.EnterprisesToUsersDomains.Any(e2u => e2u.UserDomainId == _headerContextService.GetUserDomainId())
                );

            if (oldModel is null)
            {
                throw new NotFoundException($"NOT FOUND enterprise with id {enterpriseId}");
            }

            var newModel = _mapper.Map<EnterpriseCoreDto, Enterprise>(dto);

            newModel.Id = oldModel.Id;
            newModel.EnterprisesToUsersDomains = oldModel.EnterprisesToUsersDomains;

            _context.Enterprises.Update(newModel);
            _context.SaveChanges();
        }
    }
}
