using CQRSlite.Commands;
using ei8.Data.Aggregate.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Data.Aggregate.Port.Adapter.In.InProcess
{
    public class ItemAdapter : IItemAdapter
    {
        private readonly ICommandSender commandSender;

        public ItemAdapter(ICommandSender commandSender)
        {
            this.commandSender = commandSender;
        }

        public async Task ChangeAggregate(Guid id, string newAggregate, Guid authorId, int expectedVersion)
        {
            await this.commandSender.Send(new ChangeAggregate(id, newAggregate, authorId, expectedVersion));
        }
    }
}
