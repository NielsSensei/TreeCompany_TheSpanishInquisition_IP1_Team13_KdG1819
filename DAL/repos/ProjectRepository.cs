using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Domain;
using Domain.Common;
using Domain.Projects;
using DAL.Contexts;
using DAL.Data_Transfer_Objects;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Users;

namespace DAL
{
    public class ProjectRepository : IRepository<Project>
    {
        // Added by DM
        // Modified by NVZ
        private readonly CityOfIdeasDbContext ctx;

        // Added by DM
        // Modified by NVZ & XV
        public ProjectRepository()
        {
            ctx = new CityOfIdeasDbContext();
        }

        // Added by NVZ
        // Standard Methods
        #region
        private Project ConvertToDomain(ProjectsDTO DTO)
        {
            return new Project()
            {
                Id = DTO.ProjectID,
                CurrentPhase = new Phase() { Id = DTO.CurrentPhaseID },
                User = new UIMVCUser() { Id = DTO.UserID },
                Platform = new Platform() { Id = DTO.PlatformID },
                Title = DTO.Title,
                Goal = DTO.Goal,
                Status = DTO.Status,
                Visible = DTO.Visible,
                LikeVisibility = DTO.LikeVisibility,
                ReactionCount = DTO.ReactionCount,
                LikeCount = DTO.LikeCount,
                FbLikeCount = DTO.FbLikeCount,
                TwitterLikeCount = DTO.TwitterLikeCount
            };
        }

        private ProjectsDTO ConvertToDTO(Project project)
        {
            return new ProjectsDTO()
            {
                ProjectID = project.Id,
                CurrentPhaseID = project.CurrentPhase.Id,
                UserID = project.User.Id,
                PlatformID = project.Platform.Id,
                Title = project.Title,
                Goal = project.Goal,
                Status = project.Status,
                Visible = project.Visible,
                LikeVisibility = project.LikeVisibility,
                ReactionCount = project.ReactionCount,
                LikeCount = project.LikeCount,
                FbLikeCount = project.FbLikeCount,
                TwitterLikeCount = project.TwitterLikeCount
            };
        }

        private Phase ConvertToDomain(PhasesDTO DTO)
        {
            return new Phase
            {
                Id = DTO.PhaseID,
                Project = new Project { Id = DTO.ProjectID },
                Description = DTO.Description,
                StartDate = DTO.StartDate,
                EndDate = DTO.EndDate
            };
        }

        private PhasesDTO ConvertToDTO(Phase phase)
        {
            return new PhasesDTO
            {
                PhaseID = phase.Id,
                ProjectID = phase.Project.Id,
                Description = phase.Description,
                StartDate = phase.StartDate,
                EndDate = phase.EndDate
            };
        }
        
        private int FindNextAvailableProjectId()
        {               
            int newId = ReadAll().Max(platform => platform.Id)+1;
            return newId;
        }
        
        private int FindNextAvailablePhaseId()
        {               
            int newId = ReadAllPhases().Max(platform => platform.Id)+1;
            return newId;
        }
        #endregion

        // Added by NVZ
        // Project CRUD
        #region
        /* 
         * Binnen deze methode vergelijkt hij de titels van heel het platform met de titel van het nieuwe project. Als hij een gelijkenis gevonden heeft
         * dan gooit hij de Exception. Hij doet de vergelijking op basis van een extensionmethod die zeer uitbreidbaar is.
         * 
         * @author Niels Van Zandbergen
         * @see Extensionmethods.HasMatchingWords(string left, string right);
         * @return Het aangemaakte object.
         * 
         */
            public Project Create(Project obj)
            {
            IEnumerable<Project> projects = ReadAllForPlatform(obj.Platform.Id);

            foreach(Project p in projects){
                if(ExtensionMethods.HasMatchingWords(p.Title, obj.Title) > 0)
                {
                    throw new DuplicateNameException("Dit project bestaat al of is misschien gelijkaardig. Project(ID=" + obj.Id + ") dat je wil aanmaken: " + 
                        obj.Title + ". Project(ID=" + p.Title + ") dat al bestaat: " + p.Title + ".");
                }
            }

            obj.Id = FindNextAvailableProjectId();
            ctx.Projects.Add(ConvertToDTO(obj));
            ctx.SaveChanges();

            return obj; 
            }

        /*
         *Hij haalt het project op, hij kijkt of het null is en gooit anders een exception als het zo is. De exception wordt hier gegooid binnen
         *de extensionmethod zelf.
         * 
         *@author Niels Van Zandbergen
         *@see ExtensionMethods.CheckForNotFound(Object obj, string datatype, int id)
         *@return Het gevonden project.
         * 
         */
        public Project Read(int id, bool details)
        {
            ProjectsDTO projectsDTO = null;
            projectsDTO = details ? ctx.Projects.AsNoTracking().First(p => p.ProjectID == id) : ctx.Projects.First(p => p.ProjectID == id);
            ExtensionMethods.CheckForNotFound(projectsDTO, "Project", id);
            
            return ConvertToDomain(projectsDTO);
        }

