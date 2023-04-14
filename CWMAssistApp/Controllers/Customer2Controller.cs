using CWMAssistApp.Data.Entity;
using CWMAssistApp.Data;
using CWMAssistApp.Extention;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CWMAssistApp.Models;
using CWMAssistApp.Services.Toastr;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CWMAssistApp.Controllers
{
    [Authorize]
    public class Customer2Controller : Controller
    {
        private UserManager<AppUser> _userManager;
        private ApplicationDbContext _context;

        public Customer2Controller(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult CustomerList()
        {
            CustomerVM model = new CustomerVM();
            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
                if (user == null)
                {
                    ShowToastr("Müşteri bulunamadı", ToastrType.Warning);
                    return View();
                }

                var result = _context.Customers.Where(x => x.StoreId == user.CompanyId);
                model.CustomerList = new List<Customer>();
                if (result.Any())
                {
                    model.CustomerList = result;
                }

                var headerInfo = GetCustomerListHeaderInfo(user.CompanyId);

                model.CustomerListHeaderInfo = headerInfo;
            }
            catch (Exception ex)
            {
                ShowToastr(ex.Message, ToastrType.Warning);
                return View();
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult AddCustomer(CustomerVM model)
        {
            if (!ModelState.IsValid)
            {
                ShowToastr("Girilen değerleri kontrol ediniz.", ToastrType.Warning);
                return RedirectToAction("CustomerList", "Customer2");
            }

            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
                if (user == null)
                {
                    ShowToastr("Müşteri bulunamadı", ToastrType.Warning);
                    return RedirectToAction("CustomerList", "Customer2");
                }
                

                Customer customer = new Customer()
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    ChildName = model.ChildName,
                    PhoneNumber = model.PhoneNumber,
                    ChildBirthday = model.ChildBirthday.Value,
                    StoreId = user.CompanyId,
                    CreatedDate = DateTime.Now,
                    CreatedName = user.NormalizedUserName,
                };
                _context.Add(customer);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ShowToastr(ex.Message, ToastrType.Error);
                return RedirectToAction("CustomerList", "Customer2");
            }

            ShowToastr("Kayıt yapılmıştır.", ToastrType.Success);
            return RedirectToAction("CustomerList", "Customer2");
        }
        
        public IActionResult Detail(string id)
        {
            CustomerDetailVM model;
            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
                var customer = _context.Customers.Single(x => x.Id == Guid.Parse(id));
                if (customer == null)
                {
                    ShowToastr("Müşteri bulunamadı", ToastrType.Warning);
                    return RedirectToAction("CustomerList","Customer2");
                }

                var customerPackets = _context.CustomerPackets.Where(x => x.CustomerId == customer.Id && x.Status == true).ToList();

                var packets = _context.Packets.Where(x =>x.CompanyId == user.CompanyId && x.Status);

                var customerPacketVM = new List<CustomerPacketVM>();

                foreach (var customerPacket in customerPackets)
                {
                    var packetUsedCount = _context.CustomerAppointments.Count(x => x.CustomerId == customer.Id && x.PacketId == customerPacket.Id && x.Status);
                    var packetCreatedElapsedTime = 0;
                    if (customerPacket.CreatedDate != null)
                    {
                        var elapsedTimeSpan = (TimeSpan)(DateTime.Now - customerPacket.CreatedDate);
                        packetCreatedElapsedTime = elapsedTimeSpan.Days;
                    }

                    var _rate = ((decimal)packetUsedCount / (decimal)customerPacket.PacketSize) * 100;
                    customerPacketVM.Add(new CustomerPacketVM()
                    {
                        PacketName = customerPacket.PacketName,
                        ProductName = customerPacket.ProductName,
                        ProductId = customerPacket?.ProductId,
                        UsedPieces = packetUsedCount,
                        PacketSize = customerPacket.PacketSize,
                        PacketCreatedElapsedTime = packetCreatedElapsedTime+1,
                        PacketId = customerPacket.Id,
                        Rate = (int)_rate,
                    });
                }

                var headerModel = GetCustomerDetailHeaderModel(customer.Id);

                model = new CustomerDetailVM()
                {
                    CustomerId = customer.Id,
                    CustomerName = customer.Name,
                    CustomerSurname = customer.Surname,
                    PhoneNumber = customer.PhoneNumber,
                    ChildBirthday = customer.ChildBirthday,
                    ChildName = customer.ChildName,
                    CustomerPacket = customerPacketVM,
                    TotalAppointmentCount = headerModel.TotalAppointmentCount,
                    ComplatedIncome = headerModel.ComplatedIncome,
                    CancelAppointmentCount = headerModel.CancelAppointmentCount,
                    PlannedIncome = headerModel.PlannedIncome,
                    CustomerAppointmentHistory = headerModel.CustomerAppointmentHistory
                };

                model.PacketsSelectList = new List<SelectListItem>();

                foreach (var packet in packets)
                {
                    if (customerPacketVM.Any(x=>x.ProductId == packet.ProductId) == false)
                    {
                        model.PacketsSelectList.Add(new SelectListItem(packet.Name, packet.Id.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                ShowToastr(ex.Message, ToastrType.Warning);
                return RedirectToAction("CustomerList", "Customer2");
            }
            return View(model);
        }

        public IActionResult Update(CustomerDetailVM model)
        {
            if (!ModelState.IsValid)
            {
                ShowToastr("Girilen değerler hatalı", ToastrType.Warning);
                return RedirectToAction("Detail", new { id = model.CustomerId.ToString() });
            }

            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

                if (user == null)
                {
                    ShowToastr("Kullanıcı bulunamadı", ToastrType.Warning);
                    return RedirectToAction("Login", "Account");
                }

                var entity = _context.Customers.SingleOrDefault(x => x.Id == model.CustomerId);

                if (entity == null)
                {
                    ShowToastr("Güncellenecek veri bulunamadı", ToastrType.Warning);
                    return RedirectToAction("Detail", new { id = model.CustomerId.ToString() });
                }

                if (entity.Name == model.CustomerName &&
                    entity.Surname == model.CustomerSurname &&
                    entity.PhoneNumber == model.PhoneNumber &&
                    entity.ChildBirthday.ToShortDateString() == model.ChildBirthday.ToShortDateString() &&
                    entity.ChildName == model.ChildName)
                {
                    ShowToastr("Değişiklik saptanamadı.", ToastrType.Warning);
                    return RedirectToAction("Detail", new { id = model.CustomerId.ToString() });
                }

                entity.Name = model.CustomerName;
                entity.Surname = model.CustomerSurname;
                entity.PhoneNumber = model.PhoneNumber;
                entity.ChildBirthday = model.ChildBirthday;
                entity.ChildName = model.ChildName;
                entity.UpdatedDate = DateTime.Now;
                entity.UpdatedName = user.NormalizedUserName;

                _context.Update(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ShowToastr("Değişiklik yapılırken bir hata meydana geldi", ToastrType.Error);
                return RedirectToAction("Detail", new { id = model.CustomerId });
            }

            ShowToastr("Değişiklik yapıldı", ToastrType.Success);
            return RedirectToAction("Detail", new { id = model.CustomerId });
        }

        [HttpPost]
        public JsonResult AddCustomerPacket(AddCustomerPacketVM model)
        {
            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);
                var packetIdGuid = Guid.Parse(model.PacketId);
                var packet = _context.Packets.SingleOrDefault(x => x.Id == packetIdGuid);

                if (user == null)
                {
                    return Json("Kullanıcı bulunamadı");
                }

                var customerPacket = new CustomerPacket()
                {
                    CreatedDate = DateTime.Now,
                    CreatedName = user.NormalizedUserName,
                    CustomerId = Guid.Parse(model.CustomerId),
                    PacketId = packetIdGuid,
                    Status = true,
                    PacketSize = packet.PacketSize,
                    PacketPrice = packet.PacketPrice,
                    OneLessonPrice = packet.PacketPrice / packet.PacketSize,
                    PacketName = packet.Name,
                    ProductId = packet.ProductId,
                    ProductName = packet.ProductName
                };
                _context.CustomerPackets.Add(customerPacket);
                _context.SaveChanges();
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


        public CustomerDetailHeaderModel GetCustomerDetailHeaderModel(Guid customerId)
        {
            var model = new CustomerDetailHeaderModel();
            decimal complatedTotalPrice = 0;
            decimal plannedTotalPrice = 0;
            var totalAppointmentCount = 0;
            var totalCancelledAppointmentCount = 0;
            model.CustomerAppointmentHistory = new List<CustomerAppointmentHistory>();

            var customerAppointments = _context.CustomerAppointments.Where(x => x.CustomerId == customerId && x.Status).OrderByDescending(x=>x.AppointmentDate);
            totalCancelledAppointmentCount =
                _context.CustomerAppointments.Count(x => x.CustomerId == customerId && x.Status == false);
            totalAppointmentCount = customerAppointments.Count();

            foreach (var customerAppointment in customerAppointments)
            {
                var _appointment = _context.Appointments.SingleOrDefault(x => x.Id == customerAppointment.AppointmentId);

                

                if (_appointment != null)
                {
                    if (customerAppointment.PaymentType == (int)PaymentType.Cash)
                    {
                        if (_appointment.StartDate < DateTime.Now)
                        {
                            complatedTotalPrice += _appointment.LessonPrice;
                        }
                        else
                        {
                            plannedTotalPrice += _appointment.LessonPrice;
                        }
                    }
                    else if (customerAppointment.PaymentType == (int)PaymentType.Packet)
                    {
                        if (customerAppointment.PacketId != null)
                        {
                            var customerPacket =
                                _context.CustomerPackets.SingleOrDefault(x => x.Id == customerAppointment.PacketId);

                            if (customerPacket != null)
                            {
                                if (_appointment.StartDate < DateTime.Now)
                                {
                                    complatedTotalPrice += customerPacket.OneLessonPrice;
                                }
                                else
                                {
                                    plannedTotalPrice += customerPacket.OneLessonPrice;
                                }
                            }
                        }
                    }
                    model.CustomerAppointmentHistory.Add(new CustomerAppointmentHistory()
                    {
                        Id = _appointment.Id,
                        AppointmentDate = _appointment.StartDate,
                        AppointmentTitle = _appointment.Subject,
                        PaymentType = customerAppointment.PaymentType == (int)PaymentType.Packet ? "Paket" : "Nakit"
                    });
                }
            }

            model.PlannedIncome = plannedTotalPrice;
            model.ComplatedIncome = complatedTotalPrice;
            model.TotalAppointmentCount = totalAppointmentCount;
            model.CancelAppointmentCount = totalCancelledAppointmentCount;
            return model;
        }

        public CustomerListHeaderInfo GetCustomerListHeaderInfo(Guid companyId)
        {
            var response = new CustomerListHeaderInfo();

            string _sixMonthVisiter="", _threeMonthVisiter = "", _lastOneMonthVisiter = "", _weekVisiter = "";
            var dateTimeNow = DateTime.Now;
            var lastSixMonthThreshold =
                new DateTime(dateTimeNow.AddMonths(-5).Year, dateTimeNow.AddMonths(-5).Month, 01, 0, 0, 0);
            var lastThreeMonthThreshold =
                new DateTime(dateTimeNow.AddMonths(-2).Year, dateTimeNow.AddMonths(-2).Month, 01, 0, 0, 0);
            var lastOneMonthThreshold =
                new DateTime(dateTimeNow.Year, dateTimeNow.Month, 01, 0, 0, 0);

            var startWeekDay = TimeHelper.GetWeekFirstDay();
            var endWeekDay = TimeHelper.GetWeekLastDay();

            var lastWeekStartThreshold = new DateTime(startWeekDay.Year, startWeekDay.Month, startWeekDay.Day, 0, 0, 0);
            var lastWeekEndThreshold = new DateTime(endWeekDay.Year, endWeekDay.Month, endWeekDay.Day, 23, 59, 59);


            var lastSixMonthVisiters = _context.CustomerAppointments.
                Where(x => x.CompanyId == companyId && x.Status && x.AppointmentDate > lastSixMonthThreshold);

            #region Son 6 Ay

            var sixMonthVisiter = lastSixMonthVisiters.GroupBy(x => x.CustomerId)
                .Select(g => new { g.Key, Count = g.Count() }).OrderByDescending(p => p.Count).FirstOrDefault();

            if (sixMonthVisiter != null)
            {
                var sixMonthVisitUser = _context.Customers.SingleOrDefault(x => x.Id == sixMonthVisiter.Key);
                _sixMonthVisiter = sixMonthVisitUser.Name + " " + sixMonthVisitUser.Surname + "("+ sixMonthVisiter.Count+")";
            }

            #endregion

            #region Son 3 Ay

            var threeMonthVisiter =
                lastSixMonthVisiters.Where(x => x.AppointmentDate > lastThreeMonthThreshold).GroupBy(x => x.CustomerId)
                    .Select(g => new { g.Key, Count = g.Count() }).OrderByDescending(p => p.Count).FirstOrDefault();

            if (threeMonthVisiter != null)
            {
                var threeMonthVisitUser = _context.Customers.SingleOrDefault(x => x.Id == threeMonthVisiter.Key);
                _threeMonthVisiter = threeMonthVisitUser.Name +" "+ threeMonthVisitUser.Surname +"("+ threeMonthVisiter.Count+")";
            }

            #endregion

            #region Son 1 Ay

            var oneMonthVisiter =
                lastSixMonthVisiters.Where(x => x.AppointmentDate > lastOneMonthThreshold).GroupBy(x => x.CustomerId)
                    .Select(g => new { g.Key, Count = g.Count() }).OrderByDescending(p => p.Count).FirstOrDefault();

            if (oneMonthVisiter != null)
            {
                var oneMonthVisitUser = _context.Customers.SingleOrDefault(x => x.Id == oneMonthVisiter.Key);
                _lastOneMonthVisiter = oneMonthVisitUser.Name + " " + oneMonthVisitUser.Surname +"("+oneMonthVisiter.Count+")";
            }

            #endregion

            #region Son 1 Hafta

            var lastWeekVisiter =
                lastSixMonthVisiters.Where(x => x.AppointmentDate > lastWeekStartThreshold && x.AppointmentDate < lastWeekEndThreshold).
                    GroupBy(x => x.CustomerId)
                    .Select(g => new { g.Key, Count = g.Count() }).OrderByDescending(p => p.Count).FirstOrDefault();

            if (lastWeekVisiter != null)
            {
                var lastWeekVisitUser = _context.Customers.SingleOrDefault(x => x.Id == lastWeekVisiter.Key);
                _weekVisiter = lastWeekVisitUser.Name + " " + lastWeekVisitUser.Surname + "("+ lastWeekVisiter.Count +")";
            }

            #endregion


            response.SixMonthVisiter = _sixMonthVisiter;
            response.ThreeMonthVisiter = _threeMonthVisiter;
            response.LastOneMonthVisiter = _lastOneMonthVisiter;
            response.WeekVisiter = _weekVisiter;
            return response;
        }
    }
}
