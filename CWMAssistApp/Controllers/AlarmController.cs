using CWMAssistApp.Data;
using CWMAssistApp.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CWMAssistApp.Controllers
{
    public class AlarmController : Controller
    {
        private ApplicationDbContext _context;

        public AlarmController(ApplicationDbContext context)
        {
            _context = context;
        }

        public void PushAlarm()
        {
            var alarm = new AlarmEntity()
            {
                CreatedDate = DateTime.Now,
                CreatedName = "Arduino UNO",
                Status = true,
                UpdatedDate = DateTime.Now,
                UpdatedName = "Arduino UNO"
            };

            _context.Add(alarm);
            _context.SaveChanges();
        }
    }
}
