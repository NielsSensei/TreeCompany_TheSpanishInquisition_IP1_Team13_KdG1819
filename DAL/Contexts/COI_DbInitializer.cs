
namespace DAL.Contexts
{
    /*
    * @author Niels Van Zandbergen
    */
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
