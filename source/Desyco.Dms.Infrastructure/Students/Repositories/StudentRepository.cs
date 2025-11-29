using Desyco.Dms.Domain.Students.Interfaces;
using Desyco.Dms.Domain.Students;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Students.Repositories;

public class StudentRepository(ApplicationDbContext context) : RepositoryBase<StudentEntity, int>(context), IStudentRepository;
