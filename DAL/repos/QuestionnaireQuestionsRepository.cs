using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Domain.UserInput;
using DAL.Contexts;
using DAL.Data_Transfer_Objects;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Projects;
using Domain.Users;

namespace DAL
{
    public class QuestionnaireQuestionsRepository : IRepository<QuestionnaireQuestion>
    {
        // Added by DM
        // Modified by NVZ
        private readonly CityOfIdeasDbContext ctx;

        // Added by NVZ
        public QuestionnaireQuestionsRepository()
        {
            ctx = new CityOfIdeasDbContext();
        }

        // Added by NVZ
        // Standard Methods
        #region
        private QuestionnaireQuestionsDTO ConvertToDTO(QuestionnaireQuestion obj)
        {
            return new QuestionnaireQuestionsDTO
            {
                QQuestionID = obj.Id,
                ModuleID = obj.Module.Id,
                QuestionText = obj.QuestionText,
                QType = (byte) obj.QuestionType,
                Required = obj.Optional
            };
        }

        private QuestionnaireQuestion ConvertToDomain(QuestionnaireQuestionsDTO DTO)
        {
            return new QuestionnaireQuestion
            {
                Id = DTO.QQuestionID,
                Module = new Questionnaire { Id = DTO.ModuleID },
                QuestionText = DTO.QuestionText,
                QuestionType = (QuestionType) DTO.QType,
                Optional = DTO.Required
            };
        }

        private OptionsDTO ConvertToDTO(int id, string obj, int qID)
        {
            return new OptionsDTO
            {
                OptionID = id,
                OptionText = obj,
                QQuestionID = qID
            };
        }

        private String ConvertToDomain(OptionsDTO DTO)
        {
            return DTO.OptionText;
        }

        private AnswersDTO OpenConvertToDTO(OpenAnswer obj)
        {
            return new AnswersDTO
            {
                AnswerID = obj.Id,
                QQuestionID = obj.Question.Id,
                UserID = obj.User.Id,
                AnswerText = obj.AnswerText,
            };
        }

        private AnswersDTO MultipleConvertToDTO(MultipleAnswer obj)
        {
            return new AnswersDTO
            {
                AnswerID = obj.Id,
                QQuestionID = obj.Question.Id,
                UserID = obj.User.Id
            };
        }

        private ChoicesDTO ConvertToDTO(int optionID, int answerID, int choiceID)
        {
            return new ChoicesDTO
            {
                ChoiceID = choiceID,
                AnswerID = answerID,
                OptionID = optionID
            };
        }

        private OpenAnswer ConvertToDomain(AnswersDTO DTO)
        {
            return new OpenAnswer
            {
                Id = DTO.AnswerID,
                User = new UIMVCUser { Id = DTO.UserID },
                Question = new QuestionnaireQuestion { Id = DTO.QQuestionID },
                IsUserEmail = DTO.AnswerText.Contains("@"),
                AnswerText = DTO.AnswerText
            };
        }

        private MultipleAnswer ConvertToDomain(AnswersDTO answersDTO, List<OptionsDTO> chosenOptionsDTO)
        {
            MultipleAnswer ma = null;
            ma.Id = answersDTO.AnswerID;
            ma.User = new UIMVCUser { Id = answersDTO.UserID };
            ma.Question = new QuestionnaireQuestion { Id = answersDTO.QQuestionID };
            ma.DropdownList = chosenOptionsDTO.Count == 1;

            foreach(OptionsDTO DTO in chosenOptionsDTO)
            {
                ma.Choices.Add(DTO.OptionText);
            }

            return ma;
        }
        
        private int FindNextAvailableQQuestionId()
        {               
            int newId = ReadAll().Max(qq => qq.Id)+1;
            return newId;
        }
        
        private int FindNextAvailableAnswerId()
        {               
            int newId = ReadAll().Max(answer => answer.Id)+1;
            return newId;
        }
        #endregion

