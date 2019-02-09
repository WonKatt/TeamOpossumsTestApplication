using System.Collections.Generic;
using System.Threading.Tasks;
using FlickerDbModel;
using ModelLogic.HelpClasses;
using ModelLogic.HelpClasses.Pagination_helped_classe;

namespace ModelLogic.ModelLogicInterfaces
{
    public interface IFacesModelLogic
    {
        Task FindFacesOnPhotos(IEnumerable<Photo> photos);
        void AddFaceEmotionsOnPhoto(int photoId, EmotionsFromResponse emotions);
        Task FindFacesOnNewPhotos();
        Task FindFacesOnAllPhotos();
        IEnumerable<PhotoInformationResponse> GetTopFearPaginationPhotos(int pageNumber);
       
    }
}