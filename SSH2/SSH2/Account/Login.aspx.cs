using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.IO;
using System.Web;
using System.Web.UI;

namespace SSH2.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            // Enable this once you have account confirmation enabled for password reset functionality
            ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                if (ctrlGoogleReCaptcha.Validate())
                {
                    string password = "";
                    if (textPassword.Visible == true)
                    {
                        password = Password.Text;
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
                            FailureText.Text = "Upload Status: Only JPEG files are available for upload";
                        }
                    }

                    // Validate the user password
                    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                    // Require the user to have a confirmed email before they can log on.
                    var user = manager.FindByName(userName.Text);
                    if (user != null)
                    {
                        if (!user.EmailConfirmed)
                        {
                            FailureText.Text = "Invalid login attempt. You must have a confirmed email address.";
                            ErrorMessage.Visible = true;
                            ResendConfirm.Visible = true;
                        }
                        else
                        {
                            // This doen't count login failures towards account lockout
                            // To enable password failures to trigger lockout, change to shouldLockout: true
                            var result = signinManager.PasswordSignIn(userName.Text, password, RememberMe.Checked, shouldLockout: true);

                            switch (result)
                            {
                                case SignInStatus.Success:
                                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                                    break;

                                case SignInStatus.LockedOut:
                                    Response.Redirect("/Account/Lockout");
                                    break;

                                case SignInStatus.RequiresVerification:
                                    Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
                                                                    Request.QueryString["ReturnUrl"],
                                                                    RememberMe.Checked),
                                                      true);
                                    break;

                                case SignInStatus.Failure:
                                default:
                                    FailureText.Text = "Invalid login attempt";
                                    ErrorMessage.Visible = true;
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    FailureText.Text = "Captcha Failed. Please try again";
                    ErrorMessage.Visible = true;
                }
            }
        }

        protected void SendEmailConfirmationToken(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindByName(userName.Text);
            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    string code = manager.GenerateEmailConfirmationToken(user.Id);
                    string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                    manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                    FailureText.Text = "Confirmation email sent. Please view the email and confirm your account.";
                    ErrorMessage.Visible = true;
                    ResendConfirm.Visible = false;
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