using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CWMAssistApp.Data.Entity
{
    public class Packet : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public int PacketSize { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal PacketPrice { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal OneLessonPrice { get; set; }
    }
}
