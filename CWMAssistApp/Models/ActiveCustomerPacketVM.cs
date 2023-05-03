namespace CWMAssistApp.Models
{
    public class ActiveCustomerPacketVM
    {
        public List<ActiveCustomerPacket> ActiveCustomerPackets { get; set; }
    }

    public class ActiveCustomerPacket
    {
        public string CustomerName { get; set; }
        public string CustomerPacketName { get; set; }
        public string PacketUsedCount { get; set; }
        public int RemainingCount { get; set; }
    }
}
