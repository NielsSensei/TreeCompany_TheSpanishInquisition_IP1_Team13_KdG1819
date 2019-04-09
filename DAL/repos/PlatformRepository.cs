using System.Collections.Generic;
using System.Data;
using System.Linq;
using Domain.Users;
using DAL.Contexts;
using DAL.Data_Transfer_Objects;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class PlatformRepository : IRepository<Platform>
    {
        // Added by DM
        // Modified by NVZ
        private CityOfIdeasDbContext ctx;

        // Added by NVZ
        public PlatformRepository()
        {
            ctx = new CityOfIdeasDbContext();
        }

        // Added by NVZ
        // Standard Methods
        #region
        private PlatformsDTO convertToDTO(Platform p)
        {
            return new PlatformsDTO
            {
                PlatformID = p.Id,
                Name = p.Name,
                SiteUrl = p.Url
                // TODO: (SPRINT2?) Dit kunnen oplossen
                // IconImage = p.Image 
            };
        }

        private Platform convertToDomain(PlatformsDTO DTO)
        {
            return new Platform
            {
                Id = DTO.PlatformID,
                Name = DTO.Name,
                Url = DTO.SiteUrl
                // TODO: (SPRINT2?) Dit kunnen oplossen
                // IconImage = p.Image 
            };
        }
        #endregion
        /*
        public void Update(Platform obj)
        {
            Delete(obj.Id);
            Create(obj);
        }

        public void Delete(int id)
        {
            Platform p = Read(id);
            if (p != null)
            {
                Platforms.Remove(p);
            }
        }

        public IEnumerable<Platform> ReadAll()
        {
            return Platforms;
        }
        #endregion*/

        // Added by NVZ
        // Platform CRUD
        #region
        public Platform Create(Platform obj)
        {
            IEnumerable<Platform> platforms = ReadAll();

            foreach (Platform p in platforms)
            {
                if (ExtensionMethods.HasMatchingWords(p.Name, obj.Name) > 0)
                {
                    throw new DuplicateNameException("Platform(ID=" + obj.Id + ") met naam " + obj.Name +" heeft dezelfde naam als Platform(ID=" + p.Id + 
                        " met naam " + p.Name);
                }
            }

            ctx.Platforms.Add(convertToDTO(obj));
            ctx.SaveChanges();

            return obj;
        }

        public Platform Read(int id, bool details)
        {
            PlatformsDTO platformDTO = null;

            if (details)
            {
                platformDTO = ctx.Platforms.AsNoTracking().First(p => p.PlatformID == id);
                ExtensionMethods.CheckForNotFound(platformDTO, "Platform", platformDTO.PlatformID);
            }
            else
            {
                platformDTO = ctx.Platforms.First(p => p.PlatformID == id);
                ExtensionMethods.CheckForNotFound(platformDTO, "Platform", platformDTO.PlatformID);
            }

            return convertToDomain(platformDTO);
        }
        
        public void Update(Platform obj)
        {
            PlatformsDTO newPlatform = convertToDTO(obj);
            PlatformsDTO foundPlatform = convertToDTO(Read(obj.Id, false));
            foundPlatform = newPlatform;
            ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            ctx.Platforms.Remove(convertToDTO(Read(id, false)));
            ctx.SaveChanges();
        }
        
        public IEnumerable<Platform> ReadAll()
        {
            IEnumerable<Platform> myQuery = new List<Platform>();

            foreach (PlatformsDTO DTO in ctx.Platforms)
            {
                myQuery.Append(convertToDomain(DTO));
            }

            return myQuery;
        }
        #endregion  
        /*
        public IEnumerable<PlatformOwner> ReadAllOwners(int platformID)
        {
            return platformsOwners.FindAll(p => p.PlatformID == platformID);
        }
        #endregion*/
    }
}