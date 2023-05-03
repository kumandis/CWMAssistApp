using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CWMAssistApp.Data.Entity
{
    public class CustomerPacket : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid PacketId { get; set; }
        public string PacketName { get; set; }
        public int PacketSize { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal PacketPrice { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal OneLessonPrice { get; set; }
        public Guid? ProductId { get; set; }
        public string? ProductName { get; set; }
    }
}
