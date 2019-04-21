using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Contexts
{
    class COI_DbInitializer
    {

        public static void Initialize(CityOfIdeasDbContext ctx, bool dropCreateDatabase = false)
        {
            if (dropCreateDatabase)
            {
                ctx.Database.EnsureDeleted();

            }
            else ctx.Database.EnsureCreated();         
        }
    }
}
