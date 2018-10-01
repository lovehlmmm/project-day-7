using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB.Hubs
{
    public interface INotificationHub
    {
        void SendNotification(string SentTo, Notification notification);
    }
}
