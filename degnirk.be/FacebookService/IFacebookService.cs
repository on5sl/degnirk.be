using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Service
{
    public interface IFacebookService
    {
        IEnumerable<ExpandoObject> GetLatestFacebookEvents(short numberOfEvents);

        IEnumerable<dynamic> GetFacebookEvents(DateTime from, DateTime to);

        IEnumerable<ExpandoObject> GetFacebookAlbums();

        dynamic GetCurrentUser();
    }
}