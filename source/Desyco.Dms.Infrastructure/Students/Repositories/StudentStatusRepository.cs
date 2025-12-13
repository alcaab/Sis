using Desyco.Dms.Domain.Students;
using Desyco.Dms.Domain.Students.Interfaces;

namespace Desyco.Dms.Infrastructure.Students.Repositories;

public class StudentStatusRepository(ApplicationDbContext context) : RepositoryBase<StudentStatusEntity, StudentStatus>(context), IStudentStatusRepository;