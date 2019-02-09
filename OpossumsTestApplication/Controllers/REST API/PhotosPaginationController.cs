using System.Collections.Generic;
using System.Linq;
using FlickerDbModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLogic.HelpClasses.Pagination_helped_classe;
using ModelLogic.ModelLogicInterfaces;

namespace OpossumsTestApplication.Controllers.REST_API
{
    [Route("/api/[controller]")]
    public class PhotosPaginationController : Controller
    {
        private  readonly ILogger _logger;
        private readonly IFacesModelLogic _facesModelLogic;
        
        public PhotosPaginationController(ILogger<PhotosPaginationController> logger, IFacesModelLogic facesModelLogic)
        {
            _logger = logger;
            _facesModelLogic = facesModelLogic;
        }        
        [HttpGet("[action]")]
        public IEnumerable<PhotoInformationResponse> GetPhotosWithFearEmotions([FromQuery]int pageNumber)
        {
            return _facesModelLogic.GetTopFearPaginationPhotos(pageNumber);
        }
    }
}