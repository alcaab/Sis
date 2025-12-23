using Desyco.Iam.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desyco.Iam.Infrastructure.Persistence.Configurations;

public class FeatureConfiguration : IEntityTypeConfiguration<Feature>
{
    public void Configure(EntityTypeBuilder<Feature> builder)
    {
        builder.ToTable("Features");

        builder.HasKey(f => f.Id);

        builder.HasIndex(f => f.Code).IsUnique();

        builder.Property(f => f.Code)
            .IsRequired();

        builder.Property(f => f.Description)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(f => f.Group)
            .HasMaxLength(100);

        builder.Property(f => f.CustomPermissions)
            .HasMaxLength(500);
    }
}
