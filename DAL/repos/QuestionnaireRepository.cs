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
        private CityOfIdeasDbContext ctx;

        // Added by NVZ
        // Modified by XV
        public QuestionnaireRepository()
        {
            ctx = new CityOfIdeasDbContext();
        }

        // Added by NVZ
        // Standard Methods
        #region
        private ModulesDTO convertToDTO(Questionnaire obj)
        {
            return new ModulesDTO
            {
                ModuleID = obj.Id,
                ProjectID = obj.Project.Id,
                PhaseID = obj.ParentPhase.Id,
                OnGoing = obj.OnGoing,
                LikeCount = obj.LikeCount,
                FbLikeCount = obj.FbLikeCount,
                TwitterLikeCount = obj.TwitterLikeCount,
                ShareCount = obj.ShareCount,
                RetweetCount = obj.RetweetCount,
                Tags = ExtensionMethods.ListToString(obj.Tags),
                IsQuestionnaire = true
            };
        }

        private Questionnaire convertToDomain(ModulesDTO module)
        {
            return new Questionnaire
            {
                Id = module.ModuleID,
                Project = new Project { Id = module.ProjectID },
                ParentPhase = new Phase { Id = module.PhaseID },
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

            ctx.Modules.Add(convertToDTO(obj));
            ctx.SaveChanges();

            return obj;
        }
        
        public Questionnaire Read(int id, bool details)
        {
            ModulesDTO moduleDTO = null;

            if (details)
            {
                moduleDTO = ctx.Modules.AsNoTracking().First(m => m.ModuleID == id);
                ExtensionMethods.CheckForNotFound(moduleDTO, "Questionnaire", moduleDTO.ModuleID);
            }
            else
            {
                moduleDTO = ctx.Modules.First(m => m.ModuleID == id);
                ExtensionMethods.CheckForNotFound(moduleDTO, "Questionnaire", moduleDTO.ModuleID);
            }

            return convertToDomain(moduleDTO);
        }

        public void Update(Questionnaire obj)
        {
            ModulesDTO newModule = convertToDTO(obj);
            ModulesDTO foundModule = convertToDTO(Read(obj.Id,false));
            foundModule = newModule;

            ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            ctx.Modules.Remove(convertToDTO(Read(id, false)));
            ctx.SaveChanges();
        }
        
        public IEnumerable<Questionnaire> ReadAll()
        {
            IEnumerable<Questionnaire> myQuery = new List<Questionnaire>();

            foreach (ModulesDTO DTO in ctx.Modules)
            {
                myQuery.Append(convertToDomain(DTO));
            }

            return myQuery;
        }

        public IEnumerable<Questionnaire> ReadAll(int projectID)
        {
            return ReadAll().ToList().FindAll(q => q.Project.Id == projectID);
        }
        #endregion   

        // Added by NVZ
        #region
        public string createTag(string obj, int moduleID)
        {
            ModulesDTO module = convertToDTO(Read(moduleID, false));
            module.Tags += "," + obj;
            ctx.SaveChanges();

            return obj;
        }

        public void DeleteTag(int moduleID, int tagID)
        {
            List<String> keptTags = new List<string>();
            ModulesDTO module = convertToDTO(Read(moduleID, false));
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