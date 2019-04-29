using System.Collections.Generic;
using BL;
using Domain.Projects;
using Domain.Users;

namespace UIMVC.Services
{
    public class ProjectService
    {
        private readonly ProjectManager _projectManager;
        private readonly ModuleManager _moduleManager;

        public ProjectService()
        {
            _projectManager = new ProjectManager();
            _moduleManager = new ModuleManager();
        }

        public IEnumerable<Project> GetPlatformProjects(Platform platform)
        {
            return _projectManager.GetPlatformProjects(platform);
        }

        public IEnumerable<Questionnaire> GetProjectQuestionnaires(Project project)
        {
            return _moduleManager.GetQuestionnaires(project.Id);
        }

        public IEnumerable<Ideation> GetProjectIdeations(Project project)
        {
            return _moduleManager.GetIdeations(project.Id);
        }
    }
}