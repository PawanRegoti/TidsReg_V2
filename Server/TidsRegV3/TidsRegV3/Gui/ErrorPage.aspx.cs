using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TidsRegV3.Gui
{
  public partial class ErrorPage : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void HomeButton_Click(object sender, EventArgs e)
    {
      Response.Redirect("TimeTracking.aspx");
    }
  }
}