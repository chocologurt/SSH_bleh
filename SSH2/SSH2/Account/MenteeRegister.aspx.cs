using Microsoft.AspNet.Identity; //have
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin; //have
using Microsoft.Owin.Security;
using SSH2;
using SSH2.Models;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Web; //have
using System.Web.UI; //have

namespace SSH_ASPJ.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            var manager2 = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user2 = manager2.FindByName(Username.Text);
            if (user2 == null)
            {
                string password = "";
                if (imagePassword.Visible == true)
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
                else if (textPassword.Visible == true)
                {
                    password = Password.Text;
                }
                //var userStore = new UserStore<IdentityUser>();
                //var manager = new UserManager<IdentityUser>(userStore);
                //var user = new IdentityUser() { UserName = Username.Text, Email = Email.Text };
                //IdentityResult result = manager.Create(user, base64ImgString);

                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = new ApplicationUser() { UserName = Username.Text, Email = Email.Text, PhoneNumber = userPhoneNumber.Text };

                IdentityResult result = manager.Create(user, password);

                if (result.Succeeded)
                {
                    string cs = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    SqlConnection con = new SqlConnection(cs);
                    SqlCommand cmd =
                        new SqlCommand("INSERT INTO userInfo (Username, FullName, Institution, FieldOfIndustry, Designation, RegistrationMode) VALUES(@username, @fullname, @institution, @FOI, @designation, @registrationMode)", con);
                    cmd.Parameters.AddWithValue("@username", Username.Text);
                    cmd.Parameters.AddWithValue("@fullname", fullName.Text);
                    cmd.Parameters.AddWithValue("@institution", userInstitution.Text);
                    cmd.Parameters.AddWithValue("@FOI", Convert.ToString(userFOI.SelectedValue));
                    cmd.Parameters.AddWithValue("@designation", "Student");
                    cmd.Parameters.AddWithValue("@registrationMode", "Mentee");
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    //    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    string code = manager.GenerateEmailConfirmationToken(user.Id);
                    string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                    manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                    //Configurating the Email Body using Created HTML Template
                    //string body = this.PopulateBody(user.UserName, callbackUrl);

                    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                    var userIdentity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                    Response.Redirect("/Account/EmailBeingSent.aspx");
                    //manager.SendEmail(user.Id, "Confirm your account", body);
                    //Response.Redirect("/Account/NewAccountCheckEmail");
                    //IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
            }
            else
            {
                string username2 = user2.UserName;
                string email2 = user2.Email;
                if (Username.Text == username2 && Email.Text == email2)
                {
                    ErrorMessage.Text = "UserName is already taken, please choose a different user name. <br /> This email that you have provided is already associated with another account, please give another email.";
                }
                else if (Email.Text == email2)
                {
                    ErrorMessage.Text = "This email that you have provided is already associated with another account, please give another email.";
                }
                else if (Username.Text == username2 )
                {
                    ErrorMessage.Text = "UserName is already taken, please choose a different user name. " ;

                }
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