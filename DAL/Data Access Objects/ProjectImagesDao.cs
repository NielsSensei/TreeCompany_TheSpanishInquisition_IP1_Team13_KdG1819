namespace DAL.Data_Access_Objects
{
    public class ProjectImagesDao
    {
        public int ImageId { get; set; }
        public int ProjectId { get; set; }
        public byte[] ProjectImage { get; set; }
    }
}
