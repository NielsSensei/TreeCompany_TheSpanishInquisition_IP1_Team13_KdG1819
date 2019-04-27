using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Domain.Users;

namespace BL
{
    public class PlatformManager
    {
        // Added by NG
        // Modified by NVZ
        private PlatformRepository PlatformRepo { get; set; }
        
        // Added by NG
        // Modified by NVZ
        public PlatformManager()
        {
            PlatformRepo = new PlatformRepository();
        }
        
        // Added by NG
        // Modified by NVZ
        //Platform 
        #region   
        public Platform GetPlatform(int platformId)
        {
            return PlatformRepo.Read(platformId, true);
        }
        
        public Platform MakePlatform(Platform platform)
        {
            return PlatformRepo.Create(platform);
        }

        public void EditPlatform(Platform platform)
        {
            PlatformRepo.Update(platform);
        }

        public void RemovePlatform(int platformId)
        {
            PlatformRepo.Delete(platformId);
        }

        // Added by XV
        public IEnumerable<Platform> ReadAllPlatforms()
        {
            return PlatformRepo.ReadAll();
        }
        
        #endregion
        
        // Added by NG
        // Modified by NVZ
        //PlatformOwner
        #region
        public void MakeOwner(int platformId, User newOwner)
        {
            var alteredPlatform = PlatformRepo.Read(platformId, false);
            alteredPlatform.Owners.Add(newOwner);
            PlatformRepo.Update(alteredPlatform);
        }
        
        public User GetPlatformOwner(int platformId, User owner)
        {
            var platform = PlatformRepo.Read(platformId, true);
            return platform.Owners.Find(u => u.Equals(owner));
        }
        
        public void RemovePlatformOwner(int platformId, User removedOwner)
        {
            var alteredPlatform = PlatformRepo.Read(platformId, false);
            alteredPlatform.Owners.Remove(removedOwner);
            PlatformRepo.Update(alteredPlatform);
        }

        public List<User> GetAllPlatformOwners(int platformId)
        {
            return PlatformRepo.Read(platformId, false).Owners;
        }
        
        #endregion
        
        // Added by NVZ
        // Other Methods
        #region
        /*
         * We might use this for initialisation. - NVZ
         */
        public void MakeUserToPlatform(int platformId, User user)
        {
            throw new NotImplementedException("I might need this!");
//            var alteredPlatform = PlatformRepo.Read(platformId, true);
//            alteredPlatform.AddUser(user);
//            PlatformRepo.Update(alteredPlatform);
        }

        public void EditUserFromPlatform(int newPlatformId, User user)
        {
            throw new NotImplementedException("Out of scope!");
//            var currentPlatform = PlatformRepo.Read(user.Platform.Id, false);
//            currentPlatform.Users.Remove(user);
//            PlatformRepo.Update(currentPlatform);
//            var newPlatform = PlatformRepo.Read(newPlatformId, false);
//            newPlatform.AddUser(user);
//            PlatformRepo.Update(newPlatform);
        }

        public void RemoveUserFromPlatform(int platformId, int userId)
        {
            throw new NotImplementedException();
        }
        #endregion
        
        // Added by XV
        // Methods used for creating new platforms
        
        #region PlatformCreation

        public int GetNextAvailableId()
        {
            // Select the biggest current Id from the platforms and increment it by one
            int newId = ReadAllPlatforms().Max(platform => platform.Id) + 1;
            return newId;
        }

        #endregion
    }
}