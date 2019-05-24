using System.Collections.Generic;
using System.Linq;
using DAL.repos;
using Domain.UserInput;

namespace BL
{
    /*
     * @authors Nathan Gijselings, Edwin Kai Yin Tam & Niels Van Zandbergen
     */
    public class IdeationQuestionManager : IQuestionManager<IdeationQuestion>
    {
        private IdeationQuestionsRepository IdeationQuestionRepo { get; }
        private VoteManager VoteMan { get; }

        public IdeationQuestionManager()
        {
            IdeationQuestionRepo = new IdeationQuestionsRepository();
            VoteMan = new VoteManager();
        }
    
        /*
         * @authors Edwin Kai Yin Tam & Niels Van Zandbergen
         */
        #region IdeationQuestion
        public void EditQuestion(IdeationQuestion question)
        {
            IdeationQuestionRepo.Update(question);
        }

        public IdeationQuestion GetQuestion(int questionId, bool details)
        {
            return IdeationQuestionRepo.Read(questionId, details);
        }

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
        
        /*
         * @authors Edwin Kai Yin Tam & Niels Van Zandbergen
         */
        #region Idea
        public void EditIdea(Idea idea)
        {
            IdeationQuestionRepo.Update(idea);
        }

        public Idea GetIdea(int ideaId, bool details)
        {
            return IdeationQuestionRepo.ReadWithFields(ideaId, details);
        }

        public void MakeIdea(Idea idea)
        {
            IdeationQuestionRepo.Create(idea);
        }

        public void RemoveIdea(int ideaId)
        {
            IdeationQuestionRepo.DeleteIdea(ideaId);
        }

        public List<Idea> GetIdeas(int questionId)
        {
            return IdeationQuestionRepo.ReadAllIdeasByQuestion(questionId).ToList();
        }

        public List<Idea> GetIdeas()
        {
            return IdeationQuestionRepo.ReadAllIdeas().ToList();
        }
        #endregion

        /*
         * @authors Nathan Gijselings, Edwin Kai Yin Tam & Niels Van Zandbergen
         */
        #region Vote
        public void MakeVote(int feedbackId, string userId, int? deviceId, double? x, double? y)
        {
            Idea feedback = IdeationQuestionRepo.ReadIdea(feedbackId, true);
            if (VoteMan.VerifyVotingOnFeedback(feedbackId, userId, deviceId, x, y))
            {
                feedback.VoteCount++;
                EditIdea(feedback);
            }
        }

        public bool MakeVote(int feedbackId, string userId)
        {
            Idea feedback = GetIdea(feedbackId, false);
            if (VoteMan.VerifyVotingOnFeedback(feedbackId, userId, null, null, null))
            {
                VoteMan.MakeVote(feedbackId, userId, null, null, null, true);
                feedback.VoteCount++;
                EditIdea(feedback);

                return true;
            }

            return false;
        }

        public void RemoveVotes(int ideaId)
        {
            VoteMan.RemoveVotes(ideaId);
        }
        #endregion
        
        /*
         * @authors Edwin Kai Yin Tam & Niels Van Zandbergen
         */
        #region Field
        public void RemoveField(int ideaId)
        {
            IdeationQuestionRepo.DeleteField(ideaId);
        }
        #endregion

        /*
         * @author Niels Van Zandbergen
         */
        #region Report
        public void RemoveReport(int id)
        {
            IdeationQuestionRepo.DeleteReport(id);
        }

        public void RemoveReports(int ideaId)
        {
            IdeationQuestionRepo.DeleteReports(ideaId);
        }

        public void EditReport(Report obj)
        {
            IdeationQuestionRepo.Update(obj);
        }

        public void MakeReport(Report obj)
        {
            IdeationQuestionRepo.Create(obj);
        }

        public IEnumerable<Report> GetAllReportsByIdea(int ideaId)
        {
            return IdeationQuestionRepo.ReadAllReportsByIdea(ideaId);
        }

        public Report GetReport(int reportId, bool details)
        {
            return IdeationQuestionRepo.ReadReport(reportId,details);
        }
        #endregion
    }
}
