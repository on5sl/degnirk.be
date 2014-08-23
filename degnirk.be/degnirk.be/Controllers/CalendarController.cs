using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using DTO;

using Helpers;

using Services;
using Services.Facebook;
using Services.Google;

using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class CalendarController : SurfaceController
    {
        private readonly ICalendarServices _googleService;
        private readonly ICalendarServices _facebookService;

        public CalendarController()
        {
            var googleServiceSettings = new GoogleServiceSettings(
                ConfigurationManager.AppSettings["GoogleClientIDforNativeApplication"],
                ConfigurationManager.AppSettings["GoogleClientSecret"],
                ConfigurationManager.AppSettings["GoogleEmail"],
                ConfigurationManager.AppSettings["GoogleApplicationName"]);
            this._googleService = new GoogleService(googleServiceSettings);

            this._facebookService = new FacebookService(
                ConfigurationManager.AppSettings["FacebookAppAccessToken"], 
                long.Parse(ConfigurationManager.AppSettings["FacebookPageId"]));
        }

        [OutputCache(Duration = 3600, VaryByParam = "from;to;browser_timezone")]
        public ActionResult GetEvents(long from, long to, string browser_timezone)
        {
            var dateTimeFrom = UnixTimeHelper.UnixTime(from);
            var dateTimeTo = UnixTimeHelper.UnixTime(to);
            var facebookTask = _facebookService.GetEventsTask(dateTimeFrom, dateTimeTo);
            var googleTask = _googleService.GetEventsTask(dateTimeFrom, dateTimeTo);

            var events = facebookTask.Result.ToList();
            events.AddRange(googleTask.Result);
            
            dynamic result = new
            {
                success = 1,
                result = ConvertToAjaxObject(events)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private static IEnumerable<dynamic> ConvertToAjaxObject(IEnumerable<CalendarItem> events)
        {
            var objects = events.Select(c => new
            {
                @class = c.Class,
                end = UnixTimeHelper.UnixTime(c.End),
                id = c.Id,
                start = UnixTimeHelper.UnixTime(c.Start),
                title = c.Title,
                url = c.Url
            }).Cast<dynamic>().ToList();
            return objects;
        }
    }
}