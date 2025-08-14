using DevConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevConnect.Persistence.DataModels.Configurations;

public class VerificationResendMetadataConfiguration : IEntityTypeConfiguration<VerificationResendMetadata>
{
    public void Configure(EntityTypeBuilder<VerificationResendMetadata> builder)
    {
        builder.ToTable("verificationresendlogs");

        builder.HasKey(x => x.UserId);
        
        builder.Property(x => x.LastSentAt)
            .IsRequired();
        builder.Property(x => x.AttemptCount)
            .IsRequired();
    }
}