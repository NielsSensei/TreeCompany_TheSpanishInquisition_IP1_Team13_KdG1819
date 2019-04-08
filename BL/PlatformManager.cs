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
        private PlatformRepository platformRep { get; set; }
        
        // Added by NG
        // Modified by NVZ
        public PlatformManager()
        {
            platformRep = new PlatformRepository();
        }
        
        // Added by NG
        // Modified by NVZ
        //Platform 
        #region   
        public Platform getPlatform(int id)
        {
            throw new NotImplementedException("Out of scope!");
        }
        
        public void makePlatform()
        {
            throw new NotImplementedException("Out of scope!");
        }

        public void removePlatform(int id)
        {
            throw new NotImplementedException("Out of scope!");
        }
        #endregion
        
        // Added by NG
        // Modified by NVZ
        //PlatformOwner
        #region
        public void addOwner(int platformID, User Owner)
        {
            throw new NotImplementedException("Out of scope!");
        }
        
        public PlatformOwner getPlatformOwner(int id)
        {
            throw new NotImplementedException("Out of scope!");
        }
        
        public void removePlatformOwner(int id)
        {
            throw new NotImplementedException("Out of scope!");
        }

        public List<PlatformOwner> getOwners(int platformID)
        {
            throw new NotImplementedException("Out of scope!");
        }
        #endregion
        
        // Added by NVZ
        // Other Methods
        #region
        private PlatformOwner parseUserToOwner(User owner)
        {
            throw new NotImplementedException("Out of scope!");
        }

        /*
         * We might use this for initialisation. - NVZ
         */
        public void addUserToPlatform(int platformID, User user)
        {
            throw new NotImplementedException("I might need this!");
        }

        public void editUserFromPlatform(int platformID, User user)
        {
            throw new NotImplementedException("Out of scope!");
        }     
        #endregion
    }
}