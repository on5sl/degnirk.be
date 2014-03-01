using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Helpers;

namespace Service
{
    public class GoogleService : IGoogleService
    {
        private List<List<KeyValuePair<string, string>>> _events = new List<List<KeyValuePair<string, string>>>();
        //private Events _events = new Events();
        public List<List<KeyValuePair<string, string>>> GetEvents(DateTime from, DateTime to)
        {
            var task = GetGoogleEvents(from, to);
            task.Wait();
            return _events;

        }
        private async Task GetGoogleEvents(DateTime from, DateTime to)
        {
            UserCredential userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "539942760355-ndbtaf1pi3iih0o31m6sh47vh16gr8h4.apps.googleusercontent.com",
                    ClientSecret = "L4XjviYSsciNU1TpvExREotB"
                },
                new[] { CalendarService.Scope.CalendarReadonly },
                "jhdegnirk%40gmail.com",
                CancellationToken.None);

            CalendarService calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = userCredential,
                ApplicationName = "test1"
            });

            //var test = calendarService.CalendarList.List().Execute().Items.Where(i => i.Id == "jhdegnirk@gmail.com");

            var query = calendarService.Events.List("jhdegnirk@gmail.com");
            query.TimeMin = from;
            query.TimeMax = to;
            var result = query.Execute();

            foreach (var item in result.Items)
            {
                _events.Add(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("id", item.Id),
                    new KeyValuePair<string, string>("title", item.Summary),
                    new KeyValuePair<string, string>("url", string.Empty),
                    new KeyValuePair<string, string>("class", "event-warning"),
                    new KeyValuePair<string, string>("start", (item.Start.DateTime.HasValue 
                        ? UnixTimeHelper.UnixTime(item.Start.DateTime.Value) 
                        : UnixTimeHelper.UnixTime(DateTime.Parse(item.Start.Date))).ToString()),
                    new KeyValuePair<string, string>("end", (item.End.DateTime.HasValue 
                        ? UnixTimeHelper.UnixTime(item.End.DateTime.Value) 
                        : UnixTimeHelper.UnixTime(DateTime.Parse(item.End.Date))).ToString())
                });
            }

            //result.Items.ForEach(googleEvent => _events.Add(new
            //{
            //    id = googleEvent.Id,
            //    title = googleEvent.Summary,
            //    url = "",
            //    @class = "event-warning",
            //    start =
            //        googleEvent.Start.DateTime.HasValue
            //            ? UnixTime(googleEvent.Start.DateTime.Value)
            //            : UnixTime(DateTime.Parse(googleEvent.Start.Date)),
            //    end =
            //        googleEvent.End.DateTime.HasValue
            //            ? UnixTime(googleEvent.End.DateTime.Value)
            //            : UnixTime(DateTime.Parse(googleEvent.End.Date))
            //}));

        }
    }
}
