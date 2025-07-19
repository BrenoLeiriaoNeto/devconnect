using DevConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevConnect.Persistence.DataModels.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("userprofiles");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.DisplayName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(p => p.ProfilePictureUrl)
            .HasMaxLength(500);
        builder.Property(p => p.Bio)
            .HasMaxLength(1000);
        builder.Property(p => p.Location)
            .HasMaxLength(255);
        builder.Property(p => p.CreatedAt)
            .IsRequired();
        builder.Property(p => p.ModifiedAt);
        builder.Property(p => p.DeletedAt);
    }
}