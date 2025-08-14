using DevConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevConnect.Persistence.DataModels.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        
        var comparer = new ValueComparer<List<string>>(
            (a, b) => a.SequenceEqual(b),
            list => list.Aggregate(0, (acc, item) => HashCode.Combine(acc, item.GetHashCode())),
            list => list.ToList()
        );
        
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).IsRequired().HasMaxLength(50);
        builder.Property(r => r.Description).HasMaxLength(200);
        builder.Property(r => r.Type).IsRequired();
        builder.Property(r => r.Permissions).HasConversion(
            v => string.Join(',', v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
            .Metadata.SetValueComparer(comparer);
        builder
            .HasMany(r => r.Users)
            .WithMany(u => u.Roles)
            .UsingEntity(j => j.ToTable("userauth_roles"));
        builder.Property(r => r.CreatedAt).IsRequired();
        builder.Property(r => r.CreatedBy).IsRequired();
        builder.Property(r => r.ModifiedAt);
        builder.Property(r => r.ModifiedBy);
        builder.Property(r => r.DeletedAt);
        builder.Property(r => r.DeletedBy);
    }
}