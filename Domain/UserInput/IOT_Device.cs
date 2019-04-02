using System.Collections.Generic;

namespace Domain.UserInput
{
    public class IOT_Device
    {
        // Added by NG
        // Modified by XV & NVZ
        public int Id { get; set; }
        public double LocationX { get; set; }
        public double LocationY { get; set; }

        public Vote Vote { get; set; }
        public List<Interaction> Interactions { get; set; }

        // We can use the location properties to link these two then again like I noted in User.cs,
        // keeping as less data as possible about physical voting is the better because we can sort of
        // track it back to the user which is not Tree Company their intention. Feel free to give me 
        // feedback about this - NVZ
        //public ICollection<Vote> Votes { get; set; }
    }
}