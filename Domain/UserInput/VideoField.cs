using Domain.Common;

namespace Domain.UserInput
{
    public class MediaField : Field
    {
        // Added by NG
        // Modified by EKT & NVZ & DM
        public string Url { get; set; }
        public Video UploadedVideo { get; set; }
    }
}