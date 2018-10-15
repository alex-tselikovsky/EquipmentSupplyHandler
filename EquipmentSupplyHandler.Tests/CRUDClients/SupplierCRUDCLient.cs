using ESHRepository.Model;

namespace EquipmentSupplyHandler.Tests.CRUDClients
{
    class SupplierCRUDClient : CRUDClient<Supplier>
    {
        public SupplierCRUDClient(TestWebApplicationFactory<Startup> factory) : base(factory, "/api/supplier") { }
    }
}
