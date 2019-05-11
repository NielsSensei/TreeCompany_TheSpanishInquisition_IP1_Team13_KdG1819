using System.Collections.Generic;
using Domain.Identity;

namespace Domain.Users
{
    public class Platform
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string IconImagePath { get; set; }
        public string FrontPageImagePath { get; set; }
        public string CarouselPageImagePath { get; set; }
        public List<UIMVCUser> Owners { get; set; }
        public List<UIMVCUser> Users { get; set; }
    }
}