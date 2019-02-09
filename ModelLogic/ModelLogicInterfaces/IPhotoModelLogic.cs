using System.Collections.Generic;
using System.Threading.Tasks;
using FlickerDbModel;

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
        int GetAllPhotosWithAvailableFacesCount();
        IEnumerable<Photo> GetPhotosWithAvailableFaces();
    }
}