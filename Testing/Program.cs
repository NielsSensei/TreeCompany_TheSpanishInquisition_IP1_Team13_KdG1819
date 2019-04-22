using System;
using System.Linq;
using BL;
using DAL.repos;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            IdeationQuestionManager mgr = new IdeationQuestionManager();
            IdeationQuestionsRepository repo = new IdeationQuestionsRepository();
            
            
            Console.WriteLine(repo.ReadAllIdeas().Count());
            Console.WriteLine(mgr.GetIdeas().Count);
        }
    }
}