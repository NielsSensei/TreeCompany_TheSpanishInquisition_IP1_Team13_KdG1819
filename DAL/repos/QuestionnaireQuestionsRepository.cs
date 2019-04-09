using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Domain.UserInput;
using DAL.Contexts;
using DAL.Data_Transfer_Objects;
using Microsoft.EntityFrameworkCore;
using Domain.Projects;
using Domain.Users;

namespace DAL
{
    public class QuestionnaireQuestionsRepository : IRepository<QuestionnaireQuestion>
    {
        // Added by DM
        // Modified by NVZ
        private CityOfIdeasDbContext ctx;

        // Added by NVZ
        public QuestionnaireQuestionsRepository()
        {
            ctx = new CityOfIdeasDbContext();
        }

        // Added by NVZ
        // Standard Methods
        #region
        private QuestionnaireQuestionsDTO convertToDTO(QuestionnaireQuestion obj)
        {
            return new QuestionnaireQuestionsDTO
            {
                qQuestionID = obj.Id,
                ModuleID = obj.Module.Id,
                QuestionText = obj.QuestionText,
                qType = (byte) obj.QuestionType,
                Required = obj.Optional
            };
        }

        private QuestionnaireQuestion convertToDomain(QuestionnaireQuestionsDTO DTO)
        {
            return new QuestionnaireQuestion
            {
                Id = DTO.qQuestionID,
                Module = new Questionnaire { Id = DTO.ModuleID },
                QuestionText = DTO.QuestionText,
                QuestionType = (QuestionType) DTO.qType,
                Optional = DTO.Required
            };
        }

        private OptionsDTO convertToDTO(int id, string obj, int qID)
        {
            return new OptionsDTO
            {
                OptionID = id,
                OptionText = obj,
                qQuestionID = qID
            };
        }

        private String convertToDomain(OptionsDTO DTO)
        {
            return DTO.OptionText;
        }

        private AnswersDTO OpenConvertToDTO(OpenAnswer obj)
        {
            return new AnswersDTO
            {
                AnswerID = obj.Id,
                qQuestionID = obj.Question.Id,
                UserID = obj.User.Id,
                AnswerText = obj.AnswerText,
            };
        }

        private AnswersDTO MultipleConvertToDTO(MultipleAnswer obj)
        {
            return new AnswersDTO
            {
                AnswerID = obj.Id,
                qQuestionID = obj.Question.Id,
                UserID = obj.User.Id
            };
        }

        private ChoicesDTO convertToDTO(int optionID, int answerID, int choiceID)
        {
            return new ChoicesDTO
            {
                ChoiceID = choiceID,
                AnswerID = answerID,
                OptionID = optionID
            };
        }

        private OpenAnswer convertToDomain(AnswersDTO DTO)
        {
            return new OpenAnswer
            {
                Id = DTO.AnswerID,
                User = new User { Id = DTO.UserID },
                Question = new QuestionnaireQuestion { Id = DTO.qQuestionID },
                IsUserEmail = DTO.AnswerText.Contains("@"),
                AnswerText = DTO.AnswerText
            };
        }

        private MultipleAnswer convertToDomain(AnswersDTO answersDTO, List<OptionsDTO> chosenOptionsDTO)
        {
            MultipleAnswer ma = null;
            ma.Id = answersDTO.AnswerID;
            ma.User = new User { Id = answersDTO.UserID };
            ma.Question = new QuestionnaireQuestion { Id = answersDTO.qQuestionID };
            ma.DropdownList = chosenOptionsDTO.Count == 1;

            foreach(OptionsDTO DTO in chosenOptionsDTO)
            {
                ma.Choices.Add(DTO.OptionText);
            }

            return ma;
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

            ctx.QuestionnaireQuestions.Add(convertToDTO(obj));
            ctx.SaveChanges();

            return obj;
        }
        
        public QuestionnaireQuestion Read(int id, bool details)
        {
            QuestionnaireQuestionsDTO questionnaireQuestionDTO = null;

            if (details)
            {
                questionnaireQuestionDTO = ctx.QuestionnaireQuestions.AsNoTracking().First(q => q.qQuestionID == id);
                ExtensionMethods.CheckForNotFound(questionnaireQuestionDTO, "QuestionnaireQuestion", questionnaireQuestionDTO.qQuestionID);
            }
            else
            {
                questionnaireQuestionDTO = ctx.QuestionnaireQuestions.First(q => q.qQuestionID == id);
                ExtensionMethods.CheckForNotFound(questionnaireQuestionDTO, "QuestionnaireQuestion", questionnaireQuestionDTO.qQuestionID);
            }

            return convertToDomain(questionnaireQuestionDTO);
        }

