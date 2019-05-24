using System;
using System.Collections.Generic;
using System.Data;
using Domain.Projects;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DAL.Contexts;
using DAL.Data_Access_Objects;

namespace DAL.repos
{
    /*
     * @authors David Matei, Edwin Kai Yin Tam, Niels Van Zandbergen & Xander Veldeman
     */
    public class QuestionnaireRepository : IRepository<Questionnaire>
    {
        private readonly CityOfIdeasDbContext _ctx;

        public QuestionnaireRepository()
        {
            _ctx = new CityOfIdeasDbContext();
        }

        /*
         * @author Niels Van Zandbergen
         */
        #region Conversion Methods
        private ModulesDao ConvertToDao(Questionnaire obj)
        {
            return new ModulesDao
            {
                ModuleId = obj.Id,
                ProjectId = obj.Project.Id,
                PhaseId = obj.ParentPhase.Id,
                OnGoing = obj.OnGoing,
                Title = obj.Title,
                LikeCount = obj.LikeCount,
                FbLikeCount = obj.FbLikeCount,
                TwitterLikeCount = obj.TwitterLikeCount,
                ShareCount = obj.ShareCount,
                RetweetCount = obj.RetweetCount,
                Tags = ExtensionMethods.ListToString(obj.Tags),
                ModuleType = (byte) obj.ModuleType
            };
        }

        private Questionnaire ConvertToDomain(ModulesDao module)
        {
            return new Questionnaire
            {
                Id = module.ModuleId,
                Project = new Project { Id = module.ProjectId },
                ParentPhase = new Phase { Id = module.PhaseId },
                Title = module.Title,
                OnGoing = module.OnGoing,
                LikeCount = module.LikeCount,
                FbLikeCount = module.FbLikeCount,
                TwitterLikeCount = module.TwitterLikeCount,
                ShareCount = module.ShareCount,
                RetweetCount = module.RetweetCount,
                Tags = ExtensionMethods.StringToList(module.Tags),
                ModuleType = ModuleType.Questionnaire
            };
        }
        #endregion

        /*
         * @author Niels Van Zandbergen
         */
        #region Id generation
        private int FindNextAvailableQuestionnaireId()
        {
            if (!_ctx.Modules.Any()) return 1;
            int newId = _ctx.Modules.Max(q => q.ModuleId) + 1;
            return newId;
        }
        #endregion

        /*
         * @authors David Matei, Edwin Kai Yin Tam, Niels Van Zandbergen & Xander Veldeman
         */
        #region Questionnaire CRUD
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
        public Questionnaire Create(Questionnaire obj)
        {
            IEnumerable<Questionnaire> questionnaires = ReadAll(obj.Project.Id);

            foreach (Questionnaire q in questionnaires)
            {
                if (obj.ParentPhase.Id == q.ParentPhase.Id)
                {
                    throw new DuplicateNameException("Questionnaire(ID=" + obj.Id + ") heeft dezelfde parentPhase als Questionnaire(ID=" + q.Id + "). " +
                        "De phaseID is " + obj.ParentPhase.Id + ".");
                }
            }

            obj.Id = FindNextAvailableQuestionnaireId();
            ModulesDao newModule = ConvertToDao(obj);
            _ctx.Modules.Add(newModule);
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
        public Questionnaire Read(int id, bool details)
        {
            ModulesDao moduleDao = details ? _ctx.Modules.AsNoTracking().First(m => m.ModuleId == id) : _ctx.Modules.First(m => m.ModuleId == id);
            ExtensionMethods.CheckForNotFound(moduleDao, "Questionnaire", id);

            return ConvertToDomain(moduleDao);
        }

        public void Update(Questionnaire obj)
        {
            ModulesDao newModule = ConvertToDao(obj);
            ModulesDao foundModule = _ctx.Modules.First(q => q.ModuleId == obj.Id);
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
            ModulesDao toDelete = _ctx.Modules.First(q => q.ModuleId == id);
            _ctx.Modules.Remove(toDelete);
            _ctx.SaveChanges();
        }

        public IEnumerable<Questionnaire> ReadAll()
        {
            List<Questionnaire> myQuery = new List<Questionnaire>();

            foreach (ModulesDao dao in _ctx.Modules)
            {
                if (dao.ModuleType == 0)
                {
                    Questionnaire toAdd = ConvertToDomain(dao);
                    myQuery.Add(toAdd);
                }
            }

            return myQuery;
        }

        public IEnumerable<Questionnaire> ReadAll(int projectId)
        {
            return ReadAll().ToList().FindAll(q => q.Project.Id == projectId);
        }
        #endregion

        /*
         * @author Niels Van Zandbergen
         */
        #region Tag CRUD
        public string CreateTag(string obj, int moduleId)
        {
            Questionnaire moduleWTags = Read(moduleId, false);
            string oldTags = ExtensionMethods.ListToString(moduleWTags.Tags);
            oldTags += "," + obj;
            
            moduleWTags.Tags = ExtensionMethods.StringToList(oldTags);
            Update(moduleWTags);

            return obj;
        }

        public void DeleteTag(int moduleId, int tagId)
        {
            Questionnaire moduleWTags = Read(moduleId, false);
            ModulesDao module = ConvertToDao(moduleWTags);
            List<String> keptTags = ExtensionMethods.StringToList(module.Tags);
            keptTags.RemoveAt(tagId - 1);
            module.Tags = ExtensionMethods.ListToString(keptTags);
            _ctx.SaveChanges();
        }

        public IEnumerable<String> ReadAllTags(int moduleId)
        {
            return Read(moduleId, false).Tags;
        }
        #endregion
    }
}
