using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Projects
{
    public class Phase
    {
        //Added by NG
        public int Id { get; set; }
        public Project Project { get; set; }
        public Module Module { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsCurrent { get; set; }

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