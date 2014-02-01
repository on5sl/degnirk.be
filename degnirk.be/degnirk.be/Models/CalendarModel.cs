using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using Facebook;
using Umbraco.Core;

namespace degnirk.be.Models
{
    public class CalendarModel
    {
        private const string AccessToken = "442171809217325|Q3rA6b68G7TuWYF1beRUNsLUe94";
        private const short NumberOfEvents = 3;
        private const string CreatorId = "56615007038";
        private const string EventUri = "https://www.facebook.com/events/";
        private readonly FacebookClient _facebookClient;
        public JsonArray FacebookEvents { get; private set; }
        public CalendarModel()
        {
            _facebookClient = new FacebookClient(AccessToken);
            this.GetFacebookEvents();
        }

        private void GetFacebookEvents()
        {
            dynamic facebookEvents = ((Facebook.JsonArray)(_facebookClient.Get("/fql",
                new
                {
                    q = string.Format("select eid,  name, attending_count, pic_cover, start_time from event where creator = {0} ORDER BY start_time desc limit {1}", CreatorId, NumberOfEvents)
                }) as dynamic).data);

            if (facebookEvents == null)
            {
                return;
            }
            this.FacebookEvents = new JsonArray();
            (facebookEvents as IEnumerable<dynamic>).OrderBy(fbevent => fbevent.start_time)
                .ForEach(fbevent => this.FacebookEvents.Add(new JsonObject()
                {
                    {"id", fbevent.eid},
                    {"title", fbevent.name},
                    {"url", EventUri + fbevent.eid},
                    {"class", "event-info"},
                    {"start", UnixTime(DateTime.Parse(fbevent.start_time))},
                    {"end", UnixTime(DateTime.Parse(fbevent.start_time))}
                }));
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
    }
}