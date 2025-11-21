using System.Linq.Expressions;

namespace Desyco.ContractManager.Application.Common;

public interface IJobEnqueuer
{
    string EnqueueBackgroundJob<T>(Expression<Func<T, Task>> methodCall);
}
