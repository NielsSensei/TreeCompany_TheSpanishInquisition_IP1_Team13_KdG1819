using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UIMVC.Areas.Identity.Data
{
    /*
     * @author Xander Veldeman
     */
    public class UimvcContext : IdentityDbContext<UimvcUser>
    {
        public UimvcContext(DbContextOptions<UimvcContext> options)
            : base(options)
        {
        }
    }
}
