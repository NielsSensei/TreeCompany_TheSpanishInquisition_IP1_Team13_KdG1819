using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Domain.Common;
using Domain.Projects;
using DAL.Contexts;
using DAL.Data_Transfer_Objects;
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
                IsQuestionnaire = false
            };
        }

        private IdeationsDTO ConvertToDTO(Ideation obj)
        {
            bool Org = false;

            if (obj.User.Role == Role.LOGGEDINORG)
                Org = true;

            return new IdeationsDTO
            {
                    ModuleID = obj.Id,
                    UserID = obj.User.Id,
                    ExtraInfo = obj.ExtraInfo,
                    Organisation = Org,
                    EventID = obj.Event.Id,
                    UserIdea = obj.UserIdea,
                    MediaFile = obj.Media,
                    RequiredFields = (byte) obj.RequiredFields
            };
        }

        private Ideation ConvertToDomain(IdeationsDTO DTO)
        {
            return new Ideation
            {
                Id = DTO.ModuleID,
                User = new User { Id = DTO.UserID },
                UserIdea = DTO.UserIdea,
                Event = new Event { Id = DTO.EventID },
                Media = DTO.MediaFile,
                ExtraInfo = DTO.ExtraInfo,
                RequiredFields = DTO.RequiredFields
            };
        }

        private Ideation IdeationWithModules(Ideation ideation , ModulesDTO DTO)
        {
            ideation.Project = new Project { Id = DTO.ProjectID };
            ideation.ParentPhase = new Phase { Id = DTO.PhaseID };
            ideation.OnGoing = DTO.OnGoing;
            ideation.LikeCount = DTO.LikeCount;
            ideation.FbLikeCount = DTO.FbLikeCount;
            ideation.TwitterLikeCount = DTO.TwitterLikeCount;
            ideation.ShareCount = DTO.ShareCount;
            ideation.RetweetCount = DTO.RetweetCount;
            ideation.Tags = ExtensionMethods.StringToList(DTO.Tags);
            return ideation;
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

            ctx.Modules.Add(GrabModuleInformationDTO(obj));
            ctx.Ideations.Add(ConvertToDTO(obj));
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
            Ideation found = Read(obj.Id, false);
            IdeationsDTO foundIdeation = ConvertToDTO(found);
            foundIdeation = newIdeation;

            ModulesDTO newModule = GrabModuleInformationDTO(obj);
            Ideation foundWModule = ReadWithModule(obj.Id);
            ModulesDTO foundModule = GrabModuleInformationDTO(foundWModule);
            foundModule = newModule;

            ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            Ideation toDelete = Read(id, false);
            ctx.Ideations.Remove(ConvertToDTO(toDelete));
            Ideation toDeleteModule = Read(id, false);
            ctx.Modules.Remove(GrabModuleInformationDTO(toDeleteModule));
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
            Ideation ideationWTags = Read(moduleID, false);
            ModulesDTO module = GrabModuleInformationDTO(ideationWTags);
            module.Tags += "," + obj;
            ctx.SaveChanges();

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