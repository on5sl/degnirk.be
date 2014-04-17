using System;

namespace DTO
{
    public class CalendarItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Class { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double AttendingCount { get; set; }
        public string CoverPicture { get; set; }
    }
}
