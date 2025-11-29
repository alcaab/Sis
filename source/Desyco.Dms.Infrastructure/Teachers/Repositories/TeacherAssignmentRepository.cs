using Desyco.Dms.Domain.Teachers;
using Desyco.Dms.Domain.Teachers.Interfaces;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Teachers.Repositories;

public class TeacherAssignmentRepository(ApplicationDbContext context) : RepositoryBase<TeacherAssignmentEntity, int>(context), ITeacherAssignmentRepository;