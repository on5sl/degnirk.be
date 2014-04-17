using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PictureAlbum
    {
        public string Name { get; set; }
        public string SourceUrl { get; set; }
        public string Id { get; set; }
        public string CoverPictureId { get; set; }
        public string CoverPictureThumbUrl { get; set; }
        public string LargeCoverPictureUrl { get; set; }
    }
}
