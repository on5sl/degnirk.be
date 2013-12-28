using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using degnirk.be.Helpers;
using Facebook;

namespace degnirk.be.Models
{

    public class FacebookEventsModel
    {
        private const string AccessToken = "442171809217325|Q3rA6b68G7TuWYF1beRUNsLUe94";
        private const short NumberOfEvents = 3;
        private const string CreatorId = "56615007038";
        private const string EventUri = "https://www.facebook.com/events/";
        private readonly FacebookClient _facebookClient;
        public dynamic FacebookEvents { get; private set; }

        public FacebookEventsModel()
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
            this.FacebookEvents = (facebookEvents as IEnumerable<dynamic>).Select(fbevent => new 
            {
                name = fbevent.name,
                link = EventUri + fbevent.eid,
                attendingCount = fbevent.attending_count,
                picCover = fbevent.pic_cover.source,
                startTime = fbevent.start_time
            }.ToExpando());
        }
    }
}