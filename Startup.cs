using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication8.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;

namespace WebApplication8
{
    public class Startup
    {
        //public Startup(IHostingEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //       .SetBasePath(env.ContentRootPath)
        //       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //       .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        //       .AddEnvironmentVariables();
        //    Configuration = builder.Build();
        //    //Configuration = configuration  ;
        //    //IConfiguration configuration,
        //}
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<ITagHelper, CountryService>();

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
            });
            services.AddMvc();
            var connection = @"Server=192.168.80.143;Database=NEU;Trusted_Connection=false;User Id=sa;Password=123;ConnectRetryCount=0";
            services.AddDbContext<NEUContext>(options => options.UseSqlServer(connection, b => b.UseRowNumberForPaging()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));    , ILoggerFactory loggerFactory
            //loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    //template: "{controller=DbBms}/{action=Index}/{id?}");

            template: "{controller=DbEms}/{action=Create}/{id?}");
        });
        }
    }
}
