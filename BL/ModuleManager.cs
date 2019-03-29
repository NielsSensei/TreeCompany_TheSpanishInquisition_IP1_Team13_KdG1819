using System;
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
        private IdeationRepository ideationRepo { get; set; }
        private QuestionnaireRepository questionnaireRepo { get; set; }
        private ProjectRepository projectRepo { get; set; }        //for linking a module to its project
        private IQuestionManager<QuestionnaireQuestion> questionaireQuestionMan { get; set; }
        private IQuestionManager<IdeationQuestion> ideationQuestionMan { get; set; }

        // Added by NG
        // Modified by NVZ
        public ModuleManager()
        {
            ideationRepo = new IdeationRepository();
            questionnaireRepo = new QuestionnaireRepository();
        }

        // Added by NG
        // Modified by NVZ
        //Module
        #region
        /*
        * Setter method, we might need this for certain properties but
        * certainly not all of them. Please make a difference between
        * properties you need and the ones you do not. - NVZ
        * 
        */
        public void editModule(string propName, int projectID, int moduleID)
        {
            throw new NotImplementedException("I might need this!");
        }

        /*
         * Getter for our module object, this is probably useful. - NVZ
         * 
         */
        //Modified by NG
        public Module getModule(int moduleID, bool details, bool questionnaire)
        {
            if (questionnaire)
            {
                return questionnaireRepo.Read(moduleID);
            } 

            return ideationRepo.Read(moduleID);
            
        }

        /*
         * Initialisation of our modules might be useful. - NVZ
         * 
         */
        //Modified by NG
        public void makeModule(Module module, int projectID, bool questionnaire)
        {
            if (questionnaire)
            {
                Questionnaire newQuestionnaire = (Questionnaire) module;
                projectRepo.Read(module.Id).Modules.Add(newQuestionnaire);
            }
            else
            {
                Ideation i = (Ideation) module;
                projectRepo.Read(module.Id).Modules.Add(i);
            }
        }
        
        //Added by NG
        public void updateModule(Module module, bool questionnaire)
        {
            if (questionnaire)
            {
                Questionnaire q = questionnaireRepo.Read(module.Id);
                q = (Questionnaire) module;
            }
            else
            {
                Ideation i = ideationRepo.Read(module.Id);
                i = (Ideation) module;
            }  
        }

        public void removeModule(int id, int projectID)
        {
            throw new NotImplementedException("Out of Scope!");
        }
        #endregion
        
        // Added by NVZ
        // Other Methods
        #region
        private bool verifyIfModuleEditable(int moduleID)
        {
            throw new NotImplementedException("Out of Scope!");
        }

        /*
         *  This simple method is necessary for most of the CRUD
         *  operations. -NVZ
         */
        private bool verifyIfQuestionnaire(int moduleID)
        {
            throw new NotImplementedException("I might need this!");
        }

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
         */
        public void handleModuleAction(int moduleID, string actionName)
        {
            throw new NotImplementedException("I need this!");
        }
        #endregion
    }
}