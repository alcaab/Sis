using Desyco.Dms.Domain.Students;
using Desyco.Dms.Domain.Students.Interfaces;

namespace Desyco.Dms.Infrastructure.Students.Repositories;

public class StudentGuardianRepository(ApplicationDbContext context) : RepositoryBase<StudentGuardianEntity, int>(context), IStudentGuardianRepository;