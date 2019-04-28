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
        }

        public IEnumerable<Project> GetPlatformProjects(Platform platform)
        {
            return _projectManager.GetPlatformProjects(platform);
        }
    }
}