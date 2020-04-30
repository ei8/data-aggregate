using System;
using System.Threading;
using System.Threading.Tasks;
using ei8.Data.Aggregate.Common;

namespace ei8.Data.Aggregate.Application
{
    public interface IItemQueryService
    {
        Task<ItemData> GetItemById(Guid id, CancellationToken token = default);
    }
}
