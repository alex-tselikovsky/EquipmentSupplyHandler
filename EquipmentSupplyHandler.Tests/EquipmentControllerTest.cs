using Xunit;
using System.Threading.Tasks;
using ESHRepository.Model;
using EquipmentSupplyHandler.Tests.CRUDClients;

namespace EquipmentSupplyHandler.Tests
{
    public class EquipmentControllerTest : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        EquipmentTypeCRUDClient CrudClient { get; set; }

        public EquipmentControllerTest(TestWebApplicationFactory<Startup> factory)
        {
            CrudClient = new EquipmentTypeCRUDClient(factory);
        }
        
        [Fact]
        public async Task TestCreateAndGetEquipmentType()
        {
            var equipment = new EquipmentType()
            {
                Id = "1",
                Name = "Фрезеровальный станок"
            };
            await CrudClient.CreateAsync(equipment);
            var actualEquipment = await CrudClient.GetFirstElement();

            Assert.Equal(equipment, actualEquipment);
            await CrudClient.DeleteElement("1");
        }

        [Fact]
        public async Task TestUpdateAndGetEquipmentType()
        {
            var equipment = new EquipmentType()
            {
                Id = "1",
                Name = "Фрезеровальный станок"
            };
            await CrudClient.CreateAsync(equipment);

            equipment.Name = "Молотильный станок";
            await CrudClient.UpdateAsync(equipment);
            var actualEquipment = await CrudClient.GetFirstElement();
            Assert.Equal(equipment, actualEquipment);
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
