using DevConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace DevConnect.Persistence.DataModels.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToCollection("notifications");
        
        builder.HasKey(n => n.Id);

        builder.Property(n => n.UserId)
            .IsRequired();
        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(n => n.Message)
            .HasMaxLength(1000);
        builder.Property(n => n.IsRead)
            .HasDefaultValue(false);
        builder.Property(n => n.CreatedAt)
            .IsRequired();
        
        builder.Property(n => n.ModifiedAt);
        builder.Property(n => n.DeletedAt);
    }
}