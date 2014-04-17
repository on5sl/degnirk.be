using System;
using System.Collections.Generic;

using DTO;

namespace Services.Facebook
{
    public interface IFacebookService
    {
        IEnumerable<CalendarItem> GetLatestFacebookEvents(long creatorId, short numberOfEvents);

        IEnumerable<CalendarItem> GetFacebookEvents(long creatorId, DateTime @from, DateTime to);

        IEnumerable<PictureAlbum> GetFacebookAlbums(long creatorId);

        dynamic GetCurrentUser();
    }
}