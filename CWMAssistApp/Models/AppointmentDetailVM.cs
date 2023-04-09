using CWMAssistApp.Data.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CWMAssistApp.Models
{
    public class AppointmentDetailVM
    {
        public Guid? Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Subject { get; set; }
        public string PersonalId { get; set; }
        public string? PersonalName { get; set; }
        public int Capacity { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public decimal LessonPrice { get; set; }
        public decimal TeacherPrice { get; set; }
        public string? Title { get; set; }
        public decimal Rate { get; set; }
        public int RateProgressBar { get; set; }
        public decimal DefinedIncome { get; set; }
        public decimal SummaryIncome { get; set; }
        public decimal DefinedExpense { get; set; }


        public List<SelectListItem>? TeachersSelectList { get; internal set; }
        public List<RegistrationCustomer>? RegisteredCustomer { get; set; }
        public List<Customer>? CustomerList { get; set; }
    }

    public class RegistrationCustomer
    {
        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        public int IndexNumber { get; set; }
        public string Name { get; set; }
        public string ChildName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPacket { get; set; }
        public int PacketCount { get; set; }
        public int RemainingCount { get; set; }
        public string PacketText { get; set; }
    }
}
