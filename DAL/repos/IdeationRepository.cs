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
    /*
     * @authors David Matei, Edwin Kai Yin Tam & Niels Van Zandbergen
     */
    public class IdeationRepository : IRepository<Ideation>
    {
        private readonly CityOfIdeasDbContext _ctx;

        public IdeationRepository()
        {
            _ctx = new CityOfIdeasDbContext();
        }

        /*
         * @author Niels Van Zandbergen
         */
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
                ModuleType = (byte) obj.ModuleType
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
        
        private IdeationsDao ConvertToDao(Ideation obj)
        {
            IdeationsDao dao = new IdeationsDao()
            {
                    ModuleId = obj.Id,
                    ExtraInfo = obj.ExtraInfo,
                    MediaFile = obj.MediaLink,
                    UserVote = obj.UserVote
            };

            if (obj.User != null)
            {
                dao.UserId = obj.User.Id;
            }
            
            return dao;
        }

        private Ideation ConvertToDomain(IdeationsDao dao)
        {
            return new Ideation
            {
                Id = dao.ModuleId,
                User = new UimvcUser { Id = dao.UserId },
                UserVote = dao.UserVote,
                Event = new Event { Id = dao.EventId },
                MediaLink = dao.MediaFile,
                ExtraInfo = dao.ExtraInfo
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
            ideation.ModuleType = ModuleType.Ideation;

            if (dao.Tags != null)
            {
                ideation.Tags = ExtensionMethods.StringToList(dao.Tags);
            }
            return ideation;
        }

        private IdeationSettingsDao ConvertToDao(IdeationSettings settings)
        {
            return new IdeationSettingsDao()
            {
                ModuleId = settings.Ideation.Id,
                Field = settings.Field,
                ClosedField = settings.ClosedField,
                ImageField = settings.ImageField,
                VideoField = settings.VideoField,
                MapField = settings.MapField
            };
        }

        private IdeationSettings ConvertToDomain(IdeationSettingsDao dao)
        {
            return new IdeationSettings()
            {
                Ideation = new Ideation(){ Id = dao.ModuleId },
                Field = dao.Field,
                ClosedField = dao.ClosedField,
                ImageField = dao.ImageField,
                MapField = dao.MapField,
                VideoField = dao.VideoField
            };
        }
        #endregion

        /*
         * @author Niels Van Zandbergen
         */
        #region Id generation
        private int FindNextAvailableIdeationId()
        {
            if (!_ctx.Ideations.Any()) return 1;
            int newId = _ctx.Modules.Max(q => q.ModuleId) + 1;
            return newId;
        }
        #endregion

        /*
         * @authors David Matei, Edwin Kai Yin Tam & Niels Van Zandbergen
         */
        #region Ideation CRUD
        /*
         * @documentation Niels Van Zandbergen
         *
         * Bij de meeste Create Methodes wordt er tussen een textveld vergeleken, hier is dit niet zo. Hier vergelijken we op ParentPhase, het object,
         * want een Module - lees Ideation of Questionnaire - heeft maar een Phase waar het is in opgestart en aan gekoppeld mag worden. Moest het toch
         * zijn dat er iets misloopt en toch twee Modules dezelfde Phase proberen te claimen dan wordt deze exception getriggered. Merk ook op dat we
         * hier twee tabellen moet aanspreken voor Ideation CRUD. 
         *
         * @see Domain.Projects.Module
         * @see Domain.Projects.Phase
         * 
         */
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
            IdeationSettings settings = obj.Settings;
            settings.Ideation = obj;
            IdeationSettingsDao newSettings = ConvertToDao(settings);

            _ctx.Modules.Add(newModule);
            _ctx.Ideations.Add(newIdeation);
            _ctx.IdeationSettings.Add(newSettings);
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
        public Ideation Read(int id, bool details)
        {
            IdeationsDao ideationDao = details ? _ctx.Ideations.AsNoTracking().First(m => m.ModuleId == id) : _ctx.Ideations.First(m => m.ModuleId == id);
            ExtensionMethods.CheckForNotFound(ideationDao, "Ideation", id);

            Ideation readIdeation = ConvertToDomain(ideationDao);
            IdeationSettingsDao settings = details
                ? _ctx.IdeationSettings.AsNoTracking().First(s => s.ModuleId == id)
                : _ctx.IdeationSettings.First(s => s.ModuleId == id);
            readIdeation.Settings = ConvertToDomain(settings);

            return readIdeation;
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
        public Ideation ReadWithModule(int id, bool details)
        {
            Ideation ideation = Read(id, details);
            ModulesDao dao = details ? _ctx.Modules.AsNoTracking().First(m => m.ModuleId == id) : _ctx.Modules.First(m => m.ModuleId == id);

            ideation = IdeationWithModules(ideation, dao);

            return ideation;
        }

        public void Update(Ideation obj)
        {
            IdeationsDao newIdeation = ConvertToDao(obj);
            IdeationsDao foundIdeation = _ctx.Ideations.FirstOrDefault(dto => dto.ModuleId == newIdeation.ModuleId);
            if (foundIdeation != null)
            {
                if(!String.IsNullOrEmpty(newIdeation.ExtraInfo)) foundIdeation.ExtraInfo = newIdeation.ExtraInfo;
                if(!String.IsNullOrEmpty(newIdeation.MediaFile)) foundIdeation.MediaFile = newIdeation.MediaFile;
                if(!String.IsNullOrEmpty(newIdeation.UserVote.ToString())) foundIdeation.UserVote = newIdeation.UserVote;
            }

            IdeationSettingsDao newSettings = ConvertToDao(obj.Settings);
            IdeationSettingsDao foundSettings = _ctx.IdeationSettings.FirstOrDefault(s => s.ModuleId == obj.Id);
            if (foundSettings != null)
            {
                if (!String.IsNullOrEmpty(newSettings.Field.ToString())) foundSettings.Field = newSettings.Field;
                if (!String.IsNullOrEmpty(newSettings.ClosedField.ToString())) foundSettings.ClosedField = newSettings.ClosedField;
                if (!String.IsNullOrEmpty(newSettings.MapField.ToString())) foundSettings.MapField = newSettings.MapField;
                if (!String.IsNullOrEmpty(newSettings.VideoField.ToString())) foundSettings.VideoField = newSettings.VideoField;
                if (!String.IsNullOrEmpty(newSettings.ImageField.ToString())) foundSettings.ImageField = newSettings.ImageField;
            }

            ModulesDao newModule = GrabModuleInformationDao(obj);
            ModulesDao foundModule = _ctx.Modules.FirstOrDefault(dto => dto.ModuleId == newModule.ModuleId);
            if (foundModule != null)
            {
                if(!String.IsNullOrEmpty(newModule.OnGoing.ToString())) foundModule.OnGoing = newModule.OnGoing;
                if(!String.IsNullOrEmpty(newModule.Title)) foundModule.Title = newModule.Title;
                if(newModule.LikeCount != 0) foundModule.LikeCount = newModule.LikeCount;
                if(newModule.FbLikeCount != 0) foundModule.FbLikeCount = newModule.FbLikeCount;
                if(newModule.TwitterLikeCount != 0) foundModule.TwitterLikeCount = newModule.TwitterLikeCount;
                if(newModule.ShareCount != 0) foundModule.ShareCount = newModule.ShareCount;
                if(newModule.RetweetCount != 0) foundModule.RetweetCount = newModule.RetweetCount;
                if(!String.IsNullOrEmpty(newModule.Tags)) foundModule.Tags = newModule.Tags;
            }

            if (newModule.PhaseId != foundModule.PhaseId)
            {
                if(newModule.PhaseId != 0) foundModule.PhaseId = newModule.PhaseId;
            }

            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            ModulesDao toDeleteModule = _ctx.Modules.First(r => r.ModuleId == id);
            _ctx.Modules.Remove(toDeleteModule);
            IdeationsDao toDelete = _ctx.Ideations.First(r => r.ModuleId == id);
            _ctx.Ideations.Remove(toDelete);
            IdeationSettingsDao toDeleteSettings = _ctx.IdeationSettings.First(r => r.ModuleId == id);
            _ctx.IdeationSettings.Remove(toDeleteSettings);
            
            _ctx.SaveChanges();
        }

        public IEnumerable<Ideation> ReadAll()
        {
            List<Ideation> myQuery = new List<Ideation>();

            foreach (IdeationsDao dao in _ctx.Ideations)
            {
                Ideation toAdd = ReadWithModule(dao.ModuleId, false);
                myQuery.Add(toAdd);
            }

            return myQuery;
        }

        public IEnumerable<Ideation> ReadAll(int projectId)
        {
            return ReadAll().ToList().FindAll(i => i.Project.Id == projectId);
        }
        #endregion
        
        /*
         * @authors David Matei, Edwin Kai Yin Tam & Niels Van Zandbergen
         */
        #region Tag CRUD
        public string CreateTag(string obj, int moduleId)
        {
            Ideation ideationWTags = ReadWithModule(moduleId, false);
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