        public void Update(QuestionnaireQuestion obj)
        {
            QuestionnaireQuestionsDTO newQuestionnaireQuestion = convertToDTO(obj);
            QuestionnaireQuestionsDTO foundQuestionnaireQuestion = convertToDTO(Read(obj.Id, false));
            foundQuestionnaireQuestion = newQuestionnaireQuestion;

            ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            ctx.QuestionnaireQuestions.Remove(convertToDTO(Read(id, false)));
            ctx.SaveChanges();
        }
        
        public IEnumerable<QuestionnaireQuestion> ReadAll()
        {
            IEnumerable<QuestionnaireQuestion> myQuery = new List<QuestionnaireQuestion>();

            foreach (QuestionnaireQuestionsDTO DTO in ctx.QuestionnaireQuestions)
            {
                myQuery.Append(convertToDomain(DTO));
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
            
            if(qq.QuestionType == QuestionType.OPEN || qq.QuestionType == QuestionType.MAIL)
            {
                ctx.Answers.Add(OpenConvertToDTO((OpenAnswer) obj));
            }else
            {
                MultipleAnswer ma = (MultipleAnswer)obj;
                ctx.Answers.Add(MultipleConvertToDTO(ma));
                foreach(String s in ma.Choices)
                {
                    ctx.Choices.Add(convertToDTO(ReadOptionID(s,ma.Question.Id),ma.Id,ctx.Choices.Count()+1));
                }
            }
            ctx.SaveChanges();

            return obj;
        }

        
        public OpenAnswer ReadOpenAnswer(int answerID, bool details)
        {
            AnswersDTO answersDTO = null;

            if (details)
            {
                answersDTO = ctx.Answers.AsNoTracking().First(i => i.AnswerID == answerID);
                ExtensionMethods.CheckForNotFound(answersDTO, "Answer", answerID);
            }
            else
            {
                answersDTO = ctx.Answers.First(i => i.AnswerID == answerID);
                ExtensionMethods.CheckForNotFound(answersDTO, "Answer", answerID);
            }

            return convertToDomain(answersDTO);
        }

        //TODO: HEY DO ME PLS
        public MultipleAnswer ReadMultipleAnswer(int answerID, bool details)
        {
            AnswersDTO answersDTO = null;

            if (details)
            {
                answersDTO = ctx.Answers.AsNoTracking().First(i => i.AnswerID == answerID);
                ExtensionMethods.CheckForNotFound(answersDTO, "Answer", answerID);
            }
            else
            {
                answersDTO = ctx.Answers.First(i => i.AnswerID == answerID);
                ExtensionMethods.CheckForNotFound(answersDTO, "Answer", answerID);
            }

            List<ChoicesDTO> choicesDTO = ctx.Choices.ToList().FindAll(c => c.AnswerID == answerID);
            List<OptionsDTO> optionsDTO = ctx.Options.ToList().FindAll(o => o.qQuestionID == answersDTO.qQuestionID);
            List<OptionsDTO> chosenOptionsDTO = new List<OptionsDTO>();

            foreach(OptionsDTO DTO in optionsDTO)
            {
                ChoicesDTO choice = ctx.Choices.First(c => c.OptionID == DTO.OptionID);   
                
                if(choice.ChoiceID != null)
                {
                    chosenOptionsDTO.Add(DTO);
                }
            }

            return convertToDomain(answersDTO, chosenOptionsDTO);
        }

        public void Update(Answer obj)
        {
            //Delete(obj.questionID, obj.Id);
            //Create(obj);
        }
        /*
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
        }*/
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

            ctx.Options.Add(convertToDTO(newID, obj, questionID));
            ctx.SaveChanges();

            return obj;
        }

        public String ReadOption(int optionID, int questionID)
        {
            return convertToDomain(ctx.Options.Find(optionID));
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

        public void DeleteOption(int optionID, int questionID)
        {
            ctx.Options.Remove(convertToDTO(optionID, ReadOption(optionID, questionID), questionID));
            ctx.SaveChanges();
        }
        
        public IEnumerable<string> ReadAllOptions(int QuestionID)
        {
            IEnumerable<string> myQuery = new List<string>();

            foreach (OptionsDTO DTO in ctx.Options)
            {
                if (DTO.qQuestionID == QuestionID)
                {
                    myQuery.Append(convertToDomain(DTO));
                }
            }

            return myQuery;
        }
        #endregion
    }
}