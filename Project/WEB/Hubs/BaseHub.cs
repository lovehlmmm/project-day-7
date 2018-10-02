using Constants;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WEB.Models;

namespace WEB.Hubs
{
    public class BaseHub :Hub
    {
        public static readonly ConcurrentDictionary<string, UserConnection> Users =
        new ConcurrentDictionary<string, UserConnection>(StringComparer.InvariantCultureIgnoreCase);
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
    }
}