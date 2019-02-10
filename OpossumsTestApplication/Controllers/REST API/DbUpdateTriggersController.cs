using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLogic.ModelLogicInterfaces;

namespace OpossumsTestApplication.Controllers.REST_API
{
    [Route("api/[controller]")]
    public class DbUpdateTriggersController : Controller
    {
        private readonly ILogger _logger;
        private readonly IFacesModelLogic _facesLogic;

        public DbUpdateTriggersController(ILogger<DbUpdateTriggersController> logger, IFacesModelLogic facesLogic)
        {
            _logger = logger;
            _facesLogic = facesLogic;
            
        }

       
        [HttpGet("FacesDb")]        
        public async Task<IActionResult> FacesDbUpdate()
        {

            await _facesLogic.FindFacesOnNewPhotos();
            
            return Ok();
        } 
    }
}