        // Added by NVZ
        // QuestionnaireQuestion CRUD
        #region 
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
            ctx.QuestionnaireQuestions.Add(ConvertToDTO(obj));
            ctx.SaveChanges();

            return obj;
        }
        
        public QuestionnaireQuestion Read(int id, bool details)
        {
            QuestionnaireQuestionsDTO questionnaireQuestionDTO = null;
            questionnaireQuestionDTO = details ? ctx.QuestionnaireQuestions.AsNoTracking().First(q => q.QQuestionID == id) : ctx.QuestionnaireQuestions.First(q => q.QQuestionID == id);
            ExtensionMethods.CheckForNotFound(questionnaireQuestionDTO, "QuestionnaireQuestion", id);

            return ConvertToDomain(questionnaireQuestionDTO);
        }

        public void Update(QuestionnaireQuestion obj)
        {
            QuestionnaireQuestionsDTO newQuestionnaireQuestion = ConvertToDTO(obj);
            QuestionnaireQuestionsDTO foundQuestionnaireQuestion = ctx.QuestionnaireQuestions.First(qq => qq.QQuestionID == obj.Id);
            if (foundQuestionnaireQuestion != null)
            {
                foundQuestionnaireQuestion.QuestionText = newQuestionnaireQuestion.QuestionText;
                foundQuestionnaireQuestion.QType = newQuestionnaireQuestion.QType;
                foundQuestionnaireQuestion.Required = newQuestionnaireQuestion.Required;
            }

            ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            QuestionnaireQuestionsDTO toDelete = ctx.QuestionnaireQuestions.First(qq => qq.QQuestionID == id);
            ctx.QuestionnaireQuestions.Remove(toDelete);
            ctx.SaveChanges();
        }
        
        public IEnumerable<QuestionnaireQuestion> ReadAll()
        {
            List<QuestionnaireQuestion> myQuery = new List<QuestionnaireQuestion>();

            foreach (QuestionnaireQuestionsDTO DTO in ctx.QuestionnaireQuestions)
            {
                myQuery.Add(ConvertToDomain(DTO));
            }

            return myQuery;
        }

        public IEnumerable<QuestionnaireQuestion> ReadAllByQuestionnaireId(int questionnaireId)
        {
            return ReadAll().Where(c => c.Questionnaire.Id == questionnaireId);
        }
        #endregion       
        
        // Added by NVZ
        // Answer CRUD
        /*
        Hier mogen dubbele antwoorden aangezien niemand van elkaar hoort te weten wat de antwoorden zijn. De admins hebben natuurlijk een overzichtje maar
        zij zijn de enige. Hier is er totaal geen controle op dubbele dingen zoals de rest van de objecten.
        */
        #region
        public Answer Create(Answer obj)
        {
            QuestionnaireQuestion qq = Read(obj.Question.Id, false);
            obj.Id = FindNextAvailableAnswerId();
            
            if(qq.QuestionType == QuestionType.OPEN || qq.QuestionType == QuestionType.MAIL)
            {
                ctx.Answers.Add(OpenConvertToDTO((OpenAnswer) obj));
            }else
            {
                MultipleAnswer ma = (MultipleAnswer)obj;
                ctx.Answers.Add(MultipleConvertToDTO(ma));
                foreach(String s in ma.Choices)
                {
                    int id = ReadOptionID(s, ma.Question.Id);
                    ctx.Choices.Add(ConvertToDTO(id,ma.Id,ctx.Choices.Count()+1));
                }
            }
           
            ctx.SaveChanges();

            return obj;
        }

        
        public OpenAnswer ReadOpenAnswer(int answerID, bool details)
        {
            AnswersDTO answersDTO = null;
            answersDTO = details ? ctx.Answers.AsNoTracking().First(i => i.AnswerID == answerID) : ctx.Answers.First(i => i.AnswerID == answerID);
            ExtensionMethods.CheckForNotFound(answersDTO, "Answer", answerID);

            return ConvertToDomain(answersDTO);
        }

