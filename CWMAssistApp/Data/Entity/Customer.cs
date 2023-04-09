namespace CWMAssistApp.Data.Entity
{
    public class Customer : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ChildName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ChildBirthday { get; set; }
    }
}
