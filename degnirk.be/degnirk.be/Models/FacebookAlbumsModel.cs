using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using degnirk.be.Helpers;
using Facebook;

namespace degnirk.be.Models
{
    public class FacebookAlbumsModel
    {
        private const string AccessToken = "442171809217325|Q3rA6b68G7TuWYF1beRUNsLUe94";
        private const string OwnerID = "56615007038";
        private readonly FacebookClient _facebookClient;
        public dynamic FacebookAlbums { get; private set; }

        public FacebookAlbumsModel()
        {
            _facebookClient = new FacebookClient(AccessToken);
            this.GetFacebookAlbums();
        }

        private void GetFacebookAlbums()
        {
            dynamic facebookAlbums = _facebookClient.Get("/fql",
                new
                {
                    q = new
                    {
                        coverPids = string.Format("select name, link,aid, cover_pid from album where owner = {0} AND photo_count > 0 ORDER BY created desc", OwnerID),
                        coverSrcs = "select src, src_big,aid from photo where pid in (select cover_pid from #coverPids)"
                    }
                });
            var coverPids = ((Facebook.JsonArray)facebookAlbums.data).Cast<dynamic>().FirstOrDefault(i => i.name == "coverPids");
            var coverSrcs = ((Facebook.JsonArray)facebookAlbums.data).Cast<dynamic>().FirstOrDefault(i => i.name == "coverSrcs");

            if (coverPids == null || coverSrcs == null || coverPids.fql_result_set == null || coverSrcs.fql_result_set == null)
            {
                return;
            }
            var coverSrcsFqlResultSet = coverSrcs.fql_result_set as IEnumerable<dynamic>;
            var coverPidsFqlResultSet = coverPids.fql_result_set as IEnumerable<dynamic>;
            this.FacebookAlbums = coverPidsFqlResultSet.Join(coverSrcsFqlResultSet, album => album.aid, photo => photo.aid, (album, photo) => new
            {
                name = album.name,
                link = album.link,
                aid = album.aid,
                coverPid = album.cover_pid,
                src = photo.src,
                srcBig = photo.src_big
            }.ToExpando());
        }
    }
}