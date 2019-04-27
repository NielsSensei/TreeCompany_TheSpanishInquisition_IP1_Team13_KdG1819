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
        private PlatformsDTO ConvertToDTO(Platform p)
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

        private Platform ConvertToDomain(PlatformsDTO DTO)
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
                    throw new DuplicateNameException("Platform(ID=" + obj.Id + ") met naam " + obj.Name + " heeft dezelfde naam als Platform(ID=" + p.Id +
                        " met naam " + p.Name);
                }
            }

            ctx.Platforms.Add(ConvertToDTO(obj));
            ctx.SaveChanges();

            return ConvertToDomain(ctx.Platforms.FirstOrDefault(dto => dto.Name == obj.Name));
        }

        public Platform Read(int id, bool details)
        {
            PlatformsDTO platformDTO = null;
            platformDTO = details ? ctx.Platforms.AsNoTracking().First(p => p.PlatformID == id) : ctx.Platforms.First(p => p.PlatformID == id);
            ExtensionMethods.CheckForNotFound(platformDTO, "Platform", id);

            return ConvertToDomain(platformDTO);
        }

        
        // Modified by XV
        public void Update(Platform obj)
        {
            PlatformsDTO newPlatform = ConvertToDTO(obj);
            PlatformsDTO entity = ctx.Platforms.FirstOrDefault(dto => dto.PlatformID == newPlatform.PlatformID);
            if (entity != null)
            {
                entity.Name = newPlatform.Name;
                entity.SiteUrl = newPlatform.SiteUrl;
                entity.IconImage = newPlatform.IconImage;
                ctx.Platforms.Update(entity);
            }

            ctx.SaveChanges();

            /*
            PlatformsDTO newPlatform = ConvertToDTO(obj);
            Platform found = Read(obj.Id, false);
            PlatformsDTO foundPlatform = ConvertToDTO(found);
            foundPlatform = newPlatform;
            
            ctx.Platforms.First(dto => dto.PlatformID == newPlatform.PlatformID) = newPlatform;
            
            
            
            ctx.SaveChanges();
            */
        }

        public void Delete(int id)
        {
            Platform toDelete = Read(id, false);
            ctx.Platforms.Remove(ConvertToDTO(toDelete));
            ctx.SaveChanges();
        }

        public IEnumerable<Platform> ReadAll()
        {
            List<Platform> myQuery = new List<Platform>();

            foreach (PlatformsDTO DTO in ctx.Platforms)
            {
                myQuery.Add(ConvertToDomain(DTO));
            }

            return myQuery;
        }
        #endregion  
    }
}