using Desyco.Dms.Domain.Classrooms.Interfaces;
using Desyco.Dms.Domain.Classrooms;

namespace Desyco.Dms.Infrastructure.Classrooms.Repositories;

public class ClassroomRepository(ApplicationDbContext context) : RepositoryBase<ClassroomEntity, int>(context), IClassroomRepository;