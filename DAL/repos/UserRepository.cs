using System;
using System.Collections.Generic;
using System.Data;
using Domain.Users;
using DAL.Contexts;
using DAL.Data_Transfer_Objects;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL
{
    public class UserRepository : IRepository<User>
    {
        //Added by DM 
        //Modified by NVZ
        private CityOfIdeasDbContext ctx;

        // Added by NVZ
        public UserRepository()
        {
            ctx = new CityOfIdeasDbContext();
        }

        //Added by NVZ
        //Standard Methods
        #region
        private UsersDTO convertToDTO(User obj)
        {
            return new UsersDTO
            {
                UserID = obj.Id,
                Name = obj.Name,
                Email = obj.Email,
                Password = obj.Password,
                Role = (byte) obj.Role,
                PlatformID = obj.Platform.Id
            };
        }

        private User convertToDomain(UsersDTO DTO)
        {
            return new User
            {
                Id = DTO.UserID,
                Name = DTO.Name,
                Email = DTO.Email,
                Password = DTO.Password,
                Role = (Role) DTO.Role,
                Platform = new Platform() { Id = DTO.PlatformID }
            };
        }

        private OrganisationEventsDTO convertToDTO(Event e){
            return new OrganisationEventsDTO
            {
                 EventID = e.Id,
                 UserID = e.Organisation.Id,
                 Name = e.Name,
                 Description = e.Description,
                 startDate = e.StartDate,
                 endDate = e.EndDate
            };
        }

        private Event convertToDomain(OrganisationEventsDTO DTO)
        {
            return new Event
            {
                Id = DTO.EventID,
                Organisation = new Organisation { Id = DTO.EventID },
                Name = DTO.Name,
                Description = DTO.Description,
                StartDate = DTO.startDate,
                EndDate = DTO.endDate
            };
        }
        #endregion

        //Added by DM
        //Modified by NVZ
        //User CRUD
        #region 
        public User Create(User obj)
        {
            IEnumerable<User> users = ReadAll(obj.Platform.Id);

            foreach (User u in users)
            {
                if (ExtensionMethods.HasMatchingWords(u.Name, obj.Name) > 0)
                {
                    throw new DuplicateNameException("Deze User is al gevonden. Gegeven User(ID=" + obj.Id + ") met naam: " + obj.Name + ". Gevonden User(ID=" +
                        u.Id + ") met naam: " + u.Name + ".");
                }
            }

            ctx.Users.Add(convertToDTO(obj));
            ctx.SaveChanges();

            return obj;
        }

        public User Read(int id, bool details)
        {
            UsersDTO usersDTO = null;

            if (details)
            {
                usersDTO = ctx.Users.AsNoTracking().First(u => u.UserID == id);
                ExtensionMethods.CheckForNotFound(usersDTO, "User", usersDTO.UserID);
            }
            else
            {
                usersDTO = ctx.Users.First(u => u.UserID == id);
                ExtensionMethods.CheckForNotFound(usersDTO, "User", usersDTO.UserID);
            }

            return convertToDomain(usersDTO);
        }
        
        public void Update(User obj)
        {
            UsersDTO newUser = convertToDTO(obj);
            UsersDTO foundUser = convertToDTO(Read(obj.Id, false));
            foundUser = newUser;
            ctx.SaveChanges();
        }
        
        public void Delete(int id)
        {
            ctx.Users.Remove(convertToDTO(Read(id, false)));
            ctx.SaveChanges();
        }
        
        public IEnumerable<User> ReadAll()
        {
            IEnumerable<User> myQuery = new List<User>();

            foreach (UsersDTO DTO in ctx.Users)
            {
                myQuery.Append(convertToDomain(DTO));
            }

            return myQuery;
        }

        public IEnumerable<User> ReadAll(int platformID)
        {
            return ReadAll().ToList().FindAll(u => u.Platform.Id == platformID);
        }
        #endregion

        //Added by NVZ
        //UserDetails CRUD
        #region
                
        #endregion

        // Added by NVZ
        // Event CRUD
        #region
        public Event Create(Event obj)
        {
            IEnumerable<Event> orgEvents = ReadAllEventsByUser(obj.Organisation.Id);

            foreach (Event e in orgEvents)
            {
                if (e.StartDate > obj.StartDate && e.EndDate < obj.EndDate)
                {
                    throw new DuplicateNameException("Deze event met ID " + obj.Id + " (Start: " + obj.StartDate + ", Einde: " + obj.EndDate + ") overlapt" +
                        " met een andere event met ID " + e.Id + " (Start: " + e.StartDate + ", Einde: " + e.EndDate + ")");
                }
            }

            ctx.OrganisationEvents.Add(convertToDTO(obj));
            ctx.SaveChanges();

            return obj;
        }
        
        public Event ReadUserEvent(int id, bool details)
        {
            OrganisationEventsDTO orgEventDTO = null;

            if (details)
            {
                orgEventDTO = ctx.OrganisationEvents.AsNoTracking().First(e => e.EventID == id);
                ExtensionMethods.CheckForNotFound(orgEventDTO, "Event", orgEventDTO.EventID);
            }
            else
            {
                orgEventDTO = ctx.OrganisationEvents.First(e => e.EventID == id);
                ExtensionMethods.CheckForNotFound(orgEventDTO, "Event", orgEventDTO.EventID);
            }

            return convertToDomain(orgEventDTO);
        }
        
        public void Update(Event obj)
        {
            OrganisationEventsDTO newEvent = convertToDTO(obj);
            OrganisationEventsDTO foundEvent = convertToDTO(ReadUserEvent(obj.Id, false));
            foundEvent = newEvent;
            ctx.SaveChanges();
        }
        
        public void DeleteUserEvent(int userID, int eventID)
        {
            ctx.OrganisationEvents.Remove(convertToDTO(ReadUserEvent(eventID, false)));
            ctx.SaveChanges();
        }
        
        public IEnumerable<Event> ReadAllEvents()
        {
            IEnumerable<Event> myQuery = new List<Event>();

            foreach (OrganisationEventsDTO DTO in ctx.OrganisationEvents)
            {
                myQuery.Append(convertToDomain(DTO));
            }

            return myQuery;
        }

        public IEnumerable<Event> ReadAllEventsByUser(int userID)
        {
            return ReadAllEvents().ToList().FindAll(e => e.Organisation.Id == userID);
        }
        
        public IEnumerable<Event> ReadAllEvents(int platformID)
        {
            return ReadAllEvents().ToList().FindAll(e => e.Organisation.Platform.Id == platformID);
        }
        #endregion       

        //TODO (SPRINT2?) Useractivites?
    }
}