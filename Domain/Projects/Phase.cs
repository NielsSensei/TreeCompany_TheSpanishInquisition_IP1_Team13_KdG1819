using System;

namespace Domain.Projects
{
    public class Phase
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public Module Module { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
