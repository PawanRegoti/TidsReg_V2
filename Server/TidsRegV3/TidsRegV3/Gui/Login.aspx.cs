using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TidsRegV3.BusinessLogic.Logger;
using TidsRegV3.BusinessLogic.Modules;
using TidsRegV3.BusinessLogic.SqlCore;

namespace TidsRegV3.Gui
{
  public partial class Login : System.Web.UI.Page
  {
    private LoginModule loginModule;
    public Login()
    {
      loginModule = new LoginModule(new SqlHelperBase(new ServerLogger()));
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void login_Click(object sender, EventArgs e)
    {
      var username = this.username.Text;
      var password = this.password.Text;

      if (loginModule.UserLogin(username, password))
      {
        this.Session["EmpNr"] = username;
        this.Response.Redirect("TimeTracking.aspx");
      }
      else
      {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Username or Password is invalid');", true);
      }
    }

    protected void register_Click(object sender, EventArgs e)
    {
      var regUsername = this.RegUsername.Text;
      var regPassword = this.RegPassword.Text;

      if (loginModule.RegisterUser(regUsername, regPassword))
      {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Registration Successful');", true);
      }
      else
      {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Registration Failed');", true);
      }
    }
  }
}