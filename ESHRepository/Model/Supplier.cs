namespace ESHRepository.Model
{
    public class Supplier : GUIDEntity
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public override bool Equals(object obj)
        {
            Supplier n = obj as Supplier;
            if (n == null) return false;
            return n.Id == this.Id
                && n.Address == this.Address
                && n.Email == this.Email
                && n.Name == this.Name
                && n.Phone == this.Phone;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
