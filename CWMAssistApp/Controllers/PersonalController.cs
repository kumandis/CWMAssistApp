using CWMAssistApp.Data.Entity;
using CWMAssistApp.Data;
using CWMAssistApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CWMAssistApp.Services.Toastr;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace CWMAssistApp.Controllers
{
    [Authorize]
    public class PersonalController : Controller
    {
        private UserManager<AppUser> _userManager;
        private ApplicationDbContext _context;

        public PersonalController(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult PersonalList()
        {
            var model = new PersonalVM();

            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

                if (user == null)
                {
                    ShowToastr("Kullanıcı bulunamadı", ToastrType.Warning);
                    return View();
                }

                var result = _context.Personals.Where(x => x.CompanyId == user.CompanyId && x.Status);

                model.PersonalList = new List<Personal>();

                if (result.Any())
                {
                    model.PersonalList = result;
                }

            }
            catch (Exception ex)
            {
                ShowToastr(ex.Message, ToastrType.Warning);
                return View();
            }

            return View(model);
        }

        public JsonResult GetPersonelById(string personalId)
        {
            if (personalId.IsNullOrEmpty())
            {
                return Json("Personel bulunamadı");
            }

            try
            {
                var guidPersonalId = Guid.Parse(personalId);
                var personal = _context.Personals.Where(x => x.Id == guidPersonalId);

                if (personal.IsNullOrEmpty())
                {
                    return Json("Personel bulunamadı");
                }

                return Json(personal);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult AddOrUpdatePersonal(PersonalVM model)
        {
            if (!ModelState.IsValid)
            {
                ShowToastr("Girilen değerleri kontrol ediniz.", ToastrType.Warning);
                return RedirectToAction("PersonalList", "Personal");
            }

            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
                if (user == null)
                {
                    ShowToastr("Kullanıcı bulunamadı", ToastrType.Warning);
                    return RedirectToAction("PersonalList", "Personal");
                }

                if (model.PersonalId != null && model.PersonalId != Guid.Empty)
                {
                    var personalEntity = _context.Personals.SingleOrDefault(x => x.Id == model.PersonalId);

                    personalEntity.Name = model.Name;
                    personalEntity.Profession = model.Profession;
                    personalEntity.PhoneNumber = model.PhoneNumber;
                    personalEntity.SeansPrice = model.SeansPrice;
                    personalEntity.UpdatedDate = DateTime.Now;
                    personalEntity.UpdatedName = user.NormalizedUserName;

                    _context.Update(personalEntity);
                    _context.SaveChanges();

                    ShowToastr("Personel güncellendi", ToastrType.Success);
                }
                else
                {
                    var personal = new Personal()
                    {
                        CompanyId = user.CompanyId,
                        Name = model.Name,
                        Profession = model.Profession,
                        PhoneNumber = model.PhoneNumber,
                        SeansPrice = model.SeansPrice,
                        CreatedDate = DateTime.Now,
                        CreatedName = user.NormalizedUserName,
                        Status = true
                    };

                    _context.Add(personal);
                    _context.SaveChanges();

                    ShowToastr("Personel kaydedildi", ToastrType.Success);
                }
            }
            catch (Exception ex)
            {
                ShowToastr(ex.Message, ToastrType.Error);
                return RedirectToAction("PersonalList", "Personal");
            }
            
            return RedirectToAction("PersonalList", "Personal");
        }

        [HttpPost]
        public JsonResult CancelPersonal(string personalId)
        {
            if (!ModelState.IsValid)
            {
                return Json("Personel bilgileri hatalı");
            }

            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

                if (user == null)
                {
                    return Json("Kullanıcı bulunamadı");
                }

                var personal =
                    _context.Personals.SingleOrDefault(x => x.Id == Guid.Parse(personalId));
                if (personal != null)
                {
                    personal.Status = false;
                    personal.UpdatedName = user.NormalizedUserName;
                    personal.UpdatedDate = DateTime.Now;

                    _context.Personals.Update(personal);
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
