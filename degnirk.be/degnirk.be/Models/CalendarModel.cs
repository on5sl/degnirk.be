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
                    {"url", "hyperlink"},
                    {"class", "event-important"},
                    {"start", 1362938400000},
                    {"end", 1362938400000}
                }));
        }
    }
}