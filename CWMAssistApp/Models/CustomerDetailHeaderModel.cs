using CWMAssistApp.Data.Entity;

namespace CWMAssistApp.Models
{
    public class CustomerDetailHeaderModel
    {
        public decimal ComplatedIncome { get; set; }
        public decimal PlannedIncome { get; set; }
        public int TotalAppointmentCount { get; set; }
        public int CancelAppointmentCount { get; set; }
        public List<CustomerAppointmentHistory> CustomerAppointmentHistory { get; set; }
    }

    public class CustomerAppointmentHistory
    {
        public Guid Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTitle { get; set; }
        public string PaymentType { get; set; }
        public string? ProductName { get; set; }
    }
}
