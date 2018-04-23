using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using TidsRegV3.BusinessLogic.Logger;
using TidsRegV3.BusinessLogic.Modules;
using TidsRegV3.BusinessLogic.SqlCore;

namespace TidsRegV3.Gui
{
  public partial class ProjectOverview : System.Web.UI.Page
  {
    private ProjectModule projectModule;
    private TimeLogModule timeLogModule;

    public ProjectOverview()
    {
      ISqlHelperBase sqlHelperBase = new SqlHelperBase(new ServerLogger());
      projectModule = new ProjectModule(sqlHelperBase);
      timeLogModule = new TimeLogModule(sqlHelperBase);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!Page.IsPostBack)
      {
        this.ProjectDropDown.Items.Clear();
        this.ChartViewDropDown.Items.Clear();

        var projectList = projectModule.ProjectList(this.Session["EmpNr"] as string);

        this.ProjectDropDown.Items.AddRange(projectList.Select(x => new ListItem(x)).ToArray());
        foreach (var seriesChartType in Enum.GetNames(typeof(SeriesChartType)))
        {
          this.ChartViewDropDown.Items.Add(new ListItem(seriesChartType));
        }
      }
    }

    protected void CreateGrid_Click(object sender, EventArgs e)
    {
      if (this.Calendar1.SelectedDates.Count == 0)
      {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('No time period selected.');", true);
        return;
      }

      FetchProjectData();
      CreateChart();
    }

    private void FetchProjectData()
    {
      var dates = this.Calendar1.SelectedDates;
      var projectData = timeLogModule.FetchStatistics(
        this.Session["EmpNr"] as string,
        this.ProjectDropDown.SelectedValue,
        dates[0],
        dates[dates.Count - 1]);

      ViewState["ProjectData"] = projectData;
    }

    protected void Export_Click(object sender, EventArgs e)
    {
      var sw = new StringWriter();
      var hw = new HtmlTextWriter(sw);
      var tempGridView = new GridView { DataSource = ViewState["ProjectData"] };

      tempGridView.DataBind();
      tempGridView.RenderControl(hw);

      Response.Clear();
      Response.Buffer = true;
      Response.AddHeader("content-disposition",
        "attachment;filename=ProjectOverview.xls");
      Response.Charset = "";
      Response.ContentType = "application/vnd.ms-excel";
      Response.Output.Write(sw.ToString());
      Response.Flush();
      Response.End();
    }

    private void CreateChart()
    {
      this.Chart1.Series.Clear();
      this.Chart1.Series.Add(
        new Series("Chart")
        {
          XValueMember = "WorkDay",
          YValueMembers = "TimeLog",
          ChartType = (SeriesChartType)Enum.Parse(
            typeof(SeriesChartType),
            this.ChartViewDropDown.SelectedValue)
        });

      var data = ViewState["ProjectData"] as DataTable;

      if (data.Rows.Count == 0)
      {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('No time log found for given period');", true);
        return;
      }

      this.Chart1.DataSource = data;
      this.Chart1.DataBind();

      this.ChartViewDropDown.Visible = true;
      this.UpdateChart.Visible = true;
      this.Export.Visible = true;
    }

    protected void UpdateChart_Click(object sender, EventArgs e)
    {
      this.CreateChart();
    }

    protected void Chart1_Load(object sender, EventArgs e)
    {
    }
  }
}