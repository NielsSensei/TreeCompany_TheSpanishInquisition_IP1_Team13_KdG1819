using System;
using System.Collections.Generic;
using System.Data;
using Domain.Users;

namespace DAL
{
    public class UserRepository : IRepository<User>
    {
        //Added by DM 
        //Modified by NVZ
        private List<User> Users;
        private List<Event> userEvents;

        // Added by NVZ
        public UserRepository()
        {
            //TODO: Initalisatie
        }

        //Added by DM
        //Modified by NVZ
        //User CRUD
        #region 
        public User Create(User obj)
        {
            if (!Users.Contains(obj))
            {
                return obj;
            }
            throw new DuplicateNameException("This User already exist!");
        }

        public User Read(int id)
        {
            User u = Users.Find(user => user.Id == id);
            if (u != null)
            {
                return u;
            }
            throw new KeyNotFoundException("This User can't be found!");
        }
        
        public void Update(User obj)
        {
            Delete(obj.Id);
            Create(obj);
        }
        
        public void Delete(int id)
        {
            User u = Read(id);
            if (u != null)
            {
                Users.Remove(u);
            }
        }
        
        public IEnumerable<User> ReadAll()
        {
            return Users;
        }

        //TODO: (Hotfix) User heeft een platform nu
        public IEnumerable<User> ReadAll(int platformID)
        {
            return Users.FindAll(u => u.platformID == platformID);
        }
        #endregion

        // Added by NVZ
        // Event CRUD
        //TODO: (Hotfix) Events binnen Organisation.cs
        #region
        public Event Create(Event obj)
        {
            if (!userEvents.Contains(obj))
            {
                userEvents.Add(obj);
                Organisation u = (Organisation) Users.Find(us => us.Id == obj.OrganiserId);
                u.AddEvent(obj);
                Update(u);
            }
            throw new DuplicateNameException("This Event already exists!");
        }
        
        public Event ReadUserEvent(int id)
        {
            Event oevent = userEvents.Find(e => e.Id == id);
            if (oevent != null)
            {
                return oevent;
            }
            throw new KeyNotFoundException("This Event can't be found!");
        }
        
        public void Update(Event obj)
        {
            DeleteUserEvent(obj.OrganiserId, obj.Id);
            Create(obj);
        }
        
        public void DeleteUserEvent(int userID, int eventID)
        {
            Event e = ReadUserEvent(eventID);
            if (e != null)
            {
                userEvents.Remove(e);
                Organisation u = (Organisation) Users.Find(us => us.Id == userID);
                u.Events.Remove(e);
            }
        }
        
        public IEnumerable<Event> ReadAllEvents()
        {
            return userEvents;
        }

        public IEnumerable<Event> ReadAllEventsByUser(int userID)
        {
            return userEvents.FindAll(e => e.OrganiserId == userID);
        }
        
        public IEnumerable<Event> ReadAllEvents(int platformID)
        {
            List<Event> events = new List<Event>();
            foreach (Organisation user in Users)
            {
                if (user.platformID == platformID)
                {
                    events.AddRange(user.Events);
                }
            }
            return events;
        }
        #endregion       
    }
}