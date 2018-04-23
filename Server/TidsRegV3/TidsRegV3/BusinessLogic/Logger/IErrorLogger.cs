namespace TidsRegV3.BusinessLogic.Logger
{
  public interface IErrorLogger
  {
    object Error(string message, string trace = null);
  }
}
