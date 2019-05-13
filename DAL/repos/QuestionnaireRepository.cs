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
    public class QuestionnaireRepository : IRepository<Questionnaire>
    {
        private readonly CityOfIdeasDbContext _ctx;
        
        public QuestionnaireRepository()
        {
            _ctx = new CityOfIdeasDbContext();
        }

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
                IsQuestionnaire = obj.ModuleType == ModuleType.Questionnaire
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
            };
        }
        #endregion
        
        #region Id generation
        private int FindNextAvailableQuestionnaireId()
        {
            if (!_ctx.Modules.Any()) return 1;
            int newId = _ctx.Modules.Max(q => q.ModuleId) + 1;
            return newId;
        }
        #endregion
        
        #region Questionnaire CRUD
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
            _ctx.Modules.Add(ConvertToDao(obj));
            _ctx.SaveChanges();

            return obj;
        }
        
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
                foundModule.OnGoing = newModule.OnGoing;
                foundModule.LikeCount = newModule.LikeCount;
                foundModule.Title = newModule.Title;
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
            ModulesDao toDelete = _ctx.Modules.First(q => q.ModuleId == id);
            _ctx.Modules.Remove(toDelete);
            _ctx.SaveChanges();
        }
        
        public IEnumerable<Questionnaire> ReadAll()
        {
            List<Questionnaire> myQuery = new List<Questionnaire>();

            foreach (ModulesDao dao in _ctx.Modules)
            {
                if (dao.IsQuestionnaire)
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

        #region Tag CRUD
        public string CreateTag(string obj, int moduleId)
        {
            Questionnaire moduleWTags = Read(moduleId, false);
            ModulesDao module = ConvertToDao(moduleWTags);
            module.Tags += "," + obj;
            _ctx.SaveChanges();

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
            return Read(moduleId, true).Tags;
        }
        #endregion
    }
}