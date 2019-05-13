using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    public class QuestionnaireQuestionsDTO
    {
        public int QQuestionID { get; set; }
        public int ModuleID { get; set; }
        public string QuestionText { get; set; }
        public byte QType { get; set; }
        public bool Required { get; set; }
    }
}
