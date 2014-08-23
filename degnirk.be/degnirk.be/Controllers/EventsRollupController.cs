using System.Web.Mvc;
using degnirk.be.Models;

using Services.Facebook;

using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class EventsRollupController : SurfaceController
    {
        //[OutputCache(Duration = 3600, VaryByParam = "facebookAppAccessToken;facebookPageId;numberOfEvents")]
        [ChildActionOnly]
        public PartialViewResult GetEvents(EventsRollupModel model)
        {
            if (!ModelState.IsValid)
            {
                
            }
            var facebookService = new FacebookService(model.FacebookAppAccessToken, model.FacebookPageId);
            model.Events = facebookService.GetLatestFacebookEvents(model.NumberOfEvents);
            return PartialView("EventsRollup", model);
        }
    }
}