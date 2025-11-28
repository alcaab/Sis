using Desyco.Dms.Domain.Sections.Interfaces;
using Desyco.Dms.Domain.Sections;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Sections.Repositories;

public class SectionRepository(ApplicationDbContext context) : RepositoryBase<SectionEntity, int>(context), ISectionRepository;
