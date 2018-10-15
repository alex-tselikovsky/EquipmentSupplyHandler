using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using ESHRepository.Interfaces.Model;
using ESHRepository.EF;

namespace EquipmentSupplyHandler.Tests
{
    public class AnalyticTest : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public AnalyticTest(TestWebApplicationFactory<Startup> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var serviceProvider = services.BuildServiceProvider();

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<ESHContext>();

                        // Ensure the database is created.
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
                        Utilities.DBInitializer.InitializeDbForTests(db);
                    }
                });
            })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
        }


        [Fact]
        public async Task TestAnalyticController_MonthEquipmentStatic()
        {
            var baseUri = _client.BaseAddress;
            var uri = new Uri(baseUri, "/api/analytic/MonthEquipmentStatic")
                .AddQuery("month", 1)
                .AddQuery("year", 2018)
                .AddQuery("supplierId", 1);
            var response = await _client.GetAsync(uri);
            var result1 = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var result = (await response.Content.ReadAsAsync<IEnumerable<EquipmentCount>>()).Select(i => new { i.Count, i.EquipmentId });
            var expected = Enumerable.Range(1, 6).Select(i => new
            {
                Count = 28 * i,
                EquipmentId = i.ToString()
            }).OrderByDescending(s => s.Count);
            for(int i =0; i < 6; i++)
            {
                Assert.Equal(expected.ElementAt(i), result.ElementAt(i));
            }
        }

        [Fact]
        public async Task TestAnalyticController_YearSupplierRation()
        {
            var baseUri = _client.BaseAddress;
            var uri = new Uri(baseUri, "/api/analytic/YearSupplierRatio").AddQuery("year", 2018);
            var response = await _client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var result = (await response.Content.ReadAsAsync<IEnumerable<SupplierRatio>>()).Select(i => new { i.SupplierId, i.Percentage});
            var expected = Enumerable.Range(1, 5).Select(i => new
            {
                SupplierId = i.ToString(),
                Percentage = (double)705600 * i/105840
            }).OrderByDescending(s => s.Percentage);
            for (int i = 0; i < 5; i++)
            {
                Assert.Equal(Math.Truncate(expected.ElementAt(i).Percentage*100), Math.Truncate(result.ElementAt(i).Percentage * 100));
                Assert.Equal(expected.ElementAt(i).SupplierId, result.ElementAt(i).SupplierId);
            }
         
        }


    }
}
