using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Helpers;

namespace Service
{
    public class GoogleService : IGoogleService
    {
        // See calendar.css for more styles available
        private const string EventWarning = "event-warning";

        public readonly GoogleServiceSettings GoogleServiceSettings;
        private readonly List<dynamic> _events;

        public GoogleService(GoogleServiceSettings googleServiceSettings)
        {
            _events = new List<dynamic>();
            this.GoogleServiceSettings = googleServiceSettings;
        }

        public IEnumerable<dynamic> GetEvents(DateTime from, DateTime to)
        {
            var task = GetGoogleEvents(from, to);
            task.Wait();
            return _events;

        }
        private async Task GetGoogleEvents(DateTime from, DateTime to)
        {
            var userCredential = await GetUserCredential();
            var calendarService = GetCalendarService(userCredential);

            var query = calendarService.Events.List(this.GoogleServiceSettings.Email);
            query.TimeMin = from;
            query.TimeMax = to;

            var result = query.Execute();

            ConvertToCalendarDto(result.Items);
        }

        private CalendarService GetCalendarService(UserCredential userCredential)
        {
            return new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = userCredential,
                ApplicationName = this.GoogleServiceSettings.ApplicationName
            });
        }

        private async Task<UserCredential> GetUserCredential()
        {
            return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = this.GoogleServiceSettings.ClientIDforNativeApplication,
                    ClientSecret = this.GoogleServiceSettings.ClientSecret
                },
                new[] {CalendarService.Scope.CalendarReadonly},
                HttpUtility.UrlEncode(this.GoogleServiceSettings.Email),
                CancellationToken.None);
        }

        private void ConvertToCalendarDto(IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                _events.Add(new
                {
                    id = @event.Id,
                    title = @event.Summary,
                    url = string.Empty,
                    @class = EventWarning,
                    start = @event.Start.DateTime.HasValue
                        ? UnixTimeHelper.UnixTime(@event.Start.DateTime.Value)
                        : UnixTimeHelper.UnixTime(DateTime.Parse(@event.Start.Date)),
                    end = @event.End.DateTime.HasValue
                        ? UnixTimeHelper.UnixTime(@event.End.DateTime.Value)
                        : UnixTimeHelper.UnixTime(DateTime.Parse(@event.End.Date))
                });
            }
        }
    }
}
