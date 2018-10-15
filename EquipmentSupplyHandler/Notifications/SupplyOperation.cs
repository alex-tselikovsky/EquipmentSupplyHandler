using ESHRepository.Model;
using System;

namespace EquipmentSupplyHandler.Notifications
{
    public class SupplyOperation
    {
        public Supply Supply { get; set;}
        public Operation Operation { get; set; }
    }
    [Flags]
    public enum Operation
    {
        Created=1,
        Updated=2,
        Deleted=4,
        CreatedAndDeleted = Created|Deleted,
        CreatedUpdatedAndDeleted = Created|Updated|Deleted,
        CreatedAndUpdated = Created|Updated,
    }
}
