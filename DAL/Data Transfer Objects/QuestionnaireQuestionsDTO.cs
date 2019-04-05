using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    class QuestionnaireQuestionsDTO
    {
        public int qQuestionID { get; set; }
        public int ModuleID { get; set; }
        public string QuestionText { get; set; }
        public byte qType { get; set; }
        public bool Required { get; set; }
    }
}
