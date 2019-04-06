using System.Collections.Generic;
using System.Data;
using System.Linq;
using Domain.UserInput;
using DAL.Contexts;
using DAL.Data_Transfer_Objects;
using Microsoft.EntityFrameworkCore;
using Domain.Projects;
using Domain.Users;

namespace DAL.repos
{
    public class IdeationQuestionsRepository : IRepository<IdeationQuestion>
    {
        // Added by DM
        // Modified by NVZ
        private CityOfIdeasDbContext ctx;

        // Added by NVZ
        public IdeationQuestionsRepository()
        {
            ctx = new CityOfIdeasDbContext();
        }

        // Added by NVZ
        // Standard Methods
        #region
        private IdeationQuestionsDTO convertToDTO(IdeationQuestion obj)
        {
            return new IdeationQuestionsDTO
            {
                iQuestionID = obj.Id,
                ModuleID = obj.Ideation.Id,
                QuestionTitle = obj.QuestionTitle,
                Description = obj.Description,
                WebsiteLink = obj.SiteURL,
                DeviceID = obj.Device.Id
            };
        }

        private IdeasDTO convertToDTO(Idea obj)
        {
            return new IdeasDTO
            {
                IdeaID = obj.Id,
                iQuestionID = obj.IdeaQuestion.Id,
                UserID = obj.User.Id,
                Reported = obj.Reported,
                ReviewByAdmin = obj.ReviewByAdmin,
                Visible = obj.Visible,
                VoteCount = obj.VoteCount,
                RetweetCount = obj.RetweetCount,
                ShareCount = obj.ShareCount,
                Status = obj.Status,
                VerifiedUser = obj.VerifiedUser,
                ParentID = obj.ParentIdea.Id
            };
        }

        private IdeaFieldsDTO convertToDTO(Field obj)
        {
            return new IdeaFieldsDTO
            {
                FieldID = obj.Id,
                IdeaID = obj.Idea.Id,
                FieldText = obj.Text,
                Required = obj.Required
            };
        }

        private IdeaFieldsDTO convertToDTO(ClosedField obj)
        {
            return new IdeaFieldsDTO
            {
                FieldID = obj.Id,
                IdeaID = obj.Idea.Id,
                FieldText = obj.Text,
                Required = obj.Required,
                FieldStrings = ExtensionMethods.ListToString(obj.Options)
            };
        }

        private IdeaFieldsDTO convertToDTO(ImageField obj)
        {
            return new IdeaFieldsDTO
            {
                FieldID = obj.Id,
                IdeaID = obj.Idea.Id,
                FieldText = obj.Text,
                Required = obj.Required,
                Url = obj.Url
                //UploadedImage = obj.UploadedImage,
            };
        }

        private IdeaFieldsDTO convertToDTO(VideoField obj)
        {
            return new IdeaFieldsDTO
            {
                FieldID = obj.Id,
                IdeaID = obj.Idea.Id,
                FieldText = obj.Text,
                Required = obj.Required,
                Url = obj.Url
                //UploadedVideo
            };
        }

        private IdeaFieldsDTO convertToDTO(MapField obj)
        {
            return new IdeaFieldsDTO
            {
                FieldID = obj.Id,
                IdeaID = obj.Idea.Id,
                FieldText = obj.Text,
                Required = obj.Required,
                LocationX = obj.LocationX,
                LocationY = obj.LocationY
            };
        }

        private IdeationQuestion convertToDomain(IdeationQuestionsDTO DTO)
        {
            return new IdeationQuestion
            {
                Id = DTO.iQuestionID,
                Description = DTO.Description,
                SiteURL = DTO.WebsiteLink,
                QuestionTitle = DTO.QuestionTitle,
                Device = new IOT_Device { Id = DTO.DeviceID },
                Ideation = new Ideation { Id = DTO.ModuleID }
            };
        }

        private Idea convertToDomain(IdeasDTO DTO)
        {
            return new Idea
            {
                Id = DTO.IdeaID,
                IdeaQuestion = new IdeationQuestion { Id = DTO.iQuestionID },
                User = new User { Id = DTO.UserID },
                Reported = DTO.Reported,
                ReviewByAdmin = DTO.ReviewByAdmin,
                Visible = DTO.Visible,
                VoteCount = DTO.VoteCount,
                RetweetCount = DTO.RetweetCount,
                ShareCount= DTO.ShareCount,
                Status = DTO.Status,
                VerifiedUser = DTO.VerifiedUser,
                ParentIdea = new Idea { Id = DTO.ParentID }
            };
        }

