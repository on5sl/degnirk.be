using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using DTO;

namespace degnirk.be.Models
{

    public class EventsRollupModel
    {
        public IEnumerable<CalendarItem> Events { get; set; }
        public string FacebookAppAccessToken { get; set; }
        [Range(0, long.MaxValue)]
        public long FacebookPageId { get; set; }
        [Range(0, 2)]
        public short NumberOfEvents { get; set; }
    }
}