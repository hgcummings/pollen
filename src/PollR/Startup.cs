using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Routing;
using PollR.Model;

namespace PollR
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseErrorPage(ErrorPageOptions.ShowAll);

            app.UseServices(services =>
            {
                services.AddMvc();
                services.AddSignalR();
                services.AddSingleton<PollRepository>();
            });

            app.UseSignalR();

            app.UseFileServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    null,
                    "{controller}/{action}",
                    new { controller = "Vote", action = "Index" });
            });
        }
    }
}
