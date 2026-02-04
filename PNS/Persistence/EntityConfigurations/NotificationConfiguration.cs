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
                .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<object>>(
                    (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                    c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c != null ? c.ToList() : new List<object>()))
                ;

            builder.Property(n => n.Metadata)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => v == null ? null : JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null))
                .Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<Dictionary<string, string>>(
                    (d1, d2) => d1 != null && d2 != null && d1.Count == d2.Count && !d1.Except(d2).Any(),
                    d => d == null ? 0 : d.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    d => d != null ? new Dictionary<string, string>(d) : new Dictionary<string, string>()))
                ;

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
                .WithMany(p => p.Notifications)
                .HasForeignKey(n => n.PriorityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.NotificationType)
                .WithMany(t => t.Notifications)
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