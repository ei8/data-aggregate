using CQRSlite.Events;
using Newtonsoft.Json;
using System;

namespace ei8.Data.Aggregate.Domain.Model
{
    public class AggregateChanged : IEvent
    {
        public readonly string Aggregate;

        public AggregateChanged(Guid id, string aggregate)
        {
            this.Id = id;
            this.Aggregate = aggregate;
        }

        public Guid Id { get; set; }

        public int Version { get; set; }

        [JsonProperty(PropertyName = "Timestamp")]
        public DateTimeOffset TimeStamp { get; set; }
    }
}
