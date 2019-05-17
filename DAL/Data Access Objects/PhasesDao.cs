﻿using System;

namespace DAL.Data_Access_Objects
{
    public class PhasesDao
    {
        public int PhaseId { get; set; }
        public int ProjectId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
