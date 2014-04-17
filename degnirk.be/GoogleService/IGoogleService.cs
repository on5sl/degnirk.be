using System;
using System.Collections.Generic;
using DTO;

namespace Service
{
    public interface IGoogleService
    {
        IEnumerable<CalendarItem> GetEvents(DateTime @from, DateTime to);
    }
}