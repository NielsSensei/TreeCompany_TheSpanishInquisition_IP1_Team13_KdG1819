using System;
using System.Collections.Generic;
using DAL;

namespace BL
{
    public interface IQuestionManager<T>
    {
        void defineQuestionType();
        bool verifyQuestion(int questionID);
//        void editFeedback(string propName, int feedbackID, int questionID);
        void editQuestion(string propName, int questionID);
//        List<T> getFeedback(int questionID, bool details);
        T getQuestion(int questionID, bool details);
        void handleQuestionAction(int questionID, string actionName);
        void makeQuestion(T question, int moduleID);
        void removeQuestion(int id);

        IEnumerable<T> GetAll();
        IEnumerable<T> getAllByQuestionnaireId(int id);
    }
}