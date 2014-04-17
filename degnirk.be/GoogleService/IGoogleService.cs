using System;
using System.Collections.Generic;
using DTO;

namespace Service.Google
{
    public interface IGoogleService
    {
        IEnumerable<CalendarItem> GetEvents(DateTime @from, DateTime to);
    }
}