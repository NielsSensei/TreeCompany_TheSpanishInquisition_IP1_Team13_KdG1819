using System.Collections.Generic;
using Domain.Common;
using Domain.Identity;

namespace Domain.Users
{
    public class Platform
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Image Image { get; set; }
        public List<UimvcUser> Owners { get; set; }
        public List<UimvcUser> Users { get; set; }
    }
}