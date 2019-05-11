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
        public string IconImagePath { get; set; }
        public string CarouselImagePath { get; set; }
        public string FrontPageImagePath { get; set; }
    }
}
