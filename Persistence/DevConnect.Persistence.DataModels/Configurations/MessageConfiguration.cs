using DevConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace DevConnect.Persistence.DataModels.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToCollection("messages");
        
        builder.HasKey(m => m.Id);

        builder.Property(m => m.SenderId)
            .IsRequired();
        builder.Property(m => m.ReceiverId)
            .IsRequired();
        builder.Property(m => m.Content)
            .IsRequired()
            .HasMaxLength(2000);
        builder.Property(m => m.SentAt)
            .IsRequired();
        builder.Property(m => m.IsRead)
            .HasDefaultValue(false);

        builder.Property(m => m.CreatedAt)
            .IsRequired();
        builder.Property(m => m.ModifiedAt);
        builder.Property(m => m.DeletedAt);
    }
}