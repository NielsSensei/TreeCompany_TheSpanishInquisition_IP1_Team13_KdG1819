using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common;
using Domain.Users;

namespace Domain.Projects
{
    public class Project
    {
        // Added by NG
        // Modified by XV & NVZ & EKT & DM
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public List<Phase> Phases { get; set; }
   
        public Platform Platform { get; set; }
        public User User { get; set; }

        public string Goal { get; set; }
        public string Status { get; set; }
        public bool Visible { get; set; }
        public int ReactionCount { get; set; }
        public int LikeCount { get; set; }
        public int FbLikeCount { get; set; }
        public int TwitterLikeCount { get; set; }
        public int LikeVisibility { get; set; }
        public Phase CurrentPhase { get; set; }

        public List<Image> PreviewImages { get; set; }
        public List<Module> Modules { get; set; }

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
            Status = "AFGESLOTEN";
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