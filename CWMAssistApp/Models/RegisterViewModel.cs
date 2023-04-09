using System.ComponentModel.DataAnnotations;

namespace CWMAssistApp.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Lütfen adınızı giriniz.")]
    public string FullName { get; set; }
    [Required(ErrorMessage = "Lütfen E-Posta adresinizi giriniz.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Lütfen firma adını giriniz.")]
    public string CompanyName { get; set; }
    [Required(ErrorMessage = "Lütfen şifrenizi giriniz.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required(ErrorMessage = "Lütfen şifrenizi giriniz.")]
    [DataType(DataType.Password)]
    public string RePassword { get; set; }

}