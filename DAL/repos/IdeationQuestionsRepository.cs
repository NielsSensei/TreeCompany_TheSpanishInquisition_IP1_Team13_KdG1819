using System.Collections.Generic;
using System.Data;
using System.Linq;
using Domain.UserInput;
using DAL.Contexts;
using DAL.Data_Transfer_Objects;
using Microsoft.EntityFrameworkCore;
using Domain.Projects;
using Domain.Users;
using Microsoft.EntityFrameworkCore.Internal;

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
        private IdeationQuestionsDTO ConvertToDTO(IdeationQuestion obj)
        {
            return new IdeationQuestionsDTO
            {
                IQuestionID = obj.Id,
                ModuleID = obj.Ideation.Id,
                QuestionTitle = obj.QuestionTitle,
                Description = obj.Description,
                WebsiteLink = obj.SiteURL,
                DeviceID = obj.Device.Id
            };
        }

        private IdeasDTO ConvertToDTO(Idea obj)
        {
            return new IdeasDTO
            {
                IdeaID = obj.Id,
                IQuestionID = obj.IdeaQuestion.Id,
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

        private IdeaFieldsDTO ConvertToDTO(Field obj)
        {
            return new IdeaFieldsDTO
            {
                FieldID = obj.Id,
                IdeaID = obj.Idea.Id,
                FieldText = obj.Text,
                Required = obj.Required
            };
        }

        private IdeaFieldsDTO ConvertToDTO(ClosedField obj)
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

        private IdeaFieldsDTO ConvertToDTO(ImageField obj)
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

        private IdeaFieldsDTO ConvertToDTO(VideoField obj)
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

        private IdeaFieldsDTO ConvertToDTO(MapField obj)
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

        private IdeationQuestion ConvertToDomain(IdeationQuestionsDTO DTO)
        {
            return new IdeationQuestion
            {
                Id = DTO.IQuestionID,
                Description = DTO.Description,
                SiteURL = DTO.WebsiteLink,
                QuestionTitle = DTO.QuestionTitle,
                Device = new IOT_Device { Id = DTO.DeviceID },
                Ideation = new Ideation { Id = DTO.ModuleID }
            };
        }

        private Idea ConvertToDomain(IdeasDTO DTO)
        {
            return new Idea
            {
                Id = DTO.IdeaID,
                IdeaQuestion = new IdeationQuestion { Id = DTO.IQuestionID },
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

        private Field ConvertFieldToDomain(IdeaFieldsDTO DTO)
        {
            return new Field {
                Id = DTO.FieldID,
                Idea = new Idea { Id = DTO.IdeaID },
                Required = DTO.Required,
                Text = DTO.FieldText
            };
        }

        private ClosedField ConvertClosedFieldToDomain(IdeaFieldsDTO DTO)
        {
            return new ClosedField
            {
                Id = DTO.FieldID,
                Idea = new Idea { Id = DTO.IdeaID },
                Required = DTO.Required,
                Options = ExtensionMethods.StringToList(DTO.FieldStrings)
            };
        }

        private MapField ConvertMapFieldToDomain(IdeaFieldsDTO DTO)
        {
            return new MapField
            {
                Id = DTO.FieldID,
                Idea = new Idea { Id = DTO.IdeaID },
                Required = DTO.Required,
                LocationX = (float) DTO.LocationX,
                LocationY = DTO.LocationY
            };
        }

        private ImageField ConvertImageFieldToDomain(IdeaFieldsDTO DTO)
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

        private VideoField ConvertVideoFieldToDomain(IdeaFieldsDTO DTO)
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

            ctx.IdeationQuestions.Add(ConvertToDTO(obj));
            ctx.SaveChanges();

            return obj;
        }
        
        public IdeationQuestion Read(int id, bool details)
        {
            IdeationQuestionsDTO ideationQuestionDTO = null;

            if (details)
            {
                ideationQuestionDTO = ctx.IdeationQuestions.AsNoTracking().First(i => i.IQuestionID == id);
                ExtensionMethods.CheckForNotFound(ideationQuestionDTO, "IdeationQuestion", ideationQuestionDTO.IQuestionID);
            }
            else
            {
                ideationQuestionDTO = ctx.IdeationQuestions.First(i => i.IQuestionID == id);
                ExtensionMethods.CheckForNotFound(ideationQuestionDTO, "IdeationQuestion", ideationQuestionDTO.IQuestionID);
            }

            return ConvertToDomain(ideationQuestionDTO);
        }

        public void Update(IdeationQuestion obj)
        {
            IdeationQuestionsDTO newIdeationQuestion = ConvertToDTO(obj);
            IdeationQuestionsDTO foundIdeationQuestion = ConvertToDTO(Read(obj.Id, false));
            foundIdeationQuestion = newIdeationQuestion;

            ctx.SaveChanges();
        }


        /* Ik heb momenteel de optie opengehouden zoals het zoals reddit te doen. https://imgur.com/a/oeLkyL2 zodat de Ide�n niet mee verwijderd worden.
         Een hard delete van de vraag en de hele "thread" zal waarschijnlijk ook nog kunnen, dat mag je veranderen naar bespreking met mij. -NVZ */
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

            foreach (IdeationQuestionsDTO DTO in ctx.IdeationQuestions)
            {
                myQuery.Append(ConvertToDomain(DTO));
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

            ctx.Ideas.Add(ConvertToDTO(idea));
            ctx.IdeaFields.Add(ConvertToDTO(idea.Field));
            ctx.IdeaFields.Add(ConvertToDTO(idea.Cfield));
            ctx.IdeaFields.Add(ConvertToDTO(idea.Ifield));
            ctx.IdeaFields.Add(ConvertToDTO(idea.Vfield));
            ctx.IdeaFields.Add(ConvertToDTO(idea.Mfield));

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

            return ConvertToDomain(ideasDTO);
        }
        
        public Idea ReadWithFields(int id)
        {
            Idea idea = ReadIdea(id, true); 
            List<IdeaFieldsDTO> fields = ctx.IdeaFields.ToList().FindAll(i => i.IdeaID == id);

            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].FieldText != null)
                {
                    idea.Field = ConvertFieldToDomain(fields[i]);
                } else if (fields[i].FieldStrings != null)
                {
                    idea.Cfield = ConvertClosedFieldToDomain(fields[i]);
                } else if(fields[i].LocationX > 0)
                {
                    idea.Mfield = ConvertMapFieldToDomain(fields[i]);
                } else if(fields[i].UploadedImage != null)
                {
                    idea.Ifield = ConvertImageFieldToDomain(fields[i]);
                } else if(fields[i].UploadedMedia != null)
                {
                    idea.Vfield = ConvertVideoFieldToDomain(fields[i]);
                }
            }

            return idea;
        }

        public void Update(Idea obj)
        {
            IdeasDTO newIdea = ConvertToDTO(obj);
            IdeasDTO foundIdea = ConvertToDTO(ReadIdea(obj.Id, false));
            foundIdea = newIdea;

            IdeaFieldsDTO newTextField = ConvertToDTO(obj.Field);
            IdeaFieldsDTO foundTextField = ConvertToDTO(ReadAllFields().ToList().First(f => f.Id == obj.Field.Id));
            newTextField = foundTextField;

            IdeaFieldsDTO newCField = ConvertToDTO(obj.Cfield);
            IdeaFieldsDTO foundCField = ConvertToDTO(ReadAllFields().ToList().First(f => f.Id == obj.Cfield.Id));
            newCField = foundCField;

            IdeaFieldsDTO newMField = ConvertToDTO(obj.Mfield);
            IdeaFieldsDTO foundMField = ConvertToDTO(ReadAllFields().ToList().First(f => f.Id == obj.Mfield.Id));
            newMField = foundMField;

            IdeaFieldsDTO newIField = ConvertToDTO(obj.Ifield);
            IdeaFieldsDTO foundIField = ConvertToDTO(ReadAllFields().ToList().First(f => f.Id == obj.Ifield.Id));
            newIField = foundIField;

            IdeaFieldsDTO newVField = ConvertToDTO(obj.Vfield);
            IdeaFieldsDTO foundVField = ConvertToDTO(ReadAllFields().ToList().First(f => f.Id == obj.Vfield.Id));
            newVField = foundVField;

            ctx.SaveChanges();
        }

        /* Het veiligste is het zoals reddit te doen: https://imgur.com/a/oeLkyL2. Anders worden de childs mee gedelete misschien of bestaan ze wel db wise
         * maar worden ze niet getoond. Het beste is enkel de 2 deleted tonen in dit geval. - NVZ
        */
        public void DeleteIdea(int ideaID)
        {
            Idea i = ReadWithFields(ideaID);
            i.Title = "[deleted]";
            i.Field.Text = "[deleted]";
            i.Cfield.Options = null;
            i.Ifield.UploadedImage = null;
            i.Ifield.Url = null;
            i.Mfield.LocationX = 0;
            i.Mfield.LocationY = 0;
            i.Vfield.Url = null;
            i.Vfield.UploadedVideo = null;
       
            Update(i);
        }

        public IEnumerable<Idea> ReadAllIdeas()
        {
            List<Idea> myQuery = new List<Idea>();

            foreach (IdeasDTO DTO in ctx.Ideas)
            {
                Idea idea = ReadIdea(DTO.IdeaID, false);
                myQuery.Add(idea);
            }

            return myQuery;
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
            IEnumerable<Field> myQuery = new List<Field>();

            foreach (IdeaFieldsDTO DTO in ctx.IdeaFields)
            {
                if (DTO.FieldText != null)
                {
                    myQuery.Append(ConvertFieldToDomain(DTO));
                }
                else if (DTO.FieldStrings != null)
                {
                    myQuery.Append(ConvertClosedFieldToDomain(DTO));
                }
                else if (DTO.LocationX > 0)
                {
                    myQuery.Append(ConvertMapFieldToDomain(DTO));
                }
                else if (DTO.UploadedImage != null)
                {
                    myQuery.Append(ConvertImageFieldToDomain(DTO));
                }
                else if (DTO.UploadedMedia != null)
                {
                    myQuery.Append(ConvertVideoFieldToDomain(DTO));
                }
            }

            return myQuery;
        }

        public IEnumerable<Field> ReadAllFields(int ideaID)
        {
            return ReadAllFields().ToList().FindAll(idea => idea.Idea.Id == ideaID);
        }
        #endregion 
    }
}