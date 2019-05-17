using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace UIMVC.Models
{
    public class AddIdeaModel
    {
        public int IdeationQuestionId { get; set; }
        public int ParentId { get; set; }
        public IFormCollection FormCollection { get; set; }
        public List<string> FieldStrings { get; set; }
    }
}