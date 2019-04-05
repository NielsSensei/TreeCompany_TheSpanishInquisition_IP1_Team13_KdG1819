using System.Collections.Generic;
using Domain.UserInput;

namespace DAL
{
    public class IdeationVoteRepository //: IRepository<Vote>
    {
        // Added by NVZ
        //TODO: Get rid of interactions, maybe.
        private List<Vote> votes;
        private List<IOT_Device> devices;
        //private List<Interaction> interactions;

        public IdeationVoteRepository()
        {
            //TODO: Initialisatie
        }
        
        // Added by NVZ
        // Vote CRUD
        #region
        public Vote Create(Vote obj)
        {
            votes.Add(obj);
            return obj;
        }
        
        public Vote Read(int id)
        {
            return votes.Find(vote => vote.Id == id);
        }
        
        public void Update(Vote obj)
        {
            if (Read(obj.Id) != null)
            {
                votes[obj.Id - 1] = obj;
            }
            else
            {
                throw new KeyNotFoundException("Vote with id " + obj.Id + " not found!");
            }
        }
        
        public void Delete(int id)
        {
            votes.RemoveAt(id-1);
        }
        
        public IEnumerable<Vote> ReadAll()
        {
            return votes;
        }

        /* public IEnumerable<Vote> ReadAll(int deviceID)
        {
            return votes.FindAll(vote => vote.deviceID == deviceID);
        } */
        #endregion
               
        
        // Added by NVZ
        // Devices CRUD
        #region

        //TODO: Compare of location.
        public IOT_Device Create(IOT_Device obj)
        {
            devices.Add(obj);
            return obj;
        }

        public IOT_Device ReadDevice(int deviceID)
        {
            return devices.Find(device => device.Id == deviceID);
        }

        public void Update(IOT_Device obj)
        {
            if (Read(obj.Id) != null)
            {
                devices[obj.Id - 1] = obj;
            }
            else
            {
                throw new KeyNotFoundException("Device with id " + obj.Id + " not found!");
            }
        }

        public void DeleteDevice(int id)
        {
            devices.RemoveAt(id-1);
        }

        public IEnumerable<IOT_Device> ReadAllDevices()
        {
            return devices;
        }
        #endregion
        
        // Added by NVZ
        // Interactions CRUD
        /*public Interaction Create(Interaction obj)
        {
            interactions.Add(obj);
            return obj;
        }

        public void DeleteInteraction(int deviceID, int userID)
        {
            bool deleted = false;
            foreach (Interaction i in interactions)
            {
                if (i.DeviceId == deviceID && i.UserId == userID && !deleted)
                {
                    interactions.Remove(i);
                    deleted = true;
                }                   
            }
        }

        public IEnumerable<Interaction> ReadAllInteractions(int deviceID)
        {
            return interactions.FindAll(i => i.DeviceId == deviceID);
        } */
    }
}