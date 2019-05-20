using System.Collections;
using System.Collections.Generic;
using Domain.UserInput;

namespace UIMVC.Models
{
    public class AddAnswer
    {
        public OpenAnswer OpenAnswer { get; set; }
        public MultipleAnswer MultipleAnswer { get; set; }
        public IList<CheckboxAnswer> CheckboxAnswers { get; set; }
    }

    public class CheckboxAnswer
    {
        public bool Checked { get; set; }
        public string Value { get; set; }
    }
}