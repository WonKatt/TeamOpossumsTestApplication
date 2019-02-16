using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlickerDbModel;
using Microsoft.EntityFrameworkCore;
using ModelLogic.HelpClasses.Pagination_helped_classe;
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
            _context.Photo.Include(photo=>photo.Faces);

        public IEnumerable<Photo> GetPhotosWithNoInformationAboutFaces() =>
             GetAllPhotos().Where(photo => photo.IsFace == null);

        public  async  Task SetPhotoAsWithEnabledFaces(int photoId) 
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

        public IEnumerable<Photo> GetAllPhotosWithMoreThan50percAvailableEmotions( string emotion) =>        
            GetPhotosWithAvailableFaces().Where(photo => GetValueBySpecificEmotion(photo.Faces, emotion) > 50);
        

        public IEnumerable<Photo> GetPhotosWithAvailableFaces()
        {
            return GetAllPhotos().Where(photo => photo.IsFace == true);
        }
        public double GetValueBySpecificEmotion(Faces emotions, string emotionName) =>        
            (double)emotions.GetType().GetProperty(emotionName).GetValue(emotions,null);
        
        public IEnumerable<PhotoInformationResponse> GetPhotosPagination(int pageNumber, int maxRequired, 
            List<Photo> photos)
        {
            int paginationIndex = (pageNumber - 1) * maxRequired;
            int remainingPhotosForPagination = photos.Count- paginationIndex;
            
            if (paginationIndex >= photos.Count)
            {
                return null;
            }
            else if (paginationIndex + maxRequired > photos.Count)
            {
                return photos                    
                    .GetRange(paginationIndex, remainingPhotosForPagination )
                    .Select(photo=> new PhotoInformationResponse
                    {
                        Id=photo.Id,
                        Title=photo.Title,
                        Url=photo.Url
                    });                    
            }
            else
            {
                return photos
                    .GetRange(paginationIndex,maxRequired)
                    .Select(photo=> new PhotoInformationResponse
                    {
                        Id=photo.Id,
                        Title=photo.Title,
                        Url=photo.Url
                    });
            }
        }        
    }
}