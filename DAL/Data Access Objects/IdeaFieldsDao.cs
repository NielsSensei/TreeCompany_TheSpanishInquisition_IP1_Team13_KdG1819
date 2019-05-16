namespace DAL.Data_Access_Objects
{
    public class IdeaFieldsDao
    {
        public int FieldId { get; set; }
        public int IdeaId { get; set; }
        public string FieldText { get; set; }
        public string FieldStrings { get; set; }
        public double LocationX { get; set; }
        public double LocationY { get; set; }
        public string Url { get; set; }
        public byte[] UploadedImage { get; set; }
        public byte[] UploadedMedia { get; set; }
    }
}
