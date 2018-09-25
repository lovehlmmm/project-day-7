using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WEB.Helpers
{
    public class SendEmail
    {
        public static void Send(string to,string mailbody,string subject)
        {
            string smtp = ConfigurationManager.AppSettings["smtp"];
            string mail = ConfigurationManager.AppSettings["mail"];
            string password = ConfigurationManager.AppSettings["password"];
            int port = Int32.Parse(ConfigurationManager.AppSettings["port"]);
            MailMessage message = new MailMessage(mail, to)
            {
                Subject = subject,
                Body = mailbody,
                IsBodyHtml = true
            };
            SmtpClient client = new SmtpClient(smtp, port); 
            System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential(mail, password);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                client.Send(message);
                client.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
    }
}