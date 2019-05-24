using BL;
using Microsoft.AspNetCore.Mvc;


//Controller for platforms
//Author: Edwin Kai-Yin Tam

//Edited by: David Matei, Edwin Kai-Yin Tam

    //This is a small controller to get all platforms and send them to the android app, so users can pick a platform 

namespace UIMVC.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformController : Controller
    {
        private PlatformManager platMgr;

        public PlatformController()
        {
            platMgr = new PlatformManager();
        }

        [HttpGet]
        [Route("GetPlatforms")]
        public IActionResult GetPlatforms()
        {
            var platforms = platMgr.ReadAllPlatforms();

            if (platforms == null)
                return NotFound();

            return Ok(platforms);
        }
    }
}