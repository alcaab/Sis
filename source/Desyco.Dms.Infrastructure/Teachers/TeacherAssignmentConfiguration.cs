using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Desyco.Dms.Domain.Sections;
using Desyco.Dms.Domain.Subjects;
using Desyco.Dms.Domain.Teachers;

namespace Desyco.Dms.Infrastructure.Teachers;

public class TeacherAssignmentConfiguration : IEntityTypeConfiguration<TeacherAssignmentEntity>
{
    public void Configure(EntityTypeBuilder<TeacherAssignmentEntity> builder)
    {
        builder.ToTable("TeacherAssignment");
        builder.HasKey(x => x.Id);

        builder.HasOne<TeacherEntity>().WithMany().HasForeignKey(x => x.TeacherId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<SectionEntity>().WithMany().HasForeignKey(x => x.SectionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<SubjectEntity>().WithMany().HasForeignKey(x => x.SubjectId).OnDelete(DeleteBehavior.Restrict);
    }
}
