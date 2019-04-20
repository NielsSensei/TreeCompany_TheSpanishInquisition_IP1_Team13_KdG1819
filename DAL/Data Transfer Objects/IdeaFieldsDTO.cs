using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    class IdeaFieldsDTO
    {
        public int FieldID { get; set; }
        public int IdeaID { get; set; }
        public string FieldText { get; set; }
        public string FieldStrings { get; set; }
        public float LocationX { get; set; }
        public float LocationY { get; set; }
        public bool Required { get; set; }
        public string Url { get; set; }
        public byte[] UploadedImage { get; set; }
        public byte[] UploadedMedia { get; set; }
    }
}
