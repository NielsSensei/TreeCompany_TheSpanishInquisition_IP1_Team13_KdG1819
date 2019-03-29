using System;
using Domain.Projects;

namespace Domain
{
    public class Phase
    {
        //Added by NG
        public int Id { get; set; }
        public string Description { get; set; }
        public Project Project { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Added by XV
        public Module Module { get; set; }

        // Added by EKT
        // Modified by NVZ
        // Methods

        #region

        public Phase GetPhaseInfo()
        {
            Phase info = new Phase()
            {
                Description = this.Description,
                StartDate = this.StartDate,
                EndDate = this.EndDate
            };
            return info;
        }

        public void ChangeDates(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        #endregion
    }
}