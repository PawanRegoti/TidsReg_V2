using System;
using System.Data;

namespace TidsRegV3.BusinessLogic.DataExport
{
  public interface IExport
  {
    /// <summary>
    /// The export.
    /// </summary>
    /// <param name="tbl">
    /// The tbl.
    /// </param>
    /// <param name="excelFilePath">
    /// The excel file path.
    /// </param>
    /// <exception cref="Exception">
    /// Excel errors
    /// </exception>
    void Export(DataTable tbl, string excelFilePath = null);
  }
}