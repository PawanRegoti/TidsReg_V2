using System.Data.SqlClient;
using TidsRegV3.BusinessLogic.Logger;
using TidsRegV3.BusinessLogic.SqlCore;
using TidsRegV3.BusinessLogic.Validators;

namespace TidsRegV3.BusinessLogic.Modules
{
  public class LoginModule
  {
    private ISqlHelperBase sqlHelper;
    public LoginModule(ISqlHelperBase sqlHelper)
    {
      this.sqlHelper = sqlHelper;
    }

    /// <summary>
    /// The user login.
    /// </summary>
    /// <param name="username">
    /// The username.
    /// </param>
    /// <param name="password">
    /// The password.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public bool UserLogin(string username, string password)
    {
      username = username.Trim();
      password = password.Trim();

      ValidateInput(username, password);

      var query = $"select EmployeeNr from Login_Table where EmployeeNr = @username and Password = @password";
      var parameters =
        new[]
        {
          new SqlParameter("username", username),
          new SqlParameter("password", password.GetHashCode())
        };
      var dt = sqlHelper.RunCommand(query, parameters);
      return dt.Rows.Count != 0;
    }

    /// <summary>
    /// The register user.
    /// </summary>
    /// <param name="username">
    /// The username.
    /// </param>
    /// <param name="password">
    /// The password.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public bool RegisterUser(string username, string password)
    {
      username = username.Trim();
      password = password.Trim();

      ValidateInput(username, password);

      var rows = sqlHelper.RunNonQueryCommand(
        $"Insert into Login_Table (EmployeeNr, Password) values (@username, @password)",
        new[] {new SqlParameter("username", username), new SqlParameter("password", password.GetHashCode()),});
      return rows != 0;
    }

    private void ValidateInput(string username, string password)
    {
      BasicValidator.ValidateEmployeeNr(username, sqlHelper.ErrorLogger);
      BasicValidator.ValidateAsNonSpacedString(username, sqlHelper.ErrorLogger);
      BasicValidator.ValidateAsNonSpacedString(password, sqlHelper.ErrorLogger);
    }
  }
}