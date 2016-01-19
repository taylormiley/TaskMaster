using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using TaskMaster.Services;
using TaskMaster.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using TaskMaster.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authentication.Cookies;
using System.Net;

namespace TaskMaster
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IApplicationEnvironment appEnv)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(appEnv.ApplicationBasePath)
              .AddJsonFile("config.json")
              .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
#if !DEBUG
                config.Filters.Add(new RequireHttpsAttribute());
#endif
            })
            .AddJsonOptions(opt => {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddIdentity<TaskUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") &&
                            ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }

                        return Task.FromResult(0);
                    }
                };
            })
            .AddEntityFrameworkStores<TaskContext>();

            services.AddLogging();

            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<TaskContext>();

            services.AddTransient<TaskContextSeedData>();
            services.AddScoped<ITaskRepository, TaskRepository>();

#if DEBUG
            services.AddScoped<IMailService, DebugMailService>();
#else
      services.AddScoped<IMailService, MailService>();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, TaskContextSeedData seeder, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug(LogLevel.Information);
                app.UseDeveloperExceptionPage();
            }
            else
            {
                loggerFactory.AddDebug(LogLevel.Error);
                app.UseExceptionHandler("/App/Error");
            }
           
            app.UseStaticFiles();

            app.UseIdentity();

            Mapper.Initialize(config =>
            {
                config.CreateMap<Quest, QuestViewModel>().ReverseMap();
            });

            app.UseMvc(config =>
            {
                config.MapRoute(
                  name: "Default",
                  template: "{controller}/{action}/{id?}",
                  defaults: new { controller = "App", action = "Index" }
                  );
            });

            await seeder.EnsureSeedDataAsync();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
