using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileHosting.BLL.Interfaces;
using FileHosting.BLL.Services;
using FileHosting.DAL.EF;
using FileHosting.DAL.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileHosting.WEB
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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardLimit = null;
            });
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApiContext>();
            services.AddIdentityCore<User>()
                .AddEntityFrameworkStores<ApiContext>();
            services.AddControllersWithViews();
            services.AddDbContext<ApiContext>(options =>
                options.UseSqlServer(connection));
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login";
            });
            services.AddTransient(typeof(IUserService), typeof(UserService));
            services.AddTransient(typeof(IFileService), typeof(FileService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{action=Index}/{id?}");
            });
        }
    }
}
