using System.Collections.Generic;
using Domain.UserInput;
using DAL.Contexts;
using System.Data;
using DAL.Data_Transfer_Objects;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL
{
    public class IdeationVoteRepository : IRepository<Vote>
    {
        // Added by NVZ
        private CityOfIdeasDbContext ctx;

        public IdeationVoteRepository()
        {
            ctx = new CityOfIdeasDbContext();
        }

        // Added by NVZ
        // Standard Methods
        #region
        private VotesDTO ConvertToDTO(Vote obj)
        {
            return new VotesDTO
            {
                VoteID = obj.Id,
                DeviceID = obj.Device.Id,
                InputID = obj.Idea.Id,
                InputType = 2, //Voorlopig Idee
                UserMail = obj.UserMail,
                LocationX = obj.LocationX,
                LocationY = obj.LocationY,
                Choices = ExtensionMethods.ListToString(obj.Choices)
            };
        }    

        private Vote ConvertToDomain(VotesDTO DTO)
        {
            return new Vote
            {
                Id = DTO.VoteID,
                Device = new IOT_Device { Id = DTO.DeviceID },
                Idea = new Idea { Id = DTO.InputID },
                UserMail = DTO.UserMail,
                LocationX = DTO.LocationX,
                LocationY = DTO.LocationY,
                Choices = ExtensionMethods.StringToList(DTO.Choices)
            };
        }

        private DevicesDTO ConvertToDTO(IOT_Device obj)
        {
            return new DevicesDTO
            {
                DeviceID = obj.Id,
                LocationX = obj.LocationX,
                LocationY = obj.LocationY
            };
        }

        private IOT_Device ConvertToDomain(DevicesDTO DTO)
        {
            return new IOT_Device
            {
                Id = DTO.DeviceID,
                LocationX = DTO.LocationX,
                LocationY = DTO.LocationY
            };
        }
        
        private int FindNextAvailableVoteId()
        {               
            int newId = ReadAll().Max(vote => vote.Id)+1;
            return newId;
        }
        
        private int FindNextAvailableDeviceId()
        {               
            int newId = ReadAllDevices().Max(device => device.Id)+1;
            return newId;
        }
        #endregion

        // Added by NVZ
        // Vote CRUD
        #region
        public Vote Create(Vote obj)
        {
            IEnumerable<Vote> votes = ReadAll(obj.Device.Id);

            foreach (Vote v in votes)
            {
                if(v.UserMail == obj.UserMail)
                {
                    throw new DuplicateNameException("Vote(ID=" + obj.Id + ") en Vote(ID=" + v.Id + ") hebben dezelfde email en Device(ID=" + 
                        obj.Device.Id + "), dit is absoluut niet toegestaan!");
                }
            }

            obj.Id = FindNextAvailableVoteId();
            ctx.Votes.Add(ConvertToDTO(obj));
            ctx.SaveChanges();

            return obj;
        }
        
        public Vote Read(int id, bool details)
        {
            VotesDTO voteDTO = null;
            voteDTO = details ? ctx.Votes.AsNoTracking().First(p => p.VoteID == id) : ctx.Votes.First(p => p.VoteID == id);
            ExtensionMethods.CheckForNotFound(voteDTO, "Vote", id);

            return ConvertToDomain(voteDTO);
        }
        
        public void Update(Vote obj)
        {
            VotesDTO newVote = ConvertToDTO(obj);
            VotesDTO foundVote = ctx.Votes.First(vote => vote.VoteID == obj.Id);
            if (foundVote != null)
            {
                foundVote.UserMail = newVote.UserMail;
                foundVote.LocationX = newVote.LocationX;
                foundVote.LocationY = newVote.LocationY;
            }

            ctx.SaveChanges();
        }
        
        public void Delete(int id)
        {
            VotesDTO toDelete = ctx.Votes.First(v => v.VoteID == id);
            ctx.Votes.Remove(toDelete);
            ctx.SaveChanges();
        }
        
        public IEnumerable<Vote> ReadAll()
        {
            List<Vote> myQuery = new List<Vote>();

            foreach (VotesDTO DTO in ctx.Votes)
            {
                myQuery.Add(ConvertToDomain(DTO));
            }

            return myQuery;
        }

        public IEnumerable<Vote> ReadAll(int deviceID)
        {
            return ReadAll().ToList().FindAll(vote => vote.Device.Id == deviceID);
        } 
        #endregion
               
        
        // Added by NVZ
        // Devices CRUD
        #region
        public IOT_Device Create(IOT_Device obj)
        {
            IEnumerable<IOT_Device> devices = ReadAllDevices();

            foreach (IOT_Device iot in devices)
            {
                if (iot.LocationX == obj.LocationX && iot.LocationY == obj.LocationY)
                {
                    throw new DuplicateNameException("Device(ID=" + iot.Id + ") en Device(ID=" + obj.Id + ") delen dezelfde locatie nl. " + obj.LocationX +
                        "," + obj.LocationY + ".");
                }
            }

            obj.Id = FindNextAvailableDeviceId();
            ctx.Devices.Add(ConvertToDTO(obj));
            ctx.SaveChanges();

            return obj;
        }

        public IOT_Device ReadDevice(int deviceID, bool details)
        {
            DevicesDTO deviceDTO = null;
            deviceDTO = details ? ctx.Devices.AsNoTracking().First(d => d.DeviceID == deviceID) : ctx.Devices.First(d => d.DeviceID == deviceID);
            ExtensionMethods.CheckForNotFound(deviceDTO, "IOT_Device", deviceID);

            return ConvertToDomain(deviceDTO);
        }

        public void Update(IOT_Device obj)
        {
            DevicesDTO newDevice = ConvertToDTO(obj);
            DevicesDTO foundDevice = ctx.Devices.First(d => d.DeviceID == obj.Id);
            if(foundDevice != null)
            {
                foundDevice.LocationX = newDevice.LocationX;
                foundDevice.LocationY = newDevice.LocationY;
            }
            
            ctx.SaveChanges();
        }

        public void DeleteDevice(int id)
        {
            DevicesDTO toDelete = ctx.Devices.First(d => d.DeviceID == id);
            ctx.Devices.Remove(toDelete);
            ctx.SaveChanges();
        }

        public IEnumerable<IOT_Device> ReadAllDevices()
        {
            List<IOT_Device> myQuery = new List<IOT_Device>();

            foreach (DevicesDTO DTO in ctx.Devices)
            {
                myQuery.Add(ConvertToDomain(DTO));
            }

            return myQuery;
        }
        #endregion
    }
}