using System;

namespace Helpers
{
    public class UnixTimeHelper
    {
        /// <summary>
        /// Return the Unix time in milliseconds
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long UnixTime(DateTime dateTime)
        {
            var timeSpan = (dateTime - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalMilliseconds;
        }

        public static DateTime UnixTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        } 
    }
}