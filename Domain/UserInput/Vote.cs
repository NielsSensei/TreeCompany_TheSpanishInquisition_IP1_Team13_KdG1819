using System.Collections.Generic;
using Domain.Identity;

namespace Domain.UserInput
{
    /*
     * @authors Nathan Gijselings, David Matei, Edwin Kai Yin Tam, Niels Van Zandbergen & Xander Veldeman
     */
    public class Vote
    {
        public int Id { get; set; }
        public string UserMail { get; set; }
        public float? LocationX { get; set; }
        public float? LocationY { get; set; }
        public bool Positive { get; set; }
        public Idea Idea { get; set; }
        public IotDevice Device{ get; set; }
        public UimvcUser User { get; set; }
        public List<string> Choices { get; set; }
    }
}
