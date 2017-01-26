using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ASPJ
{
    public class RelationPost
    {
        public string postedBy { get; set; }
        public Relation relation { get; set; }
        public string title { get; set; }
        public int postID { get; set; }

        public RelationPost(string postedBy,Relation relation,string title) {
            this.postedBy = postedBy;
            this.relation = relation;
            this.title = title;
        }
        public RelationPost(string postedBy, Relation relation, string title,int postID)
        {
            this.postedBy = postedBy;
            this.relation = relation;
            this.title = title;
            this.postID = postID;
        }
        public int post(string content)
        {
            int result = 99;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            try
            {
                myConnection.Open();
                SqlCommand command = new SqlCommand("postRelation", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@relationID", relation.relationID));
                command.Parameters.Add(new SqlParameter("@postedBy", postedBy));
                command.Parameters.Add(new SqlParameter("@postTitle", title));
                command.Parameters.Add(new SqlParameter("@postContent", content));


                SqlParameter parmOut = new SqlParameter("@result", SqlDbType.Int);
                parmOut.Direction = System.Data.ParameterDirection.ReturnValue;

                command.Parameters.Add(parmOut);
                command.ExecuteNonQuery();
                result = (int)parmOut.Value;
                Debug.WriteLine(result);
                myConnection.Close();
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
            return result;
        }

        public static List<RelationPost> getRelationPost(string userID)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            List<RelationPost> rList = new List<RelationPost>();
            try
            {
                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand command = new SqlCommand("getRelationPost", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("userID", userID));
                myReader = command.ExecuteReader();

                while (myReader.Read())
                {
                    
                    int relationID = Convert.ToInt32(myReader["relationID"]);
                    string postedBy = myReader["postedBy"].ToString();
                    int postID = Convert.ToInt32(myReader["postID"]);
                    string postTitle = myReader["postTitle"].ToString();
                    RelationPost r = new RelationPost(postedBy,Relation.getRelationByRelationID(relationID),postTitle,postID);

                    rList.Add(r);

                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
            
            return rList;
        }
        public static RelationPost getRelationPostByID(int postID)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            RelationPost r = null;
            try
            {
                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand command = new SqlCommand("getRelationPostByID", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("postID", postID));
                myReader = command.ExecuteReader();

                while (myReader.Read())
                {

                    int relationID = Convert.ToInt32(myReader["relationID"]);
                    string postedBy = myReader["postedBy"].ToString();
                    int postID2 = Convert.ToInt32(myReader["postID"]);
                    string postTitle = myReader["postTitle"].ToString();
                    r = new RelationPost(postedBy, Relation.getRelationByRelationID(relationID), postTitle,postID2);


                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
            return r;
        }
    }
}