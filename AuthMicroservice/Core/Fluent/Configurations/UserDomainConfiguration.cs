using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthMicroservice.Core.Fluent.Configurations
{
    public class UserDomainConfiguration : IEntityTypeConfiguration<UserDomain>
    {
        public void Configure(EntityTypeBuilder<UserDomain> modelBuilder)
        {
            modelBuilder.HasKey(u => u.Id);
            modelBuilder.Property(u => u.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(u => u.PersonId).IsRequired();

            modelBuilder.Property(u => u.Username).HasMaxLength(32).IsRequired();
            modelBuilder.Property(u => u.IsLocked).HasDefaultValue<bool>(false).IsRequired();
            modelBuilder.Property(u => u.IsExpired).HasDefaultValue<bool>(false).IsRequired();
            modelBuilder.Property(u => u.IsEnabled).HasDefaultValue<bool?>(true).IsRequired();
            modelBuilder.Property(u => u.ExpiredDate).IsRequired(false);
            modelBuilder.Property(u => u.IsConfirmed).HasDefaultValue<bool>(false).IsRequired();

            modelBuilder.HasIndex(p => p.Username).IsUnique(true);
            modelBuilder.HasIndex(p => p.PersonId).IsUnique(true);

            modelBuilder.ToTable("UsersDomains");
            modelBuilder.Property(u => u.Id).HasColumnName("Id");
            modelBuilder.Property(u => u.PersonId).HasColumnName("PersonId");
            modelBuilder.Property(u => u.Username).HasColumnName("Username");
            modelBuilder.Property(u => u.IsLocked).HasColumnName("IsLocked");
            modelBuilder.Property(u => u.IsExpired).HasColumnName("IsExpired");
            modelBuilder.Property(u => u.IsConfirmed).HasColumnName("IsConfirmed");
            modelBuilder.Property(u => u.ExpiredDate).HasColumnName("ExpiredDate");
        }
    }
}
