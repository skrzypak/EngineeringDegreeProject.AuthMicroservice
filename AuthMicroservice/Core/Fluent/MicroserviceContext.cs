using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroservice.Core.Fluent.Configurations;
using AuthMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthMicroservice.Core.Fluent
{
    public class MicroserviceContext : DbContext
    {
        public DbSet<Enterprise> Enterprises { get; set; }
        public DbSet<EnterpriseToUserDomain> EnterprisesToUsersDomains { get; set; }
        public DbSet<UserDomain> UsersDomains { get; set; }

        public MicroserviceContext(DbContextOptions options) : base(options)
        {
        }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EnterpriseConfiguration());
            modelBuilder.ApplyConfiguration(new EnterpriseToUserDomainConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new UserCredentialConfiguration());
            modelBuilder.ApplyConfiguration(new UserDomainConfiguration());
        }
        #endregion
    }
}
