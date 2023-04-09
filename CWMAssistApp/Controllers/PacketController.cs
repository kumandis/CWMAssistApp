using CWMAssistApp.Data;
using CWMAssistApp.Data.Entity;
using CWMAssistApp.Models;
using CWMAssistApp.Services.Toastr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CWMAssistApp.Controllers
{
    [Authorize]
    public class PacketController : Controller
    {
        private UserManager<AppUser> _userManager;
        private ApplicationDbContext _context;

        public PacketController(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult PacketList()
        {
            var model = new PacketVM();

            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
                var result = _context.Packets.Where(x=> x.CompanyId == user.CompanyId && x.Status);
                model.PacketList = new List<Packet>();
                if (result.Any())
                {
                    model.PacketList = result;
                }
            }
            catch (Exception ex)
            {
                ShowToastr(ex.Message, ToastrType.Warning);
                return View();
            }
            
            return View(model);
        }
        [HttpPost]
        public IActionResult AddPacket(PacketVM model)
        {
            if (!ModelState.IsValid)
            {
                ShowToastr("Girilen değerleri kontrol ediniz.", ToastrType.Warning);
                return RedirectToAction("PacketList", "Packet");
            }

            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
                if (user == null)
                {
                    ShowToastr("Müşteri bulunamadı", ToastrType.Warning);
                    return RedirectToAction("PacketList", "Packet");
                }

                var packet = new Packet()
                {
                    CompanyId = user.CompanyId,
                    Name = model.Name,
                    PacketSize = model.PacketSize,
                    PacketPrice = model.PacketPrice,
                    CreatedDate = DateTime.Now,
                    CreatedName = user.NormalizedUserName,
                    Status = true
                };

                _context.Add(packet);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                ShowToastr(ex.Message, ToastrType.Error);
                return RedirectToAction("PacketList", "Packet");
            }
            ShowToastr("Paket kaydedildi", ToastrType.Success);
            return RedirectToAction("PacketList", "Packet");
        }

        [HttpPost]
        public JsonResult CancelPacket(string packetId)
        {
            if (!ModelState.IsValid)
            {
                return Json("Paket bilgileri hatalı");
            }

            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

                if (user == null)
                {
                    return Json("Kullanıcı bulunamadı");
                }

                var packet =
                    _context.Packets.SingleOrDefault(x => x.Id == Guid.Parse(packetId));
                if (packet != null)
                {
                    packet.Status = false;
                    packet.UpdatedName = user.NormalizedUserName;
                    packet.UpdatedDate = DateTime.Now;

                    _context.Packets.Update(packet);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return Json("200");
        }

        public void ShowToastr(string message, ToastrType notificationType)
        {
            var msg = "toastr." + notificationType.ToString().ToLower() + "('" + message + "','" + notificationType + "')" + "";
            TempData["notification"] = msg;
        }
    }
}
