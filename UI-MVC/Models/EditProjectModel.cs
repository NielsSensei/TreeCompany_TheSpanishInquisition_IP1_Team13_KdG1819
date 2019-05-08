using System;

namespace UIMVC.Models
{
    public class EditProjectModel
    {
        public string Title { get; set; }
        public string Goal { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}