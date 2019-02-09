using System;
using System.Collections.Generic;

namespace FlickerDbModel
{
    public partial class Photo
    {
        public int Id { get; set; }
        public string FlickrId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool? IsFace { get; set; }
        public virtual Faces Faces { get; set; }
    }
}
