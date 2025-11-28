using Desyco.Dms.Domain.Students.Interfaces;
using Desyco.Dms.Domain.Students;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Students.Repositories;

public class StudentRepository(ApplicationDbContext context) : RepositoryBase<StudentEntity, int>(context), IStudentRepository;

public class StudentGradeRepository(ApplicationDbContext context) : RepositoryBase<StudentGradeEntity, int>(context), IStudentGradeRepository;

public class StudentGuardianRepository(ApplicationDbContext context) : RepositoryBase<StudentGuardianEntity, int>(context), IStudentGuardianRepository;

public class StudentStatusRepository(ApplicationDbContext context) : RepositoryBase<StudentStatusEntity, int>(context), IStudentStatusRepository;
