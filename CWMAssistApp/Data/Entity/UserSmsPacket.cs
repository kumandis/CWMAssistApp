namespace CWMAssistApp.Data.Entity
{
    public class UserSmsPacket : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId{ get; set; }
        public Guid SmsPacketId { get; set; }
        public string SmsPacketName { get; set; }
        public string ServiceUserName { get; set; }
        public string ServicePassword { get; set; }
        public int PacketSize { get; set; }
    }
}