        public MultipleAnswer ReadMultipleAnswer(int answerID, bool details)
        {
            AnswersDTO answersDTO = null;
            answersDTO = details ? ctx.Answers.AsNoTracking().First(i => i.AnswerID == answerID) : ctx.Answers.First(i => i.AnswerID == answerID);
            ExtensionMethods.CheckForNotFound(answersDTO, "Answer", answerID);

            List<ChoicesDTO> choicesDTO = ctx.Choices.ToList().FindAll(c => c.AnswerID == answerID);
            List<OptionsDTO> optionsDTO = ctx.Options.ToList().FindAll(o => o.QQuestionID == answersDTO.QQuestionID);
            List<OptionsDTO> chosenOptionsDTO = new List<OptionsDTO>();

            foreach(OptionsDTO DTO in optionsDTO)
            {
                ChoicesDTO choice = ctx.Choices.First(c => c.OptionID == DTO.OptionID);   
                
                if(choice.ChoiceID != null)
                {
                    chosenOptionsDTO.Add(DTO);
                }
            }

            return ConvertToDomain(answersDTO, chosenOptionsDTO);
        }

        /* Eens de questionnaire is ingevuld door de gebruiker is em ingevuld. Ik denk niet dat er enkel nut is van het te veranderen of te verwijderen.
  * Dit is nog altijd open tot discussie of course. -NVZ
  * 
  * public void Update(Answer obj)
  * public void Delete(int questionID, int answerID)
  * 
 */

        public IEnumerable<Answer> ReadAll(int questionID)
        {
            List<Answer> myQuery = new List<Answer>();

            foreach (AnswersDTO DTO in ctx.Answers.ToList().FindAll(a => a.QQuestionID == questionID))
            {
                if (ctx.Choices.Where(c => c.AnswerID == DTO.AnswerID).Count() == 0)
                {
                    myQuery.Add(ConvertToDomain(DTO));
                }
                else
                {
                    MultipleAnswer toAdd = ReadMultipleAnswer(DTO.AnswerID, false);
                    myQuery.Add(toAdd);
                }
            }

            return myQuery;
        }
        #endregion

        // Added by NVZ
        // Options CRUD
        #region
        public string Create(int questionID, string obj)
        {
            IEnumerable<string> options = ReadAllOptions(questionID);
            int newID = options.Count() + 1;

            for (int i = 0; i < options.Count(); i++)
            {
                if (ExtensionMethods.HasMatchingWords(obj, options.ElementAt(i)) > 0)
                {
                    throw new DuplicateNameException("Deze Option(ID=" + newID + ") met Optiontekst: " + obj + " is gelijkaardig aan de Option(ID=" + i + 
                        "). De Optiontekst is: " + options.ElementAt(i) + ".");
                }
            }

            ctx.Options.Add(ConvertToDTO(newID, obj, questionID));
            ctx.SaveChanges();

            return obj;
        }

        public String ReadOption(int optionID, int questionID)
        {
            return ConvertToDomain(ctx.Options.Find(optionID));
        }

        public int ReadOptionID(string optionText, int questionID)
        {
            List<string> options = ReadAllOptions(questionID).ToList();
            for(int i = 0; i < options.Count; i++)
            {
                if (options[i].Equals(optionText))
                {
                    return i + 1;
                }
            }
            throw new DuplicateNameException("Option " + optionText + " niet gevonden voor de QuestionnaireQuestion(ID=" + questionID + ").");
        }

        public void DeleteOption(int optionID)
        {
            OptionsDTO toDelete = ctx.Options.First(o => o.OptionID == optionID);
            ctx.Options.Remove(toDelete);
            ctx.SaveChanges();
        }
        
        public IEnumerable<string> ReadAllOptions(int QuestionID)
        {
            List<string> myQuery = new List<string>();

            foreach (OptionsDTO DTO in ctx.Options)
            {
                if (DTO.QQuestionID == QuestionID)
                {
                    myQuery.Append(ConvertToDomain(DTO));
                }
            }

            return myQuery;
        }
        #endregion
    }
}