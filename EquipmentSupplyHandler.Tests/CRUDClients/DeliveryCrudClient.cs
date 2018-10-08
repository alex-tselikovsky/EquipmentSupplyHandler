using System.Threading.Tasks;
using ESHRepository.Model;

namespace EquipmentSupplyHandler.Tests.CRUDClients
{
    class DeliveryCRUDClient : CRUDClient<Supply>
    {
        protected override string relativeUri =>  "/api/delivery";
        public DeliveryCRUDClient(TestWebApplicationFactory<Startup> factory) : base(factory) { }
    }
}
