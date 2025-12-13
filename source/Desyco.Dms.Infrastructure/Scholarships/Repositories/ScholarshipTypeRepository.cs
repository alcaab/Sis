using Desyco.Dms.Domain.Scholarships.Interfaces;
using Desyco.Dms.Domain.Scholarships;

namespace Desyco.Dms.Infrastructure.Scholarships.Repositories;

public class ScholarshipTypeRepository(ApplicationDbContext context) : RepositoryBase<ScholarshipTypeEntity, int>(context), IScholarshipTypeRepository;