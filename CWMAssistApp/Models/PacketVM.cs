using CWMAssistApp.Data.Entity;

namespace CWMAssistApp.Models
{
    public class PacketVM
    {
        public IEnumerable<Packet>? PacketList { get; set; }
        public string Name { get; set; }
        public int PacketSize { get; set; }
        public decimal PacketPrice { get; set; }
    }
}
