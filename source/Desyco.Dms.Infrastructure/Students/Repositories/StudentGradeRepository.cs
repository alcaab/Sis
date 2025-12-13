using Desyco.Dms.Domain.Students;
using Desyco.Dms.Domain.Students.Interfaces;

namespace Desyco.Dms.Infrastructure.Students.Repositories;

public class StudentGradeRepository(ApplicationDbContext context) : RepositoryBase<StudentGradeEntity, int>(context), IStudentGradeRepository;