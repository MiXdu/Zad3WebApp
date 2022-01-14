using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Database;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Database.Entities;

namespace WebApplication4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(
                config => config.UseSqlServer(
                    Configuration.GetConnectionString("MSSQLServer")
                    )
                );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var databaseScope = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Dodaj migrację do bazy danych
                databaseScope.Database.Migrate();

                // Dodaj testowe rekordy, jeśli tabela People jest pusta
                if (databaseScope.People.Count() == 0)
                {
                    databaseScope.People.Add(new PersonEntity("Andrzej", "Kowalski", PersonGender.MALE));
                    databaseScope.People.Add(new PersonEntity("Karolina", "Kępa", PersonGender.FEMALE));
                    databaseScope.People.Add(new PersonEntity("XYZ", "YZTZT", PersonGender.NOT_SPECIFIED));
                    databaseScope.People.Add(new PersonEntity("Krzysztof", "Bazodanowy", PersonGender.MALE));

                    databaseScope.SaveChanges();
                }

            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=List}/{id?}");
            });
        }
    }
}
