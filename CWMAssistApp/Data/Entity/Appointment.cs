using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CWMAssistApp.Data.Entity
{
    public class Appointment : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid PersonalId { get; set; }
        public Guid CompanyId { get; set; }
        public string PersonalName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Subject { get; set; }
        public int Capacity { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal LessonPrice { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal TeacherPrice { get; set; }
    }
}
