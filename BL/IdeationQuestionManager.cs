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

        public void EditQuestion(IdeationQuestion question)
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
        public void MakeQuestion(IdeationQuestion question, int moduleId)
        {
            IdeationQuestionRepo.Create(question);
        }

        public void RemoveQuestion(int questionId)
        {
            IdeationQuestionRepo.Delete(questionId);
        }

        public List<IdeationQuestion> GetAll()
        {
            return IdeationQuestionRepo.ReadAll().ToList();
        }

        public List<IdeationQuestion> GetAllByModuleId(int id)
        {
            return IdeationQuestionRepo.ReadAll(id).ToList();
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
        public void EditIdea(Idea idea)
        {
            IdeationQuestionRepo.Update(idea);
        }

        public Idea GetIdea(int ideaId)
        {
            return IdeationQuestionRepo.ReadWithFields(ideaId);
        }

        public void RemoveIdea(int ideaId)
        {
            IdeationQuestionRepo.DeleteIdea(ideaId);
        }
                
        /*
         * Getter for all Ideas on an Ideation. - NVZ
         */
        public List<Idea> GetIdeas(int questionId)
        {
            return IdeationQuestionRepo.ReadAllIdeasByQuestion(questionId).ToList();
        }

        public List<Idea> GetIdeas()
        {
            return IdeationQuestionRepo.ReadAllIdeas().ToList();
        }
        /*
         * Unfortunately I realised that we did not include this in the
         * moduling process but it is needed. - NVZ
         
        // Modified by EKT
        public void MakeIdea(int questionId, Idea idea)
        {
            throw new NotImplementedException();
            
        }*/
        #endregion

        // Added by NG
        // Vote
        public void MakeVote(int feedbackId, string userId, int? deviceId, double? x, double? y)
        {
            Idea feedback = IdeationQuestionRepo.ReadIdea(feedbackId, false);
            if (VoteMan.VerifyVotingOnFeedback(feedbackId, userId, deviceId, x, y))
            { 
                //TODO fix voting via device
                feedback.VoteCount++;
                EditIdea(feedback);
            }
        }

        public void MakeVote(int feedbackId, string userId)
        {
            Idea feedback = IdeationQuestionRepo.ReadIdea(feedbackId, false);
            if (VoteMan.VerifyVotingOnFeedback(feedbackId, userId, null, null, null))
            {
                VoteMan.MakeVote(feedbackId, userId, null, null, null, true);
                feedback.VoteCount++;
                EditIdea(feedback);
            }
        }
        
        // Added by NVZ
        // Field
        public IEnumerable<Field> GetAllFields(int ideaID)
        {
            return IdeationQuestionRepo.ReadAllFields(ideaID);
        }
        
        // Added by NVZ
        // Report
        public void RemoveReport(int id)
        {
            IdeationQuestionRepo.DeleteReport(id);
        }
        
        public void EditReport(Report obj)
        {
            IdeationQuestionRepo.Update(obj);
        }
        
        public IEnumerable<Report> GetAllReportsByIdea(int ideaID)
        {
            return IdeationQuestionRepo.ReadAllReportsByIdea(ideaID);
        }

        public Report GetReport(int reportID)
        {
            return IdeationQuestionRepo.ReadReport(reportID,false);
        }
        // Added by NVZ
        // Other Methods
        #region
        /*
         * Unlike QuestionnaireQuestion this has noting to do with the enum.
         * This is rather a system where we we work with FieldTypes. - NVZ
         */  
        public void DefineQuestionType()
        {
            //throw new NotImplementedException("I might need this!");
        } 

        /*
         * This might be a method that can be used for seeing if the question
         * is part of a module of a closed Project. If you can find other
         * uses for this boolean method be free to do so. - NVZ
        */ 
         
        public bool VerifyQuestion(int questionId)
        {
            //throw new NotImplementedException("Out of Scope!");
            return false;
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
         */
         
        public void HandleQuestionAction(int questionId, string actionName)
        {
            //throw new NotImplementedException("I might need this!");
        } 
        #endregion
    }
}