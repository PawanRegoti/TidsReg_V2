using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Newtonsoft.Json.Linq;
using TidsRegV3.BusinessLogic.Logger;
using TidsRegV3.BusinessLogic.Modules;
using TidsRegV3.BusinessLogic.SqlCore;
using TidsRegV3.BusinessLogic.Validators;

namespace TidsRegV3.Controllers
{
  [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
  public class TidsRegController : ApiController
  {

    private LoginModule loginModule;
    private ProjectModule projectModule;
    private TimeLogModule timeLogModule;
    private IErrorLogger errorLogger = new ApiLogger();

    public TidsRegController()
    {
      var sqlHelperBase = new SqlHelperBase(errorLogger);
      loginModule = new LoginModule(sqlHelperBase);
      projectModule = new ProjectModule(sqlHelperBase);
      timeLogModule = new TimeLogModule(sqlHelperBase);
    }

    [HttpOptions]
    [ResponseType(typeof(void))]
    [Route("api/TidsReg/Register")]
    [Route("api/TidsReg/FetchProjectList")]
    [Route("api/TidsReg/AddNewProject/{projectName}")]
    [Route("api/TidsReg/FetchtimeLog")]
    [Route("api/TidsReg/FetchStatistics")]
    [Route("api/TidsReg/AddTimeLog")]
    public IHttpActionResult Options()
    {
      HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Methods", "*");
      HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Headers", "*");
      return Ok();
    }

    [HttpPost]
    [Route("api/TidsReg/Register")]
    public HttpResponseMessage RegisterUser(HttpRequestMessage request)
    {
      try
      {
        if (request.Content.Headers.ContentType.MediaType != "application/json")
          return new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);

        var jsonData = request.Content.ReadAsStringAsync().Result;
        var data = JObject.Parse(jsonData);
        var username = (string)data["username"];
        var password = (string)data["password"];

        if (!loginModule.RegisterUser(username, password))
        {
          return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        return new HttpResponseMessage(HttpStatusCode.Accepted);
      }
      catch (Exception e)
      {
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      }
    }

    [HttpGet]
    [BasicAuth]
    [Route("api/TidsReg/FetchProjectList")]
    public IEnumerable<string> FetchProjectList()
    {
      var result = projectModule.ProjectList(Thread.CurrentPrincipal.Identity.Name);
      return result;
    }

    [HttpGet]
    [BasicAuth]
    [Route("api/TidsReg/AddNewProject/{projectName}")]
    public string AddNewProject(string projectName)
    {
      return projectModule.AddNewProject(projectName, Thread.CurrentPrincipal.Identity.Name)
          ? $"Successfully added Project {projectName} to your time log."
          : $"Unable to add Project {projectName} to your time log.";
    }

    [HttpPost]
    [BasicAuth]
    [Route("api/TidsReg/FetchtimeLog")]
    public HttpResponseMessage FetchTimeLog(HttpRequestMessage request)
    {
      try
      {
        if (request.Content.Headers.ContentType.MediaType != "application/json")
          return new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);

        var jsonData = request.Content.ReadAsStringAsync().Result;
        var data = JObject.Parse(jsonData);
        var fromDate = DateTime.Parse((string)data["from"]);
        var toDate = DateTime.Parse((string)data["to"]);

        var empNr = Thread.CurrentPrincipal.Identity.Name;
        var result = timeLogModule.FetchLog(empNr, projectModule.ProjectList(empNr), fromDate, toDate);

        if (result.Rows.Count == 0)
        {
          return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        return new HttpResponseMessage(HttpStatusCode.Accepted)
        {
          Content = new JsonContent(ApiHelper.DataTableToJson(result))
        };
      }
      catch (Exception e)
      {
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      }
    }

    [HttpPost]
    [BasicAuth]
    [Route("api/TidsReg/FetchStatistics")]
    public HttpResponseMessage FetchStatistics(HttpRequestMessage request)
    {
      try
      {
        if (request.Content.Headers.ContentType.MediaType != "application/json")
          return new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);

        var jsonData = request.Content.ReadAsStringAsync().Result;
        var data = JObject.Parse(jsonData);
        var projectName = (string) data["project"];
        var fromDate = DateTime.Parse((string)data["from"]);
        var toDate = DateTime.Parse((string)data["to"]);

        var empNr = Thread.CurrentPrincipal.Identity.Name;
        var result = timeLogModule.FetchStatistics(empNr, projectName, fromDate, toDate);

        if (result.Rows.Count == 0)
        {
          return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        return new HttpResponseMessage(HttpStatusCode.Accepted)
        {
          Content = new JsonContent(ApiHelper.DataTableToJson(result))
        };
      }
      catch (Exception e)
      {
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      }
    }

    [HttpPost]
    [BasicAuth]
    [Route("api/TidsReg/AddTimeLog")]
    public HttpResponseMessage AddTimeLog(HttpRequestMessage request)
    {
      try
      {
        if (request.Content.Headers.ContentType.MediaType != "application/json")
          return new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);

        var jsonData = request.Content.ReadAsStringAsync().Result;
        var data = JArray.Parse(jsonData);

        var logs = new List<Log>();
        foreach (var jToken in data)
        {
          var log = new Log()
          {
            WorkDay = DateTime.Parse((string) jToken["WorkDay"]),
            TimeLog = jToken.Children().Select(x => (JProperty) x)
              .Where(y => y.Name != "WorkDay" && !string.IsNullOrWhiteSpace((string) y.Value))
              .ToDictionary(project => (string) project.Name, hours => (int) hours.Value)
          };

          if (log.TimeLog.Sum(x => x.Value) > 24)
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
              Content = new StringContent($"Total time exceeds 24 hours for workday: {log.WorkDay.ToShortDateString()}")
            };

          logs.Add(log);
        }

        var empNr = Thread.CurrentPrincipal.Identity.Name;

        foreach (var log in logs)
        {
          foreach (var timeLog in log.TimeLog)
          {
            try
            {
              BasicValidator.ValidateAsNonSpacedString(timeLog.Key, errorLogger);
              timeLogModule.UpdateTimeLog($"{timeLog.Key}", timeLog.Value.ToString(), log.WorkDay.ToShortDateString(), empNr);
            }
            catch (Exception e)
            {
              return new HttpResponseMessage(HttpStatusCode.BadRequest)
              {
                Content = new StringContent($"Error adding log for project: {timeLog.Key}, for workday: {log.WorkDay}")
              };
            }
          }
        }

        return  new HttpResponseMessage(HttpStatusCode.Accepted);
      }
      catch (Exception e)
      {
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      }
    }

