using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    public class VotesDTO
    {
        public int VoteID { get; set; }
        public int DeviceID { get; set; }
        public int InputID { get; set; }
        public string UserID { get; set; }
        public byte InputType { get; set; }
        public string UserMail { get; set; }
        public float? LocationX { get; set; }
        public float? LocationY { get; set; }
        public string Choices { get; set; }
    }
}
