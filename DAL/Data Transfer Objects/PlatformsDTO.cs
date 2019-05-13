using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    public class PlatformsDTO
    {
        public int PlatformID { get; set; }
        public string Name { get; set; }
        public string SiteUrl { get; set; }
        public byte[] IconImage { get; set; }
    }
}
