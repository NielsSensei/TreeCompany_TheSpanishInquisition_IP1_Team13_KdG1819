using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL.Contexts;
using DAL.Data_Access_Objects;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace DAL.repos
{
    /*
     * @authors David Matei, Edwin Kai Yin Tam, Niels Van Zandbergen & Xander Veldeman
     */
    public class PlatformRepository : IRepository<Platform>
    {
        private readonly CityOfIdeasDbContext _ctx;

        public PlatformRepository()
        {
            _ctx = new CityOfIdeasDbContext();
        }

        /*
         * @authors Niels Van Zandbergen & Xander Veldeman
         */
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

        /*
         * @author Xander Veldeman
         */
        #region Id generation
        private int FindNextAvailablePlatformId()
        {
            if (!_ctx.Platforms.Any()) return 1;
            int newId = ReadAll().Max(platform => platform.Id) + 1;
            return newId;
        }
        #endregion

        /*
         * @authors David Matei, Edwin Kai Yin Tam, Niels Van Zandbergen & Xander Veldeman
         */
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

        /*
         * @documentation Niels Van Zandbergen
         *
         * @params id: Integer value die de identity van het object representeert.
         * @params details: Indien we enkel een readonly kopij nodig hebben van ons object maken we gebruik
         * van AsNoTracking. Dit verhoogt performantie en verhindert ook dat er dingen worden aangepast die niet
         * aangepast mogen worden.
         *
         * @see https://docs.microsoft.com/en-us/ef/core/querying/tracking#no-tracking-queries
         * 
         */
        public Platform Read(int id, bool details)
        {
            PlatformsDao platformDao = details ? _ctx.Platforms.AsNoTracking().FirstOrDefault(p => p.PlatformId == id) : _ctx.Platforms.FirstOrDefault(p => p.PlatformId == id);
            ExtensionMethods.CheckForNotFound(platformDao, "Platform", id);

            return ConvertToDomain(platformDao);
        }

        public void Update(Platform obj)
        {
            PlatformsDao newPlatform = ConvertToDao(obj);
            PlatformsDao foundPlatform = _ctx.Platforms.FirstOrDefault(dto => dto.PlatformId == newPlatform.PlatformId);
            if (foundPlatform != null)
            {
                if (newPlatform.Name != null) foundPlatform.Name = newPlatform.Name;
                if (newPlatform.SiteUrl != null) foundPlatform.SiteUrl = newPlatform.SiteUrl;
                if (newPlatform.IconImage != null) foundPlatform.IconImage = newPlatform.IconImage;
                if (newPlatform.CarouselImage != null) foundPlatform.CarouselImage = newPlatform.CarouselImage;
                if (newPlatform.FrontPageImage != null) foundPlatform.FrontPageImage = newPlatform.FrontPageImage;

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
