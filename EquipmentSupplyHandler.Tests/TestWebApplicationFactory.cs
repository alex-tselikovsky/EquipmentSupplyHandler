using System;
using ESHRepository.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EquipmentSupplyHandler.Tests
{
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var config = new ConfigurationBuilder().AddJsonFile("testProject.json").Build();
            builder.ConfigureServices(services =>
            {
                services.AddDbContext<ESHContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
                var serviceProvider = services.BuildServiceProvider();
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ESHContext>();

                    db.Database.OpenConnection();
                    try
                    {
                        db.Database.ExecuteSqlCommand(@"
                            Drop table Supplies                        
                            Drop table EquipmentTypes
                        Drop table Suppliers
                        ");
                    }
                    catch (Exception) { }
                    finally { }

                    db.Database.EnsureCreated();
                }
            });
        }
    }
}
