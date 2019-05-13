using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL.Contexts;
using DAL.Data_Access_Objects;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace DAL.repos
{
    public class PlatformRepository : IRepository<Platform>
    {
        private readonly CityOfIdeasDbContext _ctx;
        
        public PlatformRepository()
        {
            _ctx = new CityOfIdeasDbContext();
        }
        
        #region Conversion Methods
        private PlatformsDao ConvertToDao(Platform p)
        {
            return new PlatformsDao
            {
                PlatformId = p.Id,
                Name = p.Name,
                SiteUrl = p.Url,
                CarouselImage =  p.CarouselImage,
                IconImage = p.IconImage,
                FrontPageImage = p.FrontPageImage
            };
        }

        private Platform ConvertToDomain(PlatformsDao dao)
        {
            return new Platform
            {
                Id = dao.PlatformId,
                Name = dao.Name,
                Url = dao.SiteUrl,
                CarouselImage =  dao.CarouselImage,
                IconImage = dao.IconImage,
                FrontPageImage = dao.FrontPageImage
            };
        }
        #endregion
        
        #region Id generation
        private int FindNextAvailablePlatformId()
        {
            if (!_ctx.Platforms.Any()) return 1;
            int newId = ReadAll().Max(platform => platform.Id) + 1;
            return newId;
        }
        #endregion
        
        #region Platform CRUD
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

            obj.Id = FindNextAvailablePlatformId();
            _ctx.Platforms.Add(ConvertToDao(obj));
            _ctx.SaveChanges();

            return obj;
        }

        public Platform Read(int id, bool details)
        {
            PlatformsDao platformDao = details ? _ctx.Platforms.AsNoTracking().First(p => p.PlatformId == id) : _ctx.Platforms.First(p => p.PlatformId == id);
            ExtensionMethods.CheckForNotFound(platformDao, "Platform", id);

            return ConvertToDomain(platformDao);
        }
        
        public void Update(Platform obj)
        {
            PlatformsDao newPlatform = ConvertToDao(obj);
            PlatformsDao foundPlatform = _ctx.Platforms.FirstOrDefault(dto => dto.PlatformId == newPlatform.PlatformId);
            if (foundPlatform != null)
            {
                foundPlatform.Name = newPlatform.Name;
                foundPlatform.SiteUrl = newPlatform.SiteUrl;
                foundPlatform.IconImage = newPlatform.IconImage;
                foundPlatform.CarouselImage = newPlatform.CarouselImage;
                foundPlatform.FrontPageImage = newPlatform.FrontPageImage;
                
                _ctx.Platforms.Update(foundPlatform);
            }

            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            PlatformsDao toDelete = _ctx.Platforms.First(r => r.PlatformId == id);
            _ctx.Platforms.Remove(toDelete);
            _ctx.SaveChanges();
        }

        public IEnumerable<Platform> ReadAll()
        {
            List<Platform> myQuery = new List<Platform>();

            foreach (PlatformsDao dao in _ctx.Platforms)
            {
                myQuery.Add(ConvertToDomain(dao));
            }

            return myQuery;
        }
        #endregion
    }
}
