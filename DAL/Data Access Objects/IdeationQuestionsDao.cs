﻿namespace DAL.Data_Access_Objects
{
    /*
     * @author Sacha Buelens & Niels Van Zandbergen
     */
    public class IdeationQuestionsDao
    {
        public int IquestionId { get; set; }
        public int ModuleId { get; set; }
        public string QuestionTitle { get; set; }
        public string Description { get; set; }
        public string WebsiteLink { get; set; }
    }
}
