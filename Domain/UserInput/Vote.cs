using System.Collections.Generic;
using Domain.Identity;

namespace Domain.UserInput
{
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