using System.Collections.Generic;
using System.Threading.Tasks;
using FlickerDbModel;
using ModelLogic.HelpClasses;

namespace ModelLogic.ModelLogicInterfaces
{
    public interface IFacesModelLogic
    {
        void FindFacesOnPhotos(IEnumerable<Photo> photos);
        void AddFaceEmotionsOnPhoto(int photoId, EmotionsFromResponse emotions);
        void FindFacesOnNewPhotos();
        void FindFacesOnAllPhotos();
    }
}