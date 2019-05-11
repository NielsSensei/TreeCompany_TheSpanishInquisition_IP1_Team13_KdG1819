using System.Collections.Generic;

namespace BL
{
    public interface IQuestionManager<T>
    {
        void EditQuestion(T question);
        T GetQuestion(int questionId, bool details);
        void MakeQuestion(T question, int moduleId);
        void RemoveQuestion(int id);
        List<T> GetAll();
        List<T> GetAllByModuleId(int id);
    }
}