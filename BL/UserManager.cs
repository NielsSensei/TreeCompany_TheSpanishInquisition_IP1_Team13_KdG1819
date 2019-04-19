using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Users;
using DAL;

namespace BL
{
    public class UserManager
    {
        // Added by NG
        // Modified by NVZ
        private UserRepository UserRepo { get; set; }
        private PlatformManager PlatformMan { get; set; }
        private ProjectManager ProjectMan { get; set; }       

        // Added by NG
        // Modified by NVZ
        public UserManager()
        {
            UserRepo = new UserRepository();
            PlatformMan = new PlatformManager();
            ProjectMan = new ProjectManager();
        }
        
        // Added by NG
        // Modified by NVZ
        //User 
       #region
       
       /*
        * Setter method, we might need this for certain properties but
        * certainly not all of them. Please make a difference between
        * properties you need and the ones you do not. - NVZ
        * 
        */
       public void ChangeUser(User user)
       {
            UserRepo.Update(user);
       }
       
       /*
        * Getter method, it might be interesting to filter
        * on platform on we work out this feature but for the POC
        * it is Out of Scope. - NVZ
        * 
        */
       public User GetUser(int userId, bool details)
       {
           return UserRepo.Read(userId, details);
       }

       public List<User> GetUsers(int platformId, Role? roleLevel)
       {
           var userList = UserRepo.ReadAll(platformId).ToList();
           if (roleLevel == null) return userList;
           var filteredUserList = new List<User>();
           foreach (var user in userList)
           {
               if(user.Role.Equals(roleLevel))
                   filteredUserList.Add(user);
           }

           return filteredUserList;

       }

       public void AddAnonymousUser(User user)
       {
           throw new NotImplementedException("Out of Scope!");
       } 
       
       /*
        * We might use this for initialisation - NVZ
        */
       public void AddUser(User user)
       {
           UserRepo.Create(user);
           PlatformMan.AddUserToPlatform(user.Platform.Id, user);
       }

       public void RemoveUser(int userId)
       {
           throw new NotImplementedException("Out of Scope!");
       }

       #endregion
       
        // Added by NG
        // Modified by NVZ
        //Event 
       #region
       public void ChangeOrgEvent(int userId, int eventId)
       {
           throw new NotImplementedException("Out of Scope!");
           
       }
        
       public Event GetEvent(int eventId)
       {
           return UserRepo.ReadUserEvent(eventId, true);
       }

       public void AddEvent(int userId, Event orgEvent)
       {
           throw new NotImplementedException("Out of Scope!");
       }

       public void RemoveOrgEvent(int userId, int eventId)
       {
           UserRepo.DeleteUserEvent(userId, eventId);
       }
       #endregion
       
        // Added by NG
        // Modified by NVZ
        //Organisation 
       #region
       public void AddOrganisation(int userId)
       {
           throw new NotImplementedException("Out of Scope!");
       }

       public User GetOrganisation(int userId)
       {
           return UserRepo.ReadOrganisation(userId);
       }
       #endregion
        
        // Added by NVZ
        // Other Methods
        #region 
        private void EnactPromotion(User toPromote, User enactor, Role newRole)
        {
            throw new NotImplementedException("Out of scope!");
        }

        /*
         * This method receives an action name and is supposed to
         * return the lowest Role that has this permit.
         *
         * For example if the action is named FillInQuestionnaire then
         * the Role returned will be Anonymous.
         *
         * If you have questions or better suggestions please let me know.
         * - NVZ
         * 
         */
        private Role VerifyAction(String actionName)
        {
           throw new NotImplementedException("I might need this!"); 
        }

        /*
         * This method will compare our User parameter and verify if
         * it has the Role needed for this permission. You might be
         * able to use this in combination with the above method.
         *
         * If you have questions or better suggestions please let me know.
         * - NVZ
         * 
         */
        private bool VerifyPermission(User user, Role roleLevel)
        {
            throw new NotImplementedException("I might need this!"); 
        }

        /*
         * We have two options with this method:
         * 
         * 1. Either any call to this class is via this method.
         * 2. Either only calls outside of the UserController are for
         * this method so that it can delegate to the projectManager
         * or platformManager if it can't solve the problem.
         *
         * This method is conceived to be modular towards microservices,
         * if we have the time I'll explain why. - NVZ
         * 
         */
        public void HandleUserAction(int userId, string actionName)
        {
            throw new NotImplementedException("I need this!");
        }
        
        #endregion
    }
}