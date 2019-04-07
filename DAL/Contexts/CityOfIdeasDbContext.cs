using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Projects;
using Domain.Common;
using Domain.UserInput;
using Domain.Users;
using Domain;
using DAL.Data_Transfer_Objects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace DAL.Contexts
{
    class CityOfIdeasDbContext : DbContext
    {
        /*
         * Naar deployment toe is dit wel nuttig. Options wordt wel vaak gebruikt voor de connectionstring mee te geven als deze in een aparte
         * file zit. Je hebt de optie om ou db te linken via de onconfiguring de .usedataprovider (in ons geval .usesqlserver) te gebruiken. Of
         * alle connectionstrings in een app.config achtige file ramt. -NVZ (Uitleg door Kenneth De Keulenaer).
         */ 
        public CityOfIdeasDbContext(DbContextOptions<CityOfIdeasDbContext> options) : base(options)
        {
            COI_DbInitializer.Initialize(this, false);
        }

        public CityOfIdeasDbContext()
        {
            COI_DbInitializer.Initialize(this, false);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //TODO: (IMPORTANT) Elk teammember moet dit voor hemzelf veranderen. Dit wordt veranderd naar deployment 'pad'.
                optionsBuilder
                    .UseSqlServer("Data Source=LAPTOP-MESCK2VS;Initial Catalog=IP1_TSI_DB;Integrated Security=True", providerOptions => providerOptions.CommandTimeout(60))
                    .UseLoggerFactory(new LoggerFactory(
                        new[]
                        {
                            new DebugLoggerProvider(
                                (category, level) =>
                                    category == DbLoggerCategory.Database.Command.Name
                                    && level == LogLevel.Information
                                    )


                        }
                        ));
            }
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Relaties al gelegd - SB
        }

        public DbSet<AnswersDTO> Answers { get; set; }
        public DbSet<ChoicesDTO> Choices { get; set; }
        public DbSet<DevicesDTO> Devices { get; set; }
        public DbSet<IdeaFieldsDTO> IdeaFields { get; set; }
        public DbSet<IdeasDTO> Ideas { get; set; }
        public DbSet<IdeationQuestionsDTO> IdeationQuestion { get; set; }
        public DbSet<IdeationsDTO> Ideations { get; set; }
        public DbSet<ModulesDTO> Modules { get; set; }
        public DbSet<OptionsDTO> Options { get; set; }
        public DbSet<OrganisationEventsDTO> OrganisationEvents { get; set; }
        public DbSet<PhasesDTO> Phases { get; set; }
        public DbSet<PlatformsDTO> Platforms { get; set; }
        public DbSet<ProjectImagesDTO> ProjectImages { get; set; }
        public DbSet<ProjectsDTO> Projects { get; set; }
        public DbSet<QuestionnaireQuestionsDTO> QuestionnaireQuestions { get; set; }
        public DbSet<UserActivitiesDTO> UserActivities { get; set; }
        public DbSet<UserDetailsDTO> UserDetails { get; set; }
        public DbSet<UsersDTO> Users { get; set; }
        public DbSet<VotesDTO> Votes { get; set; }

    }
}
