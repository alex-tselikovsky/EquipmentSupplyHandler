using ESHRepository.Model;

namespace EquipmentSupplyHandler.Tests.CRUDClients
{
    class DeliveryCRUDClient : CRUDClient<Supply>
    {
        public DeliveryCRUDClient(TestWebApplicationFactory<Startup> factory) : base(factory,  "/api/delivery") { }
    }
}
