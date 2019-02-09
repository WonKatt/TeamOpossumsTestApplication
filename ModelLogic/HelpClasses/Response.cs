using System.Collections.Generic;
using FlickerDbModel;

namespace ModelLogic.HelpClasses
{
    public class Response
    {
        public string Image_id { get; set; }
        public string Request_id { get; set; }
        public int Time_used { get; set; }
        public string Eror_message { get; set; }
        public List<FaceObjectFromResponse> Faces { get; set; }
    }
}