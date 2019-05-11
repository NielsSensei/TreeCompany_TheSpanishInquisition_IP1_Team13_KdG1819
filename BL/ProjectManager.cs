using System.Collections.Generic;
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
        
        #region Project
        public void EditProject(Project project)
        {
            ProjectRepo.Update(project);
        }
        
        public Project GetProject(int projectId, bool details)
        {
            return ProjectRepo.Read(projectId, details);
        }
        
        public void MakeProject(Project project)
        {
            ProjectRepo.Create(project);
        }

        public void RemoveProject(int projectId)
        {
            ProjectRepo.Delete(projectId);
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

        public Phase GetPhase(int phaseId)
        {
            return ProjectRepo.ReadPhase(phaseId, false);
        }
        
        public void MakePhase(Phase newPhase, int projectId)
        {
            ProjectRepo.Create(newPhase);
            var alteredProject = ProjectRepo.Read(projectId, false);
            alteredProject.Phases.Add(newPhase);
            ProjectRepo.Update(alteredProject);
            if (newPhase.Module != null)
            {
                var alteredModule = ModuleMan.GetQuestionnaire(newPhase.Module.Id, false);
                alteredModule.Phases.Add(newPhase);
            }
        }
        
        public void RemovePhase(int projectId, int phaseId)
        {
            var removedPhase = ProjectRepo.ReadPhase(phaseId, false);
            var alteredProject = ProjectRepo.Read(projectId, false);
            alteredProject.Phases.Remove(removedPhase);
            if (removedPhase.Module != null)
            {
                var alteredModule = ModuleMan.GetQuestionnaire(removedPhase.Module.Id, false);
                alteredModule.Phases.Remove(removedPhase);
            }
            ProjectRepo.Delete(phaseId);
        }
        #endregion
        
        #region PlatformMethods
        public IEnumerable<Project> GetPlatformProjects(Platform platform)
        {
            return ProjectRepo.ReadAllForPlatform(platform.Id);
        }
        #endregion
    }
}