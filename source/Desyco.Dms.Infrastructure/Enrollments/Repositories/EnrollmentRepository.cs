using Desyco.Dms.Domain.Enrollments.Interfaces;
using Desyco.Dms.Domain.Enrollments;

namespace Desyco.Dms.Infrastructure.Enrollments.Repositories;

public class EnrollmentRepository(ApplicationDbContext context) : RepositoryBase<EnrollmentEntity, int>(context), IEnrollmentRepository;
