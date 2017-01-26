using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ASPJ.Admin
{
    public class Search
    {
        public string searchName { get; set; }
        public string query { get; set; }

        public Search(string searchName,string query)
        {
            this.searchName = searchName;
            this.query = query;
        }
        public void addSearch()
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            string pubKey = null;
            try
            {
                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand command = new SqlCommand("addSearch", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@searchName", searchName));
                command.Parameters.Add(new SqlParameter("@query", query));
                myReader = command.ExecuteReader();

                myConnection.Close();
            }
            catch (Exception e1)
            {
                
            }
        }
        public static List<Search> retrieveSearch()
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            List<Search> sList = new List<Search>();
            try
            {

                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand command = new SqlCommand("SELECT * FROM savedSearch",myConnection);
                
                myReader = command.ExecuteReader();
                while (myReader.Read())
                {
                    Search s = new Search(myReader["searchName"].ToString(),myReader["query"].ToString());
                    sList.Add(s);
                }
                myConnection.Close();
            }
            catch (Exception e1)
            {
                
            }
            return sList;
        }

    }
}