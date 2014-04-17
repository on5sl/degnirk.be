﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using DTO;

using Facebook;
using Helpers;

namespace Service
{
    public class FacebookService : IFacebookService
    {
        private const string EventUri = "https://www.facebook.com/events/";
        private readonly FacebookClient _facebookClient;

        public FacebookService(string accessToken)
        {
            _facebookClient = new FacebookClient(accessToken);
        }

        public IEnumerable<CalendarItem> GetLatestFacebookEvents(long creatorId, short numberOfEvents)
        {
            dynamic facebookEvents = ((JsonArray)(_facebookClient.Get("/fql",
                new
                {
                    q = string.Format("select eid,  name, attending_count, pic_cover, start_time from event where creator = {0} ORDER BY start_time desc limit {1}", creatorId, numberOfEvents)
                }) as dynamic).data);

            return facebookEvents == null ? null : ConvertEventsToDto(facebookEvents);
        }

        public IEnumerable<CalendarItem> GetFacebookEvents(long creatorId, DateTime @from, DateTime to)
        {

            dynamic facebookEvents = ((JsonArray)(_facebookClient.Get("/fql",
                new
                {
                    q = string.Format("select eid,  name, attending_count, pic_cover, start_time from event " +
                                      "where creator = {0} " +
                                      "AND start_time >= '{1}' " +
                                      "AND start_time <= '{2}' " +
                                      "ORDER BY start_time desc", creatorId, from.ToString("s"), to.ToString("s"))
                }) as dynamic).data);

            return ConvertEventsToCalendarDto(facebookEvents);
        }

        private static IEnumerable<CalendarItem> ConvertEventsToCalendarDto(dynamic events)
        {
            if(events == null) yield break;
            var enumerable = events as IEnumerable<dynamic>;
            if(enumerable == null) yield break;
            foreach (var fbevent in enumerable)
            {
                yield return new CalendarItem()
                {
                    Id = fbevent.eid,
                    Title = fbevent.name,
                    Url = EventUri + fbevent.eid,
                    Class = "event-info",
                    Start = DateTime.Parse(fbevent.start_time),
                    End = DateTime.Parse(fbevent.start_time)
                };
            }
        }

        private static IEnumerable<CalendarItem> ConvertEventsToDto(dynamic facebookEvents)
        {
            if (facebookEvents == null)
                yield break;
            var enumerable = facebookEvents as IEnumerable<dynamic>;
            if (enumerable == null)
                yield break;
            foreach (var fbevent in enumerable)
            {
                yield return new CalendarItem()
                {
                    Title = fbevent.name,
                    Url = EventUri + fbevent.eid,
                    AttendingCount = fbevent.attending_count,
                    CoverPicture = fbevent.pic_cover.source,
                    Start = DateTime.Parse(fbevent.start_time)
                };
            }
        }

        public IEnumerable<ExpandoObject> GetFacebookAlbums(long creatorId)
        {
            dynamic facebookAlbums = _facebookClient.Get("/fql",
                new
                {
                    q = new
                    {
                        coverPids = string.Format("select name, link,aid, cover_pid from album where owner = {0} AND photo_count > 0 ORDER BY created desc", creatorId),
                        coverSrcs = "select src, src_big,aid from photo where pid in (select cover_pid from #coverPids)"
                    }
                });
            var coverPids = ((JsonArray)facebookAlbums.data).Cast<dynamic>().FirstOrDefault(i => i.name == "coverPids");
            var coverSrcs = ((JsonArray)facebookAlbums.data).Cast<dynamic>().FirstOrDefault(i => i.name == "coverSrcs");

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
