using AspnetCore.TypeSafe.Server;
using AspNetCore.TypeSafe.Web.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AspNetCore.TypeSafe.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(o => o.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddTypeSafe();

            services.AddSignalR().AddJsonProtocol(o =>
            {
                o.PayloadSerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
                o.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSignalR(b => { b.MapHub<MyTypeSafeHub>("/test"); });
        }
    }
}
