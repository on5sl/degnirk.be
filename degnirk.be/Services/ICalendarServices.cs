using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DTO;

namespace Services
{
    public interface ICalendarServices
    {
        IEnumerable<CalendarItem> GetEvents(DateTime @from, DateTime to);
        Task<IEnumerable<CalendarItem>> GetEventsTask(DateTime @from, DateTime to);
    }
}
