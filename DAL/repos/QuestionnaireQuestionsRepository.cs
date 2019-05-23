using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Contexts;
using DAL.Data_Access_Objects;
using Domain.UserInput;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Projects;

namespace DAL.repos
{
    /*
     * @authors Sacha Buelens, David Matei, Edwin Kai Yin Tam, Niels Van Zandbergen & Xander Veldeman
     */
    public class QuestionnaireQuestionsRepository : IRepository<QuestionnaireQuestion>
    {
        private readonly CityOfIdeasDbContext _ctx;

        public QuestionnaireQuestionsRepository()
        {
            _ctx = new CityOfIdeasDbContext();
        }

        /*
         * @authors Sacha Buelens, Niels Van Zandbergen & Xander Veldeman
         */
        #region Conversion Methods
        private QuestionnaireQuestionsDao ConvertToDao(QuestionnaireQuestion obj)
        {
            return new QuestionnaireQuestionsDao
            {
                QquestionId = obj.Id,
                ModuleId = obj.Module.Id,
                QuestionText = obj.QuestionText,
                QType = (byte) obj.QuestionType,
                Required = obj.Optional
            };
        }

        private QuestionnaireQuestion ConvertToDomain(QuestionnaireQuestionsDao dao)
        {
            return new QuestionnaireQuestion
            {
                Id = dao.QquestionId,
                Module = new Questionnaire { Id = dao.ModuleId },
                QuestionText = dao.QuestionText,
                QuestionType = (QuestionType) dao.QType,
                Optional = dao.Required
            };
        }

        private OptionsDao ConvertToDao(int id, string obj, int qid)
        {
            return new OptionsDao
            {
                OptionId = id,
                OptionText = obj,
                QquestionId = qid
            };
        }

        private String ConvertToDomain(OptionsDao dao)
        {
            return dao.OptionText;
        }

        private AnswersDao OpenConvertToDao(OpenAnswer obj)
        {
            return new AnswersDao
            {
                AnswerId = obj.Id,
                QQuestionId = obj.Question.Id,
                UserId = obj.User.Id,
                AnswerText = obj.AnswerText,
            };
        }

        private AnswersDao MultipleConvertToDao(MultipleAnswer obj)
        {
            return new AnswersDao
            {
                AnswerId = obj.Id,
                QQuestionId = obj.Question.Id,
                UserId = obj.User.Id
            };
        }

        private ChoicesDao ConvertToDao(int optionId, int answerId, int choiceId)
        {
            return new ChoicesDao
            {
                ChoiceId = choiceId,
                AnswerId = answerId,
                OptionId = optionId
            };
        }

        private OpenAnswer ConvertToDomain(AnswersDao dao)
        {
            return new OpenAnswer
            {
                Id = dao.AnswerId,
                User = new UimvcUser { Id = dao.UserId },
                Question = new QuestionnaireQuestion { Id = dao.QQuestionId },
                IsUserEmail = dao.AnswerText.Contains("@"),
                AnswerText = dao.AnswerText
            };
        }

        private MultipleAnswer ConvertToDomain(AnswersDao answersDao, List<OptionsDao> chosenOptionsDao)
        {
            MultipleAnswer ma = new MultipleAnswer();
            ma.Id = answersDao.AnswerId;
            ma.User = new UimvcUser { Id = answersDao.UserId };
            ma.Question = new QuestionnaireQuestion { Id = answersDao.QQuestionId };
            ma.DropdownList = chosenOptionsDao.Count == 1;
            ma.Choices = new List<string>();

            foreach(OptionsDao dao in chosenOptionsDao)
            {
                ma.Choices.Add(dao.OptionText);
            }

            return ma;
        }
        #endregion

        /*
         * @author Niels Van Zandbergen
         */
        #region Id generation
        private int FindNextAvailableQQuestionId()
        {
            if (!_ctx.QuestionnaireQuestions.Any()) return 1;
            int newId = ReadAll().Max(qq => qq.Id)+1;
            return newId;
        }

        private int FindNextAvailableAnswerId()
        {
            if (!_ctx.Answers.Any()) return 1;
            int newId = _ctx.Answers.ToList().Max(answer => answer.AnswerId)+1;
            return newId;
        }

        private int FindNextAvailableOptionId()
        {
            if (!_ctx.Options.Any()) return 1;
            int newId = _ctx.Options.Max(option => option.OptionId) + 1;
            return newId;
        }
        #endregion

