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
    public class IdeationQuestionsRepository : IRepository<IdeationQuestion>
    {
        private readonly CityOfIdeasDbContext _ctx;

        public IdeationQuestionsRepository()
        {
            _ctx = new CityOfIdeasDbContext();
        }

        #region Conversion Methods
        private IdeationQuestionsDao ConvertToDao(IdeationQuestion obj)
        {
            return new IdeationQuestionsDao
            {
                IquestionId = obj.Id,
                ModuleId = obj.Ideation.Id,
                QuestionTitle = obj.QuestionTitle,
                Description = obj.Description,
                WebsiteLink = obj.SiteUrl
            };
        }

        private IdeasDao ConvertToDao(Idea obj)
        {
            IdeasDao dao =  new IdeasDao()
            {
                IdeaId = obj.Id,
                IquestionId = obj.IdeaQuestion.Id,
                UserId = obj.User.Id,
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
                ParentId = 0,
                DeviceId = 0
            };

            if (obj.ParentIdea != null)
            {
                dao.ParentId = obj.ParentIdea.Id;
            }

            if (obj.Device != null)
            {
                dao.DeviceId = obj.Device.Id;
            }

            return dao;
        }

        private IdeaFieldsDao ConvertToFieldDao(Idea idea)
        {
            IdeaFieldsDao ifd =  new IdeaFieldsDao()
            {
                FieldId = idea.Field.Id,
                IdeaId = idea.Id
            };

            if (idea.Field != null)
            {
                ifd.FieldText = idea.Field.Text;
            }

            if (idea.Cfield != null)
            {
                ifd.FieldStrings = ExtensionMethods.ListToString(idea.Cfield.Options);
            }

            if (idea.Ifield != null)
            {
                ifd.UploadedImage = idea.Ifield.UploadedImage;
            }

            if (idea.Vfield != null)
            {
                ifd.MediaLink = idea.Vfield.VideoLink;
            }

            if (idea.Mfield != null)
            {
                ifd.LocationX = idea.Mfield.LocationX;
                ifd.LocationY = idea.Mfield.LocationY; 
            }
            
            return ifd;
        }
        
        private IdeationQuestion ConvertToDomain(IdeationQuestionsDao dao)
        {
            return new IdeationQuestion
            {
                Id = dao.IquestionId,
                Description = dao.Description,
                SiteUrl = dao.WebsiteLink,
                QuestionTitle = dao.QuestionTitle,
                Ideation = new Ideation { Id = dao.ModuleId }
            };
        }

        private Idea ConvertToDomain(IdeasDao dao)
        {
            return new Idea
            {
                Id = dao.IdeaId,
                IdeaQuestion = new IdeationQuestion { Id = dao.IquestionId },
                User = new UimvcUser() { Id = dao.UserId },
                Reported = dao.Reported,
                ReviewByAdmin = dao.ReviewByAdmin,
                Title = dao.Title,
                Visible = dao.Visible,
                VoteCount = dao.VoteCount,
                RetweetCount = dao.RetweetCount,
                ShareCount= dao.ShareCount,
                Status = dao.Status,
                VerifiedUser = dao.VerifiedUser,
                IsDeleted = dao.IsDeleted,
                ParentIdea = new Idea { Id = dao.ParentId },
                Device = new IotDevice { Id = dao.DeviceId }
            };
        }

        private Report ConvertToDomain(ReportsDao dao)
        {
            return new Report()
            {
                Id = dao.ReportId,
                Idea = new Idea { Id = dao.IdeaId },
                Flagger = new UimvcUser() { Id = dao.FlaggerId },
                Reportee = new UimvcUser() { Id = dao.ReporteeId },
                Reason = dao.Reason,
                Status = (ReportStatus) dao.ReportApproved
            };
        }
        
        private ReportsDao ConvertToDao(Report obj)
        {
            return new ReportsDao()
            {
                ReportId   = obj.Id,
                IdeaId = obj.Idea.Id,
                FlaggerId = obj.Flagger.Id,
                ReporteeId = obj.Reportee.Id,
                Reason = obj.Reason,
                ReportApproved = (byte) obj.Status
            };
        }
        #endregion

        #region Id Generation
        private int FindNextAvailableIQuestionId()
        {
            if (!_ctx.IdeationQuestions.Any()) return 1;
            int newId = ReadAll().Max(iquestion => iquestion.Id)+1;
            return newId;
        }

        private int FindNextAvailableIdeaId()
        {
            if (!_ctx.Ideas.Any()) return 1;
            int newId = ReadAllIdeas().Max(idea => idea.Id)+1;
            return newId;
        }

        private int FindNextAvailableReportId()
        {
            if (!_ctx.Reports.Any()) return 1;
            int newId = ReadAllReports().Max(report => report.Id)+1;
            return newId;
        }

        private int FindNextAvailableFieldId()
        {
            if (!_ctx.IdeaFields.Any()) return 1;
            int newId = _ctx.IdeaFields.Max(f => f.FieldId)+1;
            return newId;
        }
        #endregion

        #region IdeationQuestion CRUD
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
            _ctx.IdeationQuestions.Add(ConvertToDao(obj));
            _ctx.SaveChanges();

            return obj;
        }

        public IdeationQuestion Read(int id, bool details)
        {
            IdeationQuestionsDao ideationQuestionDao = details ? _ctx.IdeationQuestions.AsNoTracking().First(i => i.IquestionId == id) : _ctx.IdeationQuestions.First(i => i.IquestionId == id);
            ExtensionMethods.CheckForNotFound(ideationQuestionDao, "IdeationQuestion", id);

            return ConvertToDomain(ideationQuestionDao);
        }

        public void Update(IdeationQuestion obj)
        {
            IdeationQuestionsDao newIdeationQuestion = ConvertToDao(obj);
            IdeationQuestionsDao foundIdeationQuestion  = _ctx.IdeationQuestions.First(i => i.IquestionId == obj.Id);

            if (foundIdeationQuestion != null)
            {
                foundIdeationQuestion.QuestionTitle = newIdeationQuestion.QuestionTitle;
                foundIdeationQuestion.Description = newIdeationQuestion.Description;
                foundIdeationQuestion.WebsiteLink = newIdeationQuestion.WebsiteLink;
            }

            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            IdeationQuestionsDao dao = _ctx.IdeationQuestions.First(d => d.IquestionId == id);
            _ctx.IdeationQuestions.Remove(dao);
            _ctx.SaveChanges();
        }

        public IEnumerable<IdeationQuestion> ReadAll()
        {
            List<IdeationQuestion> myQuery = new List<IdeationQuestion>();

            foreach (IdeationQuestionsDao dao in _ctx.IdeationQuestions)
            {
                myQuery.Add(ConvertToDomain(dao));
            }

            return myQuery;
        }

        public IEnumerable<IdeationQuestion> ReadAll(int ideationId)
        {
            return ReadAll().ToList().FindAll(i => i.Ideation.Id == ideationId);
        }
        #endregion

        #region Idea CRUD
        public Idea Create(Idea idea)
        {
            IEnumerable<Idea> ideas = ReadAllIdeasByQuestion(idea.IdeaQuestion.Id);
            
            foreach (Idea i in ideas)
            {
                if(i.Title == idea.Title && !i.IsDeleted)
                {
                    throw new DuplicateNameException("Idea(ID=" + idea.Id + ") met titel " + idea.Title + " heeft een gelijkaardige titel aan Idea(ID=" +
                        i.Id + " met titel " + i.Title + ".");
                }
            }

            idea.Id = FindNextAvailableIdeaId();
            _ctx.Ideas.Add(ConvertToDao(idea));

            IdeaFieldsDao fieldsDao = ConvertToFieldDao(idea);
            fieldsDao.FieldId = FindNextAvailableFieldId();
            _ctx.IdeaFields.Add(fieldsDao);
            
            _ctx.SaveChanges();

            return idea;
        }

        public Idea ReadIdea(int ideaId, bool details)
        {
            IdeasDao ideasDao = details ? _ctx.Ideas.AsNoTracking().First(i => i.IdeaId == ideaId) : _ctx.Ideas.First(i => i.IdeaId == ideaId);
            ExtensionMethods.CheckForNotFound(ideasDao, "Idea", ideaId);

            return ConvertToDomain(ideasDao);
        }

        public Idea ReadWithFields(int id)
        {
            Idea idea = ReadIdea(id, true);
            IdeaFieldsDao field = _ctx.IdeaFields.First(i => i.IdeaId == id);

            if (field.FieldText != null)
            {
                idea.Field = new Field(){ Id = field.FieldId, Idea = idea, Text = field.FieldText, 
                    TextLength = field.FieldText.Length };  
            }

            if (field.FieldStrings != null)
            {
                idea.Cfield = new ClosedField(){ Id = field.FieldId, Idea = idea, 
                    Options = ExtensionMethods.StringToList(field.FieldStrings) };  
            }

            if (field.LocationX != 0)
            {
                idea.Mfield = new MapField(){ Id = field.FieldId, Idea = idea, LocationX = field.LocationX, 
                    LocationY = field.LocationY };
            }

            if (field.UploadedImage != null)
            {
                idea.Ifield = new ImageField(){ Id = field.FieldId, Idea = idea, UploadedImage = field.UploadedImage };
            }

            if (field.MediaLink != null)
            {
                idea.Vfield = new VideoField(){ Id = field.FieldId, Idea = idea, VideoLink = field.MediaLink };
            }
            
            return idea;
        }

        public void Update(Idea obj)
        {
            IdeasDao newIdea = ConvertToDao(obj);
            IdeasDao foundIdea = _ctx.Ideas.FirstOrDefault(i => i.IdeaId == obj.Id);
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
                foundIdea.DeviceId = newIdea.DeviceId;
                foundIdea.IsDeleted = newIdea.IsDeleted;
                _ctx.Ideas.Update(foundIdea);
            }

            IdeaFieldsDao newIdeaFields = ConvertToFieldDao(obj);
            IdeaFieldsDao foundIdeaFields = _ctx.IdeaFields.FirstOrDefault(f => f.IdeaId == obj.Id);
            if (foundIdeaFields != null)
            {
                foundIdeaFields.FieldText = newIdeaFields.FieldText;
                foundIdeaFields.FieldStrings = newIdeaFields.FieldStrings;
                foundIdeaFields.LocationX = newIdeaFields.LocationX;
                foundIdeaFields.LocationY = newIdeaFields.LocationY;
                foundIdeaFields.UploadedImage = newIdeaFields.UploadedImage;
                foundIdeaFields.MediaLink = newIdeaFields.MediaLink;
                _ctx.IdeaFields.Update(foundIdeaFields);
            }
            
            _ctx.SaveChanges();
        }

        public void DeleteIdea(int ideaId)
        {
            IdeasDao i = _ctx.Ideas.First(d => d.IdeaId == ideaId);
            _ctx.Ideas.Remove(i);
            _ctx.SaveChanges();
        }

        public IEnumerable<Idea> ReadAllIdeas()
        {
            List<Idea> myQuery = new List<Idea>();

            foreach (IdeasDao dao in _ctx.Ideas)
            {
                Idea idea = ReadWithFields(dao.IdeaId);
                myQuery.Add(idea);
            }

            return myQuery;
        }

        public IEnumerable<Idea> ReadAllIdeasByQuestion(int questionId)
        {
            return ReadAllIdeas().ToList().FindAll(i => i.IdeaQuestion.Id == questionId);
        }

        #region Field CRUD
        public void DeleteField(int ideaId)
        {
            IdeaFieldsDao i = _ctx.IdeaFields.First(d => d.IdeaId == ideaId);
            _ctx.IdeaFields.Remove(i);
            _ctx.SaveChanges();
        }
        #endregion
        #endregion

        #region Report CRUD
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
            _ctx.Reports.Add(ConvertToDao(obj));
            _ctx.SaveChanges();

            return obj;
        }

        public Report ReadReport(int id, bool details)
        {
            ReportsDao reportsDao = details ? _ctx.Reports.AsNoTracking().First(i => i.ReportId == id) : _ctx.Reports.First(i => i.ReportId == id);
            ExtensionMethods.CheckForNotFound(reportsDao, "Report", id);

            return ConvertToDomain(reportsDao);
        }

        public void Update(Report obj)
        {
            ReportsDao newReport = ConvertToDao(obj);
            ReportsDao foundReport = _ctx.Reports.First(r => r.ReportId == obj.Id);
            if (foundReport != null)
            {
                foundReport.Reason = newReport.Reason;
                foundReport.ReportApproved = newReport.ReportApproved;
                _ctx.Reports.Update(foundReport);
            }

            _ctx.SaveChanges();
        }

        public void DeleteReport(int id)
        {
            ReportsDao toDelete = _ctx.Reports.First(r => r.ReportId == id);
            _ctx.Reports.Remove(toDelete);
            _ctx.SaveChanges();
        }

        public void DeleteReports(int ideaId)
        {
            List<Report> reports = (List<Report>) ReadAllReportsByIdea(ideaId);
            foreach (Report report in reports)
            {
                DeleteReport(report.Id);
            }
        }

        public IEnumerable<Report> ReadAllReports()
        {
            List<Report> myQuery = new List<Report>();

            foreach (ReportsDao dao in _ctx.Reports)
            {
                myQuery.Add(ConvertToDomain(dao));
            }

            return myQuery;
        }

        public IEnumerable<Report> ReadAllReportsByIdea(int ideaId)
        {
            return ReadAllReports().ToList().FindAll(r => r.Idea.Id == ideaId);
        }

        public IEnumerable<Report> ReadAllReportsByUser(string userId)
        {
            return ReadAllReports().ToList().FindAll(r => r.Reportee.Id == userId);;
        }
        #endregion
    }
}
