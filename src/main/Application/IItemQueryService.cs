using System;
using System.Threading;
using System.Threading.Tasks;
using works.ei8.Data.Aggregate.Common;

namespace works.ei8.Data.Aggregate.Application
{
    public interface IItemQueryService
    {
        Task<ItemData> GetItemById(string avatarId, Guid id, CancellationToken token = default);
    }
}
