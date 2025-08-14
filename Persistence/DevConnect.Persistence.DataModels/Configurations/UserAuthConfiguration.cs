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

        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.Property(u => u.Username).IsRequired().HasMaxLength(100);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.IsVerified).HasDefaultValue(false);
        builder.Property(u => u.VerificationToken).HasMaxLength(128);
        builder.Property(u => u.VerificationExpiry);
        builder.Property(u => u.VerifiedAt);
        builder.Property(u => u.IsTwoFactorEnabled).HasDefaultValue(false);
        builder.Property(u => u.TwoFactorSecret).HasMaxLength(128);
        builder.Property(u => u.IsLocked).HasDefaultValue(false);
        builder.Property(u => u.LockoutEnd);
        builder.Property(u => u.FailedLoginCount).HasDefaultValue(0);
        builder.Property(u => u.LastLoginAt);
        builder.Property(u => u.RefreshToken).HasMaxLength(256);
        builder.Property(u => u.RefreshTokenId).HasMaxLength(128);
        builder.Property(u => u.RefreshTokenExpiry);
        
        builder
            .HasOne(u => u.Profile)
            .WithOne()
            .HasForeignKey<UserAuth>(u => u.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.ToTable("userauth_roles"));
        builder.HasIndex(u => u.Email).IsUnique();
        
        builder.Property(u => u.CreatedAt).IsRequired();
        builder.Property(u => u.CreatedBy).IsRequired();
        builder.Property(u => u.ModifiedAt);
        builder.Property(u => u.ModifiedBy);
        builder.Property(u => u.DeletedAt);
        builder.Property(u => u.DeletedBy);
    }
}