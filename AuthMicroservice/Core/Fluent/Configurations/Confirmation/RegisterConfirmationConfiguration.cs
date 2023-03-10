using System;
using AuthMicroservice.Core.Fluent.Entities.Confirmation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthMicroservice.Core.Fluent.Configurations.Confirmation
{
    public class RegisterConfirmationConfiguration : IEntityTypeConfiguration<RegisterConfirmation>
    {
        public void Configure(EntityTypeBuilder<RegisterConfirmation> modelBuilder)
        {
            modelBuilder.HasKey(u => u.Id);
            modelBuilder.Property(u => u.Id).HasDefaultValueSql("gen_random_uuid()").IsRequired();

            modelBuilder.Property(u => u.UserDomainId).IsRequired();
            modelBuilder.Property(u => u.CreatedDate).HasDefaultValueSql("now()").IsRequired();
            modelBuilder.Property(u => u.ProcessedDate).HasDefaultValue<DateTime?>(null);

            modelBuilder.ToTable("RegisterConfirmations");
            modelBuilder.Property(u => u.Id).HasColumnName("Id");
            modelBuilder.Property(u => u.UserDomainId).HasColumnName("UserDomainId");
            modelBuilder.Property(u => u.CreatedDate).HasColumnName("CreatedDate");
            modelBuilder.Property(u => u.ProcessedDate).HasColumnName("ProcessedDate");
        }
    }
}
