using System;
using System.Collections.Generic;
using DAL;
using Domain.UserInput;

namespace BL
{
    public class QuestionnaireQuestionManager : IQuestionManager<QuestionnaireQuestion>
    {
        // Added by NVZ
        private QuestionnaireQuestionsRepository QuestionnaireQuestionRepo { get; set; }
        
        // Added by NVZ
        public QuestionnaireQuestionManager()
        {
            QuestionnaireQuestionRepo = new QuestionnaireQuestionsRepository();
        }
        
        // Added by NVZ
        // QuestionnaireQuestion
        #region 
        /*
        * Setter method, we might need this for certain properties but
        * certainly not all of them. Please make a difference between
        * properties you need and the ones you do not. - NVZ
        * 
        */
        public void ChangeQuestion(QuestionnaireQuestion question)
        {
            QuestionnaireQuestionRepo.Update(question);
        }
        
        /*
         * A getter that probably is very useful. - NVZ
         * 
         */
        public QuestionnaireQuestion GetQuestion(int questionId, bool details)
        {
            return QuestionnaireQuestionRepo.Read(questionId, details);
        }
        
        /*
         * This is going to be useful for initialisation. - NVZ
         */
        public void AddQuestion(QuestionnaireQuestion question, int moduleId)
        {
            QuestionnaireQuestionRepo.Create(question);
        }
        
        public void RemoveQuestion(int questionId)
        {
            QuestionnaireQuestionRepo.Delete(questionId);
        }

        public IEnumerable<QuestionnaireQuestion> GetAll()
        {
            return QuestionnaireQuestionRepo.ReadAll();
        }

        #endregion
        
        // Added by NVZ
        // Answer
        #region 
        /*
        * Setter method, we might need this for certain properties but
        * certainly not all of them. Please make a difference between
        * properties you need and the ones you do not. - NVZ
        * 
        */
        public void ChangeAnswer(string propName, int answerId, int questionId)
        {
            throw new NotImplementedException("I might need this!");
        }
        
        /*
         * This getter is good to show the result. - NVZ
         */
        public List<Answer> GetAnswers(int questionId, bool details)
        {
            throw new NotImplementedException("I might need this!");
        }
        
        /*
         * Unfortunately I realised that we did not include this in the
         * moduling process but it is needed. - NVZ
         */
        public void AddAnswer(Answer answer, int moduleId, int questionId)
        {
            throw new NotImplementedException("I need this!");
        }
        #endregion
           
        // Added by NVZ
        // Other Methods
        #region
        /*
         * This is to define the enum type of this question. - NVZ
         */
        public void DefineQuestionType()
        {
            throw new NotImplementedException("I might need this!");
        }
        
        /*
         * This might be to fill in the question and to verify if it has been
         * done correctly, not sure. Other uses are welcome. - NVZ
         * 
         */
        public bool VerifyQuestion(int questionId)
        {
            throw new NotImplementedException("I might need this!");
        }
        
        /*
         * We have two options with this method:
         * 
         * 1. Either any call to this class is via this method.
         * 2. Either only calls outside of the QuestionnaireController are for
         * this method so that it can delegate to the voteManager
         * if it can't solve the problem.
         *
         * This method is conceived to be modular towards microservices,
         * if we have the time I'll explain why. - NVZ
         * 
         */
        public void HandleQuestionAction(int questionId, string actionName)
        {
            throw new NotImplementedException("I might need this!");
        }

        public IEnumerable<QuestionnaireQuestion> GetAllByModuleId(int questionnaireId)
        {
            return QuestionnaireQuestionRepo.ReadAllByQuestionnaireId(questionnaireId);
        }
        #endregion










    }
}