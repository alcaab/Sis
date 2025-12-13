using Desyco.Dms.Domain.Scholarships;
using Desyco.Dms.Domain.Scholarships.Interfaces;

namespace Desyco.Dms.Infrastructure.Scholarships.Repositories;

public class ScholarshipAwardRepository(ApplicationDbContext context) : RepositoryBase<ScholarshipAwardEntity, int>(context), IScholarshipAwardRepository;