using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Events;
using ei8.Data.Aggregate.Domain.Model;
using ei8.EventSourcing.Client;
using ei8.EventSourcing.Client.In;
using neurUL.Common.Domain.Model;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ei8.Data.Aggregate.Application
{
    public class ItemCommandHandlers : 
        ICancellableCommandHandler<ChangeAggregate>        
    {
        private readonly IAuthoredEventStore eventStore;
        private readonly ISession session;

        public ItemCommandHandlers(IEventStore eventStore, ISession session)
        {
            AssertionConcern.AssertArgumentNotNull(eventStore, nameof(eventStore));
            AssertionConcern.AssertArgumentValid(
                es => es is IAuthoredEventStore,
                eventStore,
                "Specified 'eventStore' must be an IAuthoredEventStore implementation.",
                nameof(eventStore)
                );
            AssertionConcern.AssertArgumentNotNull(session, nameof(session));

            this.eventStore = (IAuthoredEventStore)eventStore;
            this.session = session;
        }

        public async Task Handle(ChangeAggregate message, CancellationToken token = default(CancellationToken))
        {
            AssertionConcern.AssertArgumentNotNull(message, nameof(message));

            this.eventStore.SetAuthor(message.AuthorId);

            if ((await this.eventStore.Get(message.Id, 0)).Count() == 0)
            {
                var item = new Item(message.Id, message.NewAggregate);
                await this.session.Add(item, token);
            }
            else
            {
                Item item = await this.session.Get<Item>(message.Id, nameof(item), message.ExpectedVersion, token);
                item.ChangeAggregate(message.NewAggregate);
            }
            
            await this.session.Commit(token);
        }
    }
}