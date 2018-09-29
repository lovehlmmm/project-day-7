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

namespace WEB.Hubs
{
    public class NotificationHub: Hub
    {
        private static readonly ConcurrentDictionary<string, UserConnection> Users =
        new ConcurrentDictionary<string, UserConnection>(StringComparer.InvariantCultureIgnoreCase);
        //public override Task OnReconnected()
        //{
        //    string username = "";
        //    var session = HttpContext.Current.Session[AppSettingConstant.NotifiSession];
        //    if (session != null)
        //    {
        //        username = session as string;
        //    }
        //    string connectionId = Context.ConnectionId;
        //    if (!Users.ContainsKey(username))
        //    {
        //        var user = Users.GetOrAdd(username, _ => new UserConnection
        //        {
        //            UserName = username,
        //            ConnectionIds = new HashSet<string>()
        //        });

        //        lock (user.ConnectionIds)
        //        {
        //            user.ConnectionIds.Add(connectionId);
        //            if (user.ConnectionIds.Count == 1)
        //            {
        //                Clients.Others.userConnected(username);
        //            }
        //        }
        //    }
        //    return base.OnReconnected();
        //}
        public override Task OnConnected()
        {
            try
            {
                var username = "";
                var cookie = Context.Request.Cookies[AppSettingConstant.LoginCookieCustomer];
                if (cookie != null)
                {
                    var cook = JsonConvert.DeserializeObject<UserLoginCookie>(Uri.UnescapeDataString(cookie.Value));
                    username = cook.Username;
                }
                string connectionId = Context.ConnectionId;
                var user = Users.GetOrAdd(username, _ => new UserConnection
                {
                    UserName = username,
                    ConnectionIds = new HashSet<string>()
                });

                lock (user.ConnectionIds)
                {
                    user.ConnectionIds.Add(connectionId);
                    if (user.ConnectionIds.Count == 1)
                    {
                        Clients.Others.userConnected(username);
                    }
                }
            }
            catch (Exception e)
            {

            } 
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            try
            {
                var username = "";
                var cookie = Context.Request.Cookies[AppSettingConstant.LoginCookieCustomer];
                if (cookie != null)
                {
                    var cook = JsonConvert.DeserializeObject<UserLoginCookie>(Uri.UnescapeDataString(cookie.Value));
                    username = cook.Username;
                }
                string connectionId = Context.ConnectionId;

                UserConnection user;
                Users.TryGetValue(username, out user);

                if (user != null)
                {
                    lock (user.ConnectionIds)
                    {
                        user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));
                        if (!user.ConnectionIds.Any())
                        {
                            UserConnection removedUser;
                            Users.TryRemove(username, out removedUser);
                            Clients.Others.userDisconnected(username);
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return base.OnDisconnected(stopCalled);
        }
        public void SendNotification(string SentTo)
        {
            try
            {
                //Get TotalNotification

                //Send To
                UserConnection receiver;
                if (Users.TryGetValue(SentTo, out receiver))
                {
                    var cid = receiver.ConnectionIds.Last();
                    var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                    context.Clients.Client(cid).broadcaastNotif(12345);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}