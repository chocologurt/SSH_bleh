using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPJ.Admin
{
    public partial class Console : System.Web.UI.Page
    {
        string userID = "Admin";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (KeyManager.checkAdminAccess(Response.Cookies["tokenA"].Value) == 1)
            {
                string lastLoginTime = KeyManager.retrieveLastLogin(userID);
                Debug.WriteLine(lastLoginTime);
                if (lastLoginTime != "")
                {
                    lastLogin.Text = "Last Login at " + lastLoginTime;
                }
                else
                {
                    lastLogin.Text = "this is your first time login in";
                }
            }
            else
            {
                Response.Redirect("Console_Login.aspx");
            }
        }
    }
}