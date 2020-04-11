using System;
using System.Collections.Generic;
using System.Text;

namespace works.ei8.Data.Aggregate.Application
{
    public interface ISettingsService
    {
        string EventSourcingInBaseUrl { get; }
        string EventSourcingOutBaseUrl { get; }
    }
}
