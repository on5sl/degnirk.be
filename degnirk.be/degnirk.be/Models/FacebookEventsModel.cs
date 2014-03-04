using System.Configuration;
using Service;

namespace degnirk.be.Models
{

    public class FacebookEventsModel
    {
        private const int NumberOfEvents = 3;
        public dynamic FacebookEvents { get; private set; }

        public FacebookEventsModel()
        {
            var facebookService = new FacebookService(
                ConfigurationManager.AppSettings["FacebookAppAccessToken"]);
            this.FacebookEvents = facebookService.GetLatestFacebookEvents(
                ConfigurationManager.AppSettings["FacebookPageId"], 
                NumberOfEvents);
        }
    }
}