using System;
using System.Collections.Generic;
using DAL;
using Domain;
using Domain.Projects;

namespace BL
{
    public class ProjectManager
    {
        // Added by NG
        // Modified by NVZ
        private ProjectRepository ProjectRepo { get; set; }
        private ModuleManager ModuleMan { get; set; }

        // Added by NG
        // Modified by NVZ
        public ProjectManager()
        {
            ProjectRepo = new ProjectRepository();
            ModuleMan = new ModuleManager();
        }
        
        // Added by NG
        // Modified by NVZ & XV
        //Project 
        #region
        /*
        * Setter method, we might need this for certain properties but
        * certainly not all of them. Please make a difference between
        * properties you need and the ones you do not. - NVZ
        * 
        */
        public void ChangeProject(Project project)
        {
            ProjectRepo.Update(project);
        }
        
        /*
         * Simple getter to get information about our Project. - NVZ
         */
        public Project GetProject(int projectId, bool details)
        {
            return ProjectRepo.Read(projectId,details);
            
            // throw new NotImplementedException("I might need this!");
        }

        /*
         * Might need this for initialisation. - NVZ
         */
        public void AddProject(Project project)
        {
            ProjectRepo.Create(project);
        }

        public void RemoveProject(int projectId)
        {
            ProjectRepo.Delete(projectId);
        }       
        #endregion
        
        // Added by NG
        // Modified by NVZ
        //Phase 
        #region
        /*
        * Setter method, we might need this for certain properties but
        * certainly not all of them. Please make a difference between
        * properties you need and the ones you do not. - NVZ
        * 
        */
        public void ChangePhase(Phase phase)
        {
            ProjectRepo.Update(phase);
        }
        
        /*
         * Might need this for initialisation - NVZ
         * 
         */
        public void AddPhase(Phase newPhase, int projectId)
        {
            throw new NotImplementedException("I might need this!");
        }

        public void RemovePhase(int phaseId, int projectId)
        {
            throw new NotImplementedException("Out of Scope!");
        }      
        #endregion
        
        // Added by NVZ
        // Other Methods
        #region
        private bool VerifyProjectEditable(int projectId)
        {
            throw new NotImplementedException("Out of scope!");
        }

        /*
         *  In case we want to show the projectpage for the POC. -NVZ
         */
        public List<Module> GetModules(int projectId, bool details)
        {
            throw new NotImplementedException("I might need this!");
        }
        
        /*
         * We have two options with this method:
         * 
         * 1. Either any call to this class is via this method.
         * 2. Either only calls outside of the ProjectController are for
         * this method so that it can delegate to the moduleManager
         * if it can't solve the problem.
         *
         * This method is conceived to be modular towards microservices,
         * if we have the time I'll explain why. - NVZ
         * 
         */
        public void HandleProjectAction(int projectId, string actionName)
        {
            throw new NotImplementedException("I need this!");
        }
        #endregion
    }
}