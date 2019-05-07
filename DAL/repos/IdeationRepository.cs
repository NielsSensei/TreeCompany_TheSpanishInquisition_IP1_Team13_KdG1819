using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Domain.Common;
using Domain.Projects;
using DAL.Contexts;
using DAL.Data_Transfer_Objects;
using Domain.Identity;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class IdeationRepository : IRepository<Ideation>
    {
        // Added by DM
        // Modified by NVZ & XV
        private CityOfIdeasDbContext ctx;

        // Added by NVZ
        public IdeationRepository()
        {
            ctx = new CityOfIdeasDbContext();
        }

        // Added by NVZ
        // Standard Methods
        #region
        private ModulesDTO GrabModuleInformationDTO(Ideation obj)
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

        // XV: TODO Create a check for organisation accounts
        private IdeationsDTO ConvertToDTO(Ideation obj)
        {
            //bool Org = obj.User.Role == Role.LOGGEDINORG;
            IdeationsDTO DTO = new IdeationsDTO()
            {
                    ModuleID = obj.Id,
                    UserID = obj.User.Id,
                    ExtraInfo = obj.ExtraInfo,
                    //MediaFile = obj.Media,
                    RequiredFields = (byte) obj.RequiredFields
            };

            if (obj.Event != null)
            {
                DTO.EventID = obj.Event.Id;
                DTO.UserIdea = obj.UserIdea;
                //DTO.Organisation = Org;
            }

            return DTO;
        }

        private Ideation ConvertToDomain(IdeationsDTO DTO)
        {
            return new Ideation
            {
                Id = DTO.ModuleID,
                User = new UIMVCUser { Id = DTO.UserID },
                UserIdea = DTO.UserIdea,
                Event = new Event { Id = DTO.EventID },
                //Media = DTO.MediaFile,
                ExtraInfo = DTO.ExtraInfo,
                RequiredFields = DTO.RequiredFields
            };
        }

        private Ideation IdeationWithModules(Ideation ideation , ModulesDTO DTO)
        {
            ideation.Project = new Project { Id = DTO.ProjectID };
            ideation.ParentPhase = new Phase { Id = DTO.PhaseID };
            ideation.Title = DTO.Title;
            ideation.OnGoing = DTO.OnGoing;
            ideation.LikeCount = DTO.LikeCount;
            ideation.FbLikeCount = DTO.FbLikeCount;
            ideation.TwitterLikeCount = DTO.TwitterLikeCount;
            ideation.ShareCount = DTO.ShareCount;
            ideation.RetweetCount = DTO.RetweetCount;
            ideation.Tags = ExtensionMethods.StringToList(DTO.Tags);
            return ideation;
        }
        
        private int FindNextAvailableIdeationId()
        {               
            int newId = ctx.Modules.Max(q => q.ModuleID) + 1;
            return newId;
        }
        #endregion

        // Added by NVZ
        // Ideation CRUD
        #region
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
            ModulesDTO newModule = GrabModuleInformationDTO(obj);
            IdeationsDTO newIdeation = ConvertToDTO(obj);
            
            ctx.Modules.Add(newModule);
            ctx.Ideations.Add(newIdeation);
            ctx.SaveChanges();

            return obj;
        }

        public Ideation Read(int id, bool details)
        {
            IdeationsDTO ideationDTO = null;
            ideationDTO = details ? ctx.Ideations.AsNoTracking().First(m => m.ModuleID == id) : ctx.Ideations.First(m => m.ModuleID == id);
            ExtensionMethods.CheckForNotFound(ideationDTO, "Ideation", id);

            return ConvertToDomain(ideationDTO);
        }

        public Ideation ReadWithModule(int id)
        {
            Ideation ideation = Read(id, true);
            ModulesDTO DTO = ctx.Modules.First(m => m.ModuleID == id);

            ideation = IdeationWithModules(ideation, DTO);

            return ideation;
        }

        public void Update(Ideation obj)
        {
            IdeationsDTO newIdeation = ConvertToDTO(obj);
            IdeationsDTO foundIdeation = ctx.Ideations.FirstOrDefault(dto => dto.ModuleID == newIdeation.ModuleID);
            if (foundIdeation != null)
            {
                foundIdeation.ExtraInfo = newIdeation.ExtraInfo;
                foundIdeation.MediaFile = newIdeation.MediaFile;
                foundIdeation.RequiredFields = newIdeation.RequiredFields;
            }
            
            ModulesDTO newModule = GrabModuleInformationDTO(obj);
            ModulesDTO foundModule = ctx.Modules.FirstOrDefault(dto => dto.ModuleID == newModule.ModuleID);
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

            ctx.SaveChanges();
        }

        public void Delete(int id)
        {            
            IdeationsDTO toDelete = ctx.Ideations.First(r => r.ModuleID == id);
            ctx.Ideations.Remove(toDelete);
            ModulesDTO toDeleteModule = ctx.Modules.First(r => r.ModuleID == id);
            ctx.Modules.Remove(toDeleteModule);
            ctx.SaveChanges();
        }
        
        public IEnumerable<Ideation> ReadAll()
        {
            List<Ideation> myQuery = new List<Ideation>();

            foreach (IdeationsDTO DTO in ctx.Ideations)
            {
                Ideation toAdd = ReadWithModule(DTO.ModuleID);
                myQuery.Add(toAdd);
            }

            return myQuery;
        }

        public IEnumerable<Ideation> ReadAll(int projectID)
        {
            return ReadAll().ToList().FindAll(i => i.Project.Id == projectID);
        }
        #endregion

        // Added by NVZ
        // Media CRUD     
        // TODO: (SPRINT2?) Als we images kunnen laden enal is het bonus, geen prioriteit tegen Sprint 1.
        #region

        public Media Create(Media obj)
        {
            //if (!mediafiles.Contains(obj))
            //{
            //    mediafiles.Add(obj);
            //}
            throw new DuplicateNameException("This MediaFile already exist!");
        }

        public Media ReadMedia(int ideationID)
        {
            //Media m = Read(ideationID).Media;
            //if (m != null)
            //{
            //    return m;
            //}
            throw new KeyNotFoundException("This Media can't be found!");
        }

        public void DeleteMedia(int ideationID)
        {
            //Media m = ReadMedia(ideationID);
            //if (m != null)
            //{
            //    mediafiles.Remove(m);
            //}
        }
        #endregion

        // Added by NVZ
        // Tag CRUD
        #region
        public string CreateTag(string obj, int moduleID)
        {
            Ideation ideationWTags = ReadWithModule(moduleID);
            string oldTags = ExtensionMethods.ListToString(ideationWTags.Tags);
            oldTags += "," + obj;

            ideationWTags.Tags = ExtensionMethods.StringToList(oldTags);
            Update(ideationWTags);
            
            return obj;
        }

        public void DeleteTag(int moduleID, int tagID)
        {
            List<String> keptTags = new List<string>();
            Ideation ideationWTags = Read(moduleID, false);
            ModulesDTO module = GrabModuleInformationDTO(ideationWTags);
            keptTags = ExtensionMethods.StringToList(module.Tags);
            keptTags.RemoveAt(tagID - 1);
            module.Tags = ExtensionMethods.ListToString(keptTags);
            ctx.SaveChanges();
        }

        public IEnumerable<String> ReadAllTags(int moduleID)
        {
            return Read(moduleID,true).Tags;
        }
        #endregion
    }
}