        /*
         * @authors Sacha Buelens, David Matei, Edwin Kai Yin Tam, Niels Van Zandbergen & Xander Veldeman
         */
        #region QuestionnaireQuestion CRUD
        public QuestionnaireQuestion Create(QuestionnaireQuestion obj)
        {
            IEnumerable<QuestionnaireQuestion> qqs = ReadAllByQuestionnaireId(obj.Questionnaire.Id);

            obj.Id = FindNextAvailableQQuestionId();

            if (obj.QuestionType == QuestionType.Drop || obj.QuestionType == QuestionType.Multi ||
                obj.QuestionType == QuestionType.Single)
            {
                foreach (string option in obj.Options)
                {
                    CreateOption(obj.Id, option);
                }
            }

            _ctx.QuestionnaireQuestions.Add(ConvertToDao(obj));
            _ctx.SaveChanges();

            return obj;
        }

        /*
         * @documentation Niels Van Zandbergen
         *
         * @params id: Integer value die de identity van het object representeert.
         * @params details: Indien we enkel een readonly kopij nodig hebben van ons object maken we gebruik
         * van AsNoTracking. Dit verhoogt performantie en verhindert ook dat er dingen worden aangepast die niet
         * aangepast mogen worden.
         *
         * @see https://docs.microsoft.com/en-us/ef/core/querying/tracking#no-tracking-queries
         * 
         */
        public QuestionnaireQuestion Read(int id, bool details)
        {
            QuestionnaireQuestionsDao questionnaireQuestionDao = details ? _ctx.QuestionnaireQuestions.AsNoTracking().First(q => q.QquestionId == id) : _ctx.QuestionnaireQuestions.First(q => q.QquestionId == id);
            ExtensionMethods.CheckForNotFound(questionnaireQuestionDao, "QuestionnaireQuestion", id);

            return ConvertToDomain(questionnaireQuestionDao);
        }

        public void Update(QuestionnaireQuestion obj)
        {
            QuestionnaireQuestionsDao newQuestionnaireQuestion = ConvertToDao(obj);
            QuestionnaireQuestionsDao foundQuestionnaireQuestion = _ctx.QuestionnaireQuestions.First(qq => qq.QquestionId == obj.Id);
            if (foundQuestionnaireQuestion != null)
            {
                if(!String.IsNullOrEmpty(newQuestionnaireQuestion.QuestionText)) foundQuestionnaireQuestion.QuestionText = newQuestionnaireQuestion.QuestionText;
                if(newQuestionnaireQuestion.QType != 0) foundQuestionnaireQuestion.QType = newQuestionnaireQuestion.QType;
                if(!String.IsNullOrEmpty(newQuestionnaireQuestion.Required.ToString())) foundQuestionnaireQuestion.Required = newQuestionnaireQuestion.Required;
            }

            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            QuestionnaireQuestionsDao toDelete = _ctx.QuestionnaireQuestions.First(qq => qq.QquestionId == id);
            _ctx.QuestionnaireQuestions.Remove(toDelete);
            _ctx.SaveChanges();
        }

        public IEnumerable<QuestionnaireQuestion> ReadAll()
        {
            List<QuestionnaireQuestion> myQuery = new List<QuestionnaireQuestion>();

            foreach (QuestionnaireQuestionsDao dao in _ctx.QuestionnaireQuestions)
            {
                myQuery.Add(ConvertToDomain(dao));
            }

            return myQuery;
        }

        public IEnumerable<QuestionnaireQuestion> ReadAllByQuestionnaireId(int questionnaireId)
        {
            return ReadAll().Where(c => c.Module.Id == questionnaireId);
        }
        #endregion

        /*
         * @authors Sacha Buelens, David Matei, Edwin Kai Yin Tam, Niels Van Zandbergen & Xander Veldeman
         */
        #region Answer CRUD
        public Answer Create(Answer obj)
        {
            QuestionnaireQuestion qq = Read(obj.Question.Id, true);
            obj.Id = FindNextAvailableAnswerId();

            if(qq.QuestionType == QuestionType.Open || qq.QuestionType == QuestionType.Mail)
            {
                _ctx.Answers.Add(OpenConvertToDao((OpenAnswer) obj));
            }else
            {
                MultipleAnswer ma = (MultipleAnswer)obj;
                _ctx.Answers.Add(MultipleConvertToDao(ma));
                foreach(String s in ma.Choices)
                {
                    int id = ReadOptionId(s, ma.Question.Id);
                    _ctx.Choices.Add(ConvertToDao(id,ma.Id,_ctx.Choices.AsNoTracking().Count()+1));
                    _ctx.SaveChanges();
                }
            }

            _ctx.SaveChanges();

            return obj;
        }

