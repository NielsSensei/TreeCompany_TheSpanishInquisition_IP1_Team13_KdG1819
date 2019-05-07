using System;
using System.Collections.Generic;
using System.Data;
using Domain;
using Domain.Projects;
using DAL.Contexts;
using DAL.Data_Transfer_Objects;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL
{
    public class QuestionnaireRepository : IRepository<Questionnaire>
    {
        // Added by DM
        // Modified by NVZ
        private readonly CityOfIdeasDbContext ctx;

        // Added by NVZ
        // Modified by XV
        public QuestionnaireRepository()
        {
            ctx = new CityOfIdeasDbContext();
        }

        // Added by NVZ
        // Standard Methods
        #region
        private ModulesDTO ConvertToDTO(Questionnaire obj)
        {
            return new ModulesDTO
            {
                ModuleID = obj.Id,
                ProjectID = obj.Project.Id,
                PhaseID = obj.ParentPhase.Id,
                OnGoing = obj.OnGoing,
                Title = obj.Title,
                LikeCount = obj.LikeCount,
                FbLikeCount = obj.FbLikeCount,
                TwitterLikeCount = obj.TwitterLikeCount,
                ShareCount = obj.ShareCount,
                RetweetCount = obj.RetweetCount,
                Tags = ExtensionMethods.ListToString(obj.Tags),
                IsQuestionnaire = obj.type == ModuleType.Questionnaire
            };
        }

        private Questionnaire ConvertToDomain(ModulesDTO module)
        {
            return new Questionnaire
            {
                Id = module.ModuleID,
                Project = new Project { Id = module.ProjectID },
                ParentPhase = new Phase { Id = module.PhaseID },
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
        
        private int FindNextAvailableQuestionnaireId()
        {
            int newId = ctx.Modules.Max(q => q.ModuleID) + 1;
            return newId;
        }
        #endregion

        // Added by NVZ
        // Questionnaire CRUD
        #region
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
            ModulesDTO newModule = ConvertToDTO(obj);
           
            ctx.Modules.Add(ConvertToDTO(obj));
            ctx.SaveChanges();

            return obj;
        }
        
        public Questionnaire Read(int id, bool details)
        {
            ModulesDTO moduleDTO = null;
            moduleDTO = details ? ctx.Modules.AsNoTracking().First(m => m.ModuleID == id) : ctx.Modules.First(m => m.ModuleID == id);
            ExtensionMethods.CheckForNotFound(moduleDTO, "Questionnaire", id);
            
            return ConvertToDomain(moduleDTO);
        }

        public void Update(Questionnaire obj)
        {
            ModulesDTO newModule = ConvertToDTO(obj);
            ModulesDTO foundModule = ctx.Modules.First(q => q.ModuleID == obj.Id);
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

            if (newModule.PhaseID != foundModule.PhaseID)
            {
                foundModule.PhaseID = newModule.PhaseID;
            }

            ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            ModulesDTO toDelete = ctx.Modules.First(q => q.ModuleID == id);
            ctx.Modules.Remove(toDelete);
            ctx.SaveChanges();
        }
        
        public IEnumerable<Questionnaire> ReadAll()
        {
            List<Questionnaire> myQuery = new List<Questionnaire>();

            foreach (ModulesDTO DTO in ctx.Modules)
            {
                if (DTO.IsQuestionnaire)
                {
                    Questionnaire toAdd = ConvertToDomain(DTO);
                    myQuery.Add(toAdd);  
                }             
            }

            return myQuery;
        }

        public IEnumerable<Questionnaire> ReadAll(int projectID)
        {
            return ReadAll().ToList().FindAll(q => q.Project.Id == projectID);
        }

        private ModulesDTO GrabModuleInformationDTO(Questionnaire obj)
        {
            ModulesDTO DTO = new ModulesDTO
            {
                ModuleID = obj.Id,
                OnGoing = obj.OnGoing,
                Title = obj.Title,
                LikeCount = obj.LikeCount,
                FbLikeCount = obj.FbLikeCount,
                TwitterLikeCount = obj.TwitterLikeCount,
                ShareCount = obj.ShareCount,
                RetweetCount = obj.RetweetCount,
                Tags = ExtensionMethods.ListToString(obj.Tags),
                IsQuestionnaire = obj.type == ModuleType.Questionnaire
            };

            if (obj.Project != null)
            {
                DTO.ProjectID = obj.Project.Id;
            }

            if (obj.ParentPhase != null)
            {
                DTO.PhaseID = obj.ParentPhase.Id;
            }

            return DTO;
        }
        #endregion   

        // Added by NVZ
        // Tag CRUD
        #region
        public string CreateTag(string obj, int moduleID)
        {
            Questionnaire moduleWTags = Read(moduleID, false);
            ModulesDTO module = ConvertToDTO(moduleWTags);
            module.Tags += "," + obj;
            ctx.SaveChanges();

            return obj;
        }

        public void DeleteTag(int moduleID, int tagID)
        {
            List<String> keptTags = new List<string>();
            Questionnaire moduleWTags = Read(moduleID, false);
            ModulesDTO module = ConvertToDTO(moduleWTags);
            keptTags = ExtensionMethods.StringToList(module.Tags);
            keptTags.RemoveAt(tagID - 1);
            module.Tags = ExtensionMethods.ListToString(keptTags);
            ctx.SaveChanges();
        }

        public IEnumerable<String> ReadAllTags(int moduleID)
        {
            return Read(moduleID, true).Tags;
        }
        #endregion
    }
}