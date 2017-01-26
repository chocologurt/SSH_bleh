using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ASPJ
{
    public class RelationPostContent
    {
        public int postID { get; set; }
        public string postBody { get; set; }
        public DateTime threadTime { get; set; }
        public string postedBy { get; set; }
        
        public RelationPostContent(int postID,string postBody,DateTime threadTime,string postedBy)
        {
            this.postID = postID;
            this.postBody = postBody;
            this.threadTime = threadTime;
            this.postedBy = postedBy;
        }
       
        public RelationPostContent(int postID, string postBody, string postedBy)
        {
            this.postID = postID;
            this.postBody = postBody;
            this.postedBy = postedBy;
        }
        public static int retrieveNumOfPost(int postID)
        {
            int count = 0;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            try
            {
                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand command = new SqlCommand("getRelationContent ", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("postID", postID));
                myReader = command.ExecuteReader();

                while (myReader.Read())
                {
                    count++;
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
            return count;
        }
        public static List<RelationPostContent> getRelationContent(int postID)
        {
            List<RelationPostContent> pList = new List<RelationPostContent>();
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            try
            {
                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand command = new SqlCommand("getRelationContent ", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("postID", postID));
                myReader = command.ExecuteReader();

                while (myReader.Read())
                {
                    string postBody = myReader["relationPostBody"].ToString();
                    string postedBy = myReader["postedBy"].ToString();
                    int postID2 = Convert.ToInt32(myReader["postID"]);
                    DateTime threadTime = Convert.ToDateTime(myReader["threadTime"]);
                    RelationPostContent r = new RelationPostContent(postID2, postBody, threadTime,postedBy);

                    pList.Add(r);

                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }

            return pList;
        }
        public int replyContent()
        {
            int success = 0;
            List<RelationPostContent> pList = new List<RelationPostContent>();
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            try
            {
                myConnection.Open();
                SqlCommand command = new SqlCommand("replyContent ", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("postID", postID));
                command.Parameters.Add(new SqlParameter("relationPostBody", postBody));
                command.Parameters.Add(new SqlParameter("postedBy", postedBy));
                success = command.ExecuteNonQuery();
                
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
            return success;
        }
   
    }
}