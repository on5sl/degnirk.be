using System.Linq;
using System.Web.Mvc;
using degnirk.be.Models;
using Service;
using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class PictureAlbumsController : SurfaceController
    {
        [ChildActionOnly]
        public PartialViewResult GetFacebookAlbums(string facebookAppAccessToken, long facebookPageId)
        {
            var facebookService = new FacebookService(facebookAppAccessToken);
            var model = new PictureAlbumsModel
            {
                Albums = facebookService.GetFacebookAlbums(facebookPageId).ToList()
            };
            return PartialView("PictureAlbums", model);
        }
    }
}