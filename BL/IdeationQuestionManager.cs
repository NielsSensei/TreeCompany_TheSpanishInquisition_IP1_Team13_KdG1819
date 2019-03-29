using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using DAL;
using Domain.UserInput;

namespace BL
{
    public class IdeationQuestionManager : IQuestionManager<IdeationQuestion>
    {
        // Added by NVZ
        private IdeationQuestionsRepository ideationQuestionRepo { get; set; }
        private VoteManager voteMan { get; set; }

        // Added by NVZ
        public IdeationQuestionManager()
        {
            ideationQuestionRepo = new IdeationQuestionsRepository();
            voteMan = new VoteManager();
        }

        // Added by NVZ
        // IdeationQuestion

        #region

        public void editQuestion(string propName, int questionID)
        {
            throw new System.NotImplementedException("Out of Scope!");
        }

        /*
         * Getter for question, probably can use this. - NVZ
         */
        // Modified by EKT
        public IdeationQuestion getQuestion(int questionID, bool details)
        {
            var ideationQuestion = ideationQuestionRepo.Read(questionID);
            if (details)
                return ideationQuestion.GetIdeationQuestionInfo();
            return ideationQuestion;
        }

        /*
         * Initialisation might be useful. - NVZ
         */
        public void makeQuestion(IdeationQuestion question, int moduleID)
        {
            throw new NotImplementedException();
        }

        public void removeQuestion(int id)
        {
            throw new System.NotImplementedException("Out of Scope!");
        }

        public IEnumerable<IdeationQuestion> GetAll()
        {
            throw new NotImplementedException();
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
        public void editFeedback(string propName, int feedbackID, int questionID)
        {
            throw new System.NotImplementedException("I might need this!");
        }

        /*
         * Getter for all Ideas on an Ideation. - NVZ
         */
        public List<Idea> getFeedback(int questionID, bool details)
        {
            var ideationQuestion = getQuestion(questionID, false);
            var feedbacksList = ideationQuestion.Ideas;
            var feedbackDetailsList = new List<Idea>();
            
            if (details)
            {
                foreach (var feedback in feedbacksList)
                {
                    feedbackDetailsList.Add(feedback.GetIdeaInfo());
                }

                return feedbackDetailsList;
            }

            return feedbacksList;
        }

        /*
         * Unfortunately I realised that we did not include this in the
         * moduling process but it is needed. - NVZ
         */
        // Modified by EKT
        public void MakeFeedback(int feedbackId, int questionId, int? parentIdeaId)
        {
//            var ideationQuestion = (IdeationQuestion) getQuestion(questionId, false);
//            var acceptedAnswerTypes = ideationQuestion.AcceptedAnswerTypes;
            
            Idea idea = new Idea();
            idea.Id = feedbackId;
            
            Field field = new Field();
            field.Id = 64;
            field.TextLength = 2000;
            field.Text = "Filler text for field";
//            acceptedAnswerTypes.Contains(typeof(ClosedField));
            
            idea.AddField(field);
            
            idea.Visible = true;
            idea.questionID = questionId;
            idea.ParentId = parentIdeaId;
            ideationQuestionRepo.Create(idea);
        }

        #endregion

        // Added by NG
        // Vote
        public void CreateVote(int feedbackID, int userID, int? deviceID, double? x, double? y)
        {
            Idea feedback = ideationQuestionRepo.ReadIdea(feedbackID);
            if (voteMan.handleVotingOnFeedback(feedbackID, userID, deviceID, x, y))
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
        public void defineQuestionType()
        {
            throw new System.NotImplementedException("I might need this!");
        }

        /*
         * This might be a method that can be used for seeing if the question
         * is part of a module of a closed Project. If you can find other
         * uses for this boolean method be free to do so. - NVZ
         
         */
        public bool verifyQuestion(int questionID)
        {
            throw new System.NotImplementedException("Out of Scope!");
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
        public void handleQuestionAction(int questionID, string actionName)
        {
            throw new System.NotImplementedException("I might need this!");
        }

        public IEnumerable<IdeationQuestion> getAllByQuestionnaireId(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}