        /*
         * @documentation Niels Van Zandbergen
         *
         * @params answerId: Integer value die de identity van het object representeert.
         * @params details: Indien we enkel een readonly kopij nodig hebben van ons object maken we gebruik
         * van AsNoTracking. Dit verhoogt performantie en verhindert ook dat er dingen worden aangepast die niet
         * aangepast mogen worden.
         *
         * @see https://docs.microsoft.com/en-us/ef/core/querying/tracking#no-tracking-queries
         * 
         */
        public OpenAnswer ReadOpenAnswer(int answerId, bool details)
        {
            AnswersDao answersDao = details ? _ctx.Answers.AsNoTracking().First(i => i.AnswerId == answerId) : _ctx.Answers.First(i => i.AnswerId == answerId);
            ExtensionMethods.CheckForNotFound(answersDao, "Answer", answerId);

            return ConvertToDomain(answersDao);
        }

        /*
         * @documentation Niels Van Zandbergen
         *
         * @params answerId: Integer value die de identity van het object representeert.
         * @params details: Indien we enkel een readonly kopij nodig hebben van ons object maken we gebruik
         * van AsNoTracking. Dit verhoogt performantie en verhindert ook dat er dingen worden aangepast die niet
         * aangepast mogen worden.
         *
         * @see https://docs.microsoft.com/en-us/ef/core/querying/tracking#no-tracking-queries
         * 
         */
        public MultipleAnswer ReadMultipleAnswer(int answerId, bool details)
        {
            AnswersDao answersDao = details ? _ctx.Answers.AsNoTracking().First(i => i.AnswerId == answerId) : _ctx.Answers.First(i => i.AnswerId == answerId);
            ExtensionMethods.CheckForNotFound(answersDao, "Answer", answerId);

            List<OptionsDao> optionsDaos = _ctx.Options.ToList().FindAll(o => o.QquestionId == answersDao.QQuestionId);
            List<OptionsDao> chosenOptionsDao = new List<OptionsDao>();

            List<ChoicesDao> choices = _ctx.Choices.Where(dao => dao.AnswerId == answersDao.AnswerId).ToList();

            foreach (ChoicesDao choicesDao in choices)
            {
                chosenOptionsDao.AddRange(optionsDaos.Where(dao => dao.OptionId == choicesDao.OptionId).ToList());
            }
            
            return ConvertToDomain(answersDao, chosenOptionsDao);
        }

        public IEnumerable<Answer> ReadAll(int questionId)
        {
            List<Answer> myQuery = new List<Answer>();

            foreach (AnswersDao dao in _ctx.Answers.ToList().FindAll(a => a.QQuestionId == questionId))
            {
                if (!_ctx.Choices.Any(c => c.AnswerId == dao.AnswerId) && dao.AnswerText != null)
                {
                    myQuery.Add(ConvertToDomain(dao));
                }
                else
                {
                    MultipleAnswer toAdd = ReadMultipleAnswer(dao.AnswerId, true);
                    myQuery.Add(toAdd);
                }
            }

            return myQuery;
        }
        #endregion

        /*
         * @authors Sacha Buelens, David Matei, Edwin Kai Yin Tam, Niels Van Zandbergen & Xander Veldeman
         */
        #region Options CRUD
        public string CreateOption(int questionId, string obj)
        {
            IEnumerable<string> options = ReadAllOptionsForQuestion(questionId);
            int newId = FindNextAvailableOptionId();

            _ctx.Options.Add(ConvertToDao(newId, obj, questionId));
            _ctx.SaveChanges();

            return obj;
        }
        
        public String ReadOption(int optionId)
        {
            return ConvertToDomain(_ctx.Options.Find(optionId));
        }
        
        public int ReadOptionId(string optionText, int questionId)
        {
            OptionsDao option = _ctx.Options.FirstOrDefault(o => o.QquestionId == questionId && o.OptionText == optionText);
            return option.OptionId;
        }

        public void DeleteOption(int optionId)
        {
            OptionsDao toDelete = _ctx.Options.First(o => o.OptionId == optionId);
            _ctx.Options.Remove(toDelete);
            _ctx.SaveChanges();
        }

        public IEnumerable<string> ReadAllOptions()
        {
            List<string> myQuery = new List<string>();

            foreach(OptionsDao dao in _ctx.Options)
            {
                myQuery.Add(ConvertToDomain(dao));
            }

            return myQuery;
        }

        public IEnumerable<string> ReadAllOptionsForQuestion(int questionId)
        {
            List<string> myQuery = new List<string>();

            foreach (OptionsDao dao in _ctx.Options)
            {
                if (dao.QquestionId == questionId)
                {
                    myQuery.Add(ConvertToDomain(dao));
                }
            }

            return myQuery;
        }
        #endregion
    }
}
