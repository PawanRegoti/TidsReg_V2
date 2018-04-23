using System;
using System.Data;

namespace TidsRegV3.BusinessLogic.DataExport
{
  /// <summary>
  /// The excel export.
  /// </summary>
  public class ExcelExport : IExport
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
    public void Export(DataTable tbl, string excelFilePath = null)
    {
      try
      {
        //if (tbl == null || tbl.Columns.Count == 0)
        //  throw new Exception("ExportToExcel: Null or empty input table!\n");

        //var excelApp = new Microsoft.Office.Interop.Excel.Application();
        //excelApp.Workbooks.Add();

        //Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;

        //for (var i = 0; i < tbl.Columns.Count; i++)
        //{
        //  workSheet.Cells[1, i + 1] = tbl.Columns[i].ColumnName;
        //}

        //for (var i = 0; i < tbl.Rows.Count; i++)
        //{
        //  for (var j = 0; j < tbl.Columns.Count; j++)
        //  {
        //    workSheet.Cells[i + 2, j + 1] = tbl.Rows[i][j];
        //  }
        //}

        //if (!string.IsNullOrEmpty(excelFilePath))
        //{
        //  try
        //  {
        //    workSheet.SaveAs(excelFilePath);
        //    excelApp.Quit();
        //    MessageBox.Show("Excel file saved!");
        //  }
        //  catch (Exception ex)
        //  {
        //    throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
        //                        + ex.Message);
        //  }
        //}
        //else
        //{ 
        //  excelApp.Visible = true;
        //}
      }
      catch (Exception ex)
      {
        throw new Exception("ExportToExcel: \n" + ex.Message);
      }
    }
  }
}