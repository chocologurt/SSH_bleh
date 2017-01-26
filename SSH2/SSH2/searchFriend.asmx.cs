using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace ASPJ
{
    /// <summary>
    /// Summary description for searchFriend
    /// </summary>
//    [WebService(Namespace = "http://localhost:13628/WebForm2.aspx")]
//    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class searchFriend : System.Web.Services.WebService
    {
        string userID = "ITmentor1";
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetLists(string friend)
        {
            // Create array of movies  
            Debug.WriteLine("this is running");
            List<string> list = new List<string>();

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            try
            {
                myConnection.Open();

                SqlDataReader myReader = null;
                SqlCommand command = new SqlCommand("SELECT * FROM users WHERE userID LIKE @userID+'%'", myConnection);
                command.Parameters.Add(new SqlParameter("userID",friend));
                myReader = command.ExecuteReader();
                while (myReader.Read())
                {
                    list.Add(myReader["userID"].ToString());
                }

                myConnection.Close();
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            } 
            // Return matching movies  
            return list.ToArray();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void deleteNotification(string time)
        {
            Debug.WriteLine("calling server method");
            Notification.deleteNotification(userID, time);
        }
    }

}
