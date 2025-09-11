// File Path: Persistence/EntityConfigurations/SmsTemplateConfiguration.cs
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class SmsTemplateConfiguration : IEntityTypeConfiguration<SmsTemplate>
    {
        public void Configure(EntityTypeBuilder<SmsTemplate> builder)
        {
            builder.ToTable("SmsTemplates");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Body)
                .IsRequired()
                .HasMaxLength(1600); // SMS character limit

            builder.HasIndex(e => e.Name)
                .IsUnique();

            // Audit fields
            builder.Property(e => e.CreatedDate)
                .IsRequired();

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(100);

            builder.Property(e => e.LastModifiedDate);

            builder.Property(e => e.LastModifiedBy)
                .HasMaxLength(100);

            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}