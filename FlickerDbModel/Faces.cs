using System;
using System.Collections.Generic;

namespace FlickerDbModel
{
    public partial class Faces
    {
        public int Id { get; set; }
        public double? Sadness { get; set; }
        public double? Neutral { get; set; }
        public double? Disgust { get; set; }
        public double? Anger { get; set; }
        public double? Surprise { get; set; }
        public double? Fear { get; set; }
        public double? Happiness { get; set; }
        public int? PhotoId { get; set; }

        public virtual Photo Photo { get; set; }
    }
}
