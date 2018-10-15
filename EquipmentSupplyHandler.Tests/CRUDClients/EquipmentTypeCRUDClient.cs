using ESHRepository.Model;

namespace EquipmentSupplyHandler.Tests.CRUDClients
{
    internal class EquipmentTypeCRUDClient : CRUDClient<EquipmentType>
    {
        public EquipmentTypeCRUDClient(TestWebApplicationFactory<Startup> factory) : base(factory, "/api/equipment") { }
    }
}
