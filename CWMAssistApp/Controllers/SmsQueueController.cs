using System.Drawing.Text;
using CWMAssistApp.Data.Entity;
using CWMAssistApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using Microsoft.Identity.Client;

namespace CWMAssistApp.Controllers
{
    public class SmsQueueController : Controller
    {
        private UserManager<AppUser> _userManager;
        private ApplicationDbContext _context;
        private const string postAddress = "https://api.netgsm.com.tr/sms/send/xml";

        public SmsQueueController(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public JsonResult SendWaitingSms()
        {
            var messageQueue = _context.Messages.Where(x => x.Status);

            var messageOwners = messageQueue.GroupBy(x => x.CompanyId).Select(s => s.Key);
            foreach (var owner in messageOwners)
            {
                var userSmsPacket = _context.UserSmsPackets.SingleOrDefault(x => x.CompanyId == owner && x.Status);
                if (userSmsPacket != null && userSmsPacket.Status)
                {
                    var msgQueue = messageQueue.Where(x => x.CompanyId == owner);

                    string cdata = "";

                    foreach (var data in msgQueue)
                    {
                        cdata+= "<mp><msg><![CDATA[" + data.MessageBody +"]]></msg><no>"+ data.ReceiverPhoneNumber +"</no></mp>";
                    }



                    string ss = "";
                    ss += "<?xml version='1.0' encoding='UTF-8'?>";
                    ss += "<mainbody>";
                    ss += "<header>";
                    ss += "<company dil='TR'>Netgsm</company>";
                    ss += "<usercode>"+userSmsPacket.ServiceUserName+"</usercode>";
                    ss += "<password>"+userSmsPacket.ServicePassword+"</password>";
                    ss += "<type>n:n</type>";
                    ss += "<msgheader>"+ userSmsPacket.ServiceUserName +"</msgheader>";
                    ss += "</header>";
                    ss += "<body>";
                    ss += cdata;
                    ss += "</body>";
                    ss += "</mainbody>";
                    var response = XMLPOST(ss);
                }
            }
            return Json("200");
        }

        private string XMLPOST(string xmlData)
        {
            try
            {
                var wUpload = new WebClient();
                var request = WebRequest.Create(postAddress) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                var bPostArray = Encoding.UTF8.GetBytes(xmlData);
                var bResponse = wUpload.UploadData(postAddress, "POST", bPostArray);
                var sReturnChars = Encoding.UTF8.GetChars(bResponse);
                var sWebPage = new string(sReturnChars);
                return sWebPage;
            }
            catch
            {
                return "-1";
            }
        }
    }
}
