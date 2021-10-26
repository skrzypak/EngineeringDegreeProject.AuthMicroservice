using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthMicroservice.Core.Fluent.Configurations
{
    public class EnterpriseConfiguration : IEntityTypeConfiguration<Enterprise>
    {
        public void Configure(EntityTypeBuilder<Enterprise> modelBuilder)
        {
            modelBuilder.HasKey(e => e.Id);
            modelBuilder.Property(e => e.Id).IsRequired();

            modelBuilder.Property(e => e.Nip).HasMaxLength(10).IsRequired();
            modelBuilder.Property(e => e.CompanyName).HasMaxLength(300).IsRequired();
            modelBuilder.Property(e => e.Email).HasMaxLength(100).IsRequired();
            modelBuilder.Property(e => e.PhoneNumber).HasMaxLength(12).IsRequired(false);
            modelBuilder.Ignore(e => e.Address);
            modelBuilder.Property(e => e.StreetAddress).HasMaxLength(100).IsRequired();
            modelBuilder.Property(e => e.City).HasMaxLength(100).IsRequired();
            modelBuilder.Property(e => e.State).HasMaxLength(100).IsRequired();
            modelBuilder.Property(e => e.PostalCode).HasMaxLength(6).IsRequired();

            modelBuilder.ToTable("Enterprises");
            modelBuilder.Property(e => e.Id).HasColumnName("Id");
            modelBuilder.Property(e => e.Nip).HasColumnName("NIP");
            modelBuilder.Property(e => e.CompanyName).HasColumnName("CompanyName");
            modelBuilder.Property(e => e.Email).HasColumnName("Email");
            modelBuilder.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber");
            modelBuilder.Property(e => e.StreetAddress).HasColumnName("StreetAddress");
            modelBuilder.Property(e => e.City).HasColumnName("City");
            modelBuilder.Property(e => e.State).HasColumnName("State");
            modelBuilder.Property(e => e.PostalCode).HasColumnName("PostalCode");
        }
    }
}
