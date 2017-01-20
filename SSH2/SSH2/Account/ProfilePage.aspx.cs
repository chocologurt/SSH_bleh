using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SSH2.Account
{
    public partial class ProfilePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MultiView1.ActiveViewIndex = 0;
                personalInfoButton.CssClass = "btn btn-info";
                SkillsOwnedButton.CssClass = "btn btn-primary";
                userPostsButton.CssClass = "btn btn-primary";
            }
        }

        protected void personalInfoButton_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
            personalInfoButton.CssClass = "btn btn-info";
            SkillsOwnedButton.CssClass = "btn btn-primary";
            userPostsButton.CssClass = "btn btn-primary";
        }

        protected void SkillsOwnedButton_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;
            personalInfoButton.CssClass = "btn btn-primary";
            SkillsOwnedButton.CssClass = "btn btn-info";
            userPostsButton.CssClass = "btn btn-primary";
        }

        protected void userPostsButton_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 2;
            personalInfoButton.CssClass = "btn btn-primary";
            SkillsOwnedButton.CssClass = "btn btn-primary";
            userPostsButton.CssClass = "btn btn-info";
        }
    }
}