using Desyco.Dms.Domain.Students;
using Desyco.Dms.Domain.Students.Interfaces;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Students.Repositories;

public class StudentGuardianRepository(ApplicationDbContext context) : RepositoryBase<StudentGuardianEntity, int>(context), IStudentGuardianRepository;