        public void Update(Project obj)
        {
            ProjectsDTO newProj = ConvertToDTO(obj);
            ProjectsDTO foundProj = ctx.Projects.First(p => p.ProjectID == obj.Id);
            if (foundProj != null)
            {
                foundProj.Title = newProj.Title;
                foundProj.Goal = newProj.Goal;
                foundProj.Status = newProj.Status;
                foundProj.Visible = newProj.Visible;
                foundProj.ReactionCount = newProj.ReactionCount;
                foundProj.LikeCount = newProj.LikeCount;
                foundProj.FbLikeCount = newProj.FbLikeCount;
                foundProj.TwitterLikeCount = newProj.TwitterLikeCount;
                foundProj.LikeVisibility = newProj.LikeVisibility;
            }

            ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            ProjectsDTO toDelete = ctx.Projects.First(p => p.ProjectID == id);
            ctx.Projects.Remove(toDelete);
            ctx.SaveChanges();
        }
        
        public IEnumerable<Project> ReadAll()
        {
            List<Project> myQuery = new List<Project>();

            foreach(ProjectsDTO DTO in ctx.Projects)
            {
                myQuery.Add(ConvertToDomain(DTO));
            }

            return myQuery;
        }

        public IEnumerable<Project> ReadAllForPlatform(int platformID)
        {
            return ReadAll().ToList().FindAll(p => p.Platform.Id == platformID);
        }
        #endregion        
        
        // Added by NVZ
        // Phase CRUD
        #region
        public Phase Create(Phase obj)
        {
            IEnumerable<Phase> phases = ReadAllPhases(obj.Project.Id);

            foreach(Phase p in phases)
            {
                if (p.StartDate > obj.StartDate && p.EndDate < obj.EndDate)
                {
                    throw new DuplicateNameException("Deze phase met ID " + obj.Id + " (Start: " + obj.StartDate + ", Einde: " + obj.EndDate + ") overlapt" +
                        " met een andere phase met ID " + p.Id + " (Start: " + p.StartDate + ", Einde: " + p.EndDate + ")");
                }
            }

            obj.Id = FindNextAvailablePhaseId();
            ctx.Phases.Add(ConvertToDTO(obj));
            ctx.SaveChanges();
            
            return obj;
        }

        public Phase ReadPhase(int phaseID, bool details)
        {
            PhasesDTO phasesDTO = null;
            phasesDTO = details ? ctx.Phases.AsNoTracking().First(p => p.PhaseID == phaseID) : ctx.Phases.First(p => p.PhaseID == phaseID);
            ExtensionMethods.CheckForNotFound(phasesDTO, "Phase", phaseID);

            return ConvertToDomain(phasesDTO);
        }

        public void Update(Phase obj)
        {
            PhasesDTO newPhase = ConvertToDTO(obj);
            PhasesDTO foundPhase = ctx.Phases.First(p => p.PhaseID == obj.Id);
            if (foundPhase != null)
            {
                foundPhase.Description = newPhase.Description;
                foundPhase.StartDate = newPhase.StartDate;
                foundPhase.EndDate = newPhase.EndDate;
            }

            ctx.SaveChanges();
        }

        public void DeletePhase(int phaseID)
        {
            PhasesDTO toDelete = ctx.Phases.First(p => p.PhaseID == phaseID);
            ctx.Phases.Remove(toDelete);
            ctx.SaveChanges();
        }

        public IEnumerable<Phase> ReadAllPhases()
        {
            List<Phase> myQuery = new List<Phase>();

            foreach (PhasesDTO DTO in ctx.Phases)
            {
                myQuery.Add(ConvertToDomain(DTO));
            }

            return myQuery;
        }
        
        public IEnumerable<Phase> ReadAllPhases(int projectID)
        {
            return ReadAllPhases().ToList().FindAll(p => p.Project.Id == projectID);
        }
        #endregion
        
        // Added by NVZ
        // Images CRUD
        // TODO: (SPRINT2?) Als we images kunnen laden enal is het bonus, geen prioriteit tegen Sprint 1.
        #region
        public Image Create(Image obj)
        {
            /* if (!images.Contains(obj))
            {
                images.Add(obj);
            } */
            throw new DuplicateNameException("This Image already exists!");
        }

        public Image Read(int projectID, int imageID)
        {
            /* Image i = Read(projectID).PreviewImages.ToList()[imageID - 1];
            if (i != null)
            {
                return i;
            } */
            throw new KeyNotFoundException("This Image can't be found!");
        }

        public void Update(Image obj)
        {
            //DeleteImage(obj);
            //Create(obj);
        }

        public void DeleteImage(Image obj)
        {
            //images.Remove(obj);
        }
        #endregion
    }
}