using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Domain;
using Domain.Common;
using Domain.Projects;
using DAL.Contexts;

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
            //ctx = new CityOfIdeasDbContext();
        }

        // Added by NVZ
        // Project CRUD
        #region
        //TODO: Projecten zijn in dit geval op gelijkaardigheid van de naam dus met een compare.
        public Project Create(Project obj)
        {
            /*if (!projects.Contains(obj))
            {
                projects.Add(obj);
            }
            throw new DuplicateNameException("This Project already exists!"); */
            return null;
        }

        public Project Read(int id)
        {
            /* Project p = projects.Find(pr => pr.Id == id);
            if (p != null)
            {
                return p;
            } */
            throw new KeyNotFoundException("This Project can't be found!");
        }

        public void Update(Project obj)
        {
            Delete(obj.Id);
            Create(obj);
        }

        public void Delete(int id)
        {
            Project p = Read(id);
            if (p != null)
            {
                //projects.Remove(p);
            }
        }
        
        public IEnumerable<Project> ReadAll()
        {
            return null;
            //return projects;
        }

        //TODO: (Hotfix) What was I thinking here?
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
            /* if (!Phases.Contains(obj))
            {
                Phases.Add(obj);
                Read(obj.Project.Id).Phases.Add(obj);
            } */
            throw new DuplicateNameException("This Phase already exists!");
        }

        public Phase ReadPhase(int projectID, int phaseID)
        {
            Phase p = Read(projectID).Phases.ToList().Find(ph => ph.Id == phaseID);
            if (p != null)
            {
                return p;
            }
            throw new KeyNotFoundException("This Phase can't be found!");
        }

        public void Update(Phase obj)
        {
            Delete(obj.Project.Id, obj.Id);
            Create(obj);
            Update(obj.Project);
        }

        public void Delete(int projectID, int phaseID)
        {
            Phase p = ReadPhase(projectID, phaseID);
            if (p != null)
            {
                //Phases.Remove(p);
                Read(projectID).Phases.Remove(p);
            }
        }

        public IEnumerable<Phase> ReadAllPhases(int projectID)
        {
            return null;
            //return Phases.FindAll(p => p.Project.Id == projectID);
        }
        #endregion
        
        // Added by NVZ
        // Images CRUD
        // TODO: Als we images kunnen laden enal is het bonus, geen prioriteit tegen Sprint 1.
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
            Image i = Read(projectID).PreviewImages.ToList()[imageID - 1];
            if (i != null)
            {
                return i;
            }
            throw new KeyNotFoundException("This Image can't be found!");
        }

        public void Update(Image obj)
        {
            DeleteImage(obj);
            Create(obj);
        }

        public void DeleteImage(Image obj)
        {
            //images.Remove(obj);
        }
        #endregion
        
        // Added by NVZ
        //TODO: Tags CRUD verlagen naar module.
        #region
        public string createTag(string obj)
        {
            /*if (!tags.Contains(obj))
            {
                tags.Add(obj);
            } */
            throw new DuplicateNameException("This Tag already exists!");
        }

        public void DeleteTag(int projectID, int tagID)
        {
            //tags.RemoveAt(tagID - 1);
        }

        public IEnumerable<String> ReadAllTags()
        {
            return null;
            //return tags;
        }
        #endregion
    }
}