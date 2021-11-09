using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Authentication;
using AuthMicroservice.Core.Exceptions;
using AuthMicroservice.Core.Fluent;
using AuthMicroservice.Core.Interfaces.Services;
using AuthMicroservice.Core.Models.Dto.Person;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthMicroservice.Core.Services
{
    public class PersonService : IPersonService
    {
        private readonly ILogger<PersonService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;
        private readonly IHeaderContextService _headerContextService;

        public PersonService(
            ILogger<PersonService> logger,
            MicroserviceContext context,
            IMapper mapper,
            IHeaderContextService headerContextService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _headerContextService = headerContextService;
        }

        public object GetYourself()
        {
            var dto = _context.UsersDomains
                .AsNoTracking()
                .Include(ud => ud.Person)
                .Where(ud => ud.Id == _headerContextService.GetUserDomainId())
                .Select(ud => new
                {
                    ud.Person.FirstName,
                    ud.Person.LastName,
                    ud.Person.FullName,
                    ud.Person.Gender,
                    ud.Person.Email,
                    ud.Person.PhoneNumber,
                    ud.Person.StreetAddress,
                    ud.Person.PostalCode,
                    ud.Person.City,
                    ud.Person.State,
                    ud.Person.Address
                })
                .FirstOrDefault();

            if (dto is null)
            {
                throw new AuthException("JWT");
            }

            return dto;
        }

        public void Update(PersonCoreDto dto)
        {
            var model = _context.UsersDomains
                .Include(ud => ud.Person)
                .Where(ud => ud.Id == _headerContextService.GetUserDomainId())
                .FirstOrDefault();

            if (model is null)
            {
                throw new AuthException("JWT");
            }

            model.Person.FirstName = dto.FirstName;
            model.Person.LastName = dto.LastName;
            model.Person.Gender = dto.Gender;
            model.Person.Email = dto.Email;
            model.Person.PhoneNumber = dto.PhoneNumber;
            model.Person.StreetAddress = dto.StreetAddress;
            model.Person.PostalCode = dto.PostalCode;
            model.Person.City = dto.City;
            model.Person.State = dto.State;

            _context.SaveChanges();
        }
    }
}
