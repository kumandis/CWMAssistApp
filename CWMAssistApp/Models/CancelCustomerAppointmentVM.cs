namespace CWMAssistApp.Models
{
    public class CancelCustomerAppointmentVM
    {
        public string CustomerAppointmentId { get; set; }
        public int CancelReason { get; set; }
    }

    public enum CancelReason
    {
        Customer = 1,
        Company = 2
    }
}