    #region Old Code

    //[HttpPost]
    //[BasicAuth]
    //[Route("api/TidsReg/AddTimeLog")]
    //public HttpResponseMessage AddTimeLog(HttpRequestMessage request)
    //{
    //  try
    //  {
    //    if (request.Content.Headers.ContentType.MediaType != "application/json")
    //      return new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);

    //    var jsonData = request.Content.ReadAsStringAsync().Result;
    //    var data = JObject.Parse(jsonData);

    //    var logs = new List<Log>();
    //    foreach (var jLog in ((JToken)data["logs"]).ToList())
    //    {
    //      JToken logEntry = jLog["log"];

    //      var log = new Log()
    //      {
    //        WorkDay = DateTime.Parse((string)logEntry["workday"]),
    //        TimeLog = ((JToken)logEntry["timelogs"]).ToDictionary(x => (string)x["project"], y => (int)y["Hours"])
    //      };

    //      if (log.TimeLog.Sum(x => x.Value) > 24)
    //        return new HttpResponseMessage(HttpStatusCode.BadRequest)
    //        {
    //          Content = new StringContent($"Total time exceeds 24 hours for workday: {log.WorkDay.ToShortDateString()}")
    //        };

    //      logs.Add(log);
    //    }

    //    var empNr = Thread.CurrentPrincipal.Identity.Name;

    //    foreach (var log in logs)
    //    {
    //      foreach (var timeLog in log.TimeLog)
    //      {
    //        try
    //        {
    //          timeLogModule.UpdateTimeLog($"{timeLog.Key}", timeLog.Value.ToString(), log.WorkDay.ToShortDateString(), empNr);
    //        }
    //        catch (Exception e)
    //        {
    //          return new HttpResponseMessage(HttpStatusCode.BadRequest)
    //          {
    //            Content = new StringContent($"Error adding log for project: {timeLog.Key}, for workday: {log.WorkDay}")
    //          };
    //        }
    //      }
    //    }

    //    return new HttpResponseMessage(HttpStatusCode.Accepted);
    //  }
    //  catch (Exception e)
    //  {
    //    return new HttpResponseMessage(HttpStatusCode.BadRequest);
    //  }
    //}

    #endregion
  }
}