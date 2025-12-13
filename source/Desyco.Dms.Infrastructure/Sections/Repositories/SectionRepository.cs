using Desyco.Dms.Domain.Sections.Interfaces;
using Desyco.Dms.Domain.Sections;

namespace Desyco.Dms.Infrastructure.Sections.Repositories;

public class SectionRepository(ApplicationDbContext context) : RepositoryBase<SectionEntity, int>(context), ISectionRepository;
