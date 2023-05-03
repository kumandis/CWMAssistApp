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
            var model = new SettingVM();

            var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

            var customerSmsPacket = _context.UserSmsPackets.SingleOrDefault(x => x.CompanyId == user.CompanyId && x.Status);
            if (customerSmsPacket != null)
            {
                model.SendSmsServiceEnable = customerSmsPacket.SendSmsEnable;
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(SettingVM model)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

            var customerSmsPacket = _context.UserSmsPackets.SingleOrDefault(x => x.CompanyId == user.CompanyId && x.Status);

            if (customerSmsPacket != null)
            {
                customerSmsPacket.SendSmsEnable = model.SendSmsServiceEnable;
                _context.Update(customerSmsPacket);
                _context.SaveChanges();
            }

            return View(model);
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
