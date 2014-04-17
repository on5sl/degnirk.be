using System.Linq;
using System.Web.Mvc;
using degnirk.be.Models;
using Service;
using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class FacebookAlbumsController : SurfaceController
    {
        [ChildActionOnly]
        public PartialViewResult GetFacebookAlbums(string facebookAppAccessToken, long facebookPageId)
        {
            var facebookService = new FacebookService(facebookAppAccessToken);
            var model = new FacebookAlbumsModel
            {
                Albums = facebookService.GetFacebookAlbums(facebookPageId).ToList()
            };
            return PartialView("FacebookAlbums", model);
        }
    }
}