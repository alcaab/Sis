using Desyco.Dms.Domain.AcademicYears.Interfaces;
using Desyco.Dms.Domain.AcademicYears;

namespace Desyco.Dms.Infrastructure.AcademicYears.Repositories;

public class AcademicYearRepository(ApplicationDbContext context) : RepositoryBase<AcademicYearEntity, int>(context), IAcademicYearRepository;
