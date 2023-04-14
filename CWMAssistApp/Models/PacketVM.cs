using CWMAssistApp.Data.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CWMAssistApp.Models
{
    public class PacketVM
    {
        public IEnumerable<Packet>? PacketList { get; set; }
        public string Name { get; set; }
        public int PacketSize { get; set; }
        public decimal PacketPrice { get; set; }
        public string SelectedProductId { get; set; }
        public List<SelectListItem>? ProductsSelectList { get; internal set; }
    }
}
