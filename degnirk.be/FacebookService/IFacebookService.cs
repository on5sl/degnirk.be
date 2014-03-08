using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Service
{
    public interface IFacebookService
    {
        IEnumerable<ExpandoObject> GetLatestFacebookEvents(long creatorId, short numberOfEvents);

        IEnumerable<dynamic> GetFacebookEvents(string creatorId, DateTime from, DateTime to);

        IEnumerable<ExpandoObject> GetFacebookAlbums(string creatorId);

        dynamic GetCurrentUser();
    }
}