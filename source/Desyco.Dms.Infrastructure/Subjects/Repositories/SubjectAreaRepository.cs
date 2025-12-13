using Desyco.Dms.Domain.Subjects;
using Desyco.Dms.Domain.Subjects.Interfaces;

namespace Desyco.Dms.Infrastructure.Subjects.Repositories;

public class SubjectAreaRepository(ApplicationDbContext context) : RepositoryBase<SubjectAreaEntity, int>(context), ISubjectAreaRepository;