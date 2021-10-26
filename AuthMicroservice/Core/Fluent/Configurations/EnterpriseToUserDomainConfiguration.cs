using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthMicroservice.Core.Fluent.Configurations
{
    public class EnterpriseToUserDomainConfiguration : IEntityTypeConfiguration<EnterpriseToUserDomain>
    { 
        public void Configure(EntityTypeBuilder<EnterpriseToUserDomain> modelBuilder)
        {
            modelBuilder.HasKey(e => e.Id);
            modelBuilder.Property(e => e.Id).IsRequired();

            modelBuilder.Property(e => e.EnterpriseId).IsRequired();
            modelBuilder.Property(e => e.UserDomainId).IsRequired();

            modelBuilder.ToTable("EnterprisesToUsersDomains");
            modelBuilder.Property(e => e.Id).HasColumnName("Id");
            modelBuilder.Property(e => e.EnterpriseId).HasColumnName("EnterpriseId");
            modelBuilder.Property(e => e.UserDomainId).HasColumnName("UserDomainId");
        }
    }
}
