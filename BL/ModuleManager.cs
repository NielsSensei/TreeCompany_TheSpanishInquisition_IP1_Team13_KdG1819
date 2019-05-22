using System.Collections.Generic;
using System.Linq;
using DAL.repos;
using Domain.Projects;
using Domain.UserInput;

namespace BL
{
    /*
     * @authors Edwin Kai Yin Tam & Niels Van Zandbergen
     */
    public class ModuleManager
    {
        private IdeationRepository IdeationRepo { get; }
        private QuestionnaireRepository QuestionnaireRepo { get; }
        private IdeationQuestionManager _ideaMgr { get; }
        
        public ModuleManager()
        {
            IdeationRepo = new IdeationRepository();
            QuestionnaireRepo = new QuestionnaireRepository();
            _ideaMgr = new IdeationQuestionManager();
        }

        /*
         * @authors Edwin Kai Yin Tam & Niels Van Zandbergen
         */
        #region Ideation
        public IEnumerable<Ideation> GetIdeations(int projectId)
        {
            List<Ideation> modules = new List<Ideation>();

            modules.AddRange(IdeationRepo.ReadAll(projectId));

            return modules;
        }

        public Ideation GetIdeation(int moduleId, bool details){
            return IdeationRepo.ReadWithModule(moduleId, details);
        }

        public Ideation GetIdeation(int phaseId, int projectId)
        {
            return IdeationRepo.ReadAll(projectId).FirstOrDefault(m => m.ParentPhase.Id == phaseId);
        }

        public void MakeIdeation(Ideation ideation)
        {
            IdeationRepo.Create(ideation);
        }

        public void EditIdeation(Ideation ideation)
        {
            IdeationRepo.Update(ideation);
        }
        #endregion

        /*
         * @authors Edwin Kai Yin Tam & Niels Van Zandbergen
         */
        #region Questionnaire
        public Questionnaire GetQuestionnaire(int moduleId, bool details)
        {
            return QuestionnaireRepo.Read(moduleId, details);
        }

        public Questionnaire GetQuestionnaire(int phaseId, int projectId)
        {
            return QuestionnaireRepo.ReadAll(projectId).FirstOrDefault(m => m.ParentPhase.Id == phaseId);
        }

        public IEnumerable<Questionnaire> GetQuestionnaires(int projectId)
        {
            List<Questionnaire> modules = new List<Questionnaire>();

            modules.AddRange(QuestionnaireRepo.ReadAll(projectId));

            return modules;
        }

        public void MakeQuestionnaire(Questionnaire questionnaire)
        {
            QuestionnaireRepo.Create(questionnaire);
        }

        public void EditQuestionnaire(Questionnaire questionnaire)
        {
            QuestionnaireRepo.Update(questionnaire);
        }
        #endregion

        /*
         * @author Niels Van Zandbergen
         */
        #region General Module Methods
        public void MakeTag(string tag, int moduleId, bool questionnaire)
        {
            if (questionnaire)
            {
                QuestionnaireRepo.CreateTag(tag, moduleId);
            }

            if (!questionnaire)
            {
                IdeationRepo.CreateTag(tag, moduleId);
            }
        }

        public void RemoveModule(int moduleId, bool questionnaire)
        {
            if (questionnaire)
            {
                QuestionnaireRepo.Delete(moduleId);
            }
            else
            {
                List<IdeationQuestion> iqs = _ideaMgr.GetAllByModuleId(moduleId);
                foreach (IdeationQuestion iq in iqs)
                {
                    List<Idea> ideas = _ideaMgr.GetIdeas(iq.Id);
                    foreach (Idea idea in ideas)
                    {
                        _ideaMgr.RemoveField(idea.Id);
                        _ideaMgr.RemoveReports(idea.Id);
                        _ideaMgr.RemoveVotes(idea.Id);
                        _ideaMgr.RemoveIdea(idea.Id);
                    }

                    _ideaMgr.RemoveQuestion(iq.Id);
                }
                
                IdeationRepo.Delete(moduleId);
            }
        }
        #endregion
    }
}
