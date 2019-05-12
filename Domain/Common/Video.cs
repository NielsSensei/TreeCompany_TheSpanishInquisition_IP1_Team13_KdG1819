namespace Domain.Common
{
    public class Video : Media
    {
        public int LengthSeconds { get; set; }
        public VideoExtentions AcceptedExtentions { get; set; }
    }

    public enum VideoExtentions
    {
        MP4, WEBM
    }
}