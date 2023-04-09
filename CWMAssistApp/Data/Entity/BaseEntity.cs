namespace CWMAssistApp.Data.Entity
{
    public class BaseEntity
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedName { get; set; }
        public string? UpdatedName { get; set; }
        public bool Status { get; set; }
    }
}
