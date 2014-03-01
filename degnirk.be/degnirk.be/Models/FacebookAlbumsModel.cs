using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Service;

namespace degnirk.be.Models
{
    public class FacebookAlbumsModel
    {
        public dynamic FacebookAlbums { get; private set; }

        public FacebookAlbumsModel()
        {
            var facebookService = new FacebookService();
            this.FacebookAlbums = facebookService.GetFacebookAlbums();
        }
    }
}