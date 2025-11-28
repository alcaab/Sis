using Desyco.Dms.Domain.Enrollments.Interfaces;
using Desyco.Dms.Domain.Enrollments;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Enrollments.Repositories;

public class EnrollmentRepository(ApplicationDbContext context) : RepositoryBase<EnrollmentEntity, int>(context), IEnrollmentRepository;
