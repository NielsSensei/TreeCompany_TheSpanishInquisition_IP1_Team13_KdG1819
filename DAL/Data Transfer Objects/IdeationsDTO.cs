using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    public class IdeationsDTO
    {
        public int ModuleID { get; set; }
        public string UserID { get; set; }
        public string ExtraInfo { get; set; }
        public bool Organisation { get; set; }
        public int EventID { get; set; }
        public bool UserIdea { get; set; }
        public byte[] MediaFile { get; set; }
        public byte RequiredFields { get; set; }
    }
}
