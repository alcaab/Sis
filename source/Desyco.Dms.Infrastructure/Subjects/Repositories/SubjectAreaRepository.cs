using Desyco.Dms.Domain.Subjects;
using Desyco.Dms.Domain.Subjects.Interfaces;
using Desyco.Dms.Infrastructure.Common;
using Desyco.Dms.Infrastructure.Common.Repositories;

namespace Desyco.Dms.Infrastructure.Subjects.Repositories;

public class SubjectAreaRepository(ApplicationDbContext context) : RepositoryBase<SubjectAreaEntity, int>(context), ISubjectAreaRepository;