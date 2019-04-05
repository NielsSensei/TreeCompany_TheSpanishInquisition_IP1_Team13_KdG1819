using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Domain.Common;
using Domain.Projects;
using DAL.Contexts;

namespace DAL
{
    public class IdeationRepository //: IRepository<Ideation>
    {
        // Added by DM
        // Modified by NVZ & XV
        private List<Ideation> ideations;
        private List<Media> mediafiles;
        private CityOfIdeasDbContext ctx;

        // Added by NVZ
        public IdeationRepository()
        {
            ctx = new CityOfIdeasDbContext();
        }
        
        // Added by NVZ
        // Ideation CRUD
        #region
        //TODO: compare op name?
        public Ideation Create(Ideation obj)
        {
            if (!ideations.Contains(obj))
            {
                ideations.Add(obj);
            }
            throw new DuplicateNameException("This Ideation already exist!");
        }

        public Ideation Read(int id)
        {
            Ideation i = ideations.Find(ideation => ideation.Id == id);
            if (i != null)
            {
                return i;
            }
            throw new KeyNotFoundException("This Ideation can't be found!");
        }

        public void Update(Ideation obj)
        {
           Delete(obj.Id);
           Create(obj);
        }

        public void Delete(int id)
        {
            Ideation ideation = Read(id);
            if (ideation != null)
            {
                ideations.Remove(ideation);
            }
        }
        
        public IEnumerable<Ideation> ReadAll()
        {
            return ideations;
        }

        public IEnumerable<Ideation> ReadAll(int projectID)
        {
            return ideations.FindAll(ideation => ideation.ParentPhase.Project.Id == projectID);
        }
        #endregion

        // Added by NVZ
        // Media CRUD     
        // TODO: (SPRINT2?) Als we images kunnen laden enal is het bonus, geen prioriteit tegen Sprint 1.
        #region

        public Media Create(Media obj)
        {
            if (!mediafiles.Contains(obj))
            {
                mediafiles.Add(obj);
            }
            throw new DuplicateNameException("This MediaFile already exist!");
        }

        public Media ReadMedia(int ideationID)
        {
            Media m = Read(ideationID).Media;
            if (m != null)
            {
                return m;
            }
            throw new KeyNotFoundException("This Media can't be found!");
        }

        public void DeleteMedia(int ideationID)
        {
            Media m = ReadMedia(ideationID);
            if (m != null)
            {
                mediafiles.Remove(m);
            }
        }
        #endregion

        // Added by NVZ
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