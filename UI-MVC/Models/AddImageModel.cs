using Domain.Projects;
using Microsoft.AspNetCore.Http;

namespace UIMVC.Models
{
    public class AddImageModel
    {
        public Project Project { get; set; }
        public IFormFile File { get; set; }
    }
}