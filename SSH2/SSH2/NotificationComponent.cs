using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR;
using System.Diagnostics;
using System.Data;

namespace ASPJ
{
    //testing
    public class NotificationComponent
    {
        DateTime lastRun;
        public void RegisterNotification(DateTime currentTime)
        {
            lastRun = currentTime;
            string conStr = System.Configuration.ConfigurationManager.
    ConnectionStrings["AdminConnection"].ConnectionString;
            string sqlCommand = @"SELECT [userID],[notificationType],[notificationContent] from [dbo].[notification] WHERE [insertedTime] > @lastRun";
            //you can notice here I have added table name like this [dbo].[Contacts] with [dbo], its mendatory when you use Sql Dependency

            using (SqlConnection con = new SqlConnection(conStr))
            {
                Debug.WriteLine("I am here");
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.Add(new SqlParameter("lastRun", lastRun));
                //        cmd.Parameters.AddWithValue("@AddedOn", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                SqlDependency.Start(conStr);
                cmd.Notification = null;

                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;
                //we must have to execute the command here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            //or you can also check => if (e.Info == SqlNotificationInfo.Insert) , if you want notification only for inserted record
            Debug.WriteLine("I am here");
            Debug.WriteLine(e.Info);
            Debug.WriteLine(e.Type);
            Debug.WriteLine(e.Source);
            if (e.Info == SqlNotificationInfo.Insert)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;
                SqlConnection myConnection = new SqlConnection();
                myConnection.ConnectionString = System.Configuration.ConfigurationManager.
        ConnectionStrings["AdminConnection"].ConnectionString;
                try
                {
                    //                  string cmd = "SELECT * FROM notification WHERE insertedTime > @lastRun";
                    myConnection.Open();
                    SqlDataReader myReader = null;
                    SqlCommand command = new SqlCommand("getNewNotification", myConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("lastRun", lastRun));
                    myReader = command.ExecuteReader();

                    while (myReader.Read())
                    {
                        //     myReader["userID"].ToString();
                        Debug.WriteLine(lastRun.ToString());

                        string user = myReader["userID"].ToString();
                        Notification n = new Notification(user, Convert.ToInt32(myReader["notificationType"])
                            , myReader["notificationContent"].ToString()
                            , Convert.ToDateTime(myReader["insertedTime"]).ToString("dd/MM/yyyy HH:mm:ss")
                            , Convert.ToInt32(myReader["notificationStatus"]), Convert.ToDateTime(myReader["insertedTime"]).ToString("yyyy-MM-dd HH: mm:ss.fff"));
                        n.notifyUser();
                        //re-register notification

                    }

                    myConnection.Close();
                }
                catch (Exception e1)
                {
                    Console.WriteLine(e1.ToString());
                }

                //from here we will send notification message to client

            }
            RegisterNotification(DateTime.Now);
        }
        
    }
}