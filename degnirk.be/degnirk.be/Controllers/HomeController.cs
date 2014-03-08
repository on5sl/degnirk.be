using System.Web.Mvc;
using degnirk.be.Models;
using umbraco;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using System.Linq;

namespace degnirk.be.Controllers
{
    public class HomeController : SurfaceController
    {
        [OutputCache(Duration = 3600, VaryByParam = "mediaPicker")]
        [ChildActionOnly]
        public PartialViewResult GetMediaPickerImages(IPublishedProperty mediaPicker)
        {
            var mediaId = (int)mediaPicker.Value;
            var umbracoHelper = new UmbracoHelper(this.UmbracoContext);
            var media = umbracoHelper.TypedMedia(mediaId);
            var images = media.Children.Select(i => "'.." + i.Url + "'").RandomOrder().ToArray();
            return PartialView("MediaPickerImages", new HomeModel(){Images = images});
        }
    }
}