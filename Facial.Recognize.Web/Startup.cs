using Facial.Recognize.Core.Data;
using Facial.Recognize.Core.Extensions;
using Facial.Recognize.Web.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Facial.Recognize.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<TrainningFaceContext>(options =>
                //options.UseSqlite("Data Source=TrainningFace.db")
                options.UseSqlServer(Configuration.GetConnectionString("FacialRecognizeDB"))
            );

            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
            });

            services.RegisterRecognize();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCors(buidler =>
            {
                buidler
               .WithOrigins(new string[] { "http://localhost:5000" })
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<WebRtcHub>("/web-rtc");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
