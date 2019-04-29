using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    class IdeationQuestionsDTO
    {
        public int IQuestionID { get; set; }
        public int ModuleID { get; set; }
        public string QuestionTitle { get; set; }
        public string Description { get; set; }
        public string WebsiteLink { get; set; }
    }
}
