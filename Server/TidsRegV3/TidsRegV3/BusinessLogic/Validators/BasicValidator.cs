using System;
using System.Text.RegularExpressions;
using TidsRegV3.BusinessLogic.Logger;

namespace TidsRegV3.BusinessLogic.Validators
{
  public static class BasicValidator
  {
    private static bool ValidateForUnwantedCharacters(string input)
    {
      if (string.IsNullOrWhiteSpace(input))
        return false;

      var regexItem = new Regex("^[a-zA-Z0-9åæøÅÆØ]*$");
      return regexItem.IsMatch(input);
    }

    public static void ValidateAsNonSpacedString(string projectName, IErrorLogger errorLogger)
    {
      if (ValidateForUnwantedCharacters(projectName))
      {
        return;
      }
      errorLogger.Error($"Invalid string: {projectName}. It should be single word and no special characters.");
    }

    public static void ValidateTimeLog(string timeLog, IErrorLogger errorLogger)
    {
      if(string.IsNullOrWhiteSpace(timeLog))
      { return; }

      if (ValidateForUnwantedCharacters(timeLog) && int.TryParse(timeLog, out int result) && result >= 0)
      {
        return;
      }
      errorLogger.Error($"invalid hours: {timeLog}");
    }

    public static void ValidateDate(string day, IErrorLogger errorLogger)
    {
      if (DateTime.TryParse(day, out DateTime result))
      {
        return;
      }
      errorLogger.Error($"invalid date: {day}");
    }

    public static void ValidateEmployeeNr(string empNr, IErrorLogger errorLogger)
    {
      if (ValidateForUnwantedCharacters(empNr) && empNr.Length < 9)
      {
        return;
      }
      errorLogger.Error($"invalid employee number: {empNr}");
    }

    public static void ValidateEntry(string empNr, string project, DateTime @from, DateTime to, IErrorLogger errorLogger)
    {
      ValidateEmployeeNr(empNr,errorLogger);
      ValidateForUnwantedCharacters(project);
      ValidateAsNonSpacedString(project, errorLogger);
      ValidateDate(from.ToShortDateString(), errorLogger);
      ValidateDate(to.ToShortDateString(), errorLogger);
    }
  }
}