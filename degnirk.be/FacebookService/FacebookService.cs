﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DTO;
using Facebook;

namespace Services.Facebook
{
    public class FacebookService : IFacebookService
    {
        private const string EventUri = "https://www.facebook.com/events/";
        private const string EventInfo = "event-info";
        private readonly FacebookClient _facebookClient;
        private readonly long _creatorId;

        public FacebookService(string accessToken, long creatorId)
        {
            _facebookClient = new FacebookClient(accessToken);
            _creatorId = creatorId;
        }

        public IEnumerable<CalendarItem> GetLatestFacebookEvents(short numberOfEvents)
        {
            dynamic facebookEvents = ((JsonArray)(_facebookClient.Get("/fql",
                new
                {
                    q = string.Format("select eid,  name, attending_count, pic_cover, start_time from event where creator = {0} ORDER BY start_time desc limit {1}", _creatorId, numberOfEvents)
                }) as dynamic).data);

            return facebookEvents == null ? null : ConvertToCalendarItems(facebookEvents);
        }

        public IEnumerable<CalendarItem> GetEvents(DateTime @from, DateTime to)
        {

            dynamic facebookEvents = ((JsonArray)(_facebookClient.Get("/fql",
                new
                {
                    q = string.Format("select eid,  name, attending_count, pic_cover, start_time from event " +
                                      "where creator = {0} " +
                                      "AND start_time >= '{1}' " +
                                      "AND start_time <= '{2}' " +
                                      "ORDER BY start_time desc", _creatorId, from.ToString("s"), to.ToString("s"))
                }) as dynamic).data);

            return ConvertToCalendarItems(facebookEvents);
        }

        private static IEnumerable<CalendarItem> ConvertToCalendarItems(dynamic events)
        {
            if(events == null) yield break;
            var enumerable = events as IEnumerable<dynamic>;
            if(enumerable == null) yield break;
            foreach (var fbevent in enumerable)
            {
                yield return new CalendarItem()
                {
                    Title = fbevent.name,
                    Url = EventUri + fbevent.eid,
                    Class = EventInfo,
                    AttendingCount = fbevent.attending_count,
                    CoverPicture = fbevent.pic_cover.source,
                    Start = DateTime.Parse(fbevent.start_time),
                    End = DateTime.Parse(fbevent.start_time)
                };
            }
        }

        public IEnumerable<PictureAlbum> GetFacebookAlbums(long creatorId)
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

        public DeGnirkMember GetCurrentUser()
        {
            dynamic user = _facebookClient.Get("me", new { fields = "name, id, email, birthday, location, link" });
            return new DeGnirkMember()
            {
                Email = user.email,
                FacebookId = long.Parse(user.id),
                FacebookLink = user.link,
                Name = user.name,
                DateOfBirth = DateTime.ParseExact(user.birthday, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None),
                Location = user.location["name"]
            };
        }

        private static IEnumerable<PictureAlbum> ConvertAlbumsToDto(IEnumerable<dynamic> coverPidsFqlResultSet, IEnumerable<dynamic> coverSrcsFqlResultSet)
        {
            return coverPidsFqlResultSet.Join(coverSrcsFqlResultSet, album => album.aid, photo => photo.aid, (album, photo) => new PictureAlbum()
            {
                Id = album.aid,
                Name = album.name,
                SourceUrl = album.link,
                CoverPictureId = album.cover_pid,
                CoverPictureThumbUrl = photo.src,
                LargeCoverPictureUrl = photo.src_big
            });
        }
    }
}
