using Desyco.Dms.Domain.Grades.Interfaces;
using Desyco.Dms.Domain.Grades;

namespace Desyco.Dms.Infrastructure.Grades.Repositories;

public class GradeRepository(ApplicationDbContext context) : RepositoryBase<GradeEntity, int>(context), IGradeRepository;
