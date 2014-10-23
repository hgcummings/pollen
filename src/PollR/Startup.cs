using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Routing;
using PollR.Model;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.WindowsAzure.Storage;

namespace PollR
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseErrorPage(ErrorPageOptions.ShowAll);

            IConfiguration configuration = new Configuration()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            app.UseServices(services =>
            {
                services.AddMvc();
                services.AddSignalR();
                services.AddSingleton<PollRepository>();
                services.AddInstance(CloudStorageAccount.Parse(
                    configuration.Get("Data:PollStorage:ConnectionString")));
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
