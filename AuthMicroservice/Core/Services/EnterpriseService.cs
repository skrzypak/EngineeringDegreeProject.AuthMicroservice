using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IUserContextService _userContextService;

        public EnterpriseService(
            ILogger<EnterpriseService> logger,
            MicroserviceContext context,
            IMapper mapper,
            AuthenticationSettings authenticationSettings,
            IUserContextService userContextService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _authenticationSettings = authenticationSettings;
            _userContextService = userContextService;
        }

        public void AddEnterpriseUser(int enterpriseId, string username, string email)
        {
            throw new NotImplementedException();
        }

        public int Create(EnterpriseCoreDto dto)
        {
            throw new NotImplementedException();
        }

        public void Delete(int enterpriseId)
        {
            throw new NotImplementedException();
        }

        public object Get()
        {
            throw new NotImplementedException();
        }

        public object GetById(int enterpriseId)
        {
            throw new NotImplementedException();
        }

        public object GetEnterpriseUserById(int enterpriseId, int enterpriseUserId)
        {
            throw new NotImplementedException();
        }

        public object GetEnterpriseUsers(int enterpriseId)
        {
            throw new NotImplementedException();
        }

        public void RemoveEnterpriseUser(int enterpriseId, int enterpriseUserId)
        {
            throw new NotImplementedException();
        }

        public int Update(int enterpriseId, EnterpriseCoreDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
