using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TidsRegV3.BusinessLogic.Logger;
using TidsRegV3.BusinessLogic.Modules;
using TidsRegV3.BusinessLogic.SqlCore;

namespace TidsRegV3.Gui
{
  public partial class TimeTracking : System.Web.UI.Page
  {
    private ProjectModule projectModule;
    private TimeLogModule timeLogModule;

    public TimeTracking()
    {
      ISqlHelperBase sqlHelperBase = new SqlHelperBase(new ServerLogger());
      projectModule = new ProjectModule(sqlHelperBase);
      timeLogModule = new TimeLogModule(sqlHelperBase);
    }
    /// <summary>
    /// The bind grid.
    /// </summary>
    /// <param name="date">
    /// The date.
    /// </param>
    public void BindGrid()
    {
      if (this.Session["GridTable"] == null)
      {
        this.GridView1.DataSource = this.CreateDataSource();
        this.GridView1.DataBind();
      }
      else
      {
        this.GridView1.DataSource = this.Session["GridTable"] as DataTable;
        this.GridView1.DataBind();
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if (this.Session["Date"] == null)
      {
        this.Session["Date"] = DateTime.Today;
      }

      if (!this.IsPostBack)
      {
        this.BindGrid();
      }
    }


    protected void route_Click(object sender, EventArgs e)
    {

    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
      this.GridView1.EditIndex = e.NewEditIndex;
      this.BindGrid();
      GridView1.Rows[e.NewEditIndex].Cells[1].Enabled = false;
    }

    protected void GridView1_RowDataBound(object sender, GridViewEditEventArgs e) { }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
      var row = this.GridView1.Rows[e.RowIndex];

      var totalHours = 0;
      for (int i = 2; i < row.Cells.Count; i++)
      {
        var hours = ((TextBox)row.Cells[i].Controls[0]).Text;
        if (!string.IsNullOrWhiteSpace(hours))
        {
          totalHours += Convert.ToInt32(hours);
        }
      }

      if (totalHours > 24)
      {
        e.Cancel = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('total hours are more than 24 hours.');", true);
        return;
      }

      for (int i = 2; i < row.Cells.Count; i++)
      {
        var timeLog = ((TextBox)row.Cells[i].Controls[0]).Text;
        var day = ((TextBox)row.Cells[1].Controls[0]).Text;
        var projectName = ((DataControlFieldCell)GridView1.HeaderRow.Cells[i]).ContainingField.HeaderText;

        timeLogModule.UpdateTimeLog(projectName, timeLog, day, this.Session["EmpNr"] as string);
      }

      this.GridView1.EditIndex = -1;
      this.Session["GridTable"] = null;
      this.BindGrid();
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
      this.GridView1.EditIndex = -1;
      this.BindGrid();
    }

    /// <summary>
    /// The create data source.
    /// </summary>
    /// <param name="date">
    /// The date.
    /// </param>
    /// <returns>
    /// The <see cref="DataTable"/>.
    /// </returns>
    private DataTable CreateDataSource()
    {
      var date = (DateTime)(this.Session["Date"] ?? DateTime.Today);
      var startOfWeek = date.AddDays(-1 * (int)date.DayOfWeek);

      var projectList = projectModule.ProjectList(this.Session["EmpNr"] as string);

      var dt = timeLogModule.FetchLog(this.Session["EmpNr"] as string, projectList, startOfWeek, startOfWeek.AddDays(7));

      this.Session["GridTable"] = dt;

      return dt;
    }

    protected void Forward_Click(object sender, EventArgs e)
    {
      this.Session["GridTable"] = null;
      this.Session["Date"] = ((DateTime)this.Session["Date"]).AddDays(7);
      this.BindGrid();
    }

    protected void Backward_Click(object sender, EventArgs e)
    {
      this.Session["GridTable"] = null;
      this.Session["Date"] = ((DateTime)this.Session["Date"]).AddDays(-7);
      this.BindGrid();
    }

    protected void Statistics_Click(object sender, EventArgs e)
    {
      Response.Redirect("ProjectOverview.aspx");
    }

    protected void AddNewProject_Click(object sender, EventArgs e)
    {
      if (projectModule.AddNewProject(this.NewProject.Text, this.Session["EmpNr"] as string))
      {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Project has been successfully added');", true);
        this.Session["GridTable"] = null;
        this.BindGrid();
      }
      else
      {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Unable to add project to the repository');", true);
      }
    }
  }
}