        private Field convertFieldToDomain(IdeaFieldsDTO DTO)
        {
            return new Field {
                Id = DTO.FieldID,
                Idea = new Idea { Id = DTO.IdeaID },
                Required = DTO.Required,
                Text = DTO.FieldText
            };
        }

        private ClosedField convertClosedFieldToDomain(IdeaFieldsDTO DTO)
        {
            return new ClosedField
            {
                Id = DTO.FieldID,
                Idea = new Idea { Id = DTO.IdeaID },
                Required = DTO.Required,
                Options = ExtensionMethods.StringToList(DTO.FieldStrings)
            };
        }

        private MapField convertMapFieldToDomain(IdeaFieldsDTO DTO)
        {
            return new MapField
            {
                Id = DTO.FieldID,
                Idea = new Idea { Id = DTO.IdeaID },
                Required = DTO.Required,
                LocationX = DTO.LocationX,
                LocationY = DTO.LocationY
            };
        }

        private ImageField convertImageFieldToDomain(IdeaFieldsDTO DTO)
        {
            return new ImageField
            {
                Id = DTO.FieldID,
                Idea = new Idea { Id = DTO.IdeaID },
                Required = DTO.Required,
                Url = DTO.Url
                //UploadedVideo= DTO.UploadedVideo
            };
        }

        private VideoField convertVideoFieldToDomain(IdeaFieldsDTO DTO)
        {
            return new VideoField
            {
                Id = DTO.FieldID,
                Idea = new Idea { Id = DTO.IdeaID },
                Required = DTO.Required,
                Url = DTO.Url
                //UploadedImage = DTO.UploadedImage
            };
        }
        #endregion

        // Added by NVZ
        // IdeationQuestion CRUD
        #region
        public IdeationQuestion Create(IdeationQuestion obj)
        {
            IEnumerable<IdeationQuestion> iqs = ReadAll(obj.Ideation.Id);

            foreach (IdeationQuestion iq in iqs)
            {
                if (ExtensionMethods.HasMatchingWords(obj.QuestionTitle ,iq.QuestionTitle) > 0)
                {
                    throw new DuplicateNameException("IdeationQuestion(ID=" + obj.Id + ") is een gelijkaardige titel aan IdeationQuestion(ID=" + iq.Id +
                        ") de titel specifiek was: " + obj.QuestionText + ".");
                }
            }

            ctx.IdeationQuestion.Add(convertToDTO(obj));
            ctx.SaveChanges();

            return obj;
        }
        
        public IdeationQuestion Read(int id, bool details)
        {
            IdeationQuestionsDTO ideationQuestionDTO = null;

            if (details)
            {
                ideationQuestionDTO = ctx.IdeationQuestion.AsNoTracking().First(i => i.iQuestionID == id);
                ExtensionMethods.CheckForNotFound(ideationQuestionDTO, "IdeationQuestion", ideationQuestionDTO.iQuestionID);
            }
            else
            {
                ideationQuestionDTO = ctx.IdeationQuestion.First(i => i.iQuestionID == id);
                ExtensionMethods.CheckForNotFound(ideationQuestionDTO, "IdeationQuestion", ideationQuestionDTO.iQuestionID);
            }

            return convertToDomain(ideationQuestionDTO);
        }

        public void Update(IdeationQuestion obj)
        {
            IdeationQuestionsDTO newIdeationQuestion = convertToDTO(obj);
            IdeationQuestionsDTO foundIdeationQuestion = convertToDTO(Read(obj.Id, false));
            foundIdeationQuestion = newIdeationQuestion;

            ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            IdeationQuestion iq = Read(id, false);
            iq.QuestionText = "[deleted]";
            iq.QuestionTitle = "[deleted]";
            iq.Description = "[deleted]";
            Update(iq);
        }

        public IEnumerable<IdeationQuestion> ReadAll()
        {
            IEnumerable<IdeationQuestion> myQuery = new List<IdeationQuestion>();

            foreach (IdeationQuestionsDTO DTO in ctx.IdeationQuestion)
            {
                myQuery.Append(convertToDomain(DTO));
            }

            return myQuery;
        }

