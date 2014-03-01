using System;
using System.Collections.Generic;

namespace Service
{
    public interface IGoogleService
    {
        IEnumerable<dynamic> GetEvents(DateTime @from, DateTime to);
    }
}