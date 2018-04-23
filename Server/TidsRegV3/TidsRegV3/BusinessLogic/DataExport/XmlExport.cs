using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TidsRegV3.BusinessLogic.DataExport
{
  /// <summary>
  /// The xml export.
  /// </summary>
  public class XmlExport : IExport
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
    public void Export(DataTable tbl, string filePath = null)
    {
      try
      {
        if (tbl == null || tbl.Columns.Count == 0)
          throw new Exception("ExportToXml: Null or empty input table!\n");

        XDocument xdoc = new XDocument(
          new XElement(
            tbl.TableName,
            from column in tbl.Columns.Cast<DataColumn>()
            where column != tbl.Columns[0]
            select new XElement(
              column.ColumnName,
              from row in tbl.AsEnumerable() select new XElement(row.Field<string>(0), row[column]))));

        if (!string.IsNullOrEmpty(filePath))
        {
          try
          {
            File.WriteAllText(filePath, xdoc.ToString());
          }

          catch (Exception ex)
          {
            throw new Exception("ExportToXml: Xml file could not be saved! Check filepath.\n"
                                + ex.Message);
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("ExportToExcel: \n" + ex.Message);
      }
    }
  }
}