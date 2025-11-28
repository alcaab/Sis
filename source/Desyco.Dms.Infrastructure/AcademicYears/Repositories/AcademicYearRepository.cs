using Desyco.Dms.Domain.AcademicYears.Interfaces;
using Desyco.Dms.Domain.AcademicYears;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.AcademicYears.Repositories;

public class AcademicYearRepository(ApplicationDbContext context) : RepositoryBase<AcademicYearEntity, int>(context), IAcademicYearRepository;
