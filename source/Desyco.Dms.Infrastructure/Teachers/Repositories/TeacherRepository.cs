using Desyco.Dms.Domain.Teachers.Interfaces;
using Desyco.Dms.Domain.Teachers;

namespace Desyco.Dms.Infrastructure.Teachers.Repositories;

public class TeacherRepository(ApplicationDbContext context) : RepositoryBase<TeacherEntity, int>(context), ITeacherRepository;