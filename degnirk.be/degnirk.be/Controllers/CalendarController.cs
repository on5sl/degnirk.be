using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Service;
using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class CalendarController : SurfaceController
    {
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
            var googleService = new GoogleService();
            var googleEvents = googleService.GetEvents(@from,to);
            return googleEvents;
        }

        private static IEnumerable<dynamic> GetFacebookEvents(DateTime from, DateTime to)
        {
            var facebookService = new FacebookService();
            return facebookService.GetFacebookEvents(from, to);
        }
    }
}