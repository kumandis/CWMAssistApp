using CWMAssistApp.Data.Entity;

namespace CWMAssistApp.Models
{
    public class PersonalVM
    {
        public IEnumerable<Personal>? PersonalList { get; set; }
        public Guid? PersonalId { get; set; }
        public string Name { get; set; }
        public string Profession { get; set; }
        public string PhoneNumber { get; set; }
        public decimal SeansPrice { get; set; }
    }
}
