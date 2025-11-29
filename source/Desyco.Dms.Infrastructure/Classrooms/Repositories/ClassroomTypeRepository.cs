using Desyco.Dms.Domain.Classrooms;
using Desyco.Dms.Domain.Classrooms.Interfaces;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Classrooms.Repositories;

public class ClassroomTypeRepository(ApplicationDbContext context) : RepositoryBase<ClassroomTypeEntity, int>(context), IClassroomTypeRepository;