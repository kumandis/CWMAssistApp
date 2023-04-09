namespace CWMAssistApp.Data.Entity
{
    public class CustomerAppointment : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid AppointmentId { get; set; }
        public int PaymentType { get; set; }
        public Guid? PacketId { get; set; }
        public Guid? CompanyId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int? CancelReason { get; set; }
    }
    public enum PaymentType{
        None = 0,
        Cash = 1,
        Packet = 2
    }
}
