using Xunit;
using System.Threading.Tasks;
using ESHRepository.Model;
using EquipmentSupplyHandler.Tests.CRUDClients;

namespace EquipmentSupplyHandler.Tests
{
    public class SupplierControllerTest : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        SupplierCRUDClient CrudClient { get; set; }

        public SupplierControllerTest(TestWebApplicationFactory<Startup> factory)
        {
            CrudClient = new SupplierCRUDClient(factory);
        }

        [Fact]
        public async Task TestCreateAndGetEquipmentType()
        {
            var expected = new Supplier()
            {
                Id = "1",
                Name = "НВС-строй",
                Address = "Васильева 17"
            };
            await CrudClient.CreateAsync(expected);
            var actual = await CrudClient.GetFirstElement();

            Assert.Equal(expected, actual);
            await CrudClient.DeleteElement("1");
        }

        [Fact]
        public async Task TestUpdateAndGetEquipmentType()
        {
            var expected = new Supplier()
            {
                Id = "1",
                Name = "НВС-строй",
                Address = "Васильева 17"
            };
            await CrudClient.CreateAsync(expected);

            expected.Name = "Ставрополь стройопторг";
            await CrudClient.UpdateAsync(expected);
            var actual = await CrudClient.GetFirstElement();
            Assert.Equal(expected, actual);
            await CrudClient.DeleteElement("1");
        }

        [Fact]
        public async Task TestGetNotFoundEquipmentType()
        {
            try
            {
                await CrudClient.GetElementById("1");
            }
            catch (System.Net.Http.HttpRequestException exception)
            {
                Assert.Contains("404", exception.Message);
                return;
            }
            Assert.False(true, "response must be not found");
        }
    }
}
