using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    public class ChoicesDTO
    {
        public int? ChoiceID { get; set; }
        public int AnswerID { get; set; }
        public int OptionID { get; set; }
    }
}
