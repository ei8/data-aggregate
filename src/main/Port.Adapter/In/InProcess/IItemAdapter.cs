using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Data.Aggregate.Port.Adapter.In.InProcess
{
    public interface IItemAdapter
    {
        Task ChangeAggregate(Guid id, string newAggregate, Guid authorId, int expectedVersion);
    }
}
