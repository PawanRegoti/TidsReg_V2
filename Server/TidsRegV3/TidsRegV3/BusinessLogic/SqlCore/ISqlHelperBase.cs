using System;
using System.Data;
using System.Data.SqlClient;
using TidsRegV3.BusinessLogic.Logger;

namespace TidsRegV3.BusinessLogic.SqlCore
{
  public interface ISqlHelperBase
  {
    IErrorLogger ErrorLogger { get; set; }

    /// <summary>
    /// The Sql date.
    /// </summary>
    /// <param name="dateTime">
    /// The date time.
    /// </param>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    string SqlDate(DateTime dateTime);
    
    int RunNonQueryCommand(string query, SqlParameter[] parameters);

    DataTable RunCommand(string query, SqlParameter[] parameters);

    void UpdateDatabase(string projectName, string timeLog, string day, string empNr);

    string GetProjectFetchQuery(string empNr, DateTime @from, DateTime to, string project);

    string GetFetchLogSelectQuery(string empNr, DateTime @from, DateTime to, string project);
  }
}