using System.Collections.Generic;

using DTO;

namespace degnirk.be.Models
{

    public class EventsRollupModel
    {
        public IEnumerable<CalendarItem> Events { get; set; }
    }
}