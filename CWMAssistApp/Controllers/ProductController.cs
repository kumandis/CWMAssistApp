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
    public class ProductController : Controller
    {
        private UserManager<AppUser> _userManager;
        private ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var model = new ProductVM();
            model.ProductList = new List<Product>();

            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
                if (user == null)
                {
                    ShowToastr("Müşteri bulunamadı", ToastrType.Warning);
                    return RedirectToAction("Index", "Product");
                }

                var result = _context.Products.Where(x => x.CompanyId == user.CompanyId && x.Status);
                if (result.Any())
                {
                    model.ProductList = result.ToList();
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
        public IActionResult AddProduct(ProductVM model)
        {
            if (!ModelState.IsValid)
            {
                ShowToastr("Girilen değerleri kontrol ediniz.", ToastrType.Warning);
                return RedirectToAction("Index", "Product");
            }

            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
                if (user == null)
                {
                    ShowToastr("Müşteri bulunamadı", ToastrType.Warning);
                    return RedirectToAction("Index", "Product");
                }

                var product = new Product()
                {
                    CompanyId = user.CompanyId,
                    Name = model.Name,
                    Price = model.Price,
                    CreatedDate = DateTime.Now,
                    CreatedName = user.NormalizedUserName,
                    Status = true
                };

                _context.Add(product);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                ShowToastr(ex.Message, ToastrType.Error);
                return RedirectToAction("Index", "Product");
            }

            ShowToastr("Hizmet kaydedildi", ToastrType.Success);
            return RedirectToAction("Index", "Product");
        }

        public JsonResult GetProductPrice(string id)
        {
            decimal price = 0;
            if (string.IsNullOrEmpty(id) || id == "0")
                return Json(price);

            var product = _context.Products.SingleOrDefault(x => x.Id == Guid.Parse(id));
            price = product?.Price ?? 0;
            return Json(price);
        }

        public void ShowToastr(string message, ToastrType notificationType)
        {
            var msg = "toastr." + notificationType.ToString().ToLower() + "('" + message + "','" + notificationType + "')" + "";
            TempData["notification"] = msg;
        }
    }
}
