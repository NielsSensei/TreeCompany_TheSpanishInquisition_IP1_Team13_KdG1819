using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UIMVC.Areas.Identity.Data;
using UIMVC.Models;

[assembly: HostingStartup(typeof(UIMVC.Areas.Identity.IdentityHostingStartup))]
namespace UIMVC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UIMVCContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("UIMVCContextConnection")));

                //TODO: Implement roles
                services.AddDefaultIdentity<UIMVCUser>(
                    config => { config.SignIn.RequireConfirmedEmail = false; })
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<UIMVCContext>()
                    .AddDefaultTokenProviders()
                    .AddDefaultUI();

                
            });
        }
    }
}