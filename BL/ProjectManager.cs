using System;
using System.Collections.Generic;
using DAL;
using Domain;

namespace BL
{
    public class ProjectManager
    {
        // Added by NG
        // Modified by NVZ
        private ProjectRepository projectRepo { get; set; }
        private ModuleManager moduleMan { get; set; }

        // Added by NG
        // Modified by NVZ
        public ProjectManager()
        {
            projectRepo = new ProjectRepository();
            moduleMan = new ModuleManager();
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
        public void editProject(string propName, int projectID)
        {
            throw new NotImplementedException("I might need this!");
        }
        
        /*
         * Simple getter to get information about our Project. - NVZ
         */
        public Project getProject(int id, bool details)
        {
            return projectRepo.Read(1);
            
            // throw new NotImplementedException("I might need this!");
        }

        /*
         * Might need this for initialisation. - NVZ
         */
        public void CreateProject()
        {
            throw new NotImplementedException("I might need this!");
        }

        public void DeleteProject(int id)
        {
            throw new NotImplementedException("Out of Scope!");
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
        public void editPhase(string propName, int projectID, int phaseID)
        {
            throw new NotImplementedException("I might need this!");
        }
        
        /*
         * Might need this for initialisation - NVZ
         * 
         */
        public void makePhase(Phase newPhase, int projectID)
        {
            throw new NotImplementedException("I might need this!");
        }

        public void removePhase(int id, int projectID)
        {
            throw new NotImplementedException("Out of Scope!");
        }      
        #endregion
        
        // Added by NVZ
        // Other Methods
        #region
        private bool verifyProjectEditable(int projectID)
        {
            throw new NotImplementedException("Out of scope!");
        }

        /*
         *  In case we want to show the projectpage for the POC. -NVZ
         */
        public List<Module> getModules(int projectID, bool details)
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
        public void handleProjectAction(int projectID, string actionName)
        {
            throw new NotImplementedException("I need this!");
        }
        #endregion
    }
}