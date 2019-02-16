using System.Collections.Generic;
using System.Threading.Tasks;
using FlickerDbModel;
using ModelLogic.HelpClasses.Pagination_helped_classe;

namespace ModelLogic.ModelLogicInterfaces
{
    public interface IPhotoModelLogic
    {
        IEnumerable<Photo> GetAllPhotos();
        IEnumerable<Photo> GetPhotosWithNoInformationAboutFaces();
        Task SetPhotoAsWithEnabledFaces(int photoId);
        Task SetPhotoAsWithOutEnabledFaces(int photoId);
        Photo GetPhotoById(int id);
        string GetPhotoUrl(int photoId);
        IEnumerable<Photo> GetAllPhotosWithMoreThan50percAvailableEmotions( string emotion);
        IEnumerable<Photo> GetPhotosWithAvailableFaces();        
        IEnumerable<PhotoInformationResponse> GetPhotosPagination(int pageNumber, int maxRequired,List<Photo> photos);
        double GetValueBySpecificEmotion(Faces emotions, string emotionName);


    }
}