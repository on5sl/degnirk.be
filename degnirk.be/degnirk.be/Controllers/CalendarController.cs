using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class CalendarController : SurfaceController
    {
        private const string AccessToken = "442171809217325|Q3rA6b68G7TuWYF1beRUNsLUe94";
        private const string CreatorId = "56615007038";
        private const string EventUri = "https://www.facebook.com/events/";
        private FacebookClient _facebookClient;
        public List<dynamic> FacebookEvents { get; private set; }

        [OutputCache(Duration = 3600, VaryByParam = "from;to;browser_timezone")]
        public ActionResult GetEvents(long from, long to, string browser_timezone)
        {
            var dateTimeFrom = UnixTime(from);
            var dateTimeTo = UnixTime(to);
            _facebookClient = new FacebookClient(AccessToken);
            GetFacebookEvents(dateTimeFrom, dateTimeTo);
            dynamic test = new
            {
                success = 1,
                result = this.FacebookEvents
            };
            return Json(test, JsonRequestBehavior.AllowGet);
        }


        private void GetFacebookEvents(DateTime from, DateTime to)
        {

            dynamic facebookEvents = ((Facebook.JsonArray)(_facebookClient.Get("/fql",
                new
                {
                    q = string.Format("select eid,  name, attending_count, pic_cover, start_time from event " +
                                      "where creator = {0} " +
                                      "AND start_time >= '{1}' " +
                                      "AND start_time <= '{2}' " +
                                      "ORDER BY start_time desc", CreatorId, from.ToString("s"), to.ToString("s"))
                }) as dynamic).data);

            if (facebookEvents == null)
            {
                return;
            }
            this.FacebookEvents = ConvertEvents(facebookEvents as IEnumerable<dynamic>);
        }

        private static List<dynamic> ConvertEvents(IEnumerable<dynamic> events)
        {
            var convertedEvents = new List<dynamic>();
            events.ForEach(fbevent => convertedEvents.Add(new
            {
                id = fbevent.eid,
                title = fbevent.name,
                url = EventUri + fbevent.eid,
                @class = "event-info",
                start = UnixTime(DateTime.Parse(fbevent.start_time)),
                end = UnixTime(DateTime.Parse(fbevent.start_time))
            }));
            return convertedEvents;
        }

        //TODO: Put this in a helper method
        /// <summary>
        /// Return the Unix time in milliseconds
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private static long UnixTime(DateTime dateTime)
        {
            var timeSpan = (dateTime - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalMilliseconds;
        }

        public static DateTime UnixTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}