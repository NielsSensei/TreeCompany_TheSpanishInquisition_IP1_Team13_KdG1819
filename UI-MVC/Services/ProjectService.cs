using System.Collections;
using System.Collections.Generic;
using BL;
using Domain.Projects;
using Domain.UserInput;
using Domain.Users;

namespace UIMVC.Services
{
    public class ProjectService
    {
        private readonly ProjectManager _projectManager;
        private readonly ModuleManager _moduleManager;
        private readonly IdeationQuestionManager _ideationQuestionManager;
        private readonly PlatformManager _platformManager;

        public ProjectService()
        {
            _projectManager = new ProjectManager();
            _moduleManager = new ModuleManager();
            _ideationQuestionManager = new IdeationQuestionManager();
            _platformManager = new PlatformManager();
        }

        public IEnumerable<Project> CollectPlatformProjects(Platform platform)
        {
            return _projectManager.GetPlatformProjects(platform);
        }

        public IEnumerable<Questionnaire> CollectProjectQuestionnaires(Project project)
        {
            return _moduleManager.GetQuestionnaires(project.Id);
        }

        public IEnumerable<Ideation> CollectProjectIdeations(Project project)
        {
            return _moduleManager.GetIdeations(project.Id);
        }

        public IEnumerable<Idea> CollectThreadIdeas(IdeationQuestion ideationQuestion)
        {
            return _ideationQuestionManager.GetIdeas(ideationQuestion.Id);
        }

        public IEnumerable<byte[]> CollectProjectImages(Project project)
        {
            return _projectManager.GetAllImages(project.Id);
        }
        
        
        #region Breadcrumbs:Platform

        public Platform GetPlatform(Module moduleIn)
        {
            Module module = _moduleManager.GetIdeation(moduleIn.Id);
            Project project = _projectManager.GetProject(module.Project.Id, false);
            return _platformManager.GetPlatform(project.Platform.Id);
        }

        #endregion

        #region BreadCrumbs:Project

        public Project GetProject(Module moduleIn)
        {
            Module module = _moduleManager.GetIdeation(moduleIn.Id);
            return _projectManager.GetProject(module.Project.Id, false);
        }

        #endregion
    }
}
