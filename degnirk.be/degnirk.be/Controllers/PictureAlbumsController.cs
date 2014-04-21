using System.Linq;
using System.Web.Mvc;
using degnirk.be.Models;

using Services.Facebook;

using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class PictureAlbumsController : SurfaceController
    {
        [ChildActionOnly]
        public PartialViewResult GetFacebookAlbums(string facebookAppAccessToken, long facebookPageId)
        {
            var facebookService = new FacebookService(facebookAppAccessToken, facebookPageId);
            var model = new PictureAlbumsModel
            {
                Albums = facebookService.GetFacebookAlbums(facebookPageId).ToList()
            };
            return PartialView("PictureAlbums", model);
        }
    }
}