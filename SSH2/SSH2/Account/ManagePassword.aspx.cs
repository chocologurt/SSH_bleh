using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.SqlClient;
using SSH2.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SSH2.Account
{
    public partial class ManagePassword : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        private bool HasPassword(ApplicationUserManager manager)
        {
            return manager.HasPassword(User.Identity.GetUserId());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (!IsPostBack)
            {
                // Determine the sections to render
                if (HasPassword(manager))
                {
                    changePasswordHolder.Visible = true;
                }
                else
                {
                    setPassword.Visible = true;
                    changePasswordHolder.Visible = false;
                }

                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/Manage");
                }
            }
        }

        protected void ChangePassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                var manager2 = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var user2 = manager2.FindByName(Context.User.Identity.GetUserName());
                string username = user2.UserName;

               

                var myPasswordHasher = new PasswordHasher();
                //string hashedpassword = myPasswordHasher.HashPassword(NewPassword.Text);

                //To prevent users from reusing their 5 most recent passwords. 
                List<String> pwdList = new List<string>(5);
                List<PasswordVerificationResult> resultsList = new List<PasswordVerificationResult>(5);

                string cs = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(cs);
                SqlCommand cmd =
                    new SqlCommand("SELECT password FROM pwList WHERE userName = @userName", con);
                cmd.Parameters.AddWithValue("@userName", username);
                con.Open();
                
           // PasswordVerificationResult results = myPasswordHasher.VerifyHashedPassword(hashedpassword2, NewPassword.Text);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pwdList.Add(Convert.ToString(reader["password"]));
                    }
                }

                con.Close();

                for (int i =0; i< pwdList.Count(); i++)
                {
                    PasswordVerificationResult results = myPasswordHasher.VerifyHashedPassword(pwdList[i], NewPassword.Text);
                    resultsList.Add(results);
                }

                if (resultsList.Contains(PasswordVerificationResult.Success))
                {
                    ErrorMessage.Text = "Please do not use a recent password";

                    
                }
                else
                {
                    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                        var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
                        IdentityResult result = manager.ChangePassword(User.Identity.GetUserId(), CurrentPassword.Text, NewPassword.Text);
                    if (result.Succeeded)
                    {
                       
                        if (pwdList.Count<5)
                        {
                            string hashedpassword = myPasswordHasher.HashPassword(NewPassword.Text);

                            string cs2 = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                            SqlConnection con2 = new SqlConnection(cs2);
                            SqlCommand cmd2 =
                                new SqlCommand("INSERT INTO pwList (userName, password) VALUES(@username, @password)", con2);
                            cmd2.Parameters.AddWithValue("@username", username);
                            cmd2.Parameters.AddWithValue("@password", hashedpassword);
                            con2.Open();
                            cmd2.ExecuteNonQuery();
                            con2.Close();

                            var user = manager.FindById(User.Identity.GetUserId());
                            signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                            Response.Redirect("~/Account/Manage?m=ChangePwdSuccess");
                        }
                        else
                        {
                           
                            string hashedpassword = myPasswordHasher.HashPassword(NewPassword.Text);

                            string cs4 = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                            SqlConnection con4 = new SqlConnection(cs4);
                            SqlCommand cmd4 =
                                new SqlCommand("INSERT INTO pwList (userName, password) VALUES(@username, @password)", con4);
                            cmd4.Parameters.AddWithValue("@username", username);
                            cmd4.Parameters.AddWithValue("@password", hashedpassword);
                            con4.Open();
                            cmd4.ExecuteNonQuery();
                            con4.Close();

                            string cs3 = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                            SqlConnection con3 = new SqlConnection(cs3);
                            SqlCommand cmd3 =
                                new SqlCommand("DELETE TOP(1) FROM pwList WHERE userName = @username ", con3);
                            cmd3.Parameters.AddWithValue("@username", username);
                            con3.Open();
                            cmd3.ExecuteNonQuery();
                            con3.Close();

                            var user = manager.FindById(User.Identity.GetUserId());
                            signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                            Response.Redirect("~/Account/Manage?m=ChangePwdSuccess");
                        }
                    }
                    else
                    {
                       AddErrors(result);

                    }
                }
            }

        }

        protected void SetPassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Create the local login info and link the local account to the user
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                IdentityResult result = manager.AddPassword(User.Identity.GetUserId(), password.Text);
                if (result.Succeeded)
                {
                    Response.Redirect("~/Account/Manage?m=SetPwdSuccess");
                }
                else
                {
                    AddErrors(result);
                }
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}