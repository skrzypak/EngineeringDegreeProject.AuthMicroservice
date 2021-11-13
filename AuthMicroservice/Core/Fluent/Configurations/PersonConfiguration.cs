using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthMicroservice.Core.Fluent.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> modelBuilder)
        {
            modelBuilder.HasKey(p => p.Id);
            modelBuilder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(p => p.FirstName).HasMaxLength(300).IsRequired();
            modelBuilder.Property(p => p.LastName).HasMaxLength(300).IsRequired();
            modelBuilder.Ignore(p => p.FullName);
            modelBuilder.Property(p => p.Gender).HasConversion<string>().HasMaxLength(5).IsRequired();
            modelBuilder.Property(p => p.Email).HasMaxLength(100).IsRequired();
            modelBuilder.Property(p => p.PhoneNumber).HasMaxLength(12).IsRequired(false);
            modelBuilder.Ignore(p => p.Address);
            modelBuilder.Property(p => p.StreetAddress).HasMaxLength(100).IsRequired(false);
            modelBuilder.Property(p => p.PostalCode).HasMaxLength(6).IsRequired(false);
            modelBuilder.Property(p => p.City).HasMaxLength(100).IsRequired(false);
            modelBuilder.Property(p => p.State).HasMaxLength(100).IsRequired(false);

            modelBuilder.HasIndex(p => p.Email).IsUnique();

            modelBuilder.ToTable("Persons");
            modelBuilder.Property(p => p.Id).HasColumnName("Id");
            modelBuilder.Property(p => p.FirstName).HasColumnName("FirstName");
            modelBuilder.Property(p => p.LastName).HasColumnName("LastName");
            modelBuilder.Property(p => p.Gender).HasColumnName("Gender");
            modelBuilder.Property(p => p.Email).HasColumnName("Email");
            modelBuilder.Property(p => p.PhoneNumber).HasColumnName("PhoneNumber");
            modelBuilder.Property(p => p.StreetAddress).HasColumnName("StreetAddress");
            modelBuilder.Property(p => p.City).HasColumnName("City");
            modelBuilder.Property(p => p.State).HasColumnName("State");
            modelBuilder.Property(p => p.PostalCode).HasColumnName("PostalCode");
        }
    }
}
