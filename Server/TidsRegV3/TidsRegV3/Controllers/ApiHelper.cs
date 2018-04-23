using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;

namespace TidsRegV3.Controllers
{
  public static class ApiHelper
  {
    public static string DataTableToJson(DataTable dt)
    {
      System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
      List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
      Dictionary<string, object> row;
      foreach (DataRow dr in dt.Rows)
      {
        row = new Dictionary<string, object>();
        foreach (DataColumn col in dt.Columns)
        {
          row.Add(col.ColumnName, dr[col].ToString());
        }
        rows.Add(row);
      }
      return serializer.Serialize(rows);
    }
  }

  public class Log
  {
    public DateTime WorkDay { get; set; }

    public Dictionary<string, int> TimeLog { get; set; }
  }

  public class JsonContent : StringContent
  {
    public JsonContent(string content)
      : this(content, Encoding.UTF8)
    {
    }

    public JsonContent(string content, Encoding encoding)
      : base(content, encoding, "application/json")
    {
    }
  }

  public class XmlContent : StringContent
  {
    public XmlContent(string content)
      : this(content, Encoding.UTF8)
    {
    }

    public XmlContent(string content, Encoding encoding)
      : base(content, encoding, "application/xml")
    {
    }
  }
}