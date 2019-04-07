using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Domain;
using Domain.Common;
using Domain.Projects;
using DAL.Contexts;
using DAL.Data_Transfer_Objects;
using Microsoft.EntityFrameworkCore;
using Domain.Users;

namespace DAL
{
    public class ProjectRepository : IRepository<Project>
    {
        // Added by DM
        // Modified by NVZ
        private CityOfIdeasDbContext ctx;

        // Added by DM
        // Modified by NVZ & XV
        public ProjectRepository()
        {
            ctx = new CityOfIdeasDbContext();
        }

        // Added by NVZ
        // Standard Methods
        #region
        private Project convertToDomain(ProjectsDTO DTO)
        {
            return new Project()
            {
                Id = DTO.ProjectID,
                CurrentPhase = new Phase() { Id = DTO.CurrentPhaseID },
                User = new User() { Id = DTO.UserID },
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

        private ProjectsDTO convertToDTO(Project project)
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

        private Phase convertToDomain(PhasesDTO DTO)
        {
            return new Phase
            {
                Id = DTO.PhaseID,
                Project = new Project { Id = DTO.ProjectID },
                Description = DTO.Description,
                StartDate = DTO.startDate,
                EndDate = DTO.endDate
            };
        }

        private PhasesDTO convertToDTO(Phase phase)
        {
            return new PhasesDTO
            {
                PhaseID = phase.Id,
                ProjectID = phase.Project.Id,
                Description = phase.Description,
                startDate = phase.StartDate,
                endDate = phase.EndDate
            };
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
            IEnumerable<Project> projects = ReadAll(obj.Platform.Id);

            foreach(Project p in projects){
                if(ExtensionMethods.HasMatchingWords(p.Title, obj.Title) > 0)
                {
                    throw new DuplicateNameException("Dit project bestaat al of is misschien gelijkaardig. Project(ID=" + obj.Id + ") dat je wil aanmaken: " + 
                        obj.Title + ". Project(ID=" + p.Title + ") dat al bestaat: " + p.Title + ".");
                }
            }

            ctx.Projects.Add(convertToDTO(obj));
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

            if (details)
            {
                projectsDTO = ctx.Projects.AsNoTracking().First(p => p.ProjectID == id);
                ExtensionMethods.CheckForNotFound(projectsDTO, "Project", projectsDTO.ProjectID);                          
            }else
            {
                projectsDTO = ctx.Projects.First(p => p.ProjectID == id);
                ExtensionMethods.CheckForNotFound(projectsDTO, "Project", projectsDTO.ProjectID);
            }
            
            return convertToDomain(projectsDTO);
        }

        public void Update(Project obj)
        {
            ProjectsDTO newProj = convertToDTO(obj);
            ProjectsDTO foundProj = convertToDTO(Read(newProj.ProjectID, false));
            foundProj = newProj;
            ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            ctx.Projects.Remove(convertToDTO(Read(id, false)));
            ctx.SaveChanges();
        }
        
        public IEnumerable<Project> ReadAll()
        {
            IEnumerable<Project> myQuery = new List<Project>();

            foreach(ProjectsDTO DTO in ctx.Projects)
            {
                myQuery.Append(convertToDomain(DTO));
            }

            return myQuery;
        }

        public IEnumerable<Project> ReadAll(int platformID)
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

            return obj;
        }

        public Phase ReadPhase(int projectID, int phaseID, bool details)
        {
            PhasesDTO phasesDTO = null;

            if (details)
            {
                phasesDTO = ctx.Phases.AsNoTracking().First(p => p.PhaseID == phaseID);
                ExtensionMethods.CheckForNotFound(phasesDTO, "Phase", phasesDTO.PhaseID);
            }
            else
            {
                phasesDTO = ctx.Phases.First(p => p.PhaseID == phaseID);
                ExtensionMethods.CheckForNotFound(phasesDTO, "Phase", phasesDTO.PhaseID);
            }

            return convertToDomain(phasesDTO);
        }

        public void Update(Phase obj)
        {
            PhasesDTO newPhase = convertToDTO(obj);
            PhasesDTO foundPhase = convertToDTO(ReadPhase(obj.Project.Id, obj.Id, false));
            foundPhase = newPhase;
            ctx.SaveChanges();
        }

        public void Delete(int projectID, int phaseID)
        {
            ctx.Phases.Remove(convertToDTO(ReadPhase(projectID,phaseID, false)));
            ctx.SaveChanges();
        }

        public IEnumerable<Phase> ReadAllPhases(int projectID)
        {
            IEnumerable<Phase> myQuery = new List<Phase>();

            foreach (PhasesDTO DTO in ctx.Phases)
            {
                myQuery.Append(convertToDomain(DTO));
            }

            return myQuery;
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