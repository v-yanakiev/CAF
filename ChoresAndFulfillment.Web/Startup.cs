using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChoresAndFulfillment.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;

using ChoresAndFulfillment.Models;
using ChoresAndFulfillment.Web.Services.Interfaces;
using ChoresAndFulfillment.Web.Services;

namespace ChoresAndFulfillment
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

            services.AddDbContext<CAFContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<User>(
                options => options.Password = new PasswordOptions()
                {
                    RequireDigit = false,
                    RequiredLength=6,
                    RequiredUniqueChars=1,
                    RequireLowercase=true,
                    RequireNonAlphanumeric=false,
                    RequireUppercase=false
                }
                )
                .AddEntityFrameworkStores<CAFContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserAndContextRepository, UserAndContextRepository>();
            services.AddScoped<IListAllJobsService, ListAllJobsService>();
            services.AddScoped<IApplyForJobService, ApplyForJobService>();
            services.AddScoped<IListWorkerJobsService, ListWorkerJobsService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(
                route => {
                    route.MapRoute(
                     name: "default",
                     template: "{controller=Home}/{action=Index}/{id?}"
                     );
                });
        }
    }
}
