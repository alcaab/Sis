using Desyco.Dms.Domain.Common.Entities;
using Desyco.Dms.Domain.Common.Interfaces;

namespace Desyco.Dms.Infrastructure.Common.Repositories;

public class RelationshipRepository(ApplicationDbContext context) : RepositoryBase<RelationshipEntity, int>(context), IRelationshipRepository;