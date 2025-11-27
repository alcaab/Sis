using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Common.Entities;
using Desyco.Dms.Domain.Guardians;
using Desyco.Dms.Domain.Students;

namespace Desyco.Dms.Infrastructure.Students;

public class StudentGuardianConfiguration : IEntityTypeConfiguration<StudentGuardianEntity>
{
    public void Configure(EntityTypeBuilder<StudentGuardianEntity> builder)
    {
        builder.ToTable("StudentGuardian");
        builder.HasKey(x => new { x.StudentId, x.GuardianId });

        builder.HasOne<StudentEntity>().WithMany().HasForeignKey(x => x.StudentId);
        builder.HasOne<GuardianEntity>().WithMany().HasForeignKey(x => x.GuardianId);
        builder.HasOne<RelationshipEntity>().WithMany().HasForeignKey(x => x.RelationShipId);
    }
}
