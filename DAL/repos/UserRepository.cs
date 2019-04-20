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
        private UsersDTO ConvertToDTO(User obj)
        {
            return new UsersDTO
            {
                UserID = obj.Id,
                Name = obj.Name,
                Email = obj.Email,
                Password = obj.Password,
                Role = (byte) (obj.Role-1),
                PlatformID = obj.Platform.Id
            };
        }

        private User ConvertToDomain(UsersDTO DTO)
        {
            return new User
            {
                Id = DTO.UserID,
                Name = DTO.Name,
                Email = DTO.Email,
                Password = DTO.Password,
                Role = (Role) DTO.Role+1,
                Platform = new Platform() { Id = DTO.PlatformID }
            };
        }

        private User UserWithDetails(User user, UserDetailsDTO DTO)
        {
            user.Id = DTO.UserID;
            user.ZipCode = DTO.Zipcode;
            user.Gender = DTO.Gender;
            user.Banned = DTO.Banned;
            user.Active = DTO.Active;
            user.Birthdate = DTO.BirthDate;
            return user;
        }

        private UserDetailsDTO RetrieveDetailsFromUser(User user)
        {
            return new UserDetailsDTO
            {
                UserID = user.Id,
                Zipcode = user.ZipCode,
                Gender = user.Gender,
                Banned = user.Banned,
                Active = user.Active,
                BirthDate = user.Birthdate
            };
        }

        private Organisation RetrieveOrganisation(Organisation user, UserDetailsDTO DTO)
        {
            user.Id = DTO.UserID;
            user.OrgName = DTO.OrgName;
            user.Description = DTO.Description;

            return user;
        }

        private UserDetailsDTO RetrieveOrganisationInfo(Organisation user)
        {
            return new UserDetailsDTO
            {
                UserID = user.Id,
                OrgName = user.OrgName,
                Description = user.Description
            };
        }

        private OrganisationEventsDTO ConvertToDTO(Event e){
            return new OrganisationEventsDTO
            {
                 EventID = e.Id,
                 UserID = e.Organisation.Id,
                 Name = e.Name,
                 Description = e.Description,
                 StartDate = e.StartDate,
                 EndDate = e.EndDate
            };
        }

        private Event ConvertToDomain(OrganisationEventsDTO DTO)
        {
            return new Event
            {
                Id = DTO.EventID,
                Organisation = new Organisation { Id = DTO.EventID },
                Name = DTO.Name,
                Description = DTO.Description,
                StartDate = DTO.StartDate,
                EndDate = DTO.EndDate
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

            ctx.Users.Add(ConvertToDTO(obj));
            ctx.UserDetails.Add(RetrieveDetailsFromUser(obj));
            ctx.SaveChanges();

            return obj;
        }

        public Organisation Create(Organisation obj)
        {
            IEnumerable<Organisation> users = ReadAllOrganisations(obj.Platform.Id);

            foreach (Organisation o in users)
            {
                if (ExtensionMethods.HasMatchingWords(o.OrgName, obj.OrgName) > 0)
                {
                    throw new DuplicateNameException("Deze Organisatie is al gevonden. Gegeven Organisatie(ID=" + obj.Id + ") met naam: " + obj.OrgName + 
                        ". Gevonden Organisatie(ID=" + o.Id + ") met naam: " + o.OrgName + ".");
                }
            }

            ctx.Users.Add(ConvertToDTO(obj));
            ctx.UserDetails.Add(RetrieveDetailsFromUser(obj));
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

            return ConvertToDomain(usersDTO);
        }
       
        public User ReadWithDetails(int id)
        {
            User user = Read(id, true);
            UserDetailsDTO DTO = ctx.UserDetails.First(u => u.UserID == id);

            user = UserWithDetails(user, DTO);

            return user;
        }

        public Organisation ReadOrganisation(int id)
        {
            return (Organisation) ReadWithDetails(id);
        }

        public void Update(User obj)
        {
            UsersDTO newUser = ConvertToDTO(obj);
            UsersDTO foundUser = ConvertToDTO(Read(obj.Id, false));
            foundUser = newUser;

            UserDetailsDTO newDetails = RetrieveDetailsFromUser(obj);
            UserDetailsDTO foundDetails = RetrieveDetailsFromUser(ReadWithDetails(obj.Id));
            foundDetails = newDetails;

            ctx.SaveChanges();
        }
        
        public void Delete(int id)
        {
            ctx.Users.Remove(ConvertToDTO(Read(id, false)));
            ctx.UserDetails.Remove(RetrieveDetailsFromUser(Read(id, false)));
            ctx.SaveChanges();
        }
        
        public IEnumerable<User> ReadAll()
        {
            IEnumerable<User> myQuery = new List<User>();

            foreach (UsersDTO DTO in ctx.Users)
            {              
                myQuery.Append(ReadWithDetails(DTO.UserID));
            }

            return myQuery;
        }

        public IEnumerable<Organisation> ReadAllOrganisations()
        {
            IEnumerable<Organisation> myQuery = new List<Organisation>();

            foreach (UsersDTO DTO in ctx.Users)
            {
                if(DTO.Role == 3)
                {
                    myQuery.Append(ReadWithDetails(DTO.UserID));
                } 
            }

            return myQuery;
        }

        public IEnumerable<User> ReadAll(int platformID)
        {
            return ReadAll().ToList().FindAll(u => u.Platform.Id == platformID);
        }

        public IEnumerable<Organisation> ReadAllOrganisations(int platformID)
        {
            return ReadAllOrganisations().ToList().FindAll(u => u.Platform.Id == platformID);
        }
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

            ctx.OrganisationEvents.Add(ConvertToDTO(obj));
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

            return ConvertToDomain(orgEventDTO);
        }
        
        public void Update(Event obj)
        {
            OrganisationEventsDTO newEvent = ConvertToDTO(obj);
            OrganisationEventsDTO foundEvent = ConvertToDTO(ReadUserEvent(obj.Id, false));
            foundEvent = newEvent;
            ctx.SaveChanges();
        }
        
        public void DeleteUserEvent(int userID, int eventID)
        {
            ctx.OrganisationEvents.Remove(ConvertToDTO(ReadUserEvent(eventID, false)));
            ctx.SaveChanges();
        }
        
        public IEnumerable<Event> ReadAllEvents()
        {
            IEnumerable<Event> myQuery = new List<Event>();

            foreach (OrganisationEventsDTO DTO in ctx.OrganisationEvents)
            {
                myQuery.Append(ConvertToDomain(DTO));
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