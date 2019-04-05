namespace Domain.Common
{
    public class Video : Media
    {
        //Added by EKT & DM
        public int LengthSeconds { get; set; }
        public VideoExtentions AcceptedExtentions { get; set; }
    }

    public enum VideoExtentions
    {
        MP4, WEBM
    }
}