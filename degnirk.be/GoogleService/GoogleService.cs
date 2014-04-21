using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Http;
using Google.Apis.Services;
using DTO;

namespace Services.Google
{
    public class GoogleService : IGoogleService
    {
        // See calendar.css for more styles available
        private const string EventWarning = "event-warning";

        public readonly IGoogleServiceSettings GoogleServiceSettings;

        public GoogleService(IGoogleServiceSettings googleServiceSettings)
        {
            this.GoogleServiceSettings = googleServiceSettings;
        }

        public IEnumerable<CalendarItem> GetEvents(DateTime from, DateTime to)
        {
            return GetEventsTask(from, to).Result;
        }

        public async Task<IEnumerable<CalendarItem>> GetEventsTask(DateTime from, DateTime to)
        {
            var userCredential = await GetUserCredential();
            var calendarService = GetCalendarService(userCredential);

            var query = calendarService.Events.List(this.GoogleServiceSettings.Email);
            query.TimeMin = from;
            query.TimeMax = to;

            var result = query.Execute();

            return ConvertToCalendarDto(result.Items);
        }

        private CalendarService GetCalendarService(IConfigurableHttpClientInitializer userCredential)
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

        private static IEnumerable<CalendarItem> ConvertToCalendarDto(IEnumerable<Event> events)
        {
            return events.Select(@event => new CalendarItem()
            {
                Title = @event.Summary,
                Url = string.Empty,
                Class = EventWarning,
                Start = @event.Start.DateTime.HasValue
                    ? @event.Start.DateTime.Value
                    : DateTime.Parse(@event.Start.Date),
                End = @event.End.DateTime.HasValue
                    ? @event.End.DateTime.Value
                    : DateTime.Parse(@event.End.Date)
            });
        }
    }
}
