using CQRSlite.Commands;
using Nancy;
using System;
using ei8.Data.Aggregate.Application;

namespace ei8.Data.Aggregate.Port.Adapter.In.Api
{
    public class ItemModule : NancyModule
    {
        public ItemModule(ICommandSender commandSender) : base("/data/aggregates")
        {
            this.Put("/{itemId}", async (parameters) =>
            {
                return await Helper.ProcessCommandResponse(
                        commandSender,
                        this.Request,
                        true,
                        (bodyAsObject, bodyAsDictionary, expectedVersion) =>
                        {
                            return new ChangeAggregate(
                                Guid.Parse(parameters.itemId.ToString()),
                                bodyAsObject.Aggregate.ToString(),
                                Guid.Parse(bodyAsObject.AuthorId.ToString()),
                                expectedVersion
                                );                            
                        },
                        "Aggregate",
                        "AuthorId"
                    );
            }
            );
        }
    }
}
