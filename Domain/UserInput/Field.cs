
namespace Domain.UserInput
{
    public class Field
    {
        // Added by NG
        // Modified by EKT & NVZ & DM
        public int Id { get; set; }
        public bool Required { get; set; }
        public string Text { get; set; }
        public int TextLength { get; set; }
        public Idea Idea{ get; set; }
        
    }
}