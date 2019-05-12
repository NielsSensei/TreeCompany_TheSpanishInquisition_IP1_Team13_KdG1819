using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL.Contexts;
using DAL.Data_Access_Objects;
using Domain.Common;
using Domain.Projects;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Users;

namespace DAL.repos
{
    public class ProjectRepository : IRepository<Project>
    {
        private readonly CityOfIdeasDbContext _ctx;
        
        public ProjectRepository()
        {
            _ctx = new CityOfIdeasDbContext();
        }
        
        #region Conversion Methods
        private Project ConvertToDomain(ProjectsDao dao)
        {
            return new Project()
            {
                Id = dao.ProjectId,
                CurrentPhase = new Phase() { Id = dao.CurrentPhaseId },
                User = new UimvcUser() { Id = dao.UserId },
                Platform = new Platform() { Id = dao.PlatformId },
                Title = dao.Title,
                Goal = dao.Goal,
                Status = dao.Status,
                Visible = dao.Visible,
                LikeVisibility = dao.LikeVisibility,
                ReactionCount = dao.ReactionCount,
                LikeCount = dao.LikeCount,
                FbLikeCount = dao.FbLikeCount,
                TwitterLikeCount = dao.TwitterLikeCount
            };
        }

        private ProjectsDao ConvertToDao(Project project)
        {
            return new ProjectsDao()
            {
                ProjectId = project.Id,
                UserId = project.User.Id,
                PlatformId = project.Platform.Id,
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

        private Phase ConvertToDomain(PhasesDao dao)
        {
            return new Phase
            {
                Id = dao.PhaseId,
                Project = new Project { Id = dao.ProjectId },
                Description = dao.Description,
                StartDate = dao.StartDate,
                EndDate = dao.EndDate
            };
        }

        private PhasesDao ConvertToDao(Phase phase)
        {
            return new PhasesDao
            {
                PhaseId = phase.Id,
                ProjectId = phase.Project.Id,
                Description = phase.Description,
                StartDate = phase.StartDate,
                EndDate = phase.EndDate
            };
        }
        #endregion
        
        #region Id generation
        private int FindNextAvailableProjectId()
        {               
            if (!_ctx.Projects.Any()) return 1;
            int newId = ReadAll().Max(platform => platform.Id)+1;
            return newId;
        }

        private int FindNextAvailablePhaseId()
        {               
            if (!_ctx.Phases.Any()) return 1;
            int newId = ReadAllPhases().Max(platform => platform.Id)+1;
            return newId;
        }
        #endregion

        #region Project CRUD
        public Project Create(Project obj)
        {
            IEnumerable<Project> projects = ReadAllForPlatform(obj.Platform.Id);

            if (projects != null)
            {
                foreach (Project p in projects)
                {
                    if (ExtensionMethods.HasMatchingWords(p.Title, obj.Title) > 0)
                    {
                        throw new DuplicateNameException(
                            "Dit project bestaat al of is misschien gelijkaardig. Project(ID=" + obj.Id +
                            ") dat je wil aanmaken: " +
                            obj.Title + ". Project(ID=" + p.Title + ") dat al bestaat: " + p.Title + ".");
                    }
                }
            }

            obj.Id = FindNextAvailableProjectId();
            _ctx.Projects.Add(ConvertToDao(obj));
            _ctx.SaveChanges();

            return obj; 
        }
        
        public Project Read(int id, bool details)
        {
            ProjectsDao projectsDao = details ? _ctx.Projects.AsNoTracking().FirstOrDefault(p => p.ProjectId == id) : _ctx.Projects.FirstOrDefault(p => p.ProjectId == id);
            ExtensionMethods.CheckForNotFound(projectsDao, "Project", id);
            
            return ConvertToDomain(projectsDao);
        }

        public void Update(Project obj)
        {
            ProjectsDao newProj = ConvertToDao(obj);
            newProj.CurrentPhaseId = obj.CurrentPhase.Id;
            
            ProjectsDao foundProj = _ctx.Projects.First(p => p.ProjectId == obj.Id);
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
                foundProj.CurrentPhaseId = newProj.CurrentPhaseId;
            }

            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            ProjectsDao toDelete = _ctx.Projects.First(p => p.ProjectId == id);
            _ctx.Projects.Remove(toDelete);
            _ctx.SaveChanges();
        }
        
        public IEnumerable<Project> ReadAll()
        {
            List<Project> myQuery = new List<Project>();

            foreach(ProjectsDao dao in _ctx.Projects)
            {
                myQuery.Add(ConvertToDomain(dao));
            }

            return myQuery;
        }

        public IEnumerable<Project> ReadAllForPlatform(int platformId)
        {
            return ReadAll().ToList().FindAll(p => p.Platform.Id == platformId);
        }
        #endregion        
        
        #region Phase CRUD
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
            _ctx.Phases.Add(ConvertToDao(obj));
            _ctx.SaveChanges();
            
            return obj;
        }

        public Phase ReadPhase(int phaseId, bool details)
        {
            PhasesDao phasesDao = details ? _ctx.Phases.AsNoTracking().First(p => p.PhaseId == phaseId) : _ctx.Phases.First(p => p.PhaseId == phaseId);
            ExtensionMethods.CheckForNotFound(phasesDao, "Phase", phaseId);

            return ConvertToDomain(phasesDao);
        }

        public void Update(Phase obj)
        {
            PhasesDao newPhase = ConvertToDao(obj);
            PhasesDao foundPhase = _ctx.Phases.First(p => p.PhaseId == obj.Id);
            if (foundPhase != null)
            {
                foundPhase.Description = newPhase.Description;
                foundPhase.StartDate = newPhase.StartDate;
                foundPhase.EndDate = newPhase.EndDate;
            }

            _ctx.SaveChanges();
        }

        public void DeletePhase(int phaseId)
        {
            PhasesDao toDelete = _ctx.Phases.First(p => p.PhaseId == phaseId);
            _ctx.Phases.Remove(toDelete);
            _ctx.SaveChanges();
        }

        public IEnumerable<Phase> ReadAllPhases()
        {
            List<Phase> myQuery = new List<Phase>();

            foreach (PhasesDao dao in _ctx.Phases)
            {
                myQuery.Add(ConvertToDomain(dao));
            }

            return myQuery;
        }
        
        public IEnumerable<Phase> ReadAllPhases(int projectId)
        {
            return ReadAllPhases().ToList().FindAll(p => p.Project.Id == projectId);
        }
        #endregion
        
        #region Images CRUD
        // TODO: (SPRINT2?) Als we images kunnen laden enal is het bonus, geen prioriteit tegen Sprint 1.
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