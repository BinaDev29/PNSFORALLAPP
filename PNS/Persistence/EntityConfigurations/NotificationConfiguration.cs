// File Path: Persistence/EntityConfigurations/NotificationConfiguration.cs
using Domain.Models;
using Persistence.Converters;
using Domain.ValueObjects; // ይህን መስመር ያክሉ
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Persistence.EntityConfigurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(n => n.To)
                .HasConversion(
                    // To JSON
                    v => JsonSerializer.Serialize(v, typeof(List<object>), new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        // EmailAddress and PhoneNumber add Converters 
                        Converters = { new EmailAddressConverter(), new PhoneNumberConverter() }
                    }),
                    // From JSON
                    v => JsonSerializer.Deserialize<List<object>>(v, new JsonSerializerOptions
                    {
                        
                        Converters = { new EmailAddressConverter(), new PhoneNumberConverter() }
                    }) ?? new List<object>())
                .HasColumnType("nvarchar(max)");

            builder.Property(n => n.Metadata)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => v == null ? null : JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null))
                .HasColumnType("nvarchar(max)");

            builder.Property(n => n.Status)
                .HasConversion<int>();

            builder.Property(n => n.IP)
                .HasMaxLength(45); // IPv6 max length

            builder.Property(n => n.Secret)
                .HasMaxLength(100);

            builder.Property(n => n.ErrorMessage)
                .HasMaxLength(2000);

            builder.HasOne(n => n.ClientApplication)
                .WithMany()
                .HasForeignKey(n => n.ClientApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.Priority)
                .WithMany()
                .HasForeignKey(n => n.PriorityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.NotificationType)
                .WithMany()
                .HasForeignKey(n => n.NotificationTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for performance
            builder.HasIndex(n => n.ClientApplicationId);
            builder.HasIndex(n => n.Status);
            builder.HasIndex(n => n.CreatedDate);
            builder.HasIndex(n => n.ScheduledAt);
            builder.HasIndex(n => new { n.ClientApplicationId, n.Status });

            // Global query filter for soft delete
            builder.HasQueryFilter(n => !n.IsDeleted);
        }
    }
}