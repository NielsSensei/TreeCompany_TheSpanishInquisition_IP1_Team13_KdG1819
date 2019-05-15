using System.Collections.Generic;
using System.Linq;
using DAL.repos;
using Domain.UserInput;

namespace BL
{
    public class QuestionnaireQuestionManager : IQuestionManager<QuestionnaireQuestion>
    {
        private QuestionnaireQuestionsRepository QuestionnaireQuestionRepo { get; }
        
        public QuestionnaireQuestionManager()
        {
            QuestionnaireQuestionRepo = new QuestionnaireQuestionsRepository();
        }
        
        #region QuestionnaireQuestion
        public void EditQuestion(QuestionnaireQuestion question)
        {
            QuestionnaireQuestionRepo.Update(question);
        }
        
        public QuestionnaireQuestion GetQuestion(int questionId, bool details)
        {
            return QuestionnaireQuestionRepo.Read(questionId, details);
        }
        
        public void MakeQuestion(QuestionnaireQuestion question, int moduleId)
        {
            QuestionnaireQuestionRepo.Create(question);
        }

        public void RemoveQuestion(int questionId)
        {
            QuestionnaireQuestionRepo.Delete(questionId);
        }

        public List<QuestionnaireQuestion> GetAll()
        {
            return QuestionnaireQuestionRepo.ReadAll().ToList();
        }
        
        public List<QuestionnaireQuestion> GetAllByModuleId(int questionnaireId)
        {
            return QuestionnaireQuestionRepo.ReadAllByQuestionnaireId(questionnaireId).ToList();
        }
        #endregion
        
        #region Answer
        public List<Answer> GetAnswers(int questionId)
        {
            return QuestionnaireQuestionRepo.ReadAll(questionId).ToList();
        }  
        
        #region Option
        public void MakeOption(int questionId, string optionText)
        {
            QuestionnaireQuestionRepo.CreateOption(questionId, optionText);
        }

        public string GetOption(int optionId, int questionId)
        {
            return QuestionnaireQuestionRepo.ReadOption(optionId, questionId);
        }

        public int GetOptionId(string optionText, int questionId)
        {
            return QuestionnaireQuestionRepo.ReadOptionId(optionText, questionId);
        }

        public void DestroyOption(int optionId)
        {
            QuestionnaireQuestionRepo.DeleteOption(optionId);
        }

        public IEnumerable<string> GetAllOptions()
        {
            return QuestionnaireQuestionRepo.ReadAllOptions();
        }

        public IEnumerable<string> GetAllOptionsForQuestion(int questionId)
        {
            return QuestionnaireQuestionRepo.ReadAllOptionsForQuestion(questionId);
        }
        #endregion
        #endregion
    }
}
