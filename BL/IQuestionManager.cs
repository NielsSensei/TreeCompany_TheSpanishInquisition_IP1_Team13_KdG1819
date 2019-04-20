using System;
using System.Collections.Generic;
using DAL;

namespace BL
{
    public interface IQuestionManager<T>
    {
        void DefineQuestionType();
        bool VerifyQuestion(int questionId);
        void EditQuestion(T question);
        T GetQuestion(int questionId, bool details);
        void HandleQuestionAction(int questionId, string actionName);
        void MakeQuestion(T question, int moduleId);
        void RemoveQuestion(int id);

        List<T> GetAll();
        List<T> GetAllByModuleId(int id);
    }
}