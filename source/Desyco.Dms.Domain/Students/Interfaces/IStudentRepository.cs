using Desyco.Dms.Domain.Common.Interfaces;
using Desyco.Dms.Domain.Students;

namespace Desyco.Dms.Domain.Students.Interfaces;

public interface IStudentRepository : IRepositoryBase<StudentEntity, int>;
