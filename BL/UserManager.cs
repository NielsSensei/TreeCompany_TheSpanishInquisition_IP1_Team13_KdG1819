using System;
using System.Collections.Generic;
using Domain.Users;
using DAL;

namespace BL
{
    public class UserManager
    {
        // Added by NG
        // Modified by NVZ
        private UserRepository userRepo { get; set; }
        private PlatformManager platformMan { get; set; }
        private ProjectManager projectMan { get; set; }       

        // Added by NG
        // Modified by NVZ
        public UserManager()
        {
            userRepo = new UserRepository();
            platformMan = new PlatformManager();
            projectMan = new ProjectManager();
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
       public void editUser(string propName, int userID)
       {
            throw new NotImplementedException("I might need this");
       }
       
       /*
        * Getter method, it might be interesting to filter
        * on platform on we work out this feature but for the POC
        * it is Out of Scope. - NVZ
        * 
        */
       public User getUser(int platformID, int userID, bool details)
       {
           throw new NotImplementedException("I might need this");
       }

       public List<User> getUsers(int platformID, Role? roleLevel, bool details)
       {
           throw new NotImplementedException("Out of Scope!");
       }

       public void makeAnonymousUser(User user)
       {
           throw new NotImplementedException("Out of Scope!");
       } 
       
       /*
        * We might use this for initialisation - NVZ
        */
       public void makeUser()
       {
           throw new NotImplementedException("I might need this");
       }

       public void removeUser(int id)
       {
           throw new NotImplementedException("Out of Scope!");
       }

       #endregion
       
        // Added by NG
        // Modified by NVZ
        //Event 
       #region
       public void editOrgEvent(int userID, int eventID)
       {
           throw new NotImplementedException("Out of Scope!");
       }
        
       public Event getEvent(int eventID)
       {
           throw new NotImplementedException("Out of Scope!");
       }

       public void makeEvent(int userID, Event orgEvent)
       {
           throw new NotImplementedException("Out of Scope!");
       }

       public void removeOrgEvent(int id, int userID)
       {
           throw new NotImplementedException("Out of Scope!");
       }
       #endregion
       
        // Added by NG
        // Modified by NVZ
        //Organisation 
       #region
       public void makeOrganisation(int userID)
       {
           throw new NotImplementedException("Out of Scope!");
       }       
       #endregion
        
        // Added by NVZ
        // Other Methods
        #region 
        private void enactPromotion(User toPromote, User enactor, Role newRole)
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
        private Role verifyAction(String actionName)
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
        private bool verifyPermission(User user, Role roleLevel)
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
        public void handleUserAction(int userID, string actionName)
        {
            throw new NotImplementedException("I need this!");
        }
        
        #endregion
    }
}