using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Common;
using Domain.Identity;
using Domain.Users;

namespace Domain.Projects
{
    public class Project
    {
        public int Id { get; set; }
        public Platform Platform { get; set; }
        public UimvcUser User { get; set; }
        public string Title { get; set; }
        public string Goal { get; set; }
        public string Status { get; set; }
        public bool Visible { get; set; }
        public int ReactionCount { get; set; }
        public int LikeCount { get; set; }
        public int FbLikeCount { get; set; }
        public int TwitterLikeCount { get; set; }
        public int LikeVisibility { get; set; }
        public Phase CurrentPhase { get; set; }
        public List<Phase> Phases { get; set; }
        public List<Image> PreviewImages { get; set; }
        public List<Module> Modules { get; set; }
    }
}