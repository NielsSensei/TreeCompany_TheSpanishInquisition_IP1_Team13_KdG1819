using System.Collections.Generic;
using Domain.Identity;

namespace Domain.Users
{
    public class Platform
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public List<UimvcUser> Owners { get; set; }
        public List<UimvcUser> Users { get; set; }
        public byte[] IconImage { get; set; }
        public byte[] CarouselImage { get; set; }
        public byte[] FrontPageImage { get; set; }
    }
}
