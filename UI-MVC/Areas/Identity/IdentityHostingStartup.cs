using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UimvcUser = Domain.Identity.UimvcUser;

[assembly: HostingStartup(typeof(UIMVC.Areas.Identity.IdentityHostingStartup))]
namespace UIMVC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<DAL.Contexts.CityOfIdeasDbContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("UIMVCContextConnection")));

                services.AddDefaultIdentity<UimvcUser>(
                    config => { config.SignIn.RequireConfirmedEmail = true; })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<DAL.Contexts.CityOfIdeasDbContext>()
                    .AddDefaultTokenProviders()
                    .AddDefaultUI();
            });
        }
    }
}
