using DAL.Data_Access_Objects;
using Microsoft.EntityFrameworkCore;
using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.Contexts
{
    public class CityOfIdeasDbContext : IdentityDbContext<UimvcUser>
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
                optionsBuilder.UseSqlite("Data Source=TheSpanishDatabase.db");
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnswersDao>().HasKey(a => a.AnswerId);
            modelBuilder.Entity<ChoicesDao>().HasKey(c => c.ChoiceId);
            modelBuilder.Entity<DevicesDao>().HasKey(d => d.DeviceId);
            modelBuilder.Entity<IdeaFieldsDao>().HasKey(f => f.FieldId);
            modelBuilder.Entity<IdeasDao>().HasKey(i => i.IdeaId);
            modelBuilder.Entity<IdeationQuestionsDao>().HasKey(i => i.IquestionId);
            modelBuilder.Entity<IdeationsDao>().HasKey(i => i.ModuleId);
            modelBuilder.Entity<ModulesDao>().HasKey(m => m.ModuleId);
            modelBuilder.Entity<OptionsDao>().HasKey(o => o.OptionId);
            modelBuilder.Entity<OrganisationEventsDao>().HasKey(o => o.EventId);
            modelBuilder.Entity<PhasesDao>().HasKey(p => p.PhaseId);
            modelBuilder.Entity<PlatformsDao>().HasKey(p => p.PlatformId);
            modelBuilder.Entity<ProjectImagesDao>().HasKey(p => p.ImageId);
            modelBuilder.Entity<ProjectsDao>().HasKey(p => p.ProjectId);
            modelBuilder.Entity<QuestionnaireQuestionsDao>().HasKey(q => q.QquestionId);
            modelBuilder.Entity<UserActivitiesDao>().HasKey(u => u.ActivityId);
            modelBuilder.Entity<VotesDao>().HasKey(v => v.VoteId);
            modelBuilder.Entity<ReportsDao>().HasKey(r => r.ReportId);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AnswersDao> Answers { get; set;  }
        public DbSet<ChoicesDao> Choices { get; set; }
        public DbSet<DevicesDao> Devices { get; set; }
        public DbSet<IdeaFieldsDao> IdeaFields { get; set; }
        public DbSet<IdeasDao> Ideas { get; set; }        
        public DbSet<IdeationQuestionsDao> IdeationQuestions { get; set; }
        public DbSet<IdeationsDao> Ideations { get; set; }
        public DbSet<ModulesDao> Modules { get; set; }
        public DbSet<OptionsDao> Options { get; set; }
        public DbSet<OrganisationEventsDao> OrganisationEvents { get; set; }
        public DbSet<PhasesDao> Phases { get; set; }
        public DbSet<PlatformsDao> Platforms { get; set; }
        public DbSet<ProjectImagesDao> ProjectImages { get; set; }
        public DbSet<ProjectsDao> Projects { get; set; }
        public DbSet<QuestionnaireQuestionsDao> QuestionnaireQuestions { get; set; }
        public DbSet<UserActivitiesDao> UserActivities { get; set; }
        public DbSet<VotesDao> Votes { get; set; }
        public DbSet<ReportsDao> Reports { get; set; }
    }
}
