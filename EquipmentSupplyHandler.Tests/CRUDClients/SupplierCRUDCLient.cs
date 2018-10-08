using ESHRepository.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace EquipmentSupplyHandler.Tests.CRUDClients
{
    class SupplierCRUDClient : CRUDClient<Supplier>
    {
        protected override string relativeUri => "/api/supplier";
        public SupplierCRUDClient(TestWebApplicationFactory<Startup> factory) : base(factory) { }
    }
}
