using System.ComponentModel.DataAnnotations;

namespace Domain.Common
{
    public class Media
    {
        //Added by NG
        //Modified by EKT & DM & NVZ
        [Key]
        public int Id { get; set; }
        public string Extension { get; set; }
        public bool UserMedia { get; set; }
        public string FileName { get; set; }
        public string BlobPath { get; set; }
    }
}