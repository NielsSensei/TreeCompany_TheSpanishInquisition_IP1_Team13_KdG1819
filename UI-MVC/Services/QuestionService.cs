using System.Collections.Generic;
using BL;
using Domain.UserInput;

namespace UIMVC.Services
{
    public class QuestionService
    {
        private readonly IdeationQuestionManager _iqMgr;
        private readonly QuestionnaireQuestionManager _qqMgr;

        public QuestionService()
        {
            _iqMgr = new IdeationQuestionManager();
            _qqMgr = new QuestionnaireQuestionManager();
        }

        public List<IdeationQuestion> CollectIdeationQuestions(int ideationId)
        {
            return _iqMgr.GetAllByModuleId(ideationId);
        }
    }
}