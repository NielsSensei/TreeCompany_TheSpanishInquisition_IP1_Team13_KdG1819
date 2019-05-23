using BL;
using Microsoft.AspNetCore.Mvc;

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