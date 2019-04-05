using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    class ProjectImagesDTO
    {
        public int ImageID { get; set; }
        public int ProjectID { get; set; }
        public byte[] ProjectImage { get; set; }
    }
}
