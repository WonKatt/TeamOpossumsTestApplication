using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlickerDbModel;
using ModelLogic.ModelLogicInterfaces;

namespace ModelLogic.ModelLogicInterfacesRealization
{
    public class PhotoModelLogic:IPhotoModelLogic
    {
        private readonly d5h6stb0hfhccqContext _context;
        public PhotoModelLogic(d5h6stb0hfhccqContext context)
        {
            _context = context;
        }

        public IEnumerable<Photo> GetAllPhotos() =>
            _context.Photo;

        public IEnumerable<Photo> GetPhotosWithNoInformationAboutFaces() =>
             GetAllPhotos().Where(photo => photo.IsFace == null);

        public async Task SetPhotoAsWithEnabledFaces(int photoId) 
        {
            GetPhotoById(photoId).IsFace = true;
            await _context.SaveChangesAsync();
        }

        public async Task SetPhotoAsWithOutEnabledFaces(int photoId)
        {
            GetPhotoById(photoId).IsFace = false;
            await _context.SaveChangesAsync();
        }

        public Photo GetPhotoById(int id) =>
            GetAllPhotos().FirstOrDefault(photo => photo.Id == id);

        public string GetPhotoUrl(int photoId) =>
            GetPhotoById(photoId).Url;
    }
}