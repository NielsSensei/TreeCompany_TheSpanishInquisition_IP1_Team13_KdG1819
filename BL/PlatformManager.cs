using System;
using System.Collections.Generic;
using DAL;
using Domain.Users;

namespace BL
{
    public class PlatformManager
    {
        // Added by NG
        // Modified by NVZ
        private PlatformRepository PlatformRepo { get; set; }
        private UserManager UserMan { get; set; }
        
        // Added by NG
        // Modified by NVZ
        public PlatformManager()
        {
            PlatformRepo = new PlatformRepository();
            UserMan = new UserManager();
        }
        
        // Added by NG
        // Modified by NVZ
        //Platform 
        #region   
        public Platform GetPlatform(int platformId)
        {
            return PlatformRepo.Read(platformId, true);
        }
        
        public void MakePlatform(Platform platform)
        {
            PlatformRepo.Create(platform);
        }

        public void EditPlatform(Platform platform)
        {
            PlatformRepo.Update(platform);
        }

        public void RemovePlatform(int platformId)
        {
            PlatformRepo.Delete(platformId);
        }
        #endregion
        
        // Added by NG
        // Modified by NVZ
        //PlatformOwner
        #region
        public void MakeOwner(int platformId, int userId)
        {
            var newOwner = UserMan.GetUser(userId, true);
            PlatformRepo.Read(platformId,false).Owners.Add(newOwner);
        }
        
        public User GetPlatformOwner(int platformId, int userId)
        {
            var platform = PlatformRepo.Read(platformId, true);
            var user = UserMan.GetUser(userId, true);
            return platform.Owners.Find(u => u.Equals(user));
        }
        
        public void RemovePlatformOwner(int platformId, int userId)
        {
            var alteredPlatform = PlatformRepo.Read(platformId, true);
            var removedOwner = UserMan.GetUser(userId, true);
            alteredPlatform.Owners.Remove(removedOwner);
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
    }
}