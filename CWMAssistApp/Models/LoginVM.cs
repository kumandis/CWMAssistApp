using System.ComponentModel.DataAnnotations;

namespace CWMAssistApp.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Lütfen mailinizi giriniz")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Lütfen şifrenizi giriniz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
