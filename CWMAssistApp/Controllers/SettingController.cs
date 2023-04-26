using CWMAssistApp.Data;
using CWMAssistApp.Data.Entity;
using CWMAssistApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CWMAssistApp.Controllers
{
    public class SettingController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<AppUser> _userManager;
        public SettingController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendBulkSms(BulkSmsModel model)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
            var result =
                new SmsQueueController(_context).SendBulkMessage(model.Message, user.CompanyId);
            return RedirectToAction("Index");
        }
    }
}
