using System.Collections.Generic;
using System.Linq;
using DAL.repos;
using Domain.Users;

namespace BL
{
    /*
     * @authors Edwin Kai Yin Tam, Niels Van Zandbergen & Xander Veldeman
     */
    public class PlatformManager
    {
        private PlatformRepository PlatformRepo { get; }

        public PlatformManager()
        {
            PlatformRepo = new PlatformRepository();
        }

        /*
         * @authors Edwin Kai Yin Tam, Niels Van Zandbergen & Xander Veldeman
         */
        #region Platform
        public Platform GetPlatform(int platformId, bool details)
        {
            return PlatformRepo.Read(platformId, details);
        }

        public Platform MakePlatform(Platform platform)
        {
            return PlatformRepo.Create(platform);
        }

        public void EditPlatform(Platform platform)
        {
            PlatformRepo.Update(platform);
        }

        public void RemovePlatform(int platformId)
        {
            PlatformRepo.Delete(platformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return PlatformRepo.ReadAll();
        }

        public IEnumerable<Platform> SearchPlatforms(string search)
        {
            return PlatformRepo.ReadAll()
                .Where(platform => platform.Name.ToLower().Contains(search.ToLower()) || platform.Url.ToLower().Contains(search.ToLower()));
        }
        #endregion
    }
}
