using System.Web.Mvc;
using degnirk.be.Models;
using Service;
using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class EventsRollupController : SurfaceController
    {
        [OutputCache(Duration = 3600, VaryByParam = "facebookAppAccessToken;facebookPageId;numberOfEvents")]
        [ChildActionOnly]
        public PartialViewResult GetEvents(string facebookAppAccessToken, long facebookPageId, short numberOfEvents)
        {
            var facebookService = new FacebookService(facebookAppAccessToken);
            var model = new EventsRollupModel
            {
                Events = facebookService.GetLatestFacebookEvents(facebookPageId, numberOfEvents)
            };
            return PartialView("EventsRollup", model);
        }
    }
}