using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data_Transfer_Objects
{
    public class UserDetailsDTO
    {
        public int UserID { get; set; }
        public string Zipcode { get; set; }
        public bool Banned { get; set; }
        public byte Gender { get; set; }
        public bool Active { get; set; }
        public DateTime BirthDate { get; set; }
        public string OrgName { get; set; }
        public string Description { get; set; }
    }
}
