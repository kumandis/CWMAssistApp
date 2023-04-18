using CWMAssistApp.Data.Entity;
using CWMAssistApp.Data;
using CWMAssistApp.Extention;
using CWMAssistApp.Models;
using CWMAssistApp.Services.Toastr;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authorization;

namespace CWMAssistApp.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private UserManager<AppUser> _userManager;
        private ApplicationDbContext _context;

        public AppointmentController(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Appointment()
        {
            var model = new AppointmentVM();

            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

                if (user == null)
                {
                    ShowToastr("Kullanıcı bulunamadı", ToastrType.Warning);
                    return View();
                }

                var personals = _context.Personals.Where(x => x.CompanyId == user.CompanyId && x.Status);
                var products = _context.Products.Where(x => x.CompanyId == user.CompanyId && x.Status);

                model.TeachersSelectList = new List<SelectListItem>();
                model.ProductsSelectList = new List<SelectListItem>();
                
                foreach (var personal in personals)
                {
                    model.TeachersSelectList.Add(new SelectListItem(personal.Name, personal.Id.ToString()));
                }

                foreach (var product in products)
                {
                    model.ProductsSelectList.Add(new SelectListItem(product.Name, product.Id.ToString()));
                }

            }
            catch (Exception ex)
            {
                ShowToastr(ex.Message, ToastrType.Warning);
                return View();
            }
            
            return View(model);
        }

        public JsonResult GetAppointments()
        {
            var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);



            var model = _context.Appointments.Where(x => x.CompanyId == user.CompanyId)
                .Select(x => new AppointmentVM()
                {
                    Id = x.Id,
                    PersonalName = x.PersonalName,
                    Subject = x.Subject,
                    Capacity = x.Capacity,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    LessonPrice = x.LessonPrice,
                    MaxAge = x.MaxAge,
                    MinAge = x.MinAge,
                    PersonalId = x.PersonalId.ToString(),
                    TeacherPrice = x.TeacherPrice
                });

            return Json(model);
        }

        public IActionResult AddOrUpdateAppointment(AppointmentVM model)
        {
            if (!ModelState.IsValid)
            {
                ShowToastr("Girilen değerler hatalı", ToastrType.Warning);
                return RedirectToAction("Appointment");
            }

            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

                if (user == null)
                {
                    ShowToastr("Kullanıcı bulunamadı", ToastrType.Warning);
                    return RedirectToAction("Appointment");
                }

                var personal = _context.Personals.SingleOrDefault(x => x.Id == Guid.Parse(model.PersonalId));

                var product = _context.Products.SingleOrDefault(x => x.Id == Guid.Parse(model.SelectedProductId));

                var weekCounter = 0;

                if (model.CheckWeek)
                {
                    var dayOfWeek = model.StartDate.DayOfWeek;

                    if (dayOfWeek != 0)
                    {
                        weekCounter = (int)(7 - dayOfWeek);
                    }
                }

                for (var i = 0; i <= weekCounter; i++)
                {
                    var appointment = new Appointment()
                    {
                        Capacity = model.Capacity,
                        CompanyId = user.CompanyId,
                        CreatedDate = DateTime.Now,
                        CreatedName = user.NormalizedUserName,
                        EndDate = model.EndDate,
                        LessonPrice = model.LessonPrice,
                        MaxAge = model.MaxAge,
                        MinAge = model.MinAge,
                        PersonalId = personal.Id,
                        StartDate = model.StartDate,
                        Subject = model.Subject,
                        TeacherPrice = model.TeacherPrice,
                        Status = true,
                        PersonalName = personal.Name,
                        ProductId = product?.Id,
                        ProductName = product?.Name,
                    };
                    _context.Appointments.Add(appointment);
                    model.StartDate = model.StartDate.AddDays(1);
                    model.EndDate = model.EndDate.AddDays(1);
                }
                
                _context.SaveChanges();

                ShowToastr("Seans oluşturuldu", ToastrType.Success);
                return RedirectToAction("Appointment");

            }
            catch (Exception ex)
            {
                ShowToastr(ex.Message, ToastrType.Warning);
                return RedirectToAction("Appointment");
            }

        }

        public IActionResult UpdateAppointment(AppointmentDetailVM model)
        {
            if (!ModelState.IsValid)
            {
                ShowToastr("Girilen değerler hatalı", ToastrType.Warning);
                return RedirectToAction("Detail", new { id = model.Id.ToString() });
            }

            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

                if (user == null)
                {
                    ShowToastr("Kullanıcı bulunamadı", ToastrType.Warning);
                    return RedirectToAction("Login", "Account");
                }

                

                var entity = _context.Appointments.SingleOrDefault(x => x.Id == model.Id);

                if (entity == null)
                {
                    ShowToastr("Güncellenecek veri bulunamadı", ToastrType.Warning);
                    return RedirectToAction("Detail", new { id = model.Id.ToString() });
                }

                if (entity.Capacity == model.Capacity &&
                entity.CompanyId == user.CompanyId &&
                entity.EndDate == model.EndDate &&
                entity.StartDate == model.StartDate &&
                entity.LessonPrice == model.LessonPrice &&
                entity.MaxAge == model.MaxAge &&
                entity.MinAge == model.MinAge &&
                entity.PersonalId == model.PersonalId &&
                entity.Subject == model.Subject &&
                entity.TeacherPrice == model.TeacherPrice &&
                entity.ProductId == model.ProductId)
                {
                    ShowToastr("Değişiklik saptanamadı.", ToastrType.Warning);
                    return RedirectToAction("Detail", new { id = model.Id.ToString() });
                }

                if (entity.PersonalId != model.PersonalId)
                {
                    var personal = _context.Personals.SingleOrDefault(x => x.Id == model.PersonalId);
                    entity.PersonalId = personal.Id;
                    entity.PersonalName = personal.Name;
                }

                if (entity.ProductId != model.ProductId)
                {
                    var product = _context.Products.SingleOrDefault(x => x.Id == model.ProductId);
                    entity.ProductId = product.Id;
                    entity.ProductName = product.Name;
                }

                entity.Capacity = model.Capacity;
                entity.CompanyId = user.CompanyId;
                entity.UpdatedName = user.NormalizedUserName;
                entity.UpdatedDate = DateTime.Now;
                entity.EndDate = model.EndDate;
                entity.StartDate = model.StartDate;
                entity.LessonPrice = model.LessonPrice;
                entity.MaxAge = model.MaxAge;
                entity.MinAge = model.MinAge;
                entity.Subject = model.Subject;
                entity.TeacherPrice = model.TeacherPrice;
                entity.Status = true;
                

                _context.Update(entity);
                _context.SaveChanges();
                
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            ShowToastr("Değişiklik yapıldı", ToastrType.Success);
            return RedirectToAction("Detail", new { id = model.Id.ToString() });
        }

        public IActionResult Detail(string id)
        {
            var model = new AppointmentDetailVM();
            try
            {
                var user = _userManager.Users.SingleOrDefault(x => HttpContext.User.Identity != null && x.UserName == HttpContext.User.Identity.Name);

                if (user == null)
                {
                    ShowToastr("Kullanıcı bulunamadı", ToastrType.Warning);
                    return RedirectToAction("Appointment");
                }

                var appointment = _context.Appointments.Single(x => x.Id == Guid.Parse(id));

                if (appointment == null)
                {
                    ShowToastr("Randevu bulunamadı", ToastrType.Warning);
                    return RedirectToAction("Appointment");
                }

                
                var registratedCustomerList =
                    _context.CustomerAppointments.Where(x => x.AppointmentId == appointment.Id && x.Status);

                var registrationCustomerList = _context.Customers.Where(x => x.StoreId == user.CompanyId && 
                                                                             !registratedCustomerList.Select(p=>p.CustomerId).Contains(x.Id)).ToList();
                
                //foreach (var customer in customerList)
                //{
                //var isRegistered = registratedCustomerList.Select(x => x.CustomerId).Contains(customer.Id);
                //if (!isRegistered)
                //{
                //var isActivePacket =
                //    _context.CustomerPackets.Where(x => x.CustomerId == customer.Id && x.Status == true).OrderBy(t=> t.CreatedDate).FirstOrDefault();
                //var _remainingCount = 0;

                //if (isActivePacket != null)
                //    _remainingCount = _context.CustomerAppointments.Count(x =>
                //        x.CustomerId == customer.Id && x.PacketId == isActivePacket.Id && x.Status);

                //registrationCustomerList.Add(new Customer()
                //{
                //    Id = customer.Id,
                //    Name = customer.Name + " "+ customer.Surname,
                //    ChildName = customer.ChildName,
                //    IsPacket = true,
                //    PacketCount = 0,  //isActivePacket == null ? 0 : isActivePacket.PacketSize,
                //    RemainingCount = 0, //_remainingCount,
                //    PacketText = "-" //isActivePacket == null ? "-" : _remainingCount+"/"+isActivePacket.PacketSize
                //});
                //}
                //}

                var registratedCustomers = new List<RegistrationCustomer>();
                var indexNumber = 1;
                decimal _definedIncome = 0;
                var _isActivePacket = false;
                foreach (var customerAppointment in registratedCustomerList)
                {
                    var customerEntity = _context.Customers.SingleOrDefault(x => x.Id == customerAppointment.CustomerId);
                    
                    if (customerAppointment.PaymentType == (int)PaymentType.Packet)
                    {
                        var customerPacket = _context.CustomerPackets.Single(x => x.Id == customerAppointment.PacketId);
                        _definedIncome += customerPacket.OneLessonPrice;
                        _isActivePacket = customerPacket.Status;
                    }
                    else
                    {
                        _definedIncome += appointment.LessonPrice;
                    }
                    registratedCustomers.Add(new RegistrationCustomer()
                    {
                        Id = customerAppointment.Id,
                        ChildName = customerEntity.ChildName,
                        IsPacket = customerAppointment.PaymentType == (int)PaymentType.Packet,
                        Name = customerEntity.Name +" "+ customerEntity.Surname,
                        PhoneNumber = customerEntity.PhoneNumber,
                        IndexNumber = indexNumber,
                        CustomerId = customerEntity.Id,
                        IsActivePacket = _isActivePacket
                    });
                    indexNumber++;
                }

                var personals = _context.Personals.Where(x => x.CompanyId == user.CompanyId && x.Status);
                model.TeachersSelectList = new List<SelectListItem>();

                foreach (var personal in personals)
                {
                    if (personal.Id != appointment.PersonalId)
                    {
                        model.TeachersSelectList.Add(new SelectListItem(personal.Name, personal.Id.ToString()));
                    }
                    
                }

                var products = _context.Products.Where(x => x.CompanyId == user.CompanyId && x.Status);
                model.ProductsSelectList = new List<SelectListItem>();

                foreach (var product in products)
                {
                    if (product.Id != appointment.ProductId)
                    {
                        model.ProductsSelectList.Add(new SelectListItem(product.Name, product.Id.ToString()));
                    }
                }

                model.Title = appointment.Subject;
                model.TitleDate = appointment.StartDate.ToLongDateString();
                model.TitleHour = appointment.StartDate.ToString("HH:mm");
                model.Capacity = appointment.Capacity;
                model.StartDate = appointment.StartDate;
                model.EndDate = appointment.EndDate;
                model.MinAge = appointment.MinAge;
                model.MaxAge = appointment.MaxAge;
                model.Subject = appointment.Subject;
                model.ProductId = appointment.ProductId;
                model.ProductName = appointment.ProductName;
                model.TeacherPrice = (int)appointment.TeacherPrice;
                model.LessonPrice = (int)appointment.LessonPrice;
                model.CustomerList = registrationCustomerList;
                model.RegisteredCustomer = registratedCustomers;
                model.Rate = ((decimal)registratedCustomers.Count / (decimal)model.Capacity) *100;
                model.RateProgressBar = model.Rate > 100 ? 100 : (int)model.Rate;
                model.DefinedExpense = model.TeacherPrice;
                model.DefinedIncome = _definedIncome;
                model.SummaryIncome = model.DefinedIncome - model.DefinedExpense;
                model.Id = appointment.Id;
                model.PersonalId = appointment.PersonalId;
                model.PersonalName = appointment.PersonalName;
            }
            catch (Exception ex)
            {
                ShowToastr(ex.Message, ToastrType.Warning);
                return RedirectToAction("Appointment");
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult SaveCustomerForAppointment(SaveCustomerForAppointmentVM model)
        {
            if (!ModelState.IsValid)
            {
                return Json("Randevu bilgileri hatalı");
            }

            return SaveCustomer(model.AppointmentId, model.CustomerIdList);
        }

        private JsonResult SaveCustomer(string modelAppointmentId, List<string> modelCustomerIdList)
        {
            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

                if (user == null)
                {
                    return Json("Kullanıcı bulunamadı");
                }
                var _appointmentGuid = Guid.Parse(modelAppointmentId);
                var _appointment = _context.Appointments.SingleOrDefault(x => x.Id == _appointmentGuid);
                CustomerAppointment customerAppointment;

                foreach (var customerId in modelCustomerIdList)
                {
                    var _customerGuid = Guid.Parse(customerId);


                    var _packetOwerflow = false;


                    var customerActivePacket = _context.CustomerPackets.Where(x => x.CustomerId == _customerGuid && x.ProductId == _appointment.ProductId && x.Status).
                        OrderBy(t => t.CreatedDate).FirstOrDefault();

                    if (customerActivePacket != null)
                    {
                        var activePacketUsedCount =
                            _context.CustomerAppointments.Count(x => x.PacketId == customerActivePacket.Id && x.Status);

                        if (activePacketUsedCount == customerActivePacket.PacketSize)
                        {
                            _packetOwerflow = true;

                        }

                        if (activePacketUsedCount + 1 >= customerActivePacket.PacketSize)
                        {
                            customerActivePacket.Status = false;

                            _context.CustomerPackets.Update(customerActivePacket);
                        }
                    }

                    var _paymentType = customerActivePacket != null && _packetOwerflow == false
                        ? PaymentType.Packet
                        : PaymentType.Cash;



                    customerAppointment = new CustomerAppointment()
                    {
                        AppointmentId = _appointmentGuid,
                        CustomerId = _customerGuid,
                        CreatedDate = DateTime.Now,
                        CreatedName = user.NormalizedUserName,
                        Status = true,
                        PaymentType = (int)_paymentType,
                        PacketId = _paymentType == PaymentType.Packet ? customerActivePacket.Id : null,
                        CompanyId = user.CompanyId,
                        AppointmentDate = _appointment.StartDate
                    };
                    _context.CustomerAppointments.Add(customerAppointment);
                    _context.SaveChanges();

                    CreateCustomerAppointmentInfoSms(user, _customerGuid, _appointment.StartDate);

                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return Json("200");
        }


        [HttpPost]
        public JsonResult CancelCustomerAppointment(CancelCustomerAppointmentVM model)
        {
            if (!ModelState.IsValid)
            {
                return Json("Randevu bilgileri hatalı");
            }

            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

                if (user == null)
                {
                    return Json("Kullanıcı bulunamadı");
                }

                var customerAppointment =
                    _context.CustomerAppointments.SingleOrDefault(x => x.Id == Guid.Parse(model.CustomerAppointmentId));
                if (customerAppointment != null)
                {
                    customerAppointment.Status = false;
                    customerAppointment.CancelReason = model.CancelReason;
                    customerAppointment.UpdatedName = user.NormalizedUserName;
                    customerAppointment.UpdatedDate = DateTime.Now;

                    _context.CustomerAppointments.Update(customerAppointment);
                    _context.SaveChanges();

                    var customerPacket =
                        _context.CustomerPackets.SingleOrDefault(x => x.Id == customerAppointment.PacketId);

                    if (customerPacket is { Status: false })
                    {
                        customerPacket.Status = true;
                        _context.CustomerPackets.Update(customerPacket);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return Json("200");
        }

        [HttpPost]
        public JsonResult DeleteAppointment(string appointmentId)
        {
            if (!ModelState.IsValid)
            {
                return Json("Randevu bilgileri hatalı");
            }

            try
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == HttpContext.User.Identity.Name);

                if (user == null)
                {
                    return Json("Kullanıcı bulunamadı");
                }

                var appointment =
                    _context.Appointments.SingleOrDefault(x => x.Id == Guid.Parse(appointmentId));
                if (appointment != null)
                {
                    var customerAppointments = _context.CustomerAppointments.Where(x => x.AppointmentId == appointment.Id).ToList();

                    foreach (var customerAppointment in customerAppointments)
                    {
                        _context.CustomerAppointments.Remove(customerAppointment);
                    }

                    _context.Appointments.Remove(appointment);

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return Json("200");
        }

        [HttpPost]
        public JsonResult DropAndResizeUpdate(DropAndResizeUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                return Json("Parametreler hatalı");
            }

            try
            {
                var _appointmentGuid = Guid.Parse(model.AppointmentId);
                var _startDate = DateTime.Parse(model.StartDate);
                var _endDate = DateTime.Parse(model.EndDate);
                var appointment = _context.Appointments.SingleOrDefault(x=> x.Id == _appointmentGuid && x.Status);
                if (appointment == null)
                {
                    return Json("Güncellenecek randevu bulunamadı");
                }

                appointment.StartDate = _startDate;
                appointment.EndDate = _endDate;

                _context.Update(appointment);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return Json("200");
        }
        private void CreateCustomerAppointmentInfoSms(AppUser user, Guid customerGuid, DateTime appointmentStartDate)
        {
            if (user.IsProPacket)
            {
                var customer = _context.Customers.SingleOrDefault(x => x.Id == customerGuid);
                var msgBody =
                    $"Sevgili {user.CompanyName} ziyaretçisi, {appointmentStartDate.ToString("f")}'da gerçekleşecek olan atölyeye kaydınız yapılmıştır.";

                var formattedPhoneNumber = SmsHelper.ConvertPhoneNumberToSmsType(customer.PhoneNumber);

                var msg = new Message()
                {
                    CompanyId = user.CompanyId,
                    CustomerId = customer.Id,
                    ReceiverPhoneNumber = formattedPhoneNumber,
                    Status = true,
                    CreatedDate = DateTime.Now,
                    CreatedName = user.NormalizedUserName,
                    MessageBody = msgBody,
                    Code = String.Empty
                };
                _context.Messages.Add(msg);
                _context.SaveChanges();
            }

        }

        [HttpPost]
        public JsonResult CopyAppointment(CopyAppointmentModel model)
        {
            try
            {
                var _appointmentId = Guid.Parse(model.AppointmentId);
                var _appointmentStartDate = DateTime.Parse(model.NewAppointmentStartDate);

                var appointment = _context.Appointments.SingleOrDefault(x => x.Id == _appointmentId);
                var appointmentTime = appointment.EndDate - appointment.StartDate;

                var _appointmentEndDate = _appointmentStartDate.AddHours(appointmentTime.Hours).AddMinutes(appointmentTime.Minutes);

                appointment.StartDate = _appointmentStartDate;
                appointment.EndDate = _appointmentEndDate;
                appointment.Id = Guid.Empty;

                var newAppointment = _context.Appointments.Add(appointment);
                _context.SaveChanges();

                var appointmentCustomers = _context.CustomerAppointments
                    .Where(x => x.AppointmentId == Guid.Parse(model.AppointmentId)).Select(s => s.CustomerId.ToString()).ToList();

                SaveCustomer(newAppointment.Entity.Id.ToString(), appointmentCustomers);
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
