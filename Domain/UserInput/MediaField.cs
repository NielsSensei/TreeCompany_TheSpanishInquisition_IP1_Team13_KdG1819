using Domain.Common;

namespace Domain.UserInput
{
    public class MediaField : Field
    {
        // Added by NG
        // Modified by EKT & NVZ
        public string Url { get; set; }
        public Media UploadedMedia { get; set; }
        
    }
}