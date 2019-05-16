using Microsoft.AspNetCore.Http;

namespace UIMVC.Models
{
    public class AddIdeaModel
    {
        public int IdeationQuestionId { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
        public string FieldText { get; set; }
        public string FieldVideo { get; set; }
        public IFormFile FieldImage { get; set; }
    }
}
