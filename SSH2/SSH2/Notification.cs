using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ASPJ
{
    public class Notification
    {
        public string user{ get; set; }
        public int notificationType { get; set; }
        public string content { get; set; }
        public string time { get; set; }
        public int notificationStatus { get; set; }
        public string exactTime { get; set; }
        public Notification(string user,int notificationType,string content,string time,int notificationStatus,string exactTime)
        {
            this.user = user;
            this.notificationType = notificationType;
            this.content = content;
            this.time = time;
            this.notificationStatus = notificationStatus;
            this.exactTime = exactTime;
        }
        public void notifyUser()
        {
            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            if (notificationType == 5)
            {
                content = "You have a friend request from " + content;
            }
            else if(notificationType == 6)
            {
                content = content + " has accepted your friend request";
            }
            else if(notificationType == 7)
            {
                content = "You have a mentor request from " + content;
            }

            notificationHub.Clients.User(user).broadcastMessage(content,exactTime);
        }
        public void addNotification(Literal list)
        {
            if (notificationType == 5)
            {
                content = "You have a friend request from " + content;
            }
            else if (notificationType == 6)
            {
                content = content + " has accepted your friend request";
            }
            else if (notificationType == 7)
            {
                content = "You have a mentor request from " + content;
            }
            list.Text += "<li id=\""+exactTime+"\">"+content+ "<input type=\"button\" value=\"X\"  onclick=\"deleteNotification('"+exactTime+"')\"></li>";

        }
        public static void deleteNotification(string userID,string time)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["AdminConnection"].ConnectionString;
            List<Notification> nlist = new List<Notification>();
            try
            {
                //              string cmd = "SELECT * FROM notification WHERE insertedTime > @lastRun";
                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand command = new SqlCommand("deleteNotification", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("userID", userID));
                command.Parameters.Add(new SqlParameter("time", time));
                Debug.WriteLine(time);
                myReader = command.ExecuteReader();

            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
        }
        public static List<Notification> retrieveOldNotification(string userID)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["AdminConnection"].ConnectionString;
            List<Notification> nlist = new List<Notification>();
            try
            {
                //              string cmd = "SELECT * FROM notification WHERE insertedTime > @lastRun";
                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand command = new SqlCommand("getOldNotification", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("userID", userID));
                myReader = command.ExecuteReader();

                while (myReader.Read())
                {
                    nlist.Add(new Notification(userID, Convert.ToInt32(myReader["notificationType"])
                            , myReader["notificationContent"].ToString()
                            , Convert.ToDateTime(myReader["insertedTime"]).ToString("dd/MM/yyyy HH:mm:ss")
                            , Convert.ToInt32(myReader["notificationStatus"]), Convert.ToDateTime(myReader["insertedTime"]).ToString("yyyy-MM-dd HH:mm:ss.fff")));
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
            return nlist;
        }

    }
}