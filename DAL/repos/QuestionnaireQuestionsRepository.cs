using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Domain.UserInput;
using DAL.Contexts;

namespace DAL
{
    public class QuestionnaireQuestionsRepository //: IRepository<QuestionnaireQuestion>
    {
        // Added by DM
        // Modified by NVZ
        private List<QuestionnaireQuestion> QuestionnaireQuestions;
        private List<Answer> answers;
        private List<string> answerOptions;
        private CityOfIdeasDbContext ctx;

        // Added by NVZ
        public QuestionnaireQuestionsRepository()
        {
            ctx = new CityOfIdeasDbContext();
        }

        // Added by NVZ
        // QuestionnaireQuestion CRUD
        #region 
        public QuestionnaireQuestion Create(QuestionnaireQuestion obj)
        {
            if (!QuestionnaireQuestions.Contains(obj))
            {
                QuestionnaireQuestions.Add(obj);
            }
            throw new DuplicateNameException("This QuestionnaireQuestion already exists!");
        }
        
        public QuestionnaireQuestion Read(int id)
        {
            QuestionnaireQuestion qq = QuestionnaireQuestions.Find(q => q.Id == id);
            if (qq != null)
            {
                return qq;
            }
            throw new KeyNotFoundException("This QuesttionnaireQuestion can't be found!");
        }

        public void Update(QuestionnaireQuestion obj)
        {
            Delete(obj.Id);
            Create(obj);
        }

        public void Delete(int id)
        {
            QuestionnaireQuestion qq = Read(id);
            if (qq != null)
            {
                QuestionnaireQuestions.Remove(qq);
            }
        }
        
        public IEnumerable<QuestionnaireQuestion> ReadAll()
        {
            return QuestionnaireQuestions;
        }

        public IEnumerable<QuestionnaireQuestion> ReadAllByQuestionnaireId(int questionnaireId)
        {
            return QuestionnaireQuestions.Where(c => c.Questionnaire.Id == questionnaireId).AsEnumerable();
        }
        #endregion       
        
        // Added by NVZ
        // Answer CRUD
        //TODO: Reform this.
        #region
        public Answer Create(Answer obj)
        {
            if (!answers.Contains(obj))
            {
                answers.Add(obj);
                Read(obj.Id).Answers.Add(obj);
            }
            throw new DuplicateNameException("This Answer already exists!");
        }

        public Answer Read(int questionID, int answerID)
        {
            Answer a = Read(questionID).Answers.ToList().Find(an => an.Id == answerID);
            if (a != null)
            {
                return a;
            }
            throw new KeyNotFoundException("This Answer can't be found!");
        }

        public Answer ReadAnswer(int answerID)
        {
            Answer a = answers.Find(an => an.Id == answerID);
            if (a != null)
            {
                return a;
            }
            throw new KeyNotFoundException("This Answer can't be found!"); 
        }
        
        public void Update(Answer obj)
        {
            //Delete(obj.questionID, obj.Id);
            //Create(obj);
        }

        public void Delete(int questionID, int answerID)
        {
            Answer a = Read(questionID, answerID);
            if (a != null)
            {
                answers.Remove(a);
                Read(questionID).Answers.Remove(a);
            }
        }
        
        public IEnumerable<Answer> ReadAll(int questionID)
        {
            return Read(questionID).Answers;
        }
        #endregion
        
        
        // Added by NVZ
        // Options CRUD
        //TODO: Reform this.
        #region
        public string Create(string obj)
        {
            if (!answerOptions.Contains(obj))
            {
                answerOptions.Add(obj);
            }
            throw new DuplicateNameException("This option already exists!");
        }

        public void DeleteOption(int optionID)
        {
            answerOptions.RemoveAt(optionID-1);
        }
        
        public IEnumerable<string> ReadAllOptions()
        {
            return answerOptions;
        }
        #endregion
    }
}