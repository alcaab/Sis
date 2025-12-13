using Desyco.Dms.Domain.Teachers;
using Desyco.Dms.Domain.Teachers.Interfaces;

namespace Desyco.Dms.Infrastructure.Teachers.Repositories;

public class TeacherAssignmentRepository(ApplicationDbContext context) : RepositoryBase<TeacherAssignmentEntity, int>(context), ITeacherAssignmentRepository;