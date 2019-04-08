using System;
using System.Collections.Generic;
using DAL;
using Domain.UserInput;

namespace BL
{
    public class QuestionnaireQuestionManager : IQuestionManager<QuestionnaireQuestion>
    {
        // Added by NVZ
        private QuestionnaireQuestionsRepository questionnaireQuestionRepo { get; set; }
        
        // Added by NVZ
        public QuestionnaireQuestionManager()
        {
            questionnaireQuestionRepo = new QuestionnaireQuestionsRepository();
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
        public void editQuestion(string propName, int questionID)
        {
            throw new System.NotImplementedException("I might need this!");
        }
        
        /*
         * A getter that probably is very useful. - NVZ
         * 
         */
        public QuestionnaireQuestion getQuestion(int questionID, bool details)
        {
            throw new System.NotImplementedException("I might need this!");
        }
        
        /*
         * This is going to be useful for initialisation. - NVZ
         */
        public void makeQuestion(QuestionnaireQuestion question, int moduleID)
        {
            throw new System.NotImplementedException("I might need this!");
        }
        
        public void removeQuestion(int id)
        {
            throw new System.NotImplementedException("Out of Scope!");
        }

        public IEnumerable<QuestionnaireQuestion> GetAll()
        {
            throw new NotImplementedException();
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
        public void editFeedback(string propName, int feedbackID, int questionID)
        {
            throw new System.NotImplementedException("I might need this!");
        }
        
        /*
         * This getter is good to show the result. - NVZ
         */
        public List<object> getFeedback(int questionID, bool details)
        {
            throw new System.NotImplementedException("I might need this!");
        }
        
        /*
         * Unfortunately I realised that we did not include this in the
         * moduling process but it is needed. - NVZ
         */
        public void makeFeedback(Object feedback, int moduleID, int questionID)
        {
            throw new System.NotImplementedException("I need this!");
        }
        #endregion
           
        // Added by NVZ
        // Other Methods
        #region
        /*
         * This is to define the enum type of this question. - NVZ
         */
        public void defineQuestionType()
        {
            throw new System.NotImplementedException("I might need this!");
        }
        
        /*
         * This might be to fill in the question and to verify if it has been
         * done correctly, not sure. Other uses are welcome. - NVZ
         * 
         */
        public bool verifyQuestion(int questionID)
        {
            throw new System.NotImplementedException("I might need this!");
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
        public void handleQuestionAction(int questionID, string actionName)
        {
            throw new System.NotImplementedException("I might need this!");
        }
        
        public IEnumerable<QuestionnaireQuestion> getAllByQuestionnaireId(int questionnaireId)
        {
            return questionnaireQuestionRepo.ReadAllByQuestionnaireId(questionnaireId);
        }
        
        #endregion










    }
}