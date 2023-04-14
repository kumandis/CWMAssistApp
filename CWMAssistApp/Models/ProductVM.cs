using CWMAssistApp.Data.Entity;

namespace CWMAssistApp.Models
{
    public class ProductVM
    {
        public IEnumerable<Product>? ProductList { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
