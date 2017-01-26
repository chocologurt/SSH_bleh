using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ASPJ
{
    public class NotificationHub : Hub
    {
        public void RetrieveNotification(string user)
        {
            List<Notification> nlist = Notification.retrieveOldNotification(user);
            foreach(Notification n in nlist)
            {
                n.notifyUser();
            }
            
        }
    }
}