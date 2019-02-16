using System.Collections.Generic;
using System.Linq;
using FlickerDbModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLogic.HelpClasses.Pagination_helped_classe;
using ModelLogic.ModelLogicInterfaces;

namespace OpossumsTestApplication.Controllers.REST_API
{
    [Route("/api/[controller]/[action]")]
    [EnableCors("AllowSpecificOrigins")]
    public class PhotosPaginationController : Controller
    {
        private  readonly ILogger _logger;
        private readonly IFacesModelLogic _facesModelLogic;
        private readonly IPhotoModelLogic _photoModelLogic;
        public PhotosPaginationController(ILogger<PhotosPaginationController> logger, 
            IFacesModelLogic facesModelLogic, IPhotoModelLogic photoModelLogic)
        {
            _photoModelLogic = photoModelLogic;
            _logger = logger;
            _facesModelLogic = facesModelLogic;
        }        
        [HttpGet]
        public IEnumerable<PhotoInformationResponse> GetPhotosWithMostEmotions([FromQuery]int pageNumber,
                                                                               [FromQuery]string emotion,
                                                                               [FromQuery]int maxRequired = 50)
        {
            if (emotion == null || pageNumber<=0) return null;
            emotion = emotion.Replace(' ','\0');
            emotion = char.ToUpper(emotion[0]) + emotion.Substring(1);
            if (!_facesModelLogic.IsEmotionParamIsLegit(emotion)) return null;
            var photos = _photoModelLogic.GetAllPhotosWithMoreThan50percAvailableEmotions(emotion)
                .OrderByDescending(photo => _photoModelLogic.GetValueBySpecificEmotion(photo.Faces, emotion))
                .ToList();
                
            return _photoModelLogic.GetPhotosPagination(pageNumber, maxRequired,photos);

        }
        [HttpGet]
        public IEnumerable<PhotoInformationResponse> GetAllPhotos([FromQuery]int pageNumber,            
                                                                  [FromQuery]int maxRequired = 50)
        {
            var photos = _photoModelLogic.GetAllPhotos().OrderBy(photo => photo.Id).ToList();   
            return pageNumber>0 ? _photoModelLogic.GetPhotosPagination(pageNumber, maxRequired,photos) : null;
        }
    }
}