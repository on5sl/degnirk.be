using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Google;
using Helpers;
using Service;
using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class CalendarController : SurfaceController
    {
        private const string AccessToken = "442171809217325|Q3rA6b68G7TuWYF1beRUNsLUe94";
        private const string CreatorId = "56615007038";
        private const string EventUri = "https://www.facebook.com/events/";
        public List<dynamic> FacebookEvents { get; private set; }

        //[OutputCache(Duration = 3600, VaryByParam = "from;to;browser_timezone")]
        public ActionResult GetEvents(long from, long to, string browser_timezone)
        {
            var dateTimeFrom = UnixTimeHelper.UnixTime(from);
            var dateTimeTo = UnixTimeHelper.UnixTime(to);
            this.FacebookEvents = new List<dynamic>();
            this.FacebookEvents.AddRange(GetFacebookEvents(dateTimeFrom, dateTimeTo));
            this.FacebookEvents.AddRange(GetGoogleEvents(dateTimeFrom, dateTimeTo));
            
            dynamic result = new
            {
                success = 1,
                result = this.FacebookEvents
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private static IEnumerable<dynamic> GetGoogleEvents(DateTime from, DateTime to)
        {
            var googleApi = new GoogleApi();
            var googleEvents = googleApi.GetEvents(from, to);
            return ConvertEvents(googleEvents);
        }

        private static IEnumerable<dynamic> GetFacebookEvents(DateTime from, DateTime to)
        {
            var facebookService = new FacebookService();
            return facebookService.GetFacebookEvents(from, to);
        }

        private static IEnumerable<dynamic> ConvertEvents(List<List<KeyValuePair<string, string>>> events)
        {
            var convertedEvents = new List<dynamic>();
            events.ForEach(googleEvent => convertedEvents.Add(new
            {
                id = googleEvent.Single(i => i.Key == "id").Value,
                title = googleEvent.Single(i => i.Key == "title").Value,
                url = googleEvent.Single(i => i.Key == "url").Value,
                @class = googleEvent.Single(i => i.Key == "class").Value,
                start = googleEvent.Single(i => i.Key == "start").Value,
                end = googleEvent.Single(i => i.Key == "end").Value
            }));
            return convertedEvents;
        }
    }
}