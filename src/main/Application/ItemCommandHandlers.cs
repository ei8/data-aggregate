using CQRSlite.Commands;
using org.neurul.Common.Domain.Model;
using org.neurul.Common.Http;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using works.ei8.EventSourcing.Client;
using works.ei8.EventSourcing.Client.In;
using works.ei8.Data.Aggregate.Domain.Model;

namespace works.ei8.Data.Aggregate.Application
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
                Helper.UrlCombine(this.settingsService.EventSourcingInBaseUrl, message.AvatarId) + "/",
                Helper.UrlCombine(this.settingsService.EventSourcingOutBaseUrl, message.AvatarId) + "/",
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