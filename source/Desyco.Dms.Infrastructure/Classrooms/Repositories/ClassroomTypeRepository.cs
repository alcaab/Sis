using Desyco.Dms.Domain.Classrooms;
using Desyco.Dms.Domain.Classrooms.Interfaces;

namespace Desyco.Dms.Infrastructure.Classrooms.Repositories;

public class ClassroomTypeRepository(ApplicationDbContext context) : RepositoryBase<ClassroomTypeEntity, int>(context), IClassroomTypeRepository;