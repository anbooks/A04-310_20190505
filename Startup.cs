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

namespace WebApplication8
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
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
            });
            services.AddMvc();
            var connection = @"Server=192.168.44.129;Database=NEU;Trusted_Connection=false;User Id=sa;Password=123;ConnectRetryCount=0";
            services.AddDbContext<NEUContext>(options => options.UseSqlServer(connection, b => b.UseRowNumberForPaging()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
