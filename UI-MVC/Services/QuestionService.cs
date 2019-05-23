using System.Collections.Generic;
using System.Linq;
using BL;
using Domain.UserInput;

namespace UIMVC.Services
{
    /**
     * @author Xander Veldeman
     */
    public class QuestionService
    {
        private readonly IdeationQuestionManager _iqMgr;

        public QuestionService()
        {
            _iqMgr = new IdeationQuestionManager();
        }

        public List<IdeationQuestion> CollectIdeationQuestions(int ideationId)
        {
            return _iqMgr.GetAllByModuleId(ideationId);
        }

        public int CollectTotalAnswerCount(QuestionnaireQuestion question)
        {
            if (question.QuestionType != QuestionType.Multi && !question.Answers.Any() &&
                question.Answers[0].GetType() != typeof(MultipleAnswer)) return question.Answers.Count;
            int count = 0;

            foreach (Answer answer in question.Answers)
            {
                count += ((MultipleAnswer) answer).Choices.Count;
            }

            return count;
        }
    }
}
