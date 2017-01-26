using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ASPJ
{
    public class Relation
    {
        public string menteeID{ get; set; }
        public string mentorID{ get; set; }
        public int relationID{ get; set; }
        public string invitationText{ get; set; }
        public Relation(string menteeID,string mentorID,string invitationText)
        {
            this.menteeID = menteeID;
            this.mentorID = mentorID;
            this.invitationText = invitationText;
        }
        public Relation(string menteeID, string mentorID,int relationID,string invitationText)
        {
            this.menteeID = menteeID;
            this.mentorID = mentorID;
            this.relationID = relationID;
            this.invitationText = invitationText;
        }
        public int sendRequest()
        {
            int result=99;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            try
            {
                myConnection.Open();
                SqlCommand command = new SqlCommand("add_mentor", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@userID", menteeID));
                command.Parameters.Add(new SqlParameter("@mentorID", mentorID));
                command.Parameters.Add(new SqlParameter("@invitationText", invitationText));


                SqlParameter parmOut = new SqlParameter("@result", SqlDbType.Int);
                parmOut.Direction = System.Data.ParameterDirection.ReturnValue;

                command.Parameters.Add(parmOut);
                command.ExecuteNonQuery();
                result = (int)parmOut.Value;

                myConnection.Close();
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
            return result;
        }
        public int acceptRequest()
        {
            int result=99;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            try
            {
                myConnection.Open();

                SqlCommand command = new SqlCommand("accept_mentor", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("userID", menteeID));
                command.Parameters.Add(new SqlParameter("mentorID", mentorID));

                SqlParameter parmOut = new SqlParameter("@result", SqlDbType.Int);
                parmOut.Direction = System.Data.ParameterDirection.ReturnValue;
                command.Parameters.Add(parmOut);
                command.ExecuteNonQuery();
                result = (int)parmOut.Value;
                
                myConnection.Close();
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
            return result;
        }
        public static Relation getRelationByRelationID(int relationID)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            Relation r = null;
            try
            {
                //              string cmd = "SELECT * FROM notification WHERE insertedTime > @lastRun";
                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand command = new SqlCommand("getRelationByRelationID", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("relationID", relationID));
                myReader = command.ExecuteReader();

                while (myReader.Read())
                {
                    string mentee = myReader["menteeID"].ToString();
                    string mentor = myReader["mentorID"].ToString();
                    string invitation = myReader["invitationText"].ToString();
                    r = new Relation(mentee, mentor, relationID, invitation);

                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
            return r;
        }
        public static Relation getRelationByID(string mentorID,string menteeID)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            Relation r=null;
            try
            {
                //              string cmd = "SELECT * FROM notification WHERE insertedTime > @lastRun";
                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand command = new SqlCommand("getRelationByID", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("userID", menteeID));
                command.Parameters.Add(new SqlParameter("mentorID", mentorID));
                myReader = command.ExecuteReader();

                while (myReader.Read())
                {
                    string mentee = myReader["menteeID"].ToString();
                    string mentor = myReader["mentorID"].ToString();
                    string invitation = myReader["invitationText"].ToString();
                    int relationID = Convert.ToInt32(myReader["relationID"]);
                    r = new Relation(mentee, mentor, relationID, invitation);

                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
            return r;
        }
        public static List<Relation> getMenteeRequest(string mentorID)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            List<Relation> rList = new List<Relation>();
            try
            {
                //              string cmd = "SELECT * FROM notification WHERE insertedTime > @lastRun";
                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand command = new SqlCommand("getMenteeRequest", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("userID", mentorID));
                myReader = command.ExecuteReader();

                while (myReader.Read())
                {
                    string mentee = myReader["menteeID"].ToString();
                    string mentor = myReader["mentorID"].ToString();
                    string invitation = myReader["invitationText"].ToString();
                    int relationID = Convert.ToInt32(myReader["relationID"]);
                    Relation r = new Relation(mentee,mentor, relationID,invitation);
                    rList.Add(r);

                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }

            return rList;
        }

        public static List<Relation> getMentor(string menteeID)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            List<Relation> rList = new List<Relation>();
            try
            {
                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand command = new SqlCommand("getMentor", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("userID", menteeID));
                myReader = command.ExecuteReader();

                while (myReader.Read())
                {
                    string mentee = myReader["menteeID"].ToString();
                    string mentor = myReader["mentorID"].ToString();
                    string invitation = myReader["invitationText"].ToString();
                    int relationID = Convert.ToInt32(myReader["relationID"]);
                    Relation r = new Relation(mentee, mentor, relationID, invitation);
                    rList.Add(r);
                    
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
            return rList;
        }

        public static List<Relation> getMentee(string mentorID)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["myConnection"].ConnectionString;
            List<Relation> rList = new List<Relation>();
            try
            {
                myConnection.Open();
                SqlDataReader myReader = null;
                SqlCommand command = new SqlCommand("getMentee", myConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("userID", mentorID));
                myReader = command.ExecuteReader();

                while (myReader.Read())
                {
                    string mentee = myReader["menteeID"].ToString();
                    string mentor = myReader["mentorID"].ToString();
                    string invitation = myReader["invitationText"].ToString();
                    int relationID = Convert.ToInt32(myReader["relationID"]);
                    Relation r = new Relation(mentee, mentor, relationID, invitation);
                    rList.Add(r);

                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.ToString());
            }
            return rList;
        }
    }
}