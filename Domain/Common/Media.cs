namespace Domain.Common
{
    public class Media
    {
        //Added by NG
        public int Width { get; set; }
        public int Height { get; set; }
        public string SavedName { get; set; }
        public string Extension { get; set; }
        public int LengthInSeconds { get; set; }
        public bool UserVideo { get; set; }
    }
}