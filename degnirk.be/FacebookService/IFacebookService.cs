using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Service
{
    public interface IFacebookService
    {
        IEnumerable<ExpandoObject> GetLatestFacebookEvents(long creatorId, short numberOfEvents);

        IEnumerable<dynamic> GetFacebookEvents(long creatorId, DateTime @from, DateTime to);

        IEnumerable<ExpandoObject> GetFacebookAlbums(long creatorId);

        dynamic GetCurrentUser();
    }
}