using System;
using NUnit.Framework;
using TidsRegV3.BusinessLogic.Logger;
using TidsRegV3.BusinessLogic.Validators;

namespace TidsRegV3Tests.BusinessLogic.Validators
{
  [TestFixture]
  public class BasicValidatorTests
  {
    IErrorLogger errorLogger = new TestLogger();

    [TestCase("test", true)]
    [TestCase("test1", true)]
    [TestCase("Test", true)]
    [TestCase("Test1", true)]
    [TestCase("1", true)]
    [TestCase("", false)]
    [TestCase("(", false)]
    [TestCase("     ", false)]
    [TestCase("test ", false)]
    [TestCase("test with spaces", false)]
    [TestCase("test-specialchar", false)]
    public void ValidateAsNonSpacedStringTest(string input, bool valid)
    {
      //Arrange, Act and Assert
      if (valid)
        BasicValidator.ValidateAsNonSpacedString(input, new ServerLogger());
      else
        Assert.Throws<Exception>(() => BasicValidator.ValidateAsNonSpacedString(input, errorLogger));
    }

    [TestCase("0", true)]
    [TestCase("10", true)]
    [TestCase("", true)]
    [TestCase("(", false)]
    [TestCase("     ", true)]
    [TestCase("0 ", false)]
    [TestCase("1 0", false)]
    [TestCase("1-0", false)]
    [TestCase("-5", false)]
    public void ValidateTimeLog(string input, bool valid)
    {
      //Arrange, Act and Assert
      if (valid)
        BasicValidator.ValidateTimeLog(input, new ServerLogger());
      else
        Assert.Throws<Exception>(() => BasicValidator.ValidateTimeLog(input, errorLogger));
    }

    [TestCase("04-01-2018", true)]
    [TestCase("2018-01-01", true)]
    [TestCase("04/01/2018", true)]
    [TestCase("10 04 2018", true)]
    [TestCase("1", false)]
    [TestCase("", false)]
    [TestCase("(", false)]
    [TestCase("Test", false)]
    [TestCase("Test1", false)]
    [TestCase("test1", false)]
    [TestCase("     ", false)]
    [TestCase("test ", false)]
    [TestCase("04-13-2018", false)]
    public void ValidateDateTest(string input, bool valid)
    {
      //Arrange, Act and Assert
      if (valid)
        BasicValidator.ValidateDate(input, new ServerLogger());
      else
        Assert.Throws<Exception>(() => BasicValidator.ValidateDate(input, errorLogger));
    }

    [TestCase("100", true)]
    [TestCase("ABC", true)]
    [TestCase("12345678", true)]
    [TestCase(" ", false)]
    [TestCase("", false)]
    [TestCase("(", false)]
    [TestCase("100 ", false)]
    [TestCase("Test 1", false)]
    [TestCase("test-1", false)]
    [TestCase("     ", false)]
    [TestCase("test ", false)]
    [TestCase("123456789", false)]
    public void ValidateEmpNr(string input, bool valid)
    {
      //Arrange, Act and Assert
      if (valid)
        BasicValidator.ValidateEmployeeNr(input, new ServerLogger());
      else
        Assert.Throws<Exception>(() => BasicValidator.ValidateDate(input, errorLogger));
    }
  }
}
