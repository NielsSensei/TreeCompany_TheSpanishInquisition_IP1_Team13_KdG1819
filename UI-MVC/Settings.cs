using Microsoft.Extensions.Configuration;

namespace UIMVC
{
    public class Settings
    {
        private IConfiguration configuration;
        public string ApiKey { get; set; }
        
        public Settings(IConfiguration configuration)
        {
            ApiKey = configuration.GetValue<string>("Google:MapsApi");
        }

        public string GetApiKey()
        {
            return ApiKey;
        }
    }
}