using CWMAssistApp.Data.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CWMAssistApp.Models
{
    public class AppointmentVM
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
        public bool CheckWeek { get; set; }


        public List<SelectListItem>? TeachersSelectList { get; internal set; }

    }
}
