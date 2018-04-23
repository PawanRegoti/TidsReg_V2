using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TidsRegV3.BusinessLogic.SqlCore;
using TidsRegV3.BusinessLogic.Validators;

namespace TidsRegV3.BusinessLogic.Modules
{
  public class ProjectModule
  {
    private ISqlHelperBase sqlHelper;
    public ProjectModule(ISqlHelperBase sqlHelper)
    {
      this.sqlHelper = sqlHelper;
    }

    /// <summary>
    /// Gets Project List.
    /// </summary>
    /// <param name="empNr"></param>
    /// <returns></returns>
    public IEnumerable<string> ProjectList(string empNr)
    {
      var query = $"select ProjectList from Projects where EmployeeNr = @empNr";
      var parameters =
        new[]
        {
          new SqlParameter("empNr", empNr)
        };

      var data = sqlHelper.RunCommand(query, parameters);
      return data.Rows.Count > 0 ? data.Rows[0].ItemArray.First().ToString().Split(',') : new string[0];
    }

    /// <summary>
    /// Add new Project.
    /// </summary>
    /// <param name="project">
    /// The project.
    /// </param>
    /// <param name="empNr">
    /// The emp Nr.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public bool AddNewProject(string project, string empNr)
    {
      project = project.Trim();

      BasicValidator.ValidateAsNonSpacedString(project, sqlHelper.ErrorLogger);
      BasicValidator.ValidateEmployeeNr(project, sqlHelper.ErrorLogger);

      var rowsCount = sqlHelper.RunNonQueryCommand(
        $@"IF Not EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Project_{project}_Log')
           BEGIN

            /****** Object:  Table [dbo].[Project_{project}_Log]    Script Date: 11-04-2018 14:17:19 ******/
            SET ANSI_NULLS ON


      SET QUOTED_IDENTIFIER ON


        CREATE TABLE[dbo].[Project_{project}_Log](

        [EmployeeNr][nchar](10) NOT NULL,

        [WorkDay][smalldatetime] NOT NULL,

        [TimeLog][smallint] NOT NULL,

        [Routed][bit] NOT NULL
        ) ON[PRIMARY]



      ALTER TABLE[dbo].[Project_{project}_Log] ADD CONSTRAINT[DF_Project_{
            project
          }_Log_TimeLog]  DEFAULT((0)) FOR[TimeLog]


      ALTER TABLE[dbo].[Project_{project}_Log] WITH CHECK ADD CONSTRAINT[FK_Project_{
            project
          }_Log_Login] FOREIGN KEY([EmployeeNr])
      REFERENCES[dbo].[Login_Table]
        ([EmployeeNr])


      ALTER TABLE[dbo].[Project_{project}_Log]
      CHECK CONSTRAINT[FK_Project_{project}_Log_Login]


      END", new SqlParameter[0]);

      var rows = sqlHelper.RunNonQueryCommand(
        @"IF NOT EXISTS(SELECT * FROM Projects WHERE EmployeeNr = @empNr)

          INSERT INTO Projects(EmployeeNr, ProjectList) VALUES(@empNr, @projectName)

          ELSE
          Update Projects set ProjectList = ProjectList + ',' + @projectName where EmployeeNr = @empNr",
        new[]
        {
          new SqlParameter("empNr", empNr),
          new SqlParameter("projectName", project),
        });

      return rowsCount != 0;
    }
  }
}