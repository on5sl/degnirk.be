using System.Web.Mvc;
using degnirk.be.Models;
using Service;
using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class FacebookEventsController : SurfaceController
    {   
        [ChildActionOnly]
        public PartialViewResult GetFacebookEvents(string facebookAppAccessToken, long facebookPageId, short numberOfEvents)
        {
            var facebookService = new FacebookService(facebookAppAccessToken);
            var model = new FacebookEventsModel
            {
                FacebookEvents = facebookService.GetLatestFacebookEvents(facebookPageId, numberOfEvents)
            };
            return PartialView("FacebookEvents", model);
        }
    }
}