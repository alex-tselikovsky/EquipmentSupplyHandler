using System;
using Xunit;
using System.Threading.Tasks;
using ESHRepository.Model;
using EquipmentSupplyHandler.Tests.CRUDClients;

namespace EquipmentSupplyHandler.Tests
{
    public class DeliveryControllerTest : IClassFixture<TestWebApplicationFactory<Startup>>
    {
        DeliveryCRUDClient CrudClient { get; set; }
        EquipmentTypeCRUDClient EquipmentCRUDClient { get; set; }
        SupplierCRUDClient SupplierCRUDClient { get; set; }
        public DeliveryControllerTest(TestWebApplicationFactory<Startup> factory)
        {
            CrudClient = new DeliveryCRUDClient(factory);
            EquipmentCRUDClient = new EquipmentTypeCRUDClient(factory);
            SupplierCRUDClient = new SupplierCRUDClient(factory);
        }


        [Fact]
        public async Task TestCreateAndGetEquipmentType()
        {
            var equipment = new EquipmentType()
            {
                Id = "2",
                Name = "Фрезеровальный станок"
            };
            await EquipmentCRUDClient.CreateAsync(equipment);

            var supplier = new Supplier()
            {
                Id = "2",
                Name = "НВС-строй",
                Address = "Васильева 17"
            };
            await SupplierCRUDClient.CreateAsync(supplier);

            var expected = new Supply()
            {
                Id = "1",
                Count = 5,
                DeliveryDate = DateTime.Now,
                EquipmentTypeId = "2",
                SupplierId = "2"
            };
            await CrudClient.CreateAsync(expected);
            var actual = await CrudClient.GetFirstElement();

            Assert.Equal(expected, actual);
            await CrudClient.DeleteElement("1");
            await EquipmentCRUDClient.DeleteElement("2");
            await SupplierCRUDClient.DeleteElement("2");
        }

        [Fact]
        public async Task TestUpdateAndGetEquipmentType()
        {
            var equipment = new EquipmentType()
            {
                Id = "2",
                Name = "Фрезеровальный станок"
            };
            await EquipmentCRUDClient.CreateAsync(equipment);

            var supplier = new Supplier()
            {
                Id = "2",
                Name = "НВС-строй",
                Address = "Васильева 17"
            };
            await SupplierCRUDClient.CreateAsync(supplier);

            var expected = new Supply()
            {
                Id = "1",
                Count = 5,
                DeliveryDate = DateTime.Now,
                EquipmentTypeId = "2",
                SupplierId = "2"
            };
            await CrudClient.CreateAsync(expected);
            expected.DeliveryDate = new DateTime(2017, 1, 1);
            await CrudClient.UpdateAsync(expected);
           
            var actual = await CrudClient.GetFirstElement();
            Assert.Equal(expected, actual);
            await CrudClient.DeleteElement("1");
            await EquipmentCRUDClient.DeleteElement("2");
            await SupplierCRUDClient.DeleteElement("2");
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
