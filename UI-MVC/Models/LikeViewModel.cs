using Domain.Projects;

namespace UIMVC.Models
{
    public class LikeViewModel
    {
        public int LikeCount { get; set; }
        public int FbLikeCount { get; set; }
        public int TwitterLikeCount { get; set; }
        public int ShareCount { get; set; }
        public int RetweetCount { get; set; }
        public LikeVisibility IconStyle { get; set; }
    }
}
