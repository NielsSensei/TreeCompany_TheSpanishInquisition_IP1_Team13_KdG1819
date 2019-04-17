using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using DAL;
using DAL.repos;
using Domain.Projects;
using Domain.UserInput;

namespace BL
{
    public class IdeationQuestionManager : IQuestionManager<IdeationQuestion>
    {
        // Added by NVZ
        private IdeationQuestionsRepository IdeationQuestionRepo { get; set; }
        private VoteManager VoteMan { get; set; }
        private ModuleManager ModuleMan { get; set; }

        // Added by NVZ
        public IdeationQuestionManager()
        {
            IdeationQuestionRepo = new IdeationQuestionsRepository();
            VoteMan = new VoteManager();
            ModuleMan = new ModuleManager();
        }

        // Added by NVZ
        // IdeationQuestion

        #region

        public void ChangeQuestion(IdeationQuestion question)
        {
            IdeationQuestionRepo.Update(question);
        }

        /*
         * Getter for question, probably can use this. - NVZ
         */
        // Modified by EKT
        public IdeationQuestion GetQuestion(int questionId, bool details)
        {
            return IdeationQuestionRepo.Read(questionId, details);
        }

        /*
         * Initialisation might be useful. - NVZ
         */
        public void AddQuestion(IdeationQuestion question, int moduleId)
        {
            IdeationQuestionRepo.Create(question);
            var alteredIdeation = (Ideation)ModuleMan.GetModule(moduleId, false, false);
            alteredIdeation.CentralQuestions.Add(question);
            ModuleMan.ChangeModule(alteredIdeation);
        }

        public void RemoveQuestion(int questionId)
        {
            IdeationQuestionRepo.Delete(questionId);
        }

        public IEnumerable<IdeationQuestion> GetAll()
        {
            return IdeationQuestionRepo.ReadAll();
        }

        #endregion

        // Added by NVZ
        // Idea

        #region

        /*
        * Setter method, we might need this for certain properties but
        * certainly not all of them. Please make a difference between
        * properties you need and the ones you do not. - NVZ
        * 
        */
        public void ChangeFeedback(string propName, int feedbackId, int questionId)
        {
            throw new NotImplementedException("I might need this!");
        }

        /*
         * Getter for all Ideas on an Ideation. - NVZ
         */
        public List<Idea> GetFeedback(int questionId, bool details)
        {
            var ideationQuestion = GetQuestion(questionId, details);
            var feedbacksList = ideationQuestion.Ideas;
            return feedbacksList;
        }

        /*
         * Unfortunately I realised that we did not include this in the
         * moduling process but it is needed. - NVZ
         */
        // Modified by EKT
        public void AddFeedback(int feedbackId, int questionId, int? parentId)
        {
            throw new NotImplementedException();
        }

        #endregion

        // Added by NG
        // Vote
        public void CreateVote(int feedbackId, int userId, int? deviceId, double? x, double? y)
        {
            Idea feedback = IdeationQuestionRepo.ReadIdea(feedbackId, false);
            if (VoteMan.handleVotingOnFeedback(feedbackId, userId, deviceId, x, y))
            {
                feedback.VoteCount++;
            }
        }

        // Added by NVZ
        // Other Methods

        #region

        /*
         * Unlike QuestionnaireQuestion this has noting to do with the enum.
         * This is rather a system where we we work with FieldTypes. - NVZ
         * 
         */
        public void DefineQuestionType()
        {
            throw new NotImplementedException("I might need this!");
        }

        /*
         * This might be a method that can be used for seeing if the question
         * is part of a module of a closed Project. If you can find other
         * uses for this boolean method be free to do so. - NVZ
         
         */
        public bool VerifyQuestion(int questionId)
        {
            throw new NotImplementedException("Out of Scope!");
        }

        /*
         * We have two options with this method:
         * 
         * 1. Either any call to this class is via this method.
         * 2. Either only calls outside of the IdeationController are for
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

        public IEnumerable<IdeationQuestion> GetAllByModuleId(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}