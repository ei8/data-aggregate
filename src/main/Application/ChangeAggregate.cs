using CQRSlite.Commands;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ei8.Data.Aggregate.Application
{
    public class ChangeAggregate : ICommand
    {
        public ChangeAggregate(Guid id, string newAggregate, Guid authorId, int expectedVersion)
        {
            AssertionConcern.AssertArgumentValid(
                g => g != Guid.Empty,
                id,
                Messages.Exception.InvalidId,
                nameof(id)
                );
            AssertionConcern.AssertArgumentNotNull(newAggregate, nameof(newAggregate));
            AssertionConcern.AssertArgumentValid(
                g => g != Guid.Empty,
                authorId,
                Messages.Exception.InvalidId,
                nameof(authorId)
                );
            AssertionConcern.AssertArgumentValid(
                i => i >= 0,
                expectedVersion,
                Messages.Exception.InvalidExpectedVersion,
                nameof(expectedVersion)
                );

            this.Id = id;
            this.NewAggregate = newAggregate;
            this.AuthorId = authorId;
            this.ExpectedVersion = expectedVersion;
        }

        public Guid Id { get; private set; }

        public string NewAggregate { get; private set; }

        public Guid AuthorId { get; set; }

        public int ExpectedVersion { get; set; }
    }
}
