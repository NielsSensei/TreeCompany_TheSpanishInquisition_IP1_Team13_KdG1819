using System;
using Domain.Projects;

namespace UIMVC.Models
{
    public class EditProjectModel
    {
        public string Title { get; set; }
        public string Goal { get; set; }
        public LikeVisibility LikeVisibility { get; set; }
        public string Status { get; set; }
        public bool Visible { get; set; }
    }
}