using System;
using System.Configuration;
using System.Web.Mvc;
using degnirk.be.Models;
using Service;
using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class FacebookEventsController : SurfaceController
    {
        private const int NumberOfEvents = 3;
        private FacebookService _facebookService;
        
        [ChildActionOnly]
        public PartialViewResult GetFacebookEvents()
        {
            var facebookService = new FacebookService(
                ConfigurationManager.AppSettings["FacebookAppAccessToken"]);
            var model = new FacebookEventsModel();
            model.FacebookEvents = facebookService.GetLatestFacebookEvents(
                ConfigurationManager.AppSettings["FacebookPageId"],
                NumberOfEvents);
            return PartialView("FacebookEvents", model);
        }
    }
}