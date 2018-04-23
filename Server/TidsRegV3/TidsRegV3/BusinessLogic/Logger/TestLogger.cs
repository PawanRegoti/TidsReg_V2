using System;

namespace TidsRegV3.BusinessLogic.Logger
{
  public class TestLogger: IErrorLogger
  {
    public object Error(string message, string trace = null)
    {
      throw new Exception(message);
    }
  }
}