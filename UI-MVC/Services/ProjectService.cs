using System.Collections;
using System.Collections.Generic;
using BL;
using Domain.Projects;
using Domain.UserInput;
using Domain.Users;

namespace UIMVC.Services
{
    /**
     * @authors Niels Van Zandbergen & Xander Veldeman
     *
     * Used as a way to get information from projects in views
     */
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

        public bool CollectVoteSettings(IdeationQuestion ideationQuestion)
        {
            Ideation ideation = _moduleManager.GetIdeation(ideationQuestion.Ideation.Id, true);

            return ideation.UserVote;
        }

        public IEnumerable<byte[]> CollectProjectImages(Project project)
        {
            return _projectManager.GetAllImages(project.Id);
        }

        public IEnumerable<Event> CollectPlatformEvents(Platform platform)
        {
            return _platformManager.GetAllEvents(platform.Id);
        }

        #region Breadcrumbs

        public Platform CollectPlatformForModule(Module moduleIn)
        {
            Module module = null;
            if (moduleIn.GetType() == typeof(Questionnaire))
            {
                module = _moduleManager.GetQuestionnaire(moduleIn.Id, false);
            }
            else if (moduleIn.GetType() == typeof(Ideation))
            {
                module = _moduleManager.GetIdeation(moduleIn.Id, false);
            }
            
            Project project = _projectManager.GetProject(module.Project.Id, false);
            return _platformManager.GetPlatform(project.Platform.Id, false);
        }

        public Platform CollectPlatformForProject(Project project)
        {
            return _platformManager.GetPlatform(project.Platform.Id, false);
        }

        public Project CollectProjectForModule(Module moduleIn)
        {
            Module module = null;
            if (moduleIn.GetType() == typeof(Questionnaire))
            {
                module = _moduleManager.GetQuestionnaire(moduleIn.Id, false);
            }
            else if (moduleIn.GetType() == typeof(Ideation))
            {
                module = _moduleManager.GetIdeation(moduleIn.Id, false);
            }
            return _projectManager.GetProject(module.Project.Id, false);
        }

        public Ideation CollectIdeationForQuestion(IdeationQuestion ideationQuestion)
        {
            return _moduleManager.GetIdeation(ideationQuestion.Ideation.Id, false);
        }

        #endregion
    }
}
