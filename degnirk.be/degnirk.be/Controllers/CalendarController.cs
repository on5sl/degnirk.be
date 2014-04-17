using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using degnirk.be.Models;

using DTO;

using Helpers;
using Service;
using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class CalendarController : SurfaceController
    {
        private readonly GoogleService _googleService;
        private readonly FacebookService _facebookService;

        public CalendarController()
        {
            var googleServiceSettings = new GoogleServiceSettings(
                ConfigurationManager.AppSettings["GoogleClientIDforNativeApplication"],
                ConfigurationManager.AppSettings["GoogleClientSecret"],
                ConfigurationManager.AppSettings["GoogleEmail"],
                ConfigurationManager.AppSettings["GoogleApplicationName"]);
            this._googleService = new GoogleService(googleServiceSettings);

            this._facebookService = new FacebookService(
                ConfigurationManager.AppSettings["FacebookAppAccessToken"]);
        }

        //[OutputCache(Duration = 3600, VaryByParam = "from;to;browser_timezone")]
        public ActionResult GetEvents(long from, long to, string browser_timezone)
        {
            var dateTimeFrom = UnixTimeHelper.UnixTime(from);
            var dateTimeTo = UnixTimeHelper.UnixTime(to);
            var events = GetFacebookEvents(dateTimeFrom, dateTimeTo);
            events.AddRange(
                this._googleService.GetEvents(dateTimeFrom, dateTimeTo));
            
            dynamic result = new
            {
                success = 1,
                result = events
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private List<dynamic> GetFacebookEvents(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            return this._facebookService.GetFacebookEvents(
                long.Parse(ConfigurationManager.AppSettings["FacebookPageId"]),
                dateTimeFrom,
                dateTimeTo)
                .Select(c => new
                {
                    @class = c.Class,
                    end = UnixTimeHelper.UnixTime(c.End),
                    id = c.Id,
                    start = UnixTimeHelper.UnixTime(c.Start),
                    title = c.Title,
                    url = c.Url
                }).Cast<dynamic>().ToList();
        }
    }
}