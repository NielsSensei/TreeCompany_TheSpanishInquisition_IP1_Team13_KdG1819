using System.Collections.Generic;
using System.Linq;
using DAL.repos;
using Domain.Projects;
using Domain.Users;

namespace BL
{
    /*
     * @authors Edwin Kai Yin Tam, Niels Van Zandbergen & Xander Veldeman
     */
    public class ProjectManager
    {
        private ProjectRepository ProjectRepo { get; }
        public ModuleManager ModuleMan { get; }

        public ProjectManager()
        {
            ProjectRepo = new ProjectRepository();
            ModuleMan = new ModuleManager();
        }

        /*
         * @authors Edwin Kai Yin Tam & Niels Van Zandbergen
         */
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

        public Project MakeProject(Project project)
        {
            Project newProject = ProjectRepo.Create(project);

            newProject.CurrentPhase = project.CurrentPhase;
            newProject.CurrentPhase.Project = newProject;

            ProjectRepo.Create(newProject.CurrentPhase);

            ProjectRepo.Update(newProject);

            return newProject;

        }

        public void RemoveProject(int projectId)
        {
            ProjectRepo.Delete(projectId);
        }

        public void MakeProjectImage(byte[] img, int project)
        {
            ProjectRepo.Create(img, project);
        }

        public void RemoveImagesForProject(int id)
        {
            ProjectRepo.DeleteImages(id);
        }

        public IEnumerable<byte[]> GetAllImages(int projectId)
        {
            return ProjectRepo.ReadAllImages(projectId);
        }
        #endregion

        /*
         * @authors Edwin Kai Yin Tam & Niels Van Zandbergen
         */
        #region Phase
        public void EditPhase(Phase phase)
        {
            ProjectRepo.Update(phase);
        }

        public IEnumerable<Phase> GetAllPhases(int projectId)
        {
            return ProjectRepo.ReadAllPhases(projectId);
        }

        public Phase GetPhase(int phaseId, bool details)
        {
            return ProjectRepo.ReadPhase(phaseId, details);
        }

        public void MakePhase(Phase newPhase)
        {
            ProjectRepo.Create(newPhase);
        }

        public void RemovePhase(int phaseId)
        {
            ProjectRepo.DeletePhase(phaseId);
        }
        #endregion

        /*
         * @author Xander Veldeman
         */
        #region PlatformMethods
        public IEnumerable<Project> GetPlatformProjects(Platform platform)
        {
            return ProjectRepo.ReadAllForPlatform(platform.Id);
        }
        
        public IEnumerable<Project> GetPlatformProjects(int platformId)
        {
            return ProjectRepo.ReadAllForPlatform(platformId);
        }
        #endregion
    }
}
