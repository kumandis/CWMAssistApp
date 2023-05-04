namespace CWMAssistApp.Models
{
    public class CustomerPacketHistoryVM
    {
        public List<CustomerPacketHistory> CustomerPackets { get; set; }

    }

    public class CustomerPacketHistory
    {
        public Guid PacketId { get; set; }
        public string PacketName { get; set; }
        public bool Status { get; set; }
        public List<AppointmentHistory> Appointments { get; set; }
    }
    public class AppointmentHistory
    {
        public string AppointmentSubject { get; set; }
        public string AppointmentDate { get; set; }

    }
}
