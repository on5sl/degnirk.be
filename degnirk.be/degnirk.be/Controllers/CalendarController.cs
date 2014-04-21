using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using DTO;
using Service.Google;
using Helpers;

using Services;
using Services.Facebook;
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

        //[OutputCache(Duration = 3600, VaryByParam = "from;to;browser_timezone")]
        public ActionResult GetEvents(long from, long to, string browser_timezone)
        {
            var dateTimeFrom = UnixTimeHelper.UnixTime(from);
            var dateTimeTo = UnixTimeHelper.UnixTime(to);
            var events = GetFacebookEvents(dateTimeFrom, dateTimeTo).ToList();
            events.AddRange(GetGoogleEvents(dateTimeFrom,dateTimeTo));
            
            dynamic result = new
            {
                success = 1,
                result = events
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<dynamic> GetGoogleEvents(DateTime from, DateTime to)
        {
            var googleEvents = this._googleService.GetEvents(from, to);
            return ConvertToAjaxObject(googleEvents);
        }

        private IEnumerable<dynamic> GetFacebookEvents(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            var facebookEvents = this._facebookService.GetEvents(dateTimeFrom,
                dateTimeTo);

            return ConvertToAjaxObject(facebookEvents);
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