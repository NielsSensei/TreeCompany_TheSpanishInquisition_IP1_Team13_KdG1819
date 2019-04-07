using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    class OptionsDTO
    {
        public int OptionID { get; set; }
        public string OptionText { get; set; }
        public int qQuestionID { get; set; }
    }
}
