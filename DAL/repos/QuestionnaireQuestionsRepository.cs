using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL.Contexts;
using DAL.Data_Access_Objects;
using Domain.UserInput;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Projects;

namespace DAL.repos
{
    public class QuestionnaireQuestionsRepository : IRepository<QuestionnaireQuestion>
    {
        private readonly CityOfIdeasDbContext _ctx;

        public QuestionnaireQuestionsRepository()
        {
            _ctx = new CityOfIdeasDbContext();
        }

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
            MultipleAnswer ma = null;
            ma.Id = answersDao.AnswerId;
            ma.User = new UimvcUser { Id = answersDao.UserId };
            ma.Question = new QuestionnaireQuestion { Id = answersDao.QQuestionId };
            ma.DropdownList = chosenOptionsDao.Count == 1;

            foreach(OptionsDao dao in chosenOptionsDao)
            {
                ma.Choices.Add(dao.OptionText);
            }

            return ma;
        }
        #endregion

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
            int newId = ReadAll().Max(answer => answer.Id)+1;
            return newId;
        }
        #endregion

        #region QuestionnaireQuestion CRUD
        public QuestionnaireQuestion Create(QuestionnaireQuestion obj)
        {
            IEnumerable<QuestionnaireQuestion> qqs = ReadAllByQuestionnaireId(obj.Questionnaire.Id);

            foreach (QuestionnaireQuestion qq in qqs)
            {
                if (ExtensionMethods.HasMatchingWords(obj.QuestionText, qq.QuestionText) > 0)
                {
                    throw new DuplicateNameException("QuestionnaireQuestion(ID=" + obj.Id + ") is een gelijkaardige vraag aan QuestionnaireQuestion(ID=" +
                        qq.Id + ") de vraag specifiek was: " + obj.QuestionText + ".");
                }
            }

            obj.Id = FindNextAvailableQQuestionId();
            _ctx.QuestionnaireQuestions.Add(ConvertToDao(obj));
            _ctx.SaveChanges();

            return obj;
        }

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
                foundQuestionnaireQuestion.QuestionText = newQuestionnaireQuestion.QuestionText;
                foundQuestionnaireQuestion.QType = newQuestionnaireQuestion.QType;
                foundQuestionnaireQuestion.Required = newQuestionnaireQuestion.Required;
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

        #region Answer CRUD
        public Answer Create(Answer obj)
        {
            QuestionnaireQuestion qq = Read(obj.Question.Id, false);
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
                    _ctx.Choices.Add(ConvertToDao(id,ma.Id,_ctx.Choices.Count()+1));
                }
            }

            _ctx.SaveChanges();

            return obj;
        }


        public OpenAnswer ReadOpenAnswer(int answerId, bool details)
        {
            AnswersDao answersDao = details ? _ctx.Answers.AsNoTracking().First(i => i.AnswerId == answerId) : _ctx.Answers.First(i => i.AnswerId == answerId);
            ExtensionMethods.CheckForNotFound(answersDao, "Answer", answerId);

            return ConvertToDomain(answersDao);
        }

        public MultipleAnswer ReadMultipleAnswer(int answerId, bool details)
        {
            AnswersDao answersDao = details ? _ctx.Answers.AsNoTracking().First(i => i.AnswerId == answerId) : _ctx.Answers.First(i => i.AnswerId == answerId);
            ExtensionMethods.CheckForNotFound(answersDao, "Answer", answerId);

            List<OptionsDao> optionsDaos = _ctx.Options.ToList().FindAll(o => o.QquestionId == answersDao.QQuestionId);
            List<OptionsDao> chosenOptionsDao = new List<OptionsDao>();

            foreach(OptionsDao dao in optionsDaos)
            {
                ChoicesDao choice = _ctx.Choices.First(c => c.OptionId == dao.OptionId);

                if(choice.ChoiceId != null)
                {
                    chosenOptionsDao.Add(dao);
                }
            }

            return ConvertToDomain(answersDao, chosenOptionsDao);
        }

        public IEnumerable<Answer> ReadAll(int questionId)
        {
            List<Answer> myQuery = new List<Answer>();

            foreach (AnswersDao dao in _ctx.Answers.ToList().FindAll(a => a.QQuestionId == questionId))
            {
                if (!_ctx.Choices.Where(c => c.AnswerId == dao.AnswerId).Any())
                {
                    myQuery.Add(ConvertToDomain(dao));
                }
                else
                {
                    MultipleAnswer toAdd = ReadMultipleAnswer(dao.AnswerId, false);
                    myQuery.Add(toAdd);
                }
            }

            return myQuery;
        }
        #endregion

        #region Options CRUD
        public string Create(int questionId, string obj)
        {
            IEnumerable<string> options = ReadAllOptions(questionId);
            int newId = options.Count() + 1;

            for (int i = 0; i < options.Count(); i++)
            {
                if (ExtensionMethods.HasMatchingWords(obj, options.ElementAt(i)) > 0)
                {
                    throw new DuplicateNameException("Deze Option(ID=" + newId + ") met Optiontekst: " + obj + " is gelijkaardig aan de Option(ID=" + i +
                        "). De Optiontekst is: " + options.ElementAt(i) + ".");
                }
            }

            _ctx.Options.Add(ConvertToDao(newId, obj, questionId));
            _ctx.SaveChanges();

            return obj;
        }

        public String ReadOption(int optionId, int questionID)
        {
            return ConvertToDomain(_ctx.Options.Find(optionId));
        }

        public int ReadOptionId(string optionText, int questionId)
        {
            List<string> options = ReadAllOptions(questionId).ToList();
            for(int i = 0; i < options.Count; i++)
            {
                if (options[i].Equals(optionText))
                {
                    return i + 1;
                }
            }
            throw new DuplicateNameException("Option " + optionText + " niet gevonden voor de QuestionnaireQuestion(ID=" + questionId + ").");
        }

        public void DeleteOption(int optionId)
        {
            OptionsDao toDelete = _ctx.Options.First(o => o.OptionId == optionId);
            _ctx.Options.Remove(toDelete);
            _ctx.SaveChanges();
        }

        public IEnumerable<string> ReadAllOptions(int questionId)
        {
            List<string> myQuery = new List<string>();

            foreach (OptionsDao dao in _ctx.Options)
            {
                if (dao.QquestionId == questionId)
                {
                    myQuery.Append(ConvertToDomain(dao));
                }
            }

            return myQuery;
        }
        #endregion
    }
}
