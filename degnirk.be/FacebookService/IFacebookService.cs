using System;
using System.Collections.Generic;

using DTO;

namespace Services.Facebook
{
    public interface IFacebookService : ICalendarServices
    {
        IEnumerable<CalendarItem> GetLatestFacebookEvents(short numberOfEvents);

        IEnumerable<PictureAlbum> GetFacebookAlbums(long creatorId);

        dynamic GetCurrentUser();
    }
}