using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Facebook;
using Helpers;

namespace Service
{
    public class FacebookService : IFacebookService
    {
        private const string AccessToken = "442171809217325|Q3rA6b68G7TuWYF1beRUNsLUe94";
        private const short NumberOfEvents = 3;
        private const string CreatorId = "56615007038";
        private const string EventUri = "https://www.facebook.com/events/";
        private readonly FacebookClient _facebookClient;

        public FacebookService(string accessToken = AccessToken)
        {
            _facebookClient = new FacebookClient(accessToken);
        }

        public IEnumerable<ExpandoObject> GetLatestFacebookEvents(short numberOfEvents)
        {
            dynamic facebookEvents = ((Facebook.JsonArray)(_facebookClient.Get("/fql",
                new
                {
                    q = string.Format("select eid,  name, attending_count, pic_cover, start_time from event where creator = {0} ORDER BY start_time desc limit {1}", CreatorId, numberOfEvents)
                }) as dynamic).data);

            return facebookEvents == null ? null : ConvertEventsToDto(facebookEvents);
        }

        public IEnumerable<dynamic> GetFacebookEvents(DateTime @from, DateTime to)
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

            return ConvertEventsToCalendarDto(facebookEvents);
        }

        private static IEnumerable<dynamic> ConvertEventsToCalendarDto(dynamic events)
        {
            if(events == null) yield break;
            var enumerable = events as IEnumerable<dynamic>;
            if(enumerable == null) yield break;
            foreach (var fbevent in enumerable)
                yield return new
                {
                    id = fbevent.eid,
                    title = fbevent.name,
                    url = EventUri + fbevent.eid,
                    @class = "event-info",
                    start = UnixTimeHelper.UnixTime(DateTime.Parse(fbevent.start_time)),
                    end = UnixTimeHelper.UnixTime(DateTime.Parse(fbevent.start_time))
                };
        }

        private static IEnumerable<ExpandoObject> ConvertEventsToDto(dynamic facebookEvents)
        {
            if (facebookEvents == null) yield break;
            var enumerable = facebookEvents as IEnumerable<dynamic>;
            if (enumerable == null) yield break;
            foreach (dynamic fbevent in enumerable)
                yield return new
                {
                    name = fbevent.name, 
                    link = EventUri + fbevent.eid, 
                    attendingCount = fbevent.attending_count, 
                    picCover = fbevent.pic_cover.source, 
                    startTime = fbevent.start_time
                }.ToExpando();
        }

        public IEnumerable<ExpandoObject> GetFacebookAlbums()
        {
            dynamic facebookAlbums = _facebookClient.Get("/fql",
                new
                {
                    q = new
                    {
                        coverPids = string.Format("select name, link,aid, cover_pid from album where owner = {0} AND photo_count > 0 ORDER BY created desc", CreatorId),
                        coverSrcs = "select src, src_big,aid from photo where pid in (select cover_pid from #coverPids)"
                    }
                });
            var coverPids = ((Facebook.JsonArray)facebookAlbums.data).Cast<dynamic>().FirstOrDefault(i => i.name == "coverPids");
            var coverSrcs = ((Facebook.JsonArray)facebookAlbums.data).Cast<dynamic>().FirstOrDefault(i => i.name == "coverSrcs");

            if (coverPids == null || coverSrcs == null || coverPids.fql_result_set == null || coverSrcs.fql_result_set == null)
            {
                return null;
            }
            var coverSrcsFqlResultSet = coverSrcs.fql_result_set as IEnumerable<dynamic>;
            var coverPidsFqlResultSet = coverPids.fql_result_set as IEnumerable<dynamic>;
            return ConvertAlbumsToDto(coverPidsFqlResultSet, coverSrcsFqlResultSet);
        }

        public dynamic GetCurrentUser()
        {
            return _facebookClient.Get("me", new { fields = "name, id, email, birthday, location, link" });
        }


        private static IEnumerable<ExpandoObject> ConvertAlbumsToDto(IEnumerable<dynamic> coverPidsFqlResultSet, IEnumerable<dynamic> coverSrcsFqlResultSet)
        {
            return coverPidsFqlResultSet.Join(coverSrcsFqlResultSet, album => album.aid, photo => photo.aid, (album, photo) => new
            {
                name = album.name,
                link = album.link,
                aid = album.aid,
                coverPid = album.cover_pid,
                src = photo.src,
                srcBig = photo.src_big
            }.ToExpando());
        }
    }
}
