using CQRSlite.Commands;
using Nancy;
using System;
using ei8.Data.Aggregate.Application;
using ei8.Data.Aggregate.Port.Adapter.In.InProcess;
using neurUL.Common.Api;
using CQRSlite.Domain.Exception;

namespace ei8.Data.Aggregate.Port.Adapter.In.Api
{
    public class ItemModule : NancyModule
    {
        public ItemModule(IItemAdapter itemAdapter) : base("/data/aggregates")
        {
            this.Put("/{itemId}", async (parameters) =>
            {
                return await this.Request.ProcessCommand(
                        async (bodyAsObject, bodyAsDictionary, expectedVersion) =>
                            await itemAdapter.ChangeAggregate(
                                Guid.Parse(parameters.itemId.ToString()),
                                bodyAsObject.Aggregate.ToString(),
                                Guid.Parse(bodyAsObject.AuthorId.ToString()),
                                expectedVersion
                                ),
                        (ex, hsc) => {
                            // TODO: immediately cause calling Polly to fail (handle specific failure http code to signal "it's not worth retrying"?)
                            // i.e. there is an issue with the data
                            HttpStatusCode result = hsc;
                            if (ex is ConcurrencyException)
                                result = HttpStatusCode.Conflict;
                            return result;
                        },
                        Array.Empty<string>(),
                        "Aggregate",
                        "AuthorId"
                    );
            }
            );
        }
    }
}
