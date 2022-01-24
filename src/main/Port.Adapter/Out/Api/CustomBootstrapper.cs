using Nancy;
using Nancy.TinyIoc;
using ei8.EventSourcing.Client;
using ei8.Data.Aggregate.Application;
using ei8.Data.Aggregate.Port.Adapter.IO.Process.Services;
using CQRSlite.Events;
using CQRSlite.Domain;

namespace ei8.Data.Aggregate.Port.Adapter.Out.Api
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        public CustomBootstrapper()
        {
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            container.Register<IEventSerializer, EventSerializer>(new EventSerializer());
            container.Register<ISettingsService, SettingsService>();
            container.Register<IEventStoreUrlService>(
                (tic, npo) => {
                    var ss = container.Resolve<ISettingsService>();
                    return new EventStoreUrlService(
                        ss.EventSourcingInBaseUrl + "/",
                        ss.EventSourcingOutBaseUrl + "/"
                        );
                });
            container.Register<IEventStore, HttpEventStoreClient>();
            container.Register<IRepository, Repository>();
            container.Register<ISession, Session>();
            container.Register<IItemQueryService, ItemQueryService>();
        }
    }
}
