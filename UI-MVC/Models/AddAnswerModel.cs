using System.Collections.Generic;
using Domain.UserInput;

namespace UIMVC.Models
{
    public class AddAnswerModel
    {
        public OpenAnswer OpenAnswer { get; set; }
        public MultipleAnswer MultipleAnswer { get; set; }
        public IList<CheckboxAnswer> CheckboxAnswers { get; set; }
        
        public CheckboxAnswer CustomAnswer { get; set; }
    }

    public class CheckboxAnswer
    {
        public bool Checked { get; set; }
        public string Value { get; set; }
    }
}