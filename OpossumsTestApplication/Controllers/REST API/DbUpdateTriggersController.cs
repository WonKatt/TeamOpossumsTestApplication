using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLogic;
using ModelLogic.ModelLogicInterfaces;

namespace OpossumsTestApplication.Controllers.REST_API
{
    
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigins")]
    public class DbUpdateTriggersController : Controller
    {
        private readonly ILogger _logger;
        private readonly IPhotoModelLogic _photoLogic;
        private readonly IApiRequest _apiRequest;

        public DbUpdateTriggersController(ILogger<DbUpdateTriggersController> logger, IPhotoModelLogic facesLogic, IApiRequest apiRequest)
        {
            _logger = logger;
            _photoLogic = facesLogic;
            _apiRequest = apiRequest;
        }

       
        [HttpGet("FacesDb")]        
        public async Task<IActionResult> FacesDbUpdate()
        {
            try
            {
                await _apiRequest.FindFacesOnPhotos(_photoLogic.GetPhotosWithNoInformationAboutFaces());
            }
            catch
            {
                return new StatusCodeResult(500);
            }
            return Ok();
        } 
    }
}