using Desyco.Dms.Domain.Subjects.Interfaces;
using Desyco.Dms.Domain.Subjects;

namespace Desyco.Dms.Infrastructure.Subjects.Repositories;

public class SubjectRepository(ApplicationDbContext context) : RepositoryBase<SubjectEntity, int>(context), ISubjectRepository;