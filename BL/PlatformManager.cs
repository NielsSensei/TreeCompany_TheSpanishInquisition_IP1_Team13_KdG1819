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
            throw new NotImplementedException("Out of scope!");
        }
        
        public void AddPlatform()
        {
            throw new NotImplementedException("Out of scope!");
        }

        public void RemovePlatform(int platformId)
        {
            throw new NotImplementedException("Out of scope!");
        }
        #endregion
        
        // Added by NG
        // Modified by NVZ
        //PlatformOwner
        #region
        public void AddOwner(int platformId, int userId)
        {
            var newOwner = UserMan.GetUser(platformId, userId, true);
            PlatformRepo.Read(platformId,false).Owners.Add(newOwner);
        }
        
        public User GetPlatformOwner(int userId)
        {
            throw new NotImplementedException("Out of scope!");
        }
        
        public void removePlatformOwner(int userId)
        {
            throw new NotImplementedException("Out of scope!");
        }

        public List<User> GetAllPlatformOwners(int platformId)
        {
            return PlatformRepo.Read(platformId, true).Owners;
        }
        #endregion
        
        // Added by NVZ
        // Other Methods
        #region
        /*
         * We might use this for initialisation. - NVZ
         */
        public void AdUserToPlatform(int platformId, User user)
        {
            throw new NotImplementedException("I might need this!");
        }

        public void ChangeUserFromPlatform(int platformId, User user)
        {
            throw new NotImplementedException("Out of scope!");
        }     
        #endregion
    }
}