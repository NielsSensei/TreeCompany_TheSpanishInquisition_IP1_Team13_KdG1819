namespace Domain.Common
{
    public class Image : Media
    {
        public ImageExtensions AcceptedExtensions { get; set; }
    }

    public enum ImageExtensions
    {
        JPEG, PNG, GIF
    }
}