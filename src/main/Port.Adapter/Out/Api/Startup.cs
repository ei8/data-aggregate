using Microsoft.AspNetCore.Builder;
using Nancy.Owin;

namespace works.ei8.Data.Aggregate.Port.Adapter.Out.Api
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(buildFunc => buildFunc.UseNancy());
        }
    }
}
