using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SSH_ASPJ.Account
{
    public partial class RegistrationSelection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void menteeButton_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("MenteeRegister.aspx");
        }

        protected void mentorButton_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("MentorRegistration.aspx");
        }
    }
}