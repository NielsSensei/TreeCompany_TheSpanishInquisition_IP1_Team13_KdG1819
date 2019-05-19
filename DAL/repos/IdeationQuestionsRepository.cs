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

        private IdeaFieldsDao ConvertToDao(Field obj)
        {
            return new IdeaFieldsDao
            {
                FieldId = obj.Id,
                IdeaId = obj.Idea.Id,
                FieldText = obj.Text
            };
        }

        private IdeaFieldsDao ConvertToDao(ClosedField obj)
        {
            return new IdeaFieldsDao
            {
                FieldId = obj.Id,
                IdeaId = obj.Idea.Id,
                FieldStrings = ExtensionMethods.ListToString(obj.Options)
            };
        }

        private IdeaFieldsDao ConvertToDao(ImageField obj)
        {
            return new IdeaFieldsDao
            {
                FieldId = obj.Id,
                IdeaId = obj.Idea.Id,
                UploadedImage = obj.UploadedImage
            };
        }

        private IdeaFieldsDao ConvertToDao(VideoField obj)
        {
            return new IdeaFieldsDao
            {
                FieldId = obj.Id,
                IdeaId = obj.Idea.Id,
                MediaLink = obj.VideoLink
            };
        }

        private IdeaFieldsDao ConvertToDao(MapField obj)
        {
            return new IdeaFieldsDao
            {
                FieldId = obj.Id,
                IdeaId = obj.Idea.Id,
                LocationX = obj.LocationX,
                LocationY = obj.LocationY
            };
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

        private Field ConvertFieldToDomain(IdeaFieldsDao dao)
        {
            return new Field {
                Id = dao.FieldId,
                Idea = new Idea { Id = dao.IdeaId },
                Text = dao.FieldText
            };
        }

        private ClosedField ConvertClosedFieldToDomain(IdeaFieldsDao dao)
        {
            return new ClosedField
            {
                Id = dao.FieldId,
                Idea = new Idea { Id = dao.IdeaId },
                Options = ExtensionMethods.StringToList(dao.FieldStrings)
            };
        }

        private MapField ConvertMapFieldToDomain(IdeaFieldsDao dao)
        {
            return new MapField
            {
                Id = dao.FieldId,
                Idea = new Idea { Id = dao.IdeaId },
                LocationX = dao.LocationX,
                LocationY = dao.LocationY
            };
        }

        private ImageField ConvertImageFieldToDomain(IdeaFieldsDao dao)
        {
            return new ImageField
            {
                Id = dao.FieldId,
                Idea = new Idea { Id = dao.IdeaId },
                UploadedImage = dao.UploadedImage
            };
        }

        private VideoField ConvertVideoFieldToDomain(IdeaFieldsDao dao)
        {
            return new VideoField
            {
                Id = dao.FieldId,
                Idea = new Idea { Id = dao.IdeaId },
                VideoLink = dao.MediaLink
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
            int newId = ReadAllFields().Max(field => field.Id)+1;
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
            int lastField = FindNextAvailableFieldId();

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

            if (idea.Field != null)
            {
                idea.Field.Id = lastField;
                lastField++;
                _ctx.IdeaFields.Add(ConvertToDao(idea.Field));   
            }

            if (idea.Cfield != null)
            {
                idea.Cfield.Id = lastField;
                lastField++;
                _ctx.IdeaFields.Add(ConvertToDao(idea.Cfield)); 
            }

            if (idea.Ifield != null)
            {
                idea.Ifield.Id = lastField;
                lastField++;
                _ctx.IdeaFields.Add(ConvertToDao(idea.Ifield));
            }

            if (idea.Vfield != null)
            {
                idea.Vfield.Id = lastField;
                lastField++;
                _ctx.IdeaFields.Add(ConvertToDao(idea.Vfield));
            }

            if (idea.Mfield != null)
            {
                idea.Mfield.Id = lastField;
                _ctx.IdeaFields.Add(ConvertToDao(idea.Mfield));
            }

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
            List<IdeaFieldsDao> fields = _ctx.IdeaFields.ToList().FindAll(i => i.IdeaId == id);

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

                if(fields[i].MediaLink != null)
                {
                    idea.Vfield = ConvertVideoFieldToDomain(fields[i]);
                }
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

            if (obj.Field != null)
            {
                IdeaFieldsDao newTextField = ConvertToDao(obj.Field);
                IdeaFieldsDao foundTextField = _ctx.IdeaFields.FirstOrDefault(f => f.IdeaId == obj.Id && !f.FieldText.Equals(null));
                if (foundTextField != null)
                {
                    foundTextField.FieldText = newTextField.FieldText;
                    _ctx.IdeaFields.Update(foundTextField);
                }
            }

            if (obj.Cfield != null)
            {
                IdeaFieldsDao newCField = ConvertToDao(obj.Cfield);
                IdeaFieldsDao foundCField = _ctx.IdeaFields.FirstOrDefault(f => f.IdeaId == obj.Id && !f.FieldStrings.Equals(null));
                if (foundCField != null)
                {
                    foundCField.FieldStrings = newCField.FieldStrings;
                    _ctx.IdeaFields.Update(foundCField);
                }
            }

            if (obj.Mfield != null)
            {
                IdeaFieldsDao newMField = ConvertToDao(obj.Mfield);
                IdeaFieldsDao foundMField = _ctx.IdeaFields.FirstOrDefault(
                    f => f.IdeaId == obj.Id && f.LocationX != 0 && f.LocationY != 0);
                if (foundMField != null)
                {
                    foundMField.LocationX = newMField.LocationX;
                    foundMField.LocationY = newMField.LocationY;
                    _ctx.IdeaFields.Update(foundMField);
                }
            }

            if (obj.Ifield != null)
            {
                IdeaFieldsDao newIField = ConvertToDao(obj.Ifield);
                IdeaFieldsDao foundIField = _ctx.IdeaFields.FirstOrDefault(f => f.IdeaId == obj.Id && f.UploadedImage != null);
                if (foundIField != null)
                {
                    foundIField.UploadedImage = newIField.UploadedImage;
                    _ctx.IdeaFields.Update(foundIField);
                }
            }

            if (obj.Vfield != null)
            {
                IdeaFieldsDao newVField = ConvertToDao(obj.Vfield);
                IdeaFieldsDao foundVField = _ctx.IdeaFields.FirstOrDefault(f => f.IdeaId == obj.Id && !f.MediaLink.Equals(null));
                if (foundVField != null)
                {
                    foundVField.MediaLink = newVField.MediaLink;
                    _ctx.IdeaFields.Update(foundVField);
                }
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
        public void DeleteField(int id)
        {
            IdeaFieldsDao i = _ctx.IdeaFields.First(d => d.FieldId == id);
            _ctx.IdeaFields.Remove(i);
            _ctx.SaveChanges();
        }

        public void DeleteFields(int ideaId)
        {
            List<Field> fields = (List<Field>) ReadAllFields(ideaId);
            foreach (Field field in fields)
            {
                DeleteField(field.Id);
            }
        }

        public IEnumerable<Field> ReadAllFields()
        {
            List<Field> myQuery = new List<Field>();

            foreach (IdeaFieldsDao dao in _ctx.IdeaFields)
            {
                if (dao.FieldText != null)
                {
                    myQuery.Add(ConvertFieldToDomain(dao));
                }
                else if (dao.FieldStrings != null)
                {
                    myQuery.Add(ConvertClosedFieldToDomain(dao));
                }
                else if (dao.LocationX > 0)
                {
                    myQuery.Add(ConvertMapFieldToDomain(dao));
                }
                else if (dao.UploadedImage != null)
                {
                    myQuery.Add(ConvertImageFieldToDomain(dao));
                }
                else if (dao.MediaLink != null)
                {
                    myQuery.Add(ConvertVideoFieldToDomain(dao));
                }
            }

            return myQuery;
        }

        public IEnumerable<Field> ReadAllFields(int ideaId)
        {
            return ReadAllFields().ToList().FindAll(idea => idea.Idea.Id == ideaId);
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
