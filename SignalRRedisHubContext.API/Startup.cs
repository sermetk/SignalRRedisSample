using SignalRRedisHubContext.API.Accessor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRRedisHubContext.Common.Contracts;
using SignalRRedisHubContext.BusinessEngine;

namespace SignalRRedisHubContext.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));
            services.AddScoped(typeof(IAccessBusinessEngine), typeof(AccessBusinessEngine));
            services.AddScoped(typeof(IMessageEngine), typeof(MessageEngine));      
            services.AddScoped(typeof(IRequestBusinessEngine), typeof(RequestBusinessEngine));
            services.AddSignalR(option=> { option.EnableDetailedErrors = true; });
            services.AddControllers();
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = $"{Configuration.GetConnectionString("RedisEndpoint")},password={Configuration.GetConnectionString("RedisPassword")}";
                option.InstanceName = Configuration.GetConnectionString("RedisInstance");
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization(); 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("/hub", options => { options.Transports = HttpTransportType.ServerSentEvents; }).RequireAuthorization();
            });
        }      
    }
}
