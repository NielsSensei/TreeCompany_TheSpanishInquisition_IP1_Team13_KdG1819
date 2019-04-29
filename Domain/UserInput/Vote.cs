using System.Collections.Generic;

namespace Domain.UserInput
{
    public class Vote
    {
        // Added by NG
        // Modified by EKT, XV and NVZ & DM
        public int Id { get; set; }
        public string UserMail { get; set; }
        public float? LocationX { get; set; }
        public float? LocationY { get; set; }
        public bool Positive { get; set; }
        public Idea Idea { get; set; }
        public IOT_Device Device{ get; set; }

        public List<string> Choices { get; set; }

        // Added by EKT
        // Modified by NVZ
        // Methods
        #region

        public void SetMail(string mail)
        {
            UserMail = mail;
        }

        public void SetLocation(List<float> coordinates)
        {
            LocationX = coordinates[0];
            LocationY = coordinates[1];
        }

        public List<float> GetLocation()
        {
            var coordinates = new List<float>();
            
            if (LocationX.HasValue && LocationY.HasValue)
            {
                coordinates[0] = LocationX.Value;
                coordinates[1] = LocationY.Value;
            }

            return coordinates;

        }
        #endregion
    }
}