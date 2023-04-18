namespace CWMAssistApp.Data.Entity
{
    public class Message : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid CustomerId { get; set; }
        public string MessageBody { get; set; }
        public string ReceiverPhoneNumber { get; set; }
        public string Code { get; set; }
    }
}
