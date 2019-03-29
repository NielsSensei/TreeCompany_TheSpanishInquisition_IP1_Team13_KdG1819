using System.Collections.Generic;

namespace Domain.UserInput
{
    public class Vote
    {
        // Added by NG
        // Modified by EKT, XV and NVZ
        public int Id { get; set; }
        private string UserMail { get; set; }
        internal float? LocationX { get; set; }
        internal float? LocationY { get; set; }
        public bool Positive { get; set; }
        
        // Note about these properties: This is going to be the only link between Vote and Idea because
        // I think this might be useful somewhere, somehow. It all depends on how we work the interaction
        // between Idea, Vote and IoT_Device will work out. Please read my comment in Vote.cs and
        // IOT_Device.cs to read more about it. - NVZ
        // public IOT_Device IotDevice { get; set; }
        // public Idea Idea { get; set; }
        public int IdeaID { get; set; }
        public int deviceID { get; set; }
        
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