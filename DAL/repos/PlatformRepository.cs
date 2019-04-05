using System.Collections.Generic;
using System.Data;
using System.Linq;
using Domain.Users;

namespace DAL
{
    public class PlatformRepository //: IRepository<Platform>
    {
        //TODO: Get rid of platformowner
        // Added by DM
        private List<Platform> Platforms;
        //private List<PlatformOwner> platformsOwners;

        // Added by NVZ
        public PlatformRepository()
        {
           //TODO: Initialisatie
        }
        
        // Added by NVZ
        // Platform CRUD
        #region
        //TODO compare op name & postcode
        public Platform Create(Platform obj)
        {
            if (!Platforms.Contains(obj))
            {
                Platforms.Add(obj);
                return obj;
            }
            throw new DuplicateNameException("This Platform already exists!");
        }

        public Platform Read(int id)
        {
            Platform p = Platforms.Find(pl => pl.Id == id);
            if (p != null)
            {
                return p;
            }
            throw new KeyNotFoundException("This Platform can't be found!");
        }

        public void Update(Platform obj)
        {
            Delete(obj.Id);
            Create(obj);
        }

        public void Delete(int id)
        {
            Platform p = Read(id);
            if (p != null)
            {
                Platforms.Remove(p);
            }
        }
        
        public IEnumerable<Platform> ReadAll()
        {
            return Platforms;
        }
        #endregion  
        
        // Added by NVZ
        // PlatformOwner CRUD
        //TODO update this so it's about users.
       
       /* #region
        public PlatformOwner Create(PlatformOwner obj)
        {
            if (!platformsOwners.Contains(obj))
            {
                platformsOwners.Add(obj);
                Read(obj.PlatformID).AddOwner(obj);
                return obj;
            }
            throw new DuplicateNameException("This PlatformOwner already exists!");
        }

        public PlatformOwner Read(int platformID, int ownerID)
        {
            PlatformOwner po = Read(platformID).Owners.ToList().Find(o => o.Id == ownerID);
            if (po != null)
            {
                return po;
            }
            throw new KeyNotFoundException("This PlatformOwner can't be found!");
        }

        public void Update(PlatformOwner obj)
        {
            Delete(obj.PlatformID, obj.Id);
            Create(obj);
        }

        public void Delete(int platformID, int ownerID)
        {
            PlatformOwner po = Read(platformID, ownerID);
            if (po != null)
            {
                platformsOwners.Remove(po);
                Platform p = Read(platformID);
                p.Owners.Remove(po);
                Update(p);
            }
        }

        public IEnumerable<PlatformOwner> ReadAllOwners()
        {
            return platformsOwners;
        }

        public IEnumerable<PlatformOwner> ReadAllOwners(int platformID)
        {
            return platformsOwners.FindAll(p => p.PlatformID == platformID);
        }
        #endregion */ 
    }
}