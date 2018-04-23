using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TidsRegV3.Gui
{
  public partial class Layout : System.Web.UI.MasterPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      LogOff.Visible = !HttpContext.Current.Request.Url.AbsolutePath.Contains("Login");
      Home.Enabled = !HttpContext.Current.Request.Url.AbsolutePath.Contains("Login");
    }

    protected void LogOff_Click(object sender, EventArgs e)
    {
      Session.Abandon();
      Session.Clear();
      Response.Redirect("Login.aspx");
    }

    protected void Home_Click(object sender, EventArgs e)
    {
      Response.Redirect("TimeTracking.aspx");
    }
  }
}