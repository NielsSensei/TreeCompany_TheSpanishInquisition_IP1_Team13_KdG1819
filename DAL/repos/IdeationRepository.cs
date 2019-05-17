using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL.Contexts;
using DAL.Data_Access_Objects;
using Domain.Projects;
using Domain.Identity;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace DAL.repos
{
    public class IdeationRepository : IRepository<Ideation>
    {
        private readonly CityOfIdeasDbContext _ctx;

        public IdeationRepository()
        {
            _ctx = new CityOfIdeasDbContext();
        }

        #region Conversion Methods
        private ModulesDao GrabModuleInformationDao(Ideation obj)
        {
            ModulesDao dao = new ModulesDao
            {
                ModuleId = obj.Id,
                OnGoing = obj.OnGoing,
                Title = obj.Title,
                LikeCount = obj.LikeCount,
                FbLikeCount = obj.FbLikeCount,
                TwitterLikeCount = obj.TwitterLikeCount,
                ShareCount = obj.ShareCount,
                RetweetCount = obj.RetweetCount,
                IsQuestionnaire = obj.ModuleType == ModuleType.Questionnaire
            };

            if (obj.Tags != null)
            {
                dao.Tags = ExtensionMethods.ListToString(obj.Tags);
            }

            if (obj.Project != null)
            {
                dao.ProjectId = obj.Project.Id;
            }

            if (obj.ParentPhase != null)
            {
                dao.PhaseId = obj.ParentPhase.Id;
            }

            return dao;
        }

        // XV: TODO Create a check for organisation accounts
        private IdeationsDao ConvertToDao(Ideation obj)
        {
            //bool Org = obj.User.Role == Role.LOGGEDINORG;
            IdeationsDao dao = new IdeationsDao()
            {
                    ModuleId = obj.Id,
                    ExtraInfo = obj.ExtraInfo
                    //MediaFile = obj.Media,
            };

            if (obj.User != null)
            {
                dao.UserId = obj.User.Id;
            }

            if (obj.RequiredFields > 0)
            {
                dao.RequiredFields = (byte) obj.RequiredFields;
            }

            if (obj.Event != null)
            {
                dao.EventId = obj.Event.Id;
                dao.UserIdea = obj.UserIdea;
                //DTO.Organisation = Org;
            }

            return dao;
        }

        private Ideation ConvertToDomain(IdeationsDao dao)
        {
            return new Ideation
            {
                Id = dao.ModuleId,
                User = new UimvcUser { Id = dao.UserId },
                UserIdea = dao.UserIdea,
                Event = new Event { Id = dao.EventId },
                MediaLink = dao.MediaFile,
                ExtraInfo = dao.ExtraInfo,
                RequiredFields = dao.RequiredFields
            };
        }

        private Ideation IdeationWithModules(Ideation ideation , ModulesDao dao)
        {
            ideation.Project = new Project { Id = dao.ProjectId };
            ideation.ParentPhase = new Phase { Id = dao.PhaseId };
            ideation.Title = dao.Title;
            ideation.OnGoing = dao.OnGoing;
            ideation.LikeCount = dao.LikeCount;
            ideation.FbLikeCount = dao.FbLikeCount;
            ideation.TwitterLikeCount = dao.TwitterLikeCount;
            ideation.ShareCount = dao.ShareCount;
            ideation.RetweetCount = dao.RetweetCount;
            ideation.Tags = new List<string>();

            if (dao.Tags != null)
            {
                ideation.Tags = ExtensionMethods.StringToList(dao.Tags);
            }
            return ideation;
        }
        #endregion

        #region Id generation
        private int FindNextAvailableIdeationId()
        {
            if (!_ctx.Ideations.Any()) return 1;
            int newId = _ctx.Modules.Max(q => q.ModuleId) + 1;
            return newId;
        }
        #endregion

        #region Ideation CRUD
        public Ideation Create(Ideation obj)
        {
            IEnumerable<Ideation> ideations = ReadAll(obj.Project.Id);

            foreach (Ideation i in ideations)
            {
                if(obj.ParentPhase.Id == i.ParentPhase.Id)
                {
                    throw new DuplicateNameException("Ideation(ID=" + obj.Id + ") heeft dezelfde parentPhase als Ideation(ID=" + i.Id + "). De phaseID is "
                        + obj.ParentPhase.Id + ".");
                }
            }

            obj.Id = FindNextAvailableIdeationId();
            ModulesDao newModule = GrabModuleInformationDao(obj);
            IdeationsDao newIdeation = ConvertToDao(obj);

            _ctx.Modules.Add(newModule);
            _ctx.Ideations.Add(newIdeation);
            _ctx.SaveChanges();

            return obj;
        }

        public Ideation Read(int id, bool details)
        {
            IdeationsDao ideationDao = details ? _ctx.Ideations.AsNoTracking().First(m => m.ModuleId == id) : _ctx.Ideations.First(m => m.ModuleId == id);
            ExtensionMethods.CheckForNotFound(ideationDao, "Ideation", id);

            return ConvertToDomain(ideationDao);
        }

        public Ideation ReadWithModule(int id)
        {
            Ideation ideation = Read(id, true);
            ModulesDao dao = _ctx.Modules.First(m => m.ModuleId == id);

            ideation = IdeationWithModules(ideation, dao);

            return ideation;
        }

        public void Update(Ideation obj)
        {
            IdeationsDao newIdeation = ConvertToDao(obj);
            IdeationsDao foundIdeation = _ctx.Ideations.FirstOrDefault(dto => dto.ModuleId == newIdeation.ModuleId);
            if (foundIdeation != null)
            {
                foundIdeation.ExtraInfo = newIdeation.ExtraInfo;
                foundIdeation.MediaFile = newIdeation.MediaFile;
                foundIdeation.RequiredFields = newIdeation.RequiredFields;
            }

            ModulesDao newModule = GrabModuleInformationDao(obj);
            ModulesDao foundModule = _ctx.Modules.FirstOrDefault(dto => dto.ModuleId == newModule.ModuleId);
            if (foundModule != null)
            {
                foundModule.OnGoing = newModule.OnGoing;
                foundModule.Title = newModule.Title;
                foundModule.LikeCount = newModule.LikeCount;
                foundModule.FbLikeCount = newModule.FbLikeCount;
                foundModule.TwitterLikeCount = newModule.TwitterLikeCount;
                foundModule.ShareCount = newModule.ShareCount;
                foundModule.RetweetCount = newModule.RetweetCount;
                foundModule.Tags = newModule.Tags;
            }

            if (newModule.PhaseId != foundModule.PhaseId)
            {
                foundModule.PhaseId = newModule.PhaseId;
            }

            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            ModulesDao toDeleteModule = _ctx.Modules.First(r => r.ModuleId == id);
            _ctx.Modules.Remove(toDeleteModule);
            IdeationsDao toDelete = _ctx.Ideations.First(r => r.ModuleId == id);
            _ctx.Ideations.Remove(toDelete);
            _ctx.SaveChanges();
        }

        public IEnumerable<Ideation> ReadAll()
        {
            List<Ideation> myQuery = new List<Ideation>();

            foreach (IdeationsDao dao in _ctx.Ideations)
            {
                Ideation toAdd = ReadWithModule(dao.ModuleId);
                myQuery.Add(toAdd);
            }

            return myQuery;
        }

        public IEnumerable<Ideation> ReadAll(int projectId)
        {
            return ReadAll().ToList().FindAll(i => i.Project.Id == projectId);
        }
        #endregion

        #region Media CRUD
        // TODO: (SPRINT2?) Als we images kunnen laden enal is het bonus, geen prioriteit tegen Sprint 1.
        /*public Media Create(Media obj)
        {
            //if (!mediafiles.Contains(obj))
            //{
            //    mediafiles.Add(obj);
            //}
            throw new DuplicateNameException("This MediaFile already exist!");
        }

        public Media ReadMedia(int ideationId)
        {
            //Media m = Read(ideationID).Media;
            //if (m != null)
            //{
            //    return m;
            //}
            throw new KeyNotFoundException("This Media can't be found!");
        }

        public void DeleteMedia(int ideationId)
        {
            //Media m = ReadMedia(ideationID);
            //if (m != null)
            //{
            //    mediafiles.Remove(m);
            //}
        } */
        #endregion

        #region Tag CRUD
        public string CreateTag(string obj, int moduleId)
        {
            Ideation ideationWTags = ReadWithModule(moduleId);
            string oldTags = ExtensionMethods.ListToString(ideationWTags.Tags);
            oldTags += "," + obj;

            ideationWTags.Tags = ExtensionMethods.StringToList(oldTags);
            Update(ideationWTags);

            return obj;
        }

        public void DeleteTag(int moduleId, int tagId)
        {
            Ideation ideationWTags = Read(moduleId, false);
            ModulesDao module = GrabModuleInformationDao(ideationWTags);
            List<String> keptTags = ExtensionMethods.StringToList(module.Tags);
            keptTags.RemoveAt(tagId - 1);
            module.Tags = ExtensionMethods.ListToString(keptTags);
            _ctx.SaveChanges();
        }

        public IEnumerable<String> ReadAllTags(int moduleId)
        {
            return Read(moduleId,true).Tags;
        }
        #endregion
    }
}
