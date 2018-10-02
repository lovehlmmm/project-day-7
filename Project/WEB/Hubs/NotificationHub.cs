using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WEB.Models;
using Constants;
using Helpers;
using Newtonsoft.Json;
using Entities;
using System.Data.SqlClient;
using Repositories;
using Services;
namespace WEB.Hubs
{
    public class NotificationHub : BaseHub,INotificationHub
    {
        IBaseRepository<Notification> _notificationService;
        public void SendNotification(string SentTo, Notification notification)
        {
            try
            {
                //Get TotalNotification

                //Send To
                UserConnection receiver;
                if (Users.TryGetValue(SentTo, out receiver))
                {
                    var count = GetNotification(SentTo);
                    var cid = receiver.ConnectionIds.Last();
                    var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                    context.Clients.Client(cid).broadcaastNotif(notification,count);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        public void GetCountNotifi()
        {
            try
            {
                //Get TotalNotification

                //Send To
                var username = "";
                var cookie = Context.Request.Cookies[AppSettingConstant.LoginCookieCustomer];
                if (cookie != null)
                {
                    var cook = JsonConvert.DeserializeObject<UserLoginCookie>(Uri.UnescapeDataString(cookie.Value));
                    username = cook.Username;
                }
                UserConnection receiver;
                if (Users.TryGetValue(username, out receiver))
                {
                    var count = GetNotification(username);
                    var cid = receiver.ConnectionIds.Last();
                    var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                    context.Clients.Client(cid).broadcaastNotifCount(count);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private int GetNotification(string username)
        {
            _notificationService = new BaseRepositoryEF<Notification>();
            return _notificationService.FindAll(n => n.SendTo == username & n.IsRead == false).ToList().Count;
        }
    }
}