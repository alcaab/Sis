using Desyco.Dms.Domain.Scholarships.Interfaces;
using Desyco.Dms.Domain.Scholarships;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Scholarships.Repositories;

public class ScholarshipTypeRepository(ApplicationDbContext context) : RepositoryBase<ScholarshipTypeEntity, int>(context), IScholarshipTypeRepository;

public class ScholarshipAwardRepository(ApplicationDbContext context) : RepositoryBase<ScholarshipAwardEntity, int>(context), IScholarshipAwardRepository;
