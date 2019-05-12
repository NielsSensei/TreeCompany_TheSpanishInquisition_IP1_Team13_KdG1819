using System.Collections.Generic;
using System.Linq;
using DAL.repos;
using Domain.Projects;
using Domain.Users;

namespace BL
{
    public class ProjectManager
    {
        private ProjectRepository ProjectRepo { get; }
        public ModuleManager ModuleMan { get; }
        
        public ProjectManager()
        {
            ProjectRepo = new ProjectRepository();
            ModuleMan = new ModuleManager();
        }
        
        public Project MakeProject(Project project)
        {
            
            Project newProject = ProjectRepo.Create(project);

            project.Id = newProject.Id;
            
            ProjectRepo.Create(project.CurrentPhase);
            
            return GetProject(newProject.Id, false);
        }

        
        #region Project
        public void EditProject(Project project)
        {
            ProjectRepo.Update(project);
        }
        
        public Project GetProject(int projectId, bool details)
        {
            Project project = ProjectRepo.Read(projectId, details);

            List<Phase> phases = new List<Phase>();
            phases = ProjectRepo.ReadAllPhases(projectId).ToList();

            project.Phases = phases;

            return project;
        }


        public void RemoveProject(int projectId)
        {
            ProjectRepo.Delete(projectId);
        }

        public IEnumerable<Project> GetAllProjectsForPlatform(int platformId)
        {
            return ProjectRepo.ReadAllForPlatform(platformId);
        }

        #endregion
        
        #region Phase
        public void EditPhase(Phase phase)
        {
            ProjectRepo.Update(phase);
        }
        
        public IEnumerable<Phase> GetAllPhases(int projectId)
        {
            return ProjectRepo.ReadAllPhases(projectId);
        }

        public Phase MakePhase(Phase newPhase, int projectId)
        {
            Phase savedPhase = ProjectRepo.Create(newPhase);
            var alteredProject = ProjectRepo.Read(projectId, false);
            alteredProject.Phases.Add(savedPhase);
            ProjectRepo.Update(alteredProject);
            if (newPhase.Module != null)
            {
                var alteredModule = ModuleMan.GetQuestionnaire(newPhase.Module.Id, false);
                alteredModule.Phases.Add(newPhase);
                //ModuleMan.EditModule(alteredModule);
            }

            return savedPhase;
        }

        public void RemovePhase(int projectId, int phaseId)
        {
            var removedPhase = ProjectRepo.ReadPhase(phaseId, false);
            var alteredProject = ProjectRepo.Read(projectId, false);
            alteredProject.Phases.Remove(removedPhase);
            ProjectRepo.DeletePhase(removedPhase.Id);
        }

        public Phase GetPhase(int phaseId)
        {
            return ProjectRepo.ReadPhase(phaseId, false);
        }

        #endregion

        // Added by NVZ
        // Other Methods

        #region

        /*
        private bool VerifyProjectEditable(int projectId)
        {
            throw new NotImplementedException("Out of scope!");
        } */

        /*
         *  In case we want to show the projectpage for the POC. -NVZ
         
        public List<Module> GetModules(int projectId, bool details)
        {
            throw new NotImplementedException("I might need this!");
        } */

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
        public void HandleProjectAction(int projectId, string actionName)
        {
            throw new NotImplementedException("I need this!");
        } */

        #endregion
        
        // Added by XV
        // Methods for Platform

        #region PlatformMethods

        public IEnumerable<Project> GetPlatformProjects(Platform platform)
        {
            return ProjectRepo.ReadAllForPlatform(platform.Id);
        }

        #endregion
    }
}