namespace DAL.Data_Access_Objects
{
    /*
     * @authors Sacha Buelens & Niels Van Zandbergen
     */
    public class ProjectImagesDao
    {
        public int ImageId { get; set; }
        public int ProjectId { get; set; }
        public byte[] ProjectImage { get; set; }
    }
}
