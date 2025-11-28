using Desyco.Dms.Domain.Common.Interfaces;
using Desyco.Dms.Domain.Enrollments;

namespace Desyco.Dms.Domain.Enrollments.Interfaces;

public interface IEnrollmentRepository : IRepositoryBase<EnrollmentEntity, int>;
