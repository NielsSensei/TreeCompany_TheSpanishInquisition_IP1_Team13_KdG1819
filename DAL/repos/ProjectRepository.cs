using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL.Contexts;
using DAL.Data_Access_Objects;
using Domain.Projects;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Users;

namespace DAL.repos
{
    /*
     * @authors David Matei, Edwin Kai Yin Tam & Niels Van Zandbergen
     */
    public class ProjectRepository : IRepository<Project>
    {
        private readonly CityOfIdeasDbContext _ctx;

        public ProjectRepository()
        {
            _ctx = new CityOfIdeasDbContext();
        }

        /*
         * @author Niels Van Zandbergen
         */
        #region Conversion Methods
        private Project ConvertToDomain(ProjectsDao dao)
        {
            return new Project()
            {
                Id = dao.ProjectId,
                CurrentPhase = new Phase() {Id = dao.CurrentPhaseId},
                User = new UimvcUser() {Id = dao.UserId},
                Platform = new Platform() {Id = dao.PlatformId},
                Title = dao.Title,
                Goal = dao.Goal,
                Status = dao.Status,
                Visible = dao.Visible,
                LikeVisibility = (LikeVisibility) dao.LikeVisibility,
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
                CurrentPhaseId = project.CurrentPhase.Id,
                UserId = project.User.Id,
                PlatformId = project.Platform.Id,
                Title = project.Title,
                Goal = project.Goal,
                Status = project.Status,
                Visible = project.Visible,
                LikeVisibility = (byte) project.LikeVisibility,
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
                Project = new Project {Id = dao.ProjectId},
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

        private ProjectImagesDao ConvertToDao(byte[] obj, int projectId, int imageId)
        {
            return new ProjectImagesDao()
            {
                ProjectId = projectId,
                ProjectImage = obj,
                ImageId = imageId
            };
        }

        private byte[] ConvertToDomain(ProjectImagesDao dao)
        {
            return dao.ProjectImage;
        }

        #endregion

        /*
         * @author Niels Van Zandbergen
         */
        #region Id generation

        private int FindNextAvailableProjectId()
        {
            if (!_ctx.Projects.Any()) return 1;
            int newId = ReadAll().Max(platform => platform.Id) + 1;
            return newId;
        }

        private int FindNextAvailablePhaseId()
        {
            if (!_ctx.Phases.Any()) return 1;
            int newId = ReadAllPhases().Max(platform => platform.Id) + 1;
            return newId;
        }

        private int FindNextAvailableImageId()
        {
            if (!_ctx.ProjectImages.Any()) return 1;
            int newId = _ctx.ProjectImages.Count()+1;
            return newId;
        }
        #endregion

        /*
         * @authors David Matei, Edwin Kai Yin Tam & Niels Van Zandbergen
         */
        #region Project CRUD

        public Project Create(Project obj)
        {
            IEnumerable<Project> projects = ReadAllForPlatform(obj.Platform.Id);

            foreach (Project p in projects)
            {
                if (ExtensionMethods.HasMatchingWords(p.Title, obj.Title) > 0)
                {
                    throw new DuplicateNameException(
                        "Dit project bestaat al of is misschien gelijkaardig. Project(ID=" + obj.Id +
                        ") dat je wil aanmaken: " +
                        obj.Title + ". Project(ID=" + p.Id + ") dat al bestaat: " + p.Title + ".");
                }
            }

            obj.Id = FindNextAvailableProjectId();
            _ctx.Projects.Add(ConvertToDao(obj));
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
        public Project Read(int id, bool details)
        {
            ProjectsDao projectsDao = details
                ? _ctx.Projects.AsNoTracking().FirstOrDefault(p => p.ProjectId == id)
                : _ctx.Projects.FirstOrDefault(p => p.ProjectId == id);
            ExtensionMethods.CheckForNotFound(projectsDao, "Project", id);

            return ConvertToDomain(projectsDao);
        }

        public void Update(Project obj)
        {
            ProjectsDao newProj = ConvertToDao(obj);
            ProjectsDao foundProj = _ctx.Projects.First(p => p.ProjectId == obj.Id);
            if (foundProj != null)
            {
                if(!String.IsNullOrEmpty(newProj.Title)) foundProj.Title = newProj.Title;
                if(!String.IsNullOrEmpty(newProj.Goal)) foundProj.Goal = newProj.Goal;
                if(newProj.CurrentPhaseId != 0) foundProj.CurrentPhaseId = newProj.CurrentPhaseId;
                if(!String.IsNullOrEmpty(newProj.Status)) foundProj.Status = newProj.Status;
                if(!String.IsNullOrEmpty(newProj.Visible.ToString())) foundProj.Visible = newProj.Visible;
                if(newProj.ReactionCount != 0) foundProj.ReactionCount = newProj.ReactionCount;
                if(newProj.LikeCount != 0) foundProj.LikeCount = newProj.LikeCount;
                if(newProj.FbLikeCount != 0) foundProj.FbLikeCount = newProj.FbLikeCount;
                if(newProj.TwitterLikeCount != 0) foundProj.TwitterLikeCount = newProj.TwitterLikeCount;
                foundProj.LikeVisibility = newProj.LikeVisibility;
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

            foreach (ProjectsDao dao in _ctx.Projects)
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

        /*
         * @authors David Matei, Edwin Kai Yin Tam & Niels Van Zandbergen
         */
        #region Phase CRUD

        public Phase Create(Phase obj)
        {
            IEnumerable<Phase> phases = ReadAllPhases(obj.Project.Id);

            foreach (Phase p in phases)
            {
                if (ExtensionMethods.VerifyOverlap(p.StartDate, p.EndDate, obj.StartDate, obj.EndDate))
                {
                    throw new DuplicateNameException("Deze phase met ID " + obj.Id + " (Start: " + obj.StartDate +
                                                     ", Einde: " + obj.EndDate + ") overlapt" +
                                                     " met een andere phase met ID " + p.Id + " (Start: " +
                                                     p.StartDate + ", Einde: " + p.EndDate + ")");
                }
            }

            obj.Id = FindNextAvailablePhaseId();
            _ctx.Phases.Add(ConvertToDao(obj));
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
        public Phase ReadPhase(int phaseId, bool details)
        {
            PhasesDao phasesDao = details
                ? _ctx.Phases.AsNoTracking().FirstOrDefault(p => p.PhaseId == phaseId)
                : _ctx.Phases.FirstOrDefault(p => p.PhaseId == phaseId);
            ExtensionMethods.CheckForNotFound(phasesDao, "Phase", phaseId);

            return ConvertToDomain(phasesDao);
        }

        public void Update(Phase obj)
        {
            PhasesDao newPhase = ConvertToDao(obj);
            PhasesDao foundPhase = _ctx.Phases.First(p => p.PhaseId == obj.Id);
            if (foundPhase != null)
            {
                if(!String.IsNullOrEmpty(newPhase.Description)) foundPhase.Description = newPhase.Description;
                if(newPhase.StartDate != DateTime.MinValue) foundPhase.StartDate = newPhase.StartDate;
                if(newPhase.EndDate != DateTime.MinValue) foundPhase.EndDate = newPhase.EndDate;
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

        /*
         * @author Niels Van Zandbergen
         */
        #region Images CRUD
        public void Create(byte[] obj, int projectId)
        {
            ProjectImagesDao dao = ConvertToDao(obj, projectId, FindNextAvailableImageId());
            _ctx.ProjectImages.Add(dao);
            _ctx.SaveChanges();
        }

        public void DeleteImages(int projectId)
        {
            foreach (ProjectImagesDao img in _ctx.ProjectImages.Where(i => i.ProjectId == projectId))
            {
                _ctx.ProjectImages.Remove(img);
            }

            _ctx.SaveChanges();
        }

        public List<byte[]> ReadAllImages(int projectId)
        {
            List<byte[]> myQuery = new List<byte[]>();

            foreach (ProjectImagesDao img in _ctx.ProjectImages.Where(i => i.ProjectId == projectId))
            {
                byte[] byteImg = ConvertToDomain(img);
                myQuery.Add(byteImg);
            }

            return myQuery;
        }
        #endregion
    }
}
