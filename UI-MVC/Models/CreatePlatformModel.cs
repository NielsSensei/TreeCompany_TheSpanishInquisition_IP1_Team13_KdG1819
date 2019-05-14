using Microsoft.AspNetCore.Http;

namespace UIMVC.Models
{
    public class CreatePlatformModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public IFormFile IconImage { get; set; }
        public IFormFile CarouselImage { get; set; }
        public IFormFile FrontPageImage { get; set; }
    }
}
