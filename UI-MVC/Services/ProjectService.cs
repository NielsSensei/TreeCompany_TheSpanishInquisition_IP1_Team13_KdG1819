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

        public ProjectService()
        {
            _projectManager = new ProjectManager();
            _moduleManager = new ModuleManager();
            _ideationQuestionManager = new IdeationQuestionManager();
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
    }
}
