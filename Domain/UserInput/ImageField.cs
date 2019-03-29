using Domain.Common;

namespace Domain.UserInput
{
    public class ImageField : Field
    {
        // Added by NG
        // Modified by EKT & NVZ
        public string URL { get; set; }
        public Image UploadImage { get; set; }
        
    }
}