namespace Domain.Common
{
    public class Image : Media
    {
    //Added by NG
    //Modified by EKT & DM
        public ImageExtensions AcceptedExtensions { get; set; }
    }

    public enum ImageExtensions
    {
        JPEG, PNG, GIF
    }
}