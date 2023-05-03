namespace CWMAssistApp.Models
{
    public class IndexVM
    {
        public string MonthName { get; set; }
        public int TotalAppointmentCount { get; set; }
        public int TotalVisitorCount { get; set; }
        public int TotalUniqueVisitorCount { get; set; }
        public string TimelineStartText { get; set; }
        public List<AppointmentIndexVm> Appointments { get; set; }
        public int DailyVisitorCount { get; set; }
        public decimal DailyTotalIncome { get; set; }
        public int TotalActivePacketCount { get; set; }
    }

    public class CustomerAppointmentIndexVm
    {
        public int RowNumber { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class AppointmentIndexVm
    {
        public List<CustomerAppointmentIndexVm> CustomerAppointments { get; set; }
        public Guid AppointmentId { get; set; }
        public string TeacherName { get; set; }
        public string AppointmentHour { get; set; }
        public string Subject { get; set; }
    }
}
