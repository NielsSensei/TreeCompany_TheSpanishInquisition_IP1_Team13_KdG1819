using System.Collections.Generic;
using Domain.UserInput;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DAL.Contexts;
using DAL.Data_Access_Objects;
using Domain.Identity;

namespace DAL.repos
{
    public class IdeationVoteRepository : IRepository<Vote>
    {
        private readonly CityOfIdeasDbContext _ctx;

        public IdeationVoteRepository()
        {
            _ctx = new CityOfIdeasDbContext();
        }

        #region Conversion Methods
        private VotesDao ConvertToDao(Vote obj)
        {
            VotesDao v = new VotesDao
            {
                VoteId = obj.Id,
                DeviceId = obj.Device.Id,
                InputId = obj.Idea.Id,
                InputType = 2, //Voorlopig Idee
                UserMail = obj.UserMail,
                LocationX = obj.LocationX,
                LocationY = obj.LocationY,
                Choices = ExtensionMethods.ListToString(obj.Choices)
            };

            if (obj.Device != null)
            {
                v.DeviceId = obj.Device.Id;
            }

            return v;
        }

        private Vote ConvertToDomain(VotesDao dao)
        {
            Vote v = new Vote
            {
                Id = dao.VoteId,
                User = new UimvcUser() { Id = dao.UserId},
                Idea = new Idea { Id = dao.InputId },
                UserMail = dao.UserMail,
                LocationX = dao.LocationX,
                LocationY = dao.LocationY,
                Choices = ExtensionMethods.StringToList(dao.Choices)
            };

            if (dao.DeviceId != 0)
            {
                v.Device = new IotDevice(){ Id = dao.DeviceId};
            }

            return v;
        }

        private DevicesDao ConvertToDao(IotDevice obj)
        {
            return new DevicesDao
            {
                DeviceId = obj.Id,
                LocationX = obj.LocationX,
                LocationY = obj.LocationY
            };
        }

        private IotDevice ConvertToDomain(DevicesDao dao)
        {
            return new IotDevice
            {
                Id = dao.DeviceId,
                LocationX = dao.LocationX,
                LocationY = dao.LocationY
            };
        }
        #endregion

        #region Id generation
        private int FindNextAvailableVoteId()
        {
            if (!_ctx.Votes.Any()) return 1;
            int newId = ReadAll().Max(vote => vote.Id) + 1;
            return newId;
        }

        private int FindNextAvailableDeviceId()
        {
            if (!_ctx.Devices.Any()) return 1;
            int newId = ReadAllDevices().Max(device => device.Id)+1;
            return newId;
        }
        #endregion

        #region Vote CRUD
        public Vote Create(Vote obj)
        {
            IEnumerable<Vote> votes = ReadAll(obj.Device.Id);

            foreach (Vote v in votes)
            {
                if(v.UserMail == obj.UserMail)
                {
                    if(v.UserMail == obj.UserMail)
                    {
                        throw new DuplicateNameException("Vote(ID=" + obj.Id + ") en Vote(ID=" + v.Id + ") hebben dezelfde email en Device(ID=" +
                                                         obj.Device.Id + "), dit is absoluut niet toegestaan!");
                    }
                }
            }

            obj.Id = FindNextAvailableVoteId();
            _ctx.Votes.Add(ConvertToDao(obj));
            _ctx.SaveChanges();

            return obj;
        }

        public Vote Read(int id, bool details)
        {
            VotesDao voteDao = details ? _ctx.Votes.AsNoTracking().First(p => p.VoteId == id) : _ctx.Votes.First(p => p.VoteId == id);
            ExtensionMethods.CheckForNotFound(voteDao, "Vote", id);

            return ConvertToDomain(voteDao);
        }

        public void Update(Vote obj)
        {
            VotesDao newVote = ConvertToDao(obj);
            VotesDao foundVote = _ctx.Votes.First(vote => vote.VoteId == obj.Id);
            if (foundVote != null)
            {
                foundVote.UserMail = newVote.UserMail;
                foundVote.LocationX = newVote.LocationX;
                foundVote.LocationY = newVote.LocationY;
            }

            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            VotesDao toDelete = _ctx.Votes.First(v => v.VoteId == id);
            _ctx.Votes.Remove(toDelete);
            _ctx.SaveChanges();
        }

        public void DeleteVotes(int id)
        {
            List<Vote> votes = (List<Vote>) ReadAllByIdea(id);

            foreach (Vote vote in votes)
            {
                Delete(vote.Id);
            }
        }

        public IEnumerable<Vote> ReadAll()
        {
            List<Vote> myQuery = new List<Vote>();

            foreach (VotesDao dao in _ctx.Votes)
            {
                myQuery.Add(ConvertToDomain(dao));
            }

            return myQuery;
        }

        public IEnumerable<Vote> ReadAll(int deviceId)
        {
            return ReadAll().ToList().FindAll(vote => vote.Device.Id == deviceId);
        }

        public IEnumerable<Vote> ReadAllByIdea(int ideaId)
        {
            return ReadAll().ToList().FindAll(vote => vote.Idea.Id == ideaId);
        }
        #endregion

        #region Devices CRUD
        public IotDevice Create(IotDevice obj)
        {
            IEnumerable<IotDevice> devices = ReadAllDevices();

            foreach (IotDevice iot in devices)
            {
                if (iot.LocationX == obj.LocationX && iot.LocationY == obj.LocationY)
                {
                    throw new DuplicateNameException("Device(ID=" + iot.Id + ") en Device(ID=" + obj.Id + ") delen dezelfde locatie nl. " + obj.LocationX +
                        "," + obj.LocationY + ".");
                }
            }

            obj.Id = FindNextAvailableDeviceId();
            _ctx.Devices.Add(ConvertToDao(obj));
            _ctx.SaveChanges();

            return obj;
        }

        public IotDevice ReadDevice(int deviceId, bool details)
        {
            DevicesDao deviceDao = details ? _ctx.Devices.AsNoTracking().First(d => d.DeviceId == deviceId) : _ctx.Devices.First(d => d.DeviceId == deviceId);
            ExtensionMethods.CheckForNotFound(deviceDao, "IOT_Device", deviceId);

            return ConvertToDomain(deviceDao);
        }

        public void Update(IotDevice obj)
        {
            DevicesDao newDevice = ConvertToDao(obj);
            DevicesDao foundDevice = _ctx.Devices.First(d => d.DeviceId == obj.Id);
            if(foundDevice != null)
            {
                foundDevice.LocationX = newDevice.LocationX;
                foundDevice.LocationY = newDevice.LocationY;
            }

            _ctx.SaveChanges();
        }

        public void DeleteDevice(int id)
        {
            DevicesDao toDelete = _ctx.Devices.First(d => d.DeviceId == id);
            _ctx.Devices.Remove(toDelete);
            _ctx.SaveChanges();
        }

        public IEnumerable<IotDevice> ReadAllDevices()
        {
            List<IotDevice> myQuery = new List<IotDevice>();

            foreach (DevicesDao dao in _ctx.Devices)
            {
                myQuery.Add(ConvertToDomain(dao));
            }

            return myQuery;
        }
        #endregion
    }
}
