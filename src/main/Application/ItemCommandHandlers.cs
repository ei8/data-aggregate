using CQRSlite.Commands;
using neurUL.Common.Domain.Model;
using neurUL.Common.Http;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ei8.EventSourcing.Client;
using ei8.EventSourcing.Client.In;
using ei8.Data.Aggregate.Domain.Model;

namespace ei8.Data.Aggregate.Application
{
    public class ItemCommandHandlers : 
        ICancellableCommandHandler<ChangeAggregate>        
    {
        private readonly IEventSourceFactory eventSourceFactory;
        private readonly ISettingsService settingsService;

        public ItemCommandHandlers(IEventSourceFactory eventSourceFactory, ISettingsService settingsService)
        {
            AssertionConcern.AssertArgumentNotNull(eventSourceFactory, nameof(eventSourceFactory));
            AssertionConcern.AssertArgumentNotNull(settingsService, nameof(settingsService));

            this.eventSourceFactory = eventSourceFactory;
            this.settingsService = settingsService;
        }

        public async Task Handle(ChangeAggregate message, CancellationToken token = default(CancellationToken))
        {
            AssertionConcern.AssertArgumentNotNull(message, nameof(message));

            var eventSource = this.eventSourceFactory.Create(
                this.settingsService.EventSourcingInBaseUrl + "/",
                this.settingsService.EventSourcingOutBaseUrl + "/",
                message.AuthorId
                );

            if ((await eventSource.EventStoreClient.Get(message.Id, 0)).Count() == 0)
            {
                var item = new Item(message.Id, message.NewAggregate);
                await eventSource.Session.Add(item, token);
            }
            else
            {
                Item item = await eventSource.Session.Get<Item>(message.Id, nameof(item), message.ExpectedVersion, token);
                item.ChangeAggregate(message.NewAggregate);
            }
            
            await eventSource.Session.Commit(token);
        }
    }
}