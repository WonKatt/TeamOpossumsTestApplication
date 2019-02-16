using System.Collections.Generic;
using System.Threading.Tasks;
using FlickerDbModel;
using ModelLogic.HelpClasses;

namespace ModelLogic
{
    public interface IApiRequest
    {
        Task<string> GetJsonSchemaResponse(string imageUrl);
        EmotionsFromResponse GetEmotionsObjectFromJson(string jsonSchema);
        Task FindFacesOnPhotos(IEnumerable<Photo> photos);
    }
}