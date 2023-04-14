using CWMAssistApp.Data.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CWMAssistApp.Models
{
    public class CustomerDetailVM
    {
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string ChildName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime ChildBirthday { get; set; }
        public decimal ComplatedIncome { get; set; }
        public int TotalAppointmentCount { get; set; }
        public int CancelAppointmentCount { get; set; }
        public decimal PlannedIncome { get; set; }
        public List<CustomerPacketVM>? CustomerPacket { get; set; }
        public List<SelectListItem>? PacketsSelectList { get; internal set; }
        public List<CustomerAppointmentHistory>? CustomerAppointmentHistory { get; set; }
    }

    public class CustomerPacketVM
    {
        public Guid PacketId { get; set; }
        public string PacketName { get; set; }
        public string? ProductName { get; set; }
        public Guid? ProductId { get; set; }
        public int PacketSize { get; set; }
        public int UsedPieces { get; set; }
        public int PacketCreatedElapsedTime { get; set; }
        public int Rate { get; set; }
    }
}
