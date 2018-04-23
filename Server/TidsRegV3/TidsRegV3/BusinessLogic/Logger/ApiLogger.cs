using System;
using System.Net;
using System.Net.Http;

namespace TidsRegV3.BusinessLogic.Logger
{
  public class ApiLogger : IErrorLogger
  {
    public object Error(string message, string trace = null)
    {
      throw new Exception(message);
    }
  }
}