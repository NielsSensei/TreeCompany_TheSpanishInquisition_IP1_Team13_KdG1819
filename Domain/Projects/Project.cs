using System.Collections.Generic;
using System.Linq;
using Domain.Common;
using Domain.UserInput;

namespace Domain
{
    public class Project
    {
        // Added by NG
        // Modified by XV & NVZ
        public int Id { get; set; }
        public string Title { get; set; }
        public string Goal { get; set; }
        public ICollection<Phase> Phases { get; set; }
        public bool Open { get; set; }
        public ICollection<Module> Modules { get; set; }
        public ICollection<Image> PreviewImages { get; set; }
        public Phase CurrentPhase { get; set; }
        public int myPlatformOwner { get; set; }
        
        // Added by EKT
        // Modified by NVZ
        // Methods
        #region
        
        public Project GetProjectInfo()
        {
            Project info = new Project()
            {
               Title = this.Title,
               Goal = this.Goal,
               CurrentPhase = this.CurrentPhase
            };
            return info;
        }

        public void CloseProject()
        {
            Open = false;
        }

        public void AddModule(Module module)
        {
            Modules.Add(module);
        }

        public void AddPhase(Phase phase)
        {
            Phases.Add(phase);
        }

        public void AddImage(Image image)
        {
            PreviewImages.Add(image);
        }

        public void SetCurrentPhase(int phaseID)
        {
            CurrentPhase = Phases.FirstOrDefault(p => p.Id == phaseID);
        }
        #endregion
    }
}