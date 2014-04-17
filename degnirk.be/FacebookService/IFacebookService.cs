using System;
using System.Collections.Generic;
using System.Dynamic;

using DTO;

namespace Service
{
    public interface IFacebookService
    {
        IEnumerable<CalendarItem> GetLatestFacebookEvents(long creatorId, short numberOfEvents);

        IEnumerable<CalendarItem> GetFacebookEvents(long creatorId, DateTime @from, DateTime to);

        IEnumerable<ExpandoObject> GetFacebookAlbums(long creatorId);

        dynamic GetCurrentUser();
    }
}