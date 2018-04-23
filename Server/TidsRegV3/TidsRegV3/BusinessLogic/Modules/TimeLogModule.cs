using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TidsRegV3.BusinessLogic.SqlCore;
using TidsRegV3.BusinessLogic.Validators;

namespace TidsRegV3.BusinessLogic.Modules
{
  public class TimeLogModule
  {
    private ISqlHelperBase sqlHelper;
    public TimeLogModule(ISqlHelperBase sqlHelper)
    {
      this.sqlHelper = sqlHelper;
    }

    /// <summary>
    /// The fetch log.
    /// </summary>
    /// <param name="empNr">
    /// The emp nr.
    /// </param>
    /// <param name="projectList">
    /// The project list.
    /// </param>
    /// <param name="from">
    /// The from.
    /// </param>
    /// <param name="to">
    /// The to.
    /// </param>
    /// <returns>
    /// The <see cref="DataTable"/>.
    /// </returns>
    /// <exception cref="Exception">
    /// Unable to fetch log.
    /// </exception>
    public DataTable FetchLog(string empNr, IEnumerable<string> projectList, DateTime from, DateTime to)
    {
      BasicValidator.ValidateEntry(empNr, "DummyValue", from, to, sqlHelper.ErrorLogger);

      if(!projectList.Any())
        return new DataTable();

      var projectArray = projectList.Select(x => x.Trim()).ToArray();
      var joinQuery = sqlHelper.GetFetchLogSelectQuery(empNr, @from, to, projectArray[0]);
      for (int i = 1; i < projectArray.Length; i++)
      {
        BasicValidator.ValidateAsNonSpacedString(projectArray[i], sqlHelper.ErrorLogger);

        var joinSuffix =
          $" full outer join {sqlHelper.GetFetchLogSelectQuery(empNr, @from, to, projectArray[i])} on {projectArray[0]}.WorkDay = {projectArray[i]}.WorkDay ";
        joinQuery += joinSuffix;
      }

      string coalease_value = projectList.Count() > 1
        ? $"coalesce( {string.Join(",", projectList.Select(x => $"{x}.WorkDay"))} )"
        : $"{projectList.First()}.WorkDay";

      var query =
        $"select convert( varchar(10), {coalease_value}, 120) as WorkDay, {string.Join(",", projectList.Select(x => $"{x}.TimeLog as {x}"))} from {joinQuery} order by {projectArray[0]}.WorkDay";

      var dt = sqlHelper.RunCommand(query, new SqlParameter[0]);

      var grid = new DataTable();
      grid.Columns.Add("WorkDay");
      grid.PrimaryKey = new[] { grid.Columns["WorkDay"] };
      grid.Columns.AddRange(projectList.Select(x => new DataColumn($"{x}")).ToArray());

      for (var i = from; i <= to; i = i.AddDays(1))
      {
        grid.Rows.Add(i.ToShortDateString());
      }

      grid.BeginLoadData();
      foreach (DataRow dtrow in dt.Rows)
      {
        var itemArray = new string[dtrow.ItemArray.Length];
        itemArray[0] = DateTime.Parse(dtrow.ItemArray[0].ToString()).ToShortDateString();
        for (var i = 1; i < dtrow.ItemArray.Length; i++)
        {
          itemArray[i] = dtrow.ItemArray[i].ToString();
        }

        grid.LoadDataRow(itemArray.Select(x => (object)x).ToArray(), LoadOption.OverwriteChanges);
      }
      grid.EndLoadData();

      return grid;
    }

    /// <summary>
    /// The fetch statistics.
    /// </summary>
    /// <param name="empNr">
    /// The emp nr.
    /// </param>
    /// <param name="project">
    /// The project.
    /// </param>
    /// <param name="from">
    /// The from.
    /// </param>
    /// <param name="to">
    /// The to.
    /// </param>
    /// <returns>
    /// The <see cref="DataTable"/>.
    /// </returns>
    /// <exception cref="Exception">
    /// Statistic Fetch failed.
    /// </exception>
    public DataTable FetchStatistics(string empNr, string project, DateTime from, DateTime to)
    {
      BasicValidator.ValidateEntry(empNr, project, from, to, sqlHelper.ErrorLogger);

      if (sqlHelper.RunCommand($"SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Project_{project}_Log'",
            new SqlParameter[0]).Rows.Count == 0)
        return new DataTable();

      return sqlHelper.RunCommand($"{sqlHelper.GetProjectFetchQuery(empNr, from, to, project)} order by WorkDay", new SqlParameter[0]);
    }

    /// <summary>
    /// The update database.
    /// </summary>
    /// <param name="projectName">
    /// The project Name.
    /// </param>
    /// <param name="timeLog">
    /// The time Log.
    /// </param>
    /// <param name="day">
    /// The day.
    /// </param>
    /// <param name="empNr">
    /// The emp nr.
    /// </param>
    /// <exception cref="Exception">
    /// Update Failed.
    /// </exception>
    public void UpdateTimeLog(string projectName, string timeLog, string day, string empNr)
    {
      sqlHelper.UpdateDatabase(projectName, timeLog, day, empNr);
    }
  }
}