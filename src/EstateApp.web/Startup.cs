using System;
using System.ComponentModel.Design;
using System.Net.Mime;
using System.Threading.Tasks;
using EsatateApp.Data.DatabaseContexts.ApplicationDbContext;
using EsatateApp.Data.DatabaseContexts.AuthenticationDbContext;
using EsatateApp.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EstateApp.web
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
            services.AddDbContextPool<AuthenticationDbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("AuthenticationConnection"),
            sqlServerOptions =>{
                sqlServerOptions.MigrationsAssembly("EstateApp.Data");
            }
            ));

            services.AddDbContextPool<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("ApplicationConnection"),
            sqlServerOptions =>{
                sqlServerOptions.MigrationsAssembly("EstateApp.Data");
            }
            ));

            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AuthenticationDbContext>()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options => 
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp)
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            MigrationDatabaseContext(svp);
            CreateDefaultUsersAndRoles(svp).GetAwaiter().GetResult();
        }

        public void MigrationDatabaseContext(IServiceProvider svp)
        {
            var authenticationDbContext = svp.GetRequiredService<AuthenticationDbContext>();
            authenticationDbContext.Database.Migrate();

            var applicationDbContext = svp.GetRequiredService<ApplicationDbContext>();
            applicationDbContext.Database.Migrate();
        }

        public async Task CreateDefaultUsersAndRoles (IServiceProvider svp)
        {
            string[] roles = new string[] { "systemAdministrator", "Agent", "User"};
            var UserEmail = "admin@estateapp.com";
            var userPassword = "Thelordisgud2me";

             var roleManager = svp.GetRequiredService<RoleManager<IdentityRole>>();
            foreach(var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if(!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole{ Name = role });
                }
            }

            var userManager = svp.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync( UserEmail);
            if(user is null)
            {
                 user = new ApplicationUser
                {
                    Email =  UserEmail,
                    UserName =  UserEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "+2349060697346",
                    PhoneNumberConfirmed = true
                };

                await userManager.CreateAsync(user, userPassword);

                await userManager.AddToRolesAsync(user, roles);
            }

           
        }

    }
}
