using System.Collections.Generic;

namespace Domain.UserInput
{
    public class MultipleAnswer : Answer
    {
        // Added by NG
        public List<string> RegularAnswers { get; set; }
        public List<string> ExtraAnswers { get; set; }
        public List<string> Choices { get; set; }

        // Added by EKT
        // Modified by NVZ
        // Methods
        #region

        public void AddOption(string option)
        {
            RegularAnswers.Add(option);
        }

        public void AddUserOption(string option)
        {
            ExtraAnswers.Add(option);
        }

        public List<string> GatherAnswers()
        {
            var allAnswers = new List<string>(RegularAnswers.Count +
                                                ExtraAnswers.Count);
            allAnswers.AddRange(RegularAnswers);
            allAnswers.AddRange(ExtraAnswers);

            return allAnswers;
        }
        #endregion
    }
}