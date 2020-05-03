using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TweetBookAPI.Data;

namespace TweetBookAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //Below does Auto Migration on run (manually we do like "dotnet ef migrations add init" or "add - migration init")
            var host=CreateHostBuilder(args).Build();
            using(var serviceScope = host.Services.CreateScope())
            {
                //any migration, we run in program.cs not startup.cs
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
                await dbContext.Database.MigrateAsync();

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManager.RoleExistsAsync(roleName: "Admin"))
                {
                    var adminRole = new IdentityRole("Admin");
                    await roleManager.CreateAsync(adminRole);
                }
                if (!await roleManager.RoleExistsAsync(roleName: "Poster"))
                {
                    var adminRole = new IdentityRole("Poster");
                    await roleManager.CreateAsync(adminRole);
                }
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
