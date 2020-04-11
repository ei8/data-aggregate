using Nancy;
using Nancy.Responses;
using Newtonsoft.Json;
using works.ei8.Data.Aggregate.Application;

namespace works.ei8.Data.Aggregate.Port.Adapter.Out.Api
{
    public class ItemModule : NancyModule
    {
        public ItemModule(IItemQueryService itemQueryService) : base("/{avatarId}/data/aggregates")
        {
            this.Get("/{itemId}", async (parameters) => new TextResponse(JsonConvert.SerializeObject(
                await itemQueryService.GetItemById(parameters.avatarId, parameters.itemId))
                )
                );
        }
    }
}