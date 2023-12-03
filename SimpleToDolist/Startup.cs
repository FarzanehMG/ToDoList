using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared;
using SimpleToDoList.Application;
using SimpleToDoList.Application.Contracts;
using SimpleToDoList.Application.Contracts.Account;
using SimpleToDoList.Infrastructure;
using System;
using Microsoft.AspNetCore.Server.HttpSys;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SimpleToDoList.Application.Contracts.Employee;
using SimpleToDoList.Application.Contracts.Project;

namespace SimpleToDolist
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
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout as needed
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddRazorPages();
            services.AddTransient<IToDoApplication, ToDoApplication>();
            services.AddTransient<IAccountApplication, AccountApplication>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IEmployeeApplication, EmployeeApplication>();
            services.AddTransient<IProjectApplication, ProjectApplication>();

            services.AddDbContext<ToDoContext>(options => options.UseSqlServer("Data Source=192.168.1.8;Initial Catalog=ToDoTest2;User ID=karamouz;Password=Vision@190"));

            services.AddHttpContextAccessor();
            services.AddSession();


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/ToDoList";
                });



            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //});


            /*services.AddAuthentication(o => {
                o.DefaultAuthenticateScheme = "test";
                o.DefaultScheme = "test";
            }).AddScheme<>;*/


            //services.AddAuthorization(options =>
            //{
            //    options.FallbackPolicy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();
            //});*/

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
