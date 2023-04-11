using CWMAssistApp.Data.Entity;
using CWMAssistApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CWMAssistApp.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Bilgilerinizi kontrol ediniz.");
                return View(model);
            }

            try
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(String.Empty, "Kullanıcı adı veya şifre hatalı.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Appointment", "Appointment");
                }
                ModelState.AddModelError(String.Empty, "Kullanıcı adı veya şifre hatalı.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Bilgilerinizi kontrol ediniz.");
                return View(model);
            }

            if (model.Password != model.RePassword)
            {
                ModelState.AddModelError(String.Empty, "Şifreler eşleşmiyor.");
                return View(model);
            }

            try
            {
                AppUser appUser = new AppUser()
                {
                    CompanyId = Guid.NewGuid(),
                    CompanyName = model.CompanyName,
                    Email = model.Email,
                    IsAdmin = true,
                    FullName = model.FullName,
                    UserName = model.Email
                };
                IdentityResult result = _userManager.CreateAsync(appUser,model.Password).Result;

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(String.Empty, result.Errors.FirstOrDefault()?.Description);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View(model);
            }
            return View("Login");
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Login");
        }
        public IActionResult Denied()
        {
            return View();
        }

        public static string ConvertEnglishChar(string text)
        {

            char[] turkishChars = { 'ı', 'ğ', 'İ', 'Ğ', 'ç', 'Ç', 'ş', 'Ş', 'ö', 'Ö','ü','Ü',' '};
            char[] englishChars = { 'i', 'g', 'I', 'G', 'c', 'C', 's', 'S', 'o', 'O', 'u', 'U', '.' };
            for (int i = 0; i < turkishChars.Length; i++)
            {

                text = text.Replace(turkishChars[i], englishChars[i]);

            }
            return text;
        }
    }
}
