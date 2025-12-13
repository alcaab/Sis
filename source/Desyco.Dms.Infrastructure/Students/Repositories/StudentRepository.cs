using Desyco.Dms.Domain.Students.Interfaces;
using Desyco.Dms.Domain.Students;

namespace Desyco.Dms.Infrastructure.Students.Repositories;

public class StudentRepository(ApplicationDbContext context) : RepositoryBase<StudentEntity, int>(context), IStudentRepository;
