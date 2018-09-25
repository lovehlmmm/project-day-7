using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WEB.Models;

namespace WEB.Helpers
{
    public class SendEmail
    {
        public static void Send(string to, string mailbody, string subject, List<CartItem> cartItems = null,string folder=null)
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
            if (cartItems != null &folder!=null)
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                                               mailbody,
                                               Encoding.UTF8,
                                               MediaTypeNames.Text.Html);
                foreach (var item in cartItems)
                {
                    string mediaType =item.ImageType;
                    string url = string.Format("{0}/{1}{2}", ConfigurationManager.AppSettings["folderImage"], folder, item.ImageTitle);
                    LinkedResource img = new LinkedResource(url, mediaType);
                    // Make sure you set all these values!!!
                    img.ContentId = item.ImageTitle;
                    img.ContentType.MediaType = mediaType;
                    img.TransferEncoding = TransferEncoding.Base64;
                    img.ContentType.Name = img.ContentId;
                    img.ContentLink = new Uri("cid:" + img.ContentId);
                    htmlView.LinkedResources.Add(img);
                    message.AlternateViews.Add(htmlView);
                }
            }
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