using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CWMAssistApp.Data.Entity
{
    public class Personal : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Profession { get; set; }
        public string PhoneNumber { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal SeansPrice { get; set; }
        public WorkingType WorkingType { get; set; }
    }

    public enum WorkingType
    {
        [Description("Tam Zamanlı")]
        FullTime = 0,
        [Description("Seans Başı Ücretli")]
        PartTime = 1
    }
}
