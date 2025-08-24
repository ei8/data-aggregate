using neurUL.Common.CqrsLite;
using neurUL.Common.Domain.Model;
using System;

namespace ei8.Data.Aggregate.Domain.Model
{
    /// <summary>
    /// Represents an Item.
    /// </summary>
    public class Item : AssertiveAggregateRoot
    {
        private Item() { }

        /// <summary>
        /// Constructs an Item.
        /// </summary>
        /// <param name="id"></param>
        public Item(Guid id, string aggregate)
        {
            AssertionConcern.AssertArgumentValid(i => i != Guid.Empty, id, Messages.Exception.IdEmpty, nameof(id));
            AssertionConcern.AssertArgumentNotNull(aggregate, nameof(aggregate));

            this.Id = id;
            this.ApplyChange(new AggregateChanged(id, aggregate));
        }
        
        public string Aggregate { get; private set; }

        public void ChangeAggregate(string newAggregate)
        {
            AssertionConcern.AssertArgumentValid(
                na => na == null || na.Trim().Length > 0,
                newAggregate,
                "Specified aggregate must either be null or not an empty string.",
                nameof(newAggregate));

            if (newAggregate != this.Aggregate)
                base.ApplyChange(new AggregateChanged(this.Id, newAggregate));
        }

        private void Apply(AggregateChanged e)
        {
            this.Aggregate = e.Aggregate;
        }
    }
}
