using org.neurul.Common.Domain.Model;
using System;

namespace works.ei8.Data.Aggregate.Domain.Model
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
            AssertionConcern.AssertArgumentNotNull(newAggregate, nameof(newAggregate));

            if (newAggregate != this.Aggregate)
                base.ApplyChange(new AggregateChanged(this.Id, newAggregate));
        }

        private void Apply(AggregateChanged e)
        {
            this.Aggregate = e.Aggregate;
        }
    }
}
