using System;
using System.Collections.Generic;

using DTO;

namespace Services.Facebook
{
    public interface IFacebookService
    {
        IEnumerable<CalendarItem> GetLatestFacebookEvents(short numberOfEvents);

        IEnumerable<CalendarItem> GetFacebookEvents(DateTime @from, DateTime to);

        IEnumerable<PictureAlbum> GetFacebookAlbums(long creatorId);

        dynamic GetCurrentUser();
    }
}