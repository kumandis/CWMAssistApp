using Microsoft.AspNetCore.Mvc;

namespace CWMAssistApp.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
