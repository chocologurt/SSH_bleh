using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using SSH2;
using SSH2.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.IO;
using System.Data.SqlClient;

namespace SSH_ASPJ.Account
{
    public partial class MentorRegistration : Page
    {

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            string password = "";

            if (textPassword.Visible == true)
            {
                password = MentorPassword.Text;
            }
            else if (imagePassword.Visible == true)
            {
                string fileExt = Path.GetExtension(imagePasswordControl.PostedFile.FileName);
                if (fileExt == ".jpg")
                {
                    // string filename = Path.GetFileName(imagePasswordControl.FileName);
                    byte[] imgbyte = imagePasswordControl.FileBytes;
                    //convert byte[] to Base64 string
                    string base64ImgString = Convert.ToBase64String(imgbyte);
                    password = base64ImgString;
                }
                else
                {

                    ErrorMessage.Text = "Upload Status: Only JPEG files are available for upload";
                }
            }
            //var userStore = new UserStore<IdentityUser>();
            //var manager = new UserManager<IdentityUser>(userStore);
            //// Debug.WriteLine(manager);
            ////var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            //var user = new IdentityUser() { UserName = mentorUsername.Text, Email = MentorEmail.Text };
            //IdentityResult result = manager.Create(user, MentorPassword.Text);

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = new ApplicationUser() { UserName = mentorUsername.Text, Email = MentorEmail.Text, PhoneNumber = MentorPhoneNumber.Text };
            IdentityResult result = manager.Create(user, password);


            if (result.Succeeded)
            {

                string cs = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(cs);
                SqlCommand cmd =
                    new SqlCommand("INSERT INTO users (userId, userInstitution, userMode, userDesignation, userFieldOfIndustry, FullName) VALUES(@userId, @institution,@registrationMode, @designation, @userFOI, @fullname )", con);
                cmd.Parameters.AddWithValue("@userId", mentorUsername.Text);
                cmd.Parameters.AddWithValue("@institution", MentorInstitution.Text);
                cmd.Parameters.AddWithValue("@registrationMode", 2);
                cmd.Parameters.AddWithValue("@designation", "Student");
                cmd.Parameters.AddWithValue("@userFOI", Convert.ToString(MentorFOI.SelectedValue));
                cmd.Parameters.AddWithValue("@fullname", mentorFullName.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();




                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                string code = manager.GenerateEmailConfirmationToken(user.Id);
                string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                // manager.SignIn(user, isPersistent: false, rememberBrowser: false);
                //IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);

                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                var userIdentity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                Response.Redirect("/Account/EmailBeingSent.aspx");
            }
            else
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }

   

        protected void PasswordSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.PasswordSelection.SelectedValue == "1")
            {
                this.textPassword.Visible = true;
                this.imagePassword.Visible = false;
            }

            else if (this.PasswordSelection.SelectedValue == "2")
            {
                this.textPassword.Visible = false;
                this.imagePassword.Visible = true;
            }
        }
    }
}