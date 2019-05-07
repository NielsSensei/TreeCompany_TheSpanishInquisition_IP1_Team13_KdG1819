using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Domain;
using Domain.Projects;
using Domain.UserInput;

namespace BL
{
    public class ModuleManager
    {
        // Added by NG
        // Modified by NVZ
        private IdeationRepository IdeationRepo { get; set; }
        private QuestionnaireRepository QuestionnaireRepo { get; set; }
        private ProjectManager ProjectMan { get; set; }        //for linking a module to its project
        private IQuestionManager<QuestionnaireQuestion> QuestionaireQuestionMan { get; set; }
        private IQuestionManager<IdeationQuestion> IdeationQuestionMan { get; set; }

        // Added by NG
        // Modified by NVZ
        public ModuleManager()
        {
            IdeationRepo = new IdeationRepository();
            QuestionnaireRepo = new QuestionnaireRepository();
        }

        // Added by NG
        // Modified by NVZ
        //Module
        #region

        public IEnumerable<Ideation> GetIdeations(int projectId)
        {
            List<Ideation> modules = new List<Ideation>();
            
            modules.AddRange(IdeationRepo.ReadAll(projectId));

            return modules;
        }
        /*
         * Getter for our module object, this is probably useful. - NVZ
         * 
         */
        //Modified by NG
        public Module GetModule(int moduleId, bool details, bool questionnaire)
        {
            if (questionnaire)
            {
                return QuestionnaireRepo.Read(moduleId, details);
            } 

            return IdeationRepo.ReadWithModule(moduleId);            
        }

        /*
         * Voor het aanmaken van een nieuwe module moeten we eerst zien of er een of meerdere fases beschikbaar zijn.
         * Hiervoor halen we modules op op basis van de fase, hier weten we niet of het een ideation is of niet. Hij kijkt
         * eerst tussen de questionnaires of het bestaat, als hierop een exception komt probeert hij het tussen de ideations.
         * Indien hij daar ook een exception geeft weten we dat de fase 'vrij' is. -NVZ
         * 
         */
        public Module GetModule(int phaseId, int projectID)
        {
            try
            {      
                return QuestionnaireRepo.ReadAll(projectID).First(m => m.ParentPhase.Id == phaseId);;
            }
            catch (InvalidOperationException exceptionnbr1)
            {
                try
                {
                    return IdeationRepo.ReadAll(projectID).First(m => m.ParentPhase.Id == phaseId);
                }
                catch (InvalidOperationException exceptionnbr2)
                {
                    return null;
                }
            }           
        }

        // Added by NVZ       
        public IEnumerable<Questionnaire> GetQuestionnaires(int projectId)
        {
            List<Questionnaire> modules = new List<Questionnaire>();

            modules.AddRange(QuestionnaireRepo.ReadAll(projectId));            
            
            return modules;
        }
        /*
         * Initialisation of our modules might be useful. - NVZ
         * 
         */
        //Added by NVZ
        public void MakeQuestionnaire(Questionnaire questionnaire)
        {
            QuestionnaireRepo.Create(questionnaire);
        }
        
        //Modified by NVZ
        public void MakeIdeation(Ideation ideation)
        {
            IdeationRepo.Create(ideation);
        }
        
        //Added by NVZ 
        public void MakeTag(string Tag, int moduleID, bool questionnaire)
        {
            if (questionnaire)
            {
                QuestionnaireRepo.CreateTag(Tag, moduleID);
            }

            if (!questionnaire)
            {
                IdeationRepo.CreateTag(Tag, moduleID);
            }
        }
        
        //Added by NG
        public void EditModule(Module module)
        {
            if (module.GetType() == typeof(Questionnaire))
            {
                QuestionnaireRepo.Update((Questionnaire)module);
            }
            else
            {
                IdeationRepo.Update((Ideation)module);
            }             
        }

        public void RemoveModule(int moduleId, int projectId, bool questionnaire)
        {
            if (questionnaire)
            {
                QuestionnaireRepo.Delete(moduleId);    
            }
            else
            {
                IdeationRepo.Delete(moduleId);
            }
        }
        #endregion
        
        // Added by NVZ
        // Other Methods
        #region
        /*
        private bool VerifyIfModuleEditable(int moduleId)
        {
            throw new NotImplementedException("Out of Scope!");
        }

        /*
         *  This simple method is necessary for most of the CRUD
         *  operations. -NVZ
         
        private bool VerifyIfQuestionnaire(int moduleId)
        {
            throw new NotImplementedException("I might need this!");
        } */

        /*
         * We have two options with this method:
         * 
         * 1. Either any call to this class is via this method.
         * 2. Either only calls outside of the IdeationController or
         * the QuestionnaireController are for this method so that it
         * can delegate to the right IQuestionManager if it can't solve
         * the problem.
         *
         * This method is conceived to be modular towards microservices,
         * if we have the time I'll explain why. - NVZ
         * 
         
        public void HandleModuleAction(int moduleId, string actionName)
        {
            throw new NotImplementedException("I need this!");
        } */
        #endregion
    }
}