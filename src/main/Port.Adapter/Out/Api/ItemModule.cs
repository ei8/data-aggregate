using Nancy;
using Nancy.Responses;
using Newtonsoft.Json;
using ei8.Data.Aggregate.Application;

namespace ei8.Data.Aggregate.Port.Adapter.Out.Api
{
    public class ItemModule : NancyModule
    {
        public ItemModule(IItemQueryService itemQueryService) : base("/data/aggregates")
        {
            this.Get("/{itemId}", async (parameters) => new TextResponse(JsonConvert.SerializeObject(
                await itemQueryService.GetItemById(parameters.itemId))
                )
                );
        }
    }
}