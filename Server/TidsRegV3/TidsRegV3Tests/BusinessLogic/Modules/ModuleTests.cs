using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using NUnit.Mocks;
using TidsRegV3.BusinessLogic.Logger;
using TidsRegV3.BusinessLogic.Modules;
using TidsRegV3.BusinessLogic.SqlCore;

namespace TidsRegV3Tests.BusinessLogic.Modules
{
  [TestFixture]
  public class ModuleTests
  {
    IErrorLogger errorLogger;
    private LoginModule loginModule;
    private ProjectModule projectModule;
    private TimeLogModule timeLogModule;
    public ModuleTests()
    {
      errorLogger = new TestLogger(){ };
    }

    [TestCase("100 ", true)]
    [TestCase(" 100", true)]
    [TestCase("1 00", false)]
    [TestCase("101", true)]
    public void Login_Test(string username, bool isvalid)
    {
      var password = "password";
      var dt = (username == "100")
        ? CreateDataTable(new List<Dictionary<string, string>>
        {
          {
            new Dictionary<string, string>
            {
              {nameof(username), "100"},
              {nameof(password), password},
            }
          }
        })
        : new DataTable();
      
      var sqlMock = GetSqlHelperBase(dt);

      loginModule = new LoginModule(sqlMock);
      
      if (isvalid)
        loginModule.UserLogin(username, password);
      else
        Assert.Throws<Exception>(() => loginModule.UserLogin(username, password));
    }

    ISqlHelperBase GetSqlHelperBase(DataTable dt, int ddlOutput = 0)
    {
      var sqlMock = Substitute.For<ISqlHelperBase>();
      sqlMock.ErrorLogger.ReturnsForAnyArgs(errorLogger);
      sqlMock.RunCommand(Arg.Any<string>(), new[] {Arg.Any<SqlParameter>()}).ReturnsForAnyArgs(dt);
      sqlMock.RunNonQueryCommand(Arg.Any<string>(), new[] {Arg.Any<SqlParameter>()}).ReturnsForAnyArgs(ddlOutput);
      return sqlMock;
    }

    DataTable CreateDataTable(List<Dictionary<string,string>> rows)
    {
      var dt = new DataTable();
      dt.Columns.AddRange(rows.First().Select(x => new DataColumn(x.Key)).ToArray());

      foreach (var row in rows)
      {
        var dr = dt.NewRow();
        foreach (var cell in row)
        {
          dr[cell.Key] = cell.Value;
        }
        dt.Rows.Add(dr);
      }

      return dt;
    }
  }
}
