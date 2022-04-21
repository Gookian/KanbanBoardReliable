using Kanban.Server.Handlers;
using Kanban.Server.SocketsManager;

namespace Kanban.Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebSocketManager();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();
            app.MapSockets("/message", serviceProvider.GetService<MessageHandler>());
            app.UseStaticFiles();
        }
    }
}

