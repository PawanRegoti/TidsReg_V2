using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace TidsRegV3.BusinessLogic.Logger
{
  public class ServerLogger : IErrorLogger
  {
    public object Error(string message, string trace = null)
    {
      LogWrite(message);

      throw new Exception(message);
    }

    void LogWrite(string logMessage)
    {
      var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      try
      {
        using (StreamWriter w = File.AppendText(path + "\\" + "log.txt"))
        {
          Log(logMessage, w);
        }
      }
      catch (Exception ex)
      {
        return;
      }
    }
    void Log(string logMessage, TextWriter txtWriter)
    {
      try
      {
        txtWriter.Write("\r\nLog Entry : ");
        txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
          DateTime.Now.ToLongDateString());
        txtWriter.WriteLine("  :");
        txtWriter.WriteLine("  :{0}", logMessage);
        txtWriter.WriteLine("-------------------------------");
      }
      catch (Exception ex)
      {
        return;
      }
    }

  }
}