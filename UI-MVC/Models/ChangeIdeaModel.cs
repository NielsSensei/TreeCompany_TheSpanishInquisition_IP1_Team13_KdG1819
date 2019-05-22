using System.Collections.Generic;
using Domain.UserInput;
using Microsoft.AspNetCore.Http;

namespace UIMVC.Models
{
    public class ChangeIdeaModel
    {
        public int IdeationQuestionId { get; set; }
        public int ParentId { get; set; }
        public Idea Idea { get; set; }
        public IFormCollection FormCollection { get; set; }
        public List<string> FieldStrings { get; set; }
    }
}