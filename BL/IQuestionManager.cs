using System;
using System.Collections.Generic;
using DAL;

namespace BL
{
    public interface IQuestionManager<T>
    {
        void DefineQuestionType();
        bool VerifyQuestion(int questionId);
//        void editFeedback(string propName, int feedbackID, int questionID);
        void ChangeQuestion(T question);
//        List<T> getFeedback(int questionID, bool details);
        T GetQuestion(int questionId, bool details);
        void HandleQuestionAction(int questionId, string actionName);
        void AddQuestion(T question, int moduleId);
        void RemoveQuestion(int id);

        List<T> GetAll();
        List<T> GetAllByModuleId(int id);
    }
}