using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Contexts;
using Company.DAL.Models;
using Company.PL.Controllers;
using Company.PL.MappingProfiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company.PL
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
            //Allow Dependencey Injection for controller to make clr create object from controller
            services.AddControllersWithViews();

            //Allow Dependencey Injection
            services.AddDbContext<CompanyDbContext>( Options => 
            { Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")); },ServiceLifetime.Scoped /* ServiceLifetime.Scoped by defalut*/ );

            services.AddScoped<IDepartmentRepository,DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            //allow Dependencey Injection for clr to make object from class implement IMapper 
            //then pass object from employee profile that make configuraing of mapping on it
            services.AddAutoMapper( M=>M.AddProfile(new EmployeeProfile()));

            services.AddScoped<IUnitOfWork,UnitOfWork>();

            services.AddIdentity<ApplicationUer, IdentityRole>(Options=>
            {
                Options.Password.RequireNonAlphanumeric = true;
                Options.Password.RequireDigit = true;
                Options.Password.RequireLowercase=true;
                Options.Password.RequireUppercase=true;

            })
                .AddEntityFrameworkStores<CompanyDbContext>()
                .AddDefaultTokenProviders();
                

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "Account/Login";
                    options.AccessDeniedPath= "Home/Error";
                });
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
                    pattern: "{controller=Account}/{action=SignUp}");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Search",
                    pattern: "{controller=Employee}/{action=Index}/{name?}");
            });
        }
    }
}
