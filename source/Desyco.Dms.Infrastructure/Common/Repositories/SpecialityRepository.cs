using Desyco.Dms.Domain.Common.Entities;
using Desyco.Dms.Domain.Common.Interfaces;

namespace Desyco.Dms.Infrastructure.Common.Repositories;

public class SpecialityRepository(ApplicationDbContext context) : RepositoryBase<SpecialityEntity, int>(context), ISpecialityRepository;