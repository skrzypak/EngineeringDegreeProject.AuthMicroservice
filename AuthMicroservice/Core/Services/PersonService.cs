using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuthMicroservice.Core.Exceptions;
using AuthMicroservice.Core.Fluent;
using AuthMicroservice.Core.Fluent.Entities;
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
        private readonly IUserContextService _userContextService;

        public PersonService(
            ILogger<PersonService> logger,
            MicroserviceContext context,
            IMapper mapper,
            IUserContextService userContextService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public object GetYourself()
        {
            throw new NotImplementedException();
        }

        public object Update(PersonCoreDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
