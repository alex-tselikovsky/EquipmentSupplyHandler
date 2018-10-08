using ESHRepository.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipmentSupplyHandler.Tests.CRUDClients
{
    internal class EquipmentTypeCRUDClient : CRUDClient<EquipmentType>
    {
        protected override string relativeUri => "/api/equipment";
        public EquipmentTypeCRUDClient(TestWebApplicationFactory<Startup> factory) : base(factory) { }
    }
}
