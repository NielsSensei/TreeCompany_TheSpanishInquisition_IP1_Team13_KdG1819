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

        //TODO bespreek issue #22
        public Vote Vote { get; set; }
        public List<Interaction> Interactions { get; set; }
    }
}