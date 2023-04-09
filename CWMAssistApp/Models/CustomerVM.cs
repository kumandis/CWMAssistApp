using System.ComponentModel.DataAnnotations;
using CWMAssistApp.Data.Entity;
using Microsoft.Build.Framework;
using Microsoft.VisualBasic;

namespace CWMAssistApp.Models
{
    public class CustomerVM
    {
        public IEnumerable<Customer>? CustomerList { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ChildName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? ChildBirthday { get; set; }
        public CustomerListHeaderInfo? CustomerListHeaderInfo { get; set; }
    }

    public class CustomerListHeaderInfo
    {
        public string SixMonthVisiter { get; set; }
        public string ThreeMonthVisiter { get; set; }
        public string LastOneMonthVisiter { get; set; }
        public string WeekVisiter { get; set; }
    }
}
