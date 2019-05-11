namespace DAL.Data_Access_Objects
{
    public class PlatformsDao
    {
        public int PlatformId { get; set; }
        public string Name { get; set; }
        public string SiteUrl { get; set; }
        public byte[] IconImage { get; set; }
    }
}
