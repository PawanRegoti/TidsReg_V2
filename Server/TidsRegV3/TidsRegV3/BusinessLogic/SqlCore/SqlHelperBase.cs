using System;
using System.Data;
using System.Data.SqlClient;
using TidsRegV3.BusinessLogic.Logger;
using TidsRegV3.BusinessLogic.Validators;

namespace TidsRegV3.BusinessLogic.SqlCore
{
  /// <summary>
  /// The sql helper.
  /// </summary>
  public class SqlHelperBase : ISqlHelperBase
  {
    private string trace = nameof(SqlHelperBase);

    public IErrorLogger ErrorLogger { get; set; }

    public SqlHelperBase(IErrorLogger errorLogger)
    {
      this.ErrorLogger = errorLogger;
    }


    /// <summary>
    /// The connection string.
    /// </summary>
    public readonly string ConnectionString =
      "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\TidsReg.mdf;Integrated Security = True";
    
    /// <summary>
    /// The Sql date.
    /// </summary>
    /// <param name="dateTime">
    /// The date time.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public string SqlDate(DateTime dateTime)
    {
      return dateTime.Date.ToString("yyyy-MM-dd HH:mm:ss");
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
    public void UpdateDatabase(string projectName, string timeLog, string day, string empNr)
    {
      projectName = projectName.Trim();
      timeLog = timeLog.Trim();

      BasicValidator.ValidateAsNonSpacedString(projectName, ErrorLogger);
      BasicValidator.ValidateAsNonSpacedString(empNr, ErrorLogger);
      BasicValidator.ValidateTimeLog(timeLog, ErrorLogger);
      BasicValidator.ValidateDate(day, ErrorLogger);

      var tableName = $"Project_{projectName}_Log";
      using (var conn = new SqlConnection())
      {
        var command = new SqlCommand($"select count(*) from {tableName} where WorkDay = @day and EmployeeNr = @empNr ", conn);
        command.Parameters.AddRange(CreateSqlParameters(timeLog, day, empNr));

        conn.ConnectionString = ConnectionString;
        try
        {
          conn.Open();
          if ((int)command.ExecuteScalar() > 0)
          {
            var updateCommand = new SqlCommand($"update {tableName} set TimeLog = @value where WorkDay = @day and EmployeeNr = @empNr ", conn);
            updateCommand.Parameters.AddRange(CreateSqlParameters(timeLog, day, empNr));

            var rowsUpdated = updateCommand.ExecuteNonQuery();
            Console.WriteLine($"rows updated: {rowsUpdated}");
          }
          else
          {
            var insertCommand = new SqlCommand($"insert into {tableName} (EmployeeNr, WorkDay, TimeLog, Routed) values (@empNr, @day, @value, 0) ", conn);
            insertCommand.Parameters.AddRange(CreateSqlParameters(timeLog, day, empNr));

            var rowsInserted = insertCommand.ExecuteNonQuery();
            Console.WriteLine($"rows updated: {rowsInserted}");
          }
        }
        catch (Exception e)
        {
          ErrorLogger.Error(e.Message, trace);
        }
        finally
        {
          conn.Close();
          conn.Dispose();
        }
      }
    }

    /// <summary>
    /// The create sql parameters.
    /// </summary>
    /// <param name="timeLog">
    /// The time log.
    /// </param>
    /// <param name="day">
    /// The day.
    /// </param>
    /// <param name="empNr">
    /// The emp nr.
    /// </param>
    /// <returns>
    /// The <see cref="SqlParameter[]"/>.
    /// </returns>
    protected SqlParameter[] CreateSqlParameters(string timeLog, string day, string empNr)
    {
      return new[]
               {
                 new SqlParameter("empNr", empNr),
                 new SqlParameter("day", Convert.ToDateTime(day.Trim())),
                 new SqlParameter("value", timeLog),
               };
    }

    /// <summary>
    /// Runs non query command.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public int RunNonQueryCommand(string query, SqlParameter[] parameters)
    {
      int rowsChanged = 0;
      using (var conn = new SqlConnection())
      {
        var command = new SqlCommand(query, conn);
        command.Parameters.AddRange(parameters);
        conn.ConnectionString = ConnectionString;
        try
        {
          conn.Open();
          rowsChanged = command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
          ErrorLogger.Error(e.Message, trace);
        }
        finally
        {
          conn.Close();
          conn.Dispose();
        }
      }

      return rowsChanged;
    }

    /// <summary>
    /// The run command.
    /// </summary>
    /// <param name="query">
    /// The query.
    /// </param>
    /// <param name="parameters"></param>
    /// <returns>
    /// The <see cref="DataTable"/>.
    /// </returns>
    /// <exception cref="Exception">
    /// </exception>
    public DataTable RunCommand(string query, SqlParameter[] parameters)
    {
      var dt = new DataTable();

      using (var conn = new SqlConnection())
      {
        var command = new SqlCommand(query, conn);
        command.Parameters.AddRange(parameters);
        conn.ConnectionString = ConnectionString;
        try
        {
          conn.Open();
          dt.Load(command.ExecuteReader());
        }
        catch (Exception e)
        {
          ErrorLogger.Error(e.Message, trace);
        }
        finally
        {
          conn.Close();
          conn.Dispose();
        }
      }

      return dt;
    }

    /// <summary>
    /// The get fetch log select query.
    /// </summary>
    /// <param name="empNr">
    /// The emp nr.
    /// </param>
    /// <param name="from">
    /// The from.
    /// </param>
    /// <param name="to">
    /// The to.
    /// </param>
    /// <param name="project">
    /// The project.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public string GetFetchLogSelectQuery(string empNr, DateTime @from, DateTime to, string project)
    {
      return $" ({GetProjectFetchQuery(empNr, from, to, project)}) as {project} ";
    }

    /// <summary>
    /// The get project fetch query.
    /// </summary>
    /// <param name="empNr">
    /// The emp nr.
    /// </param>
    /// <param name="from">
    /// The from.
    /// </param>
    /// <param name="to">
    /// The to.
    /// </param>
    /// <param name="project">
    /// The project.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public string GetProjectFetchQuery(string empNr, DateTime @from, DateTime to, string project)
    {
      return
        $"select WorkDay, TimeLog  from Project_{project}_Log where EmployeeNr = '{empNr}' and(WorkDay >= '{SqlDate(@from)}' and WorkDay <= '{SqlDate(to)}')";
    }
  }
}