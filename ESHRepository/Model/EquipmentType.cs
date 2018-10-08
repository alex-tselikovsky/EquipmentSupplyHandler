namespace ESHRepository.Model
{
    public class EquipmentType : GUIDEntity
    {
        public string Name { get; set; }
        public override bool Equals(object obj)
        {
            EquipmentType n = obj as EquipmentType;
            if (n == null) return false;
            return n.Id == this.Id && n.Name == this.Name;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }

}
