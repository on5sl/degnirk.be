using System;
using System.Linq;
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
        // See https://console.developers.google.com fore more info
        private const string ClientIDforNativeApplication = "539942760355-ndbtaf1pi3iih0o31m6sh47vh16gr8h4.apps.googleusercontent.com";
        private const string ClientSecret = "L4XjviYSsciNU1TpvExREotB";
        private const string Email = "jhdegnirk@gmail.com";

        // See https://console.developers.google.com/project for this constant
        private const string ApplicationName = "test1";

        private readonly List<List<KeyValuePair<string, string>>> _events = new List<List<KeyValuePair<string, string>>>();
        //private Events _events = new Events();
        public IEnumerable<dynamic> GetEvents(DateTime @from, DateTime to)
        {
            var task = GetGoogleEvents(from, to);
            task.Wait();
            return ConvertEvents(_events);

        }
        private async Task GetGoogleEvents(DateTime from, DateTime to)
        {
            UserCredential userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = ClientIDforNativeApplication,
                    ClientSecret = ClientSecret
                },
                new[] { CalendarService.Scope.CalendarReadonly },
                "jhdegnirk%40gmail.com",
                CancellationToken.None);

            CalendarService calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = userCredential,
                ApplicationName = ApplicationName
            });

            var query = calendarService.Events.List(Email);
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
        }

        private static IEnumerable<dynamic> ConvertEvents(List<List<KeyValuePair<string, string>>> events)
        {
            var convertedEvents = new List<dynamic>();
            events.ForEach(googleEvent => convertedEvents.Add(new
            {
                id = googleEvent.Single(i => i.Key == "id").Value,
                title = googleEvent.Single(i => i.Key == "title").Value,
                url = googleEvent.Single(i => i.Key == "url").Value,
                @class = googleEvent.Single(i => i.Key == "class").Value,
                start = googleEvent.Single(i => i.Key == "start").Value,
                end = googleEvent.Single(i => i.Key == "end").Value
            }));
            return convertedEvents;
        }
    }
}
