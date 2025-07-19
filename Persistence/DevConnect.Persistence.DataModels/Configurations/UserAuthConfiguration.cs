using DevConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevConnect.Persistence.DataModels.Configurations;

public class UserAuthConfiguration : IEntityTypeConfiguration<UserAuth>
{
    public void Configure(EntityTypeBuilder<UserAuth> builder)
    {
        builder.ToTable("userauths");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(u => u.PasswordHash)
            .IsRequired();
        builder.Property(u => u.IsVerified)
            .HasDefaultValue(false);
        builder.Property(u => u.CreatedAt)
            .IsRequired();
        builder.Property(u => u.ModifiedAt);
        builder.Property(u => u.DeletedAt);
    }
}