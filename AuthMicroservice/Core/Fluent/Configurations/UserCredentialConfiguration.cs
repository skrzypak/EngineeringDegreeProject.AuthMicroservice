using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthMicroservice.Core.Fluent.Configurations
{
    public class UserCredentialConfiguration : IEntityTypeConfiguration<UserCredential>
    {
        public void Configure(EntityTypeBuilder<UserCredential> modelBuilder)
        {
            modelBuilder.HasKey(u => u.Id);
            modelBuilder.Property(u => u.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(u => u.UserDomainId).IsRequired();

            modelBuilder.Property(u => u.Password).IsRequired();
            modelBuilder.Property(u => u.IsExpired).HasDefaultValue<bool>(false).IsRequired();
            modelBuilder.Property(u => u.ExpiredDate).IsRequired(false);

            modelBuilder.ToTable("UsersCredentials");
            modelBuilder.Property(u => u.Id).HasColumnName("Id");
            modelBuilder.Property(u => u.UserDomainId).HasColumnName("UserDomainId");
            modelBuilder.Property(u => u.Password).HasColumnName("Password");
            modelBuilder.Property(u => u.IsExpired).HasColumnName("IsExpired");
            modelBuilder.Property(u => u.ExpiredDate).HasColumnName("ExpiredDate");
        }
    }
}
