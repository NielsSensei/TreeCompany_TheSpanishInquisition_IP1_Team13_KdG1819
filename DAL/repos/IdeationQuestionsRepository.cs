using System.Collections.Generic;
using System.Data;
using System.Linq;
using Domain.UserInput;
using DAL.Contexts;
using DAL.Data_Transfer_Objects;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Projects;
using Domain.Users;

namespace DAL.repos
{
    public class IdeationQuestionsRepository : IRepository<IdeationQuestion>
    {
        // Added by DM
        // Modified by NVZ
        private readonly CityOfIdeasDbContext ctx;

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
                WebsiteLink = obj.SiteURL              
            };
        }

        private IdeasDTO ConvertToDTO(Idea obj)
        {
            IdeasDTO DTO =  new IdeasDTO()
            {
                IdeaID = obj.Id,
                IQuestionID = obj.IdeaQuestion.Id,
                UserID = obj.User.Id,
                Reported = obj.Reported,
                ReviewByAdmin = obj.ReviewByAdmin,
                Title = obj.Title,
                Visible = obj.Visible,
                VoteCount = obj.VoteCount,
                RetweetCount = obj.RetweetCount,
                ShareCount = obj.ShareCount,
                Status = obj.Status,
                VerifiedUser = obj.VerifiedUser,
                IsDeleted = obj.IsDeleted,
                ParentID = obj.ParentIdea.Id,
                DeviceID = obj.Device.Id
            };

            if (obj.ParentIdea == null)
            {
                DTO.ParentID = 0;
            }

            if (obj.Device == null)
            {
                DTO.DeviceID = 0;
            }
            
            return DTO;
        }

        private IdeaFieldsDTO ConvertToDTO(Field obj)
        {
            return new IdeaFieldsDTO
            {
                FieldID = obj.Id,
                IdeaID = obj.Idea.Id,
                FieldText = obj.Text
            };
        }

        private IdeaFieldsDTO ConvertToDTO(ClosedField obj)
        {
            return new IdeaFieldsDTO
            {
                FieldID = obj.Id,
                IdeaID = obj.Idea.Id,
                FieldText = obj.Text,
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
                Ideation = new Ideation { Id = DTO.ModuleID }
            };
        }

        private Idea ConvertToDomain(IdeasDTO DTO)
        {
            return new Idea
            {
                Id = DTO.IdeaID,
                IdeaQuestion = new IdeationQuestion { Id = DTO.IQuestionID },
                User = new UIMVCUser() { Id = DTO.UserID },
                Reported = DTO.Reported,
                ReviewByAdmin = DTO.ReviewByAdmin,
                Title = DTO.Title,
                Visible = DTO.Visible,
                VoteCount = DTO.VoteCount,
                RetweetCount = DTO.RetweetCount,
                ShareCount= DTO.ShareCount,
                Status = DTO.Status,
                VerifiedUser = DTO.VerifiedUser,
                IsDeleted = DTO.IsDeleted,
                ParentIdea = new Idea { Id = DTO.ParentID },
                Device = new IOT_Device { Id = DTO.DeviceID }
            };
        }

        private Report ConvertToDomain(ReportsDTO DTO)
        {
            return new Report()
            {
                Id = DTO.ReportID,
                Idea = new Idea { Id = DTO.IdeaID },
                Flagger = new UIMVCUser() { Id = DTO.FlaggerID },
                Reportee = new UIMVCUser() { Id = DTO.ReporteeID },
                Reason = DTO.Reason,
                Status = (ReportStatus) DTO.ReportApproved
            };
        }
        
        private Field ConvertFieldToDomain(IdeaFieldsDTO DTO)
        {
            return new Field {
                Id = DTO.FieldID,
                Idea = new Idea { Id = DTO.IdeaID },
                Text = DTO.FieldText
            };
        }

        private ClosedField ConvertClosedFieldToDomain(IdeaFieldsDTO DTO)
        {
            return new ClosedField
            {
                Id = DTO.FieldID,
                Idea = new Idea { Id = DTO.IdeaID },
                Options = ExtensionMethods.StringToList(DTO.FieldStrings)
            };
        }

        private MapField ConvertMapFieldToDomain(IdeaFieldsDTO DTO)
        {
            return new MapField
            {
                Id = DTO.FieldID,
                Idea = new Idea { Id = DTO.IdeaID },
                LocationX = DTO.LocationX,
                LocationY = DTO.LocationY
            };
        }

        private ImageField ConvertImageFieldToDomain(IdeaFieldsDTO DTO)
        {
            return new ImageField
            {
                Id = DTO.FieldID,
                Idea = new Idea { Id = DTO.IdeaID },
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
                Url = DTO.Url
                //UploadedImage = DTO.UploadedImage
            };
        }

        private ReportsDTO ConvertToDTO(Report obj)
        {
            return new ReportsDTO()
            {
                ReportID   = obj.Id,
                IdeaID = obj.Idea.Id,
                FlaggerID = obj.Flagger.Id,
                ReporteeID = obj.Reportee.Id,
                Reason = obj.Reason,
                ReportApproved = (byte) obj.Status
            };
        }
        
        private int FindNextAvailableIQuestionId()
        {
            if (!ctx.IdeationQuestions.Any()) return 1;
            int newId = ReadAll().Max(IQuestion => IQuestion.Id)+1;
            return newId;
        }

        private int FindNextAvailableIdeaId()
        {
            if (!ctx.Ideas.Any()) return 1;
            int newId = ReadAllIdeas().Max(idea => idea.Id)+1;
            return newId;
        }
                
        private int FindNextAvailableReportId()
        {
            if (!ctx.Reports.Any()) return 1;
            int newId = ReadAllReports().Max(report => report.Id)+1;
            return newId;
        }

        private int FindNextAvailableFieldId()
        {
            if (!ctx.IdeaFields.Any()) return 1;
            int newId = ReadAllFields().Max(field => field.Id)+1;
            return newId;
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

            obj.Id = FindNextAvailableIQuestionId();
            ctx.IdeationQuestions.Add(ConvertToDTO(obj));           
            ctx.SaveChanges();

            return obj;
        }
        
        public IdeationQuestion Read(int id, bool details)
        {
            IdeationQuestionsDTO ideationQuestionDTO = null;
            ideationQuestionDTO = details ? ctx.IdeationQuestions.AsNoTracking().First(i => i.IQuestionID == id) : ctx.IdeationQuestions.First(i => i.IQuestionID == id);
            ExtensionMethods.CheckForNotFound(ideationQuestionDTO, "IdeationQuestion", id);

            return ConvertToDomain(ideationQuestionDTO);
        }

        public void Update(IdeationQuestion obj)
        {
            IdeationQuestionsDTO newIdeationQuestion = ConvertToDTO(obj);
            IdeationQuestionsDTO foundIdeationQuestion  = ctx.IdeationQuestions.First(i => i.IQuestionID == obj.Id);

            if (foundIdeationQuestion != null)
            {
                foundIdeationQuestion.QuestionTitle = newIdeationQuestion.QuestionTitle;
                foundIdeationQuestion.Description = newIdeationQuestion.Description;
                foundIdeationQuestion.WebsiteLink = newIdeationQuestion.WebsiteLink;
            }

            ctx.SaveChanges();
        }
        
        public void Delete(int id)
        {
            IdeationQuestionsDTO dto = ctx.IdeationQuestions.First(d => d.IQuestionID == id);
            ctx.IdeationQuestions.Remove(dto);
            ctx.SaveChanges();
        }

        public IEnumerable<IdeationQuestion> ReadAll()
        {
            List<IdeationQuestion> myQuery = new List<IdeationQuestion>();

            foreach (IdeationQuestionsDTO DTO in ctx.IdeationQuestions)
            {
                myQuery.Add(ConvertToDomain(DTO));
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
            
            idea.Id = FindNextAvailableIdeaId();
           
            ctx.Ideas.Add(ConvertToDTO(idea));

            if (idea.Field != null)
            {
                idea.Field.Id = FindNextAvailableFieldId();
                ctx.IdeaFields.Add(ConvertToDTO(idea.Field));   
            }

            if (idea.Cfield != null)
            {
                idea.Cfield.Id = FindNextAvailableFieldId();
                ctx.IdeaFields.Add(ConvertToDTO(idea.Cfield)); 
            }
            
            if (idea.Ifield != null)
            {
                idea.Ifield.Id = FindNextAvailableFieldId();
                ctx.IdeaFields.Add(ConvertToDTO(idea.Ifield));
            }
            
            if (idea.Vfield != null)
            {
                idea.Vfield.Id = FindNextAvailableFieldId();
                ctx.IdeaFields.Add(ConvertToDTO(idea.Vfield));
            }
            
            if (idea.Mfield != null)
            {
                idea.Mfield.Id = FindNextAvailableFieldId();
                ctx.IdeaFields.Add(ConvertToDTO(idea.Mfield));
            }

            ctx.SaveChanges();

            return idea;
        }

        public Idea ReadIdea(int ideaID, bool details)
        {
            IdeasDTO ideasDTO = null;
            ideasDTO = details ? ctx.Ideas.AsNoTracking().First(i => i.IdeaID == ideaID) : ctx.Ideas.First(i => i.IdeaID == ideaID);          
            ExtensionMethods.CheckForNotFound(ideasDTO, "Idea", ideaID);

            return ConvertToDomain(ideasDTO);
        }
        
        public Idea ReadWithFields(int id)
        {
            Idea idea = ReadIdea(id, true); 
            List<IdeaFieldsDTO> fields = ctx.IdeaFields.ToList().FindAll(i => i.IdeaID == id);
           
            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].FieldText != null && idea.Field == null)
                {
                    idea.Field = ConvertFieldToDomain(fields[i]);                   
                } 
                
                if (fields[i].FieldStrings != null)
                {
                    idea.Cfield = ConvertClosedFieldToDomain(fields[i]);
                } 
                
                if(fields[i].LocationX > 0)
                {
                    idea.Mfield = ConvertMapFieldToDomain(fields[i]);
                } 
                
                if(fields[i].UploadedImage != null)
                {
                    idea.Ifield = ConvertImageFieldToDomain(fields[i]);
                } 
                
                if(fields[i].UploadedMedia != null)
                {
                    idea.Vfield = ConvertVideoFieldToDomain(fields[i]);
                }
            }

            return idea;
        }

        public void Update(Idea obj)
        {
            IdeasDTO newIdea = ConvertToDTO(obj);
            IdeasDTO foundIdea = ctx.Ideas.First(i => i.IdeaID == obj.Id);
            if (foundIdea != null)
            {
                foundIdea.Title = newIdea.Title;
                foundIdea.Reported = newIdea.Reported;
                foundIdea.ReviewByAdmin = newIdea.ReviewByAdmin;
                foundIdea.Visible = newIdea.Visible;
                foundIdea.VoteCount = newIdea.VoteCount;
                foundIdea.RetweetCount = newIdea.RetweetCount;
                foundIdea.ShareCount = newIdea.ShareCount;
                foundIdea.Status = newIdea.Status;
                foundIdea.VerifiedUser = newIdea.VerifiedUser;
                foundIdea.DeviceID = newIdea.DeviceID;
                foundIdea.IsDeleted = newIdea.IsDeleted;
                ctx.Ideas.Update(foundIdea);
            }
           
            if (obj.Field != null)
            {
                IdeaFieldsDTO newTextField = ConvertToDTO(obj.Field);
                IdeaFieldsDTO foundTextField = ctx.IdeaFields.First(f => f.FieldID == obj.Field.Id);
                if (foundTextField != null)
                {
                    foundTextField.FieldText = newTextField.FieldText;
                    ctx.IdeaFields.Update(foundTextField);
                }
            }

            if (obj.Cfield != null)
            {
                IdeaFieldsDTO newCField = ConvertToDTO(obj.Cfield);
                IdeaFieldsDTO foundCField = ctx.IdeaFields.First(f => f.FieldID == obj.Cfield.Id);
                if (foundCField != null)
                {
                    foundCField.FieldStrings = newCField.FieldStrings;
                    ctx.IdeaFields.Update(foundCField);
                }
            }
            
            if (obj.Mfield != null)
            {
                IdeaFieldsDTO newMField = ConvertToDTO(obj.Mfield);
                IdeaFieldsDTO foundMField = ctx.IdeaFields.First(f => f.FieldID == obj.Mfield.Id);
                if (foundMField != null)
                {
                    foundMField.LocationX = newMField.LocationX;
                    foundMField.LocationY = newMField.LocationY;
                    foundMField.Url = newMField.Url;
                    ctx.IdeaFields.Update(foundMField);
                }
            }
            
            if (obj.Ifield != null)
            {
                IdeaFieldsDTO newIField = ConvertToDTO(obj.Ifield);
                IdeaFieldsDTO foundIField = ctx.IdeaFields.First(f => f.FieldID == obj.Ifield.Id);
                if (foundIField != null)
                {
                    foundIField.Url = newIField.Url;
                    foundIField.UploadedImage = newIField.UploadedImage;
                    ctx.IdeaFields.Update(foundIField);
                }                               
            }
            
            if (obj.Vfield != null)
            {
                IdeaFieldsDTO newVField = ConvertToDTO(obj.Vfield);
                IdeaFieldsDTO foundVField = ctx.IdeaFields.First(f => f.FieldID == obj.Vfield.Id);
                if (foundVField != null)
                {
                    foundVField.Url = newVField.Url;
                    foundVField.UploadedMedia = newVField.UploadedMedia;
                    ctx.IdeaFields.Update(foundVField);
                }
            }

            ctx.SaveChanges();
        }

        public void DeleteField(int id)
        {
            IdeaFieldsDTO i = ctx.IdeaFields.First(f => f.FieldID == id);
            ctx.IdeaFields.Remove(i);
            ctx.SaveChanges();
        }
        
        public void DeleteFields(int ideaID)
        {
            List<Field> fields = (List<Field>) ReadAllFields(ideaID);

            foreach (Field field in fields)
            {
                DeleteField(field.Id);
            }
        }
 
        public void DeleteIdea(int ideaID)
        {
            IdeasDTO i = ctx.Ideas.First(byeIdea => byeIdea.IdeaID == ideaID);
            ctx.Ideas.Remove(i);
            ctx.SaveChanges();
        }

        public IEnumerable<Idea> ReadAllIdeas()
        {
            List<Idea> myQuery = new List<Idea>();

            foreach (IdeasDTO DTO in ctx.Ideas)
            {
                Idea idea = ReadWithFields(DTO.IdeaID);
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
            List<Field> myQuery = new List<Field>();

            foreach (IdeaFieldsDTO DTO in ctx.IdeaFields)
            {
                if (DTO.FieldText != null)
                {
                    myQuery.Add(ConvertFieldToDomain(DTO));
                }
                else if (DTO.FieldStrings != null)
                {
                    myQuery.Add(ConvertClosedFieldToDomain(DTO));
                }
                else if (DTO.LocationX > 0)
                {
                    myQuery.Add(ConvertMapFieldToDomain(DTO));
                }
                else if (DTO.UploadedImage != null)
                {
                    myQuery.Add(ConvertImageFieldToDomain(DTO));
                }
                else if (DTO.UploadedMedia != null)
                {
                    myQuery.Add(ConvertVideoFieldToDomain(DTO));
                }
            }

            return myQuery;
        }

        public IEnumerable<Field> ReadAllFields(int ideaID)
        {
            return ReadAllFields().ToList().FindAll(idea => idea.Idea.Id == ideaID);
        }
        #endregion 
        
        // Added by NVZ
        // Report CRUD
        #region
        public Report Create(Report obj)
        {
            IEnumerable<Report> rs = ReadAllReportsByIdea(obj.Idea.Id);

            foreach (Report r in rs)
            {
                if (ExtensionMethods.HasMatchingWords(obj.Reason ,r.Reason) > 0)
                {
                    throw new DuplicateNameException("Dit Idee heeft al een gelijkaardig rapport. Het gevonden Rapport(ID= " + r.Id + ") met tekst: " + r.Reason + "is gelijkaardig" +
                                                     " aan het gegeven Rapport(ID= " + obj.Id + ") met tekst: " + obj.Reason + ".");
                }
            }

            obj.Id = FindNextAvailableReportId();
            ctx.Reports.Add(ConvertToDTO(obj));
            ctx.SaveChanges();

            return obj;
        }

        public Report ReadReport(int id, bool details)
        {
            ReportsDTO reportsDTO = null;
            reportsDTO = details ? ctx.Reports.AsNoTracking().First(i => i.ReportID == id) : ctx.Reports.First(i => i.ReportID == id);         
            ExtensionMethods.CheckForNotFound(reportsDTO, "Report", id);

            return ConvertToDomain(reportsDTO);
        }

        public void Update(Report obj)
        {
            ReportsDTO newReport = ConvertToDTO(obj);
            ReportsDTO foundReport = ctx.Reports.First(r => r.ReportID == obj.Id);
            if (foundReport != null)
            {
                foundReport.Reason = newReport.Reason;
                foundReport.ReportApproved = newReport.ReportApproved;
                ctx.Reports.Update(foundReport);
            }
            
            ctx.SaveChanges();
        }

        public void DeleteReport(int id)
        {
            ReportsDTO toDelete = ctx.Reports.First(r => r.ReportID == id);
            ctx.Reports.Remove(toDelete);
            ctx.SaveChanges();
        }

        public void DeleteReports(int ideaID)
        {
            List<Report> reports = (List<Report>) ReadAllReportsByIdea(ideaID);

            foreach (Report report in reports)
            {
               DeleteReport(report.Id);
            }
        }
        public IEnumerable<Report> ReadAllReports()
        {
            List<Report> myQuery = new List<Report>();

            foreach (ReportsDTO DTO in ctx.Reports)
            {
                myQuery.Add(ConvertToDomain(DTO));
            }

            return myQuery;
        }

        public IEnumerable<Report> ReadAllReportsByIdea(int IdeaID)
        {
            return ReadAllReports().ToList().FindAll(r => r.Idea.Id == IdeaID);
        }

        public IEnumerable<Report> ReadAllReportsByUser(string UserID)
        {
            return ReadAllReports().ToList().FindAll(r => r.Reportee.Id == UserID);;
        }
        #endregion
    }
}