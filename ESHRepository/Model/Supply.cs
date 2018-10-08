using System;

namespace ESHRepository.Model
{
    public class Supply : GUIDEntity
    {
        public Supplier Supplier { get; set; }
        public string SupplierId { get; set; }
        public EquipmentType EquipmentType { get; set; }

        public string EquipmentTypeId { get; set; }
        public int Count { get; set; }
        public DateTime DeliveryDate { get; set; }

        public override bool Equals(object obj)
        {
            Supply n = obj as Supply;
            if (n == null) return false;
            return n.Id == this.Id
                && n.Count == this.Count
                && n.DeliveryDate == this.DeliveryDate
                && n.EquipmentTypeId == this.EquipmentTypeId
                && n.SupplierId == this.SupplierId;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