        public IEnumerable<IdeationQuestion> ReadAll(int ideationID)
        {
            return ReadAll().ToList().FindAll(i => i.Ideation.Id == ideationID);
        }
        #endregion

        // Added by NVZ
        // Idea CRUD
        #region
        public Idea Create(Idea idea)
        {
            IEnumerable<Idea> ideas = ReadAllIdeasByQuestion(idea.IdeaQuestion.Id);

            foreach (Idea i in ideas)
            {
                if(ExtensionMethods.HasMatchingWords(i.Title, idea.Title) > 0)
                {
                    throw new DuplicateNameException("Idea(ID=" + idea.Id + ") met titel " + idea.Title + " heeft een gelijkaardige titel aan Idea(ID=" +
                        i.Id + " met titel " + i.Title + ".");
                }
            }

            ctx.Ideas.Add(convertToDTO(idea));
            ctx.IdeaFields.Add(convertToDTO(idea.Field));
            ctx.IdeaFields.Add(convertToDTO(idea.Cfield));
            ctx.IdeaFields.Add(convertToDTO(idea.Ifield));
            ctx.IdeaFields.Add(convertToDTO(idea.Vfield));
            ctx.IdeaFields.Add(convertToDTO(idea.Mfield));

            ctx.SaveChanges();

            return idea;
        }

        public Idea ReadIdea(int ideaID, bool details)
        {
            IdeasDTO ideasDTO = null;

            if (details)
            {
                ideasDTO = ctx.Ideas.AsNoTracking().First(i => i.IdeaID == ideaID);
                ExtensionMethods.CheckForNotFound(ideasDTO, "Idea", ideasDTO.IdeaID);
            }
            else
            {
                ideasDTO = ctx.Ideas.First(i => i.IdeaID == ideaID);
                ExtensionMethods.CheckForNotFound(ideasDTO, "Idea", ideasDTO.IdeaID);
            }

            return convertToDomain(ideasDTO);
        }
        
        public Idea ReadWithFields(int id)
        {
            Idea idea = ReadIdea(id, true); 
            List<IdeaFieldsDTO> fields = ctx.IdeaFields.ToList().FindAll(i => i.IdeaID == id);

            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].FieldText != null)
                {
                    idea.Field = convertFieldToDomain(fields[i]);
                } else if (fields[i].FieldStrings != null)
                {
                    idea.Cfield = convertClosedFieldToDomain(fields[i]);
                } else if(fields[i].LocationX != null)
                {
                    idea.Mfield = convertMapFieldToDomain(fields[i]);
                } else if(fields[i].UploadedImage != null)
                {
                    idea.Ifield = convertImageFieldToDomain(fields[i]);
                } else if(fields[i].uploadedMedia != null)
                {
                    idea.Vfield = convertVideoFieldToDomain(fields[i]);
                }
            }

            return idea;
        }

        public void Update(Idea obj)
        {
            IdeasDTO newIdea = convertToDTO(obj);
            IdeasDTO foundIdea = convertToDTO(ReadIdea(obj.Id, false));
            foundIdea = newIdea;

            IdeaFieldsDTO newTextField = convertToDTO(obj.Field);
            //IdeaFieldsDTO foundTextField = convertToDTO


            ctx.SaveChanges();
        }

        //TODO: set op [deleted]
        public void Delete(int questionID, int ideaID)
        {
            Idea toDelete = Read(questionID, ideaID);
            if (toDelete != null)
            {
                ideas.Remove(toDelete);
                Read(questionID).Ideas.Remove(toDelete);
            }
        }

        public IEnumerable<Idea> ReadAllIdeas()
        {
            return null;
        }

        public IEnumerable<Idea> ReadAllIdeasByQuestion(int questionID)
        {
            return ReadAllIdeas().ToList().FindAll(i => i.IdeaQuestion.Id == questionID);
        }

        public IEnumerable<Idea> ReadAllChilds(int parentId)
        {
            return ReadAllIdeas().ToList().FindAll(idea => idea.ParentIdea.Id == parentId);
        }

        public IEnumerable<Field> ReadAllFields()
        {
            return null;
        }

        public IEnumerable<Field> ReadAllFields(int ideaID)
        {
            return ReadAllFields().ToList().FindAll(idea => idea.Idea.Id == ideaID);
        }
        #endregion 
    }
}