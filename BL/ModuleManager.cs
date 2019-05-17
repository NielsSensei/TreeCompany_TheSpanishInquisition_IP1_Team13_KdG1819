using System.Collections.Generic;
using System.Linq;
using DAL.repos;
using Domain.Projects;

namespace BL
{
    public class ModuleManager
    {
        private IdeationRepository IdeationRepo { get; }
        private QuestionnaireRepository QuestionnaireRepo { get; }

        public ModuleManager()
        {
            IdeationRepo = new IdeationRepository();
            QuestionnaireRepo = new QuestionnaireRepository();
        }

        #region Ideation
        public IEnumerable<Ideation> GetIdeations(int projectId)
        {
            List<Ideation> modules = new List<Ideation>();

            modules.AddRange(IdeationRepo.ReadAll(projectId));

            return modules;
        }

        public Ideation GetIdeation(int moduleId){
            return IdeationRepo.ReadWithModule(moduleId);
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
                var removedIdeation = IdeationRepo.Read(moduleId, true);
                ProjectMan.GetProject(projectId, false).Modules.Remove(removedIdeation);
                IdeationRepo.Delete(moduleId);
            }
        }
        #endregion
    }
}
