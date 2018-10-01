using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Constants;
using Helpers;
using System.Web.Mvc;
using WEB.Areas.Admin.Models;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;

namespace WEB.Areas.Admin.Controllers
{
    public class SettingController : Controller
    {
        // GET: Admin/Setting
        public ActionResult Index()
        {
            string smtp = ConfigurationManager.AppSettings["smtp"];
            string mail = ConfigurationManager.AppSettings["mail"];
            string password = ConfigurationManager.AppSettings["password"];
            int port = Int32.Parse(ConfigurationManager.AppSettings["port"]);
            EmailAppModel emailAppModel = new EmailAppModel()
            {
                Email = mail,
                Password = password,
                Port = port.ToString(),
                Stmp = smtp
            };
            ViewBag.AppMail = emailAppModel;
            return View();
        }
        public JsonResult UpdateEmailSetting(string emailApp,string email, string key = null)
        {
            string message = "";
            var appModel = JsonConvert.DeserializeObject<EmailAppModel>(emailApp);
            try
            {
                if (key == null |key.Trim()=="")
                {
                    try
                    {
                        var body = RandomString(6);
                        SessionHelper.SetSession(body, AppSettingConstant.KeyEmailSettingSession);
                        var messageMail = new MailMessage();
                        messageMail.To.Add(new MailAddress(email));  // replace with valid value 
                        messageMail.From = new MailAddress(appModel.Email);  // replace with valid value
                        messageMail.Subject = "Key";
                        messageMail.Body = string.Format(body);
                        messageMail.IsBodyHtml = true;

                        using (var smtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = appModel.Email,  // replace with valid value
                                Password = appModel.Password  // replace with valid value
                            };
                            smtp.EnableSsl = true;
                            smtp.UseDefaultCredentials = false;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.Credentials = credential;
                            smtp.Host = appModel.Stmp;
                            smtp.Port = int.Parse(appModel.Port);
                             smtp.Send(messageMail);
                            return Json(new { status = true, message }, JsonRequestBehavior.AllowGet);
                        }
                        
                    }
                    catch (Exception e)
                    {

                        
                    }
                }
                   var session = SessionHelper.GetSession(AppSettingConstant.KeyEmailSettingSession) as string;
                    if (session != null)
                    {
                        if (session.Equals(key))
                        {
                            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                            config.AppSettings.Settings["smtp"].Value = appModel.Stmp;
                            config.AppSettings.Settings["mail"].Value = appModel.Email;
                            config.AppSettings.Settings["password"].Value = appModel.Password;
                            config.AppSettings.Settings["port"].Value = appModel.Port;
                            config.Save(ConfigurationSaveMode.Modified, true);
                            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
                            Session.Remove(AppSettingConstant.KeyEmailSettingSession);
                            return Json(new { status = false, message }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            catch (Exception)
            {


            }
            return Json(new { status = false, message }, JsonRequestBehavior.AllowGet);
        }

        private static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}