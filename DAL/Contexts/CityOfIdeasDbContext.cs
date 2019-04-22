﻿using Microsoft.EntityFrameworkCore;
using DAL.Data_Transfer_Objects;

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
                optionsBuilder.UseSqlite("Data Source=IP1_TSI_DB.sqlite");
            }
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnswersDTO>().HasKey(a => a.AnswerID);
            modelBuilder.Entity<ChoicesDTO>().HasKey(c => c.ChoiceID);
            modelBuilder.Entity<DevicesDTO>().HasKey(d => d.DeviceID);
            modelBuilder.Entity<IdeaFieldsDTO>().HasKey(f => f.FieldID);
            modelBuilder.Entity<IdeasDTO>().HasKey(i => i.IdeaID);
            modelBuilder.Entity<IdeationQuestionsDTO>().HasKey(i => i.IQuestionID);
            modelBuilder.Entity<IdeationsDTO>().HasKey(i => i.ModuleID);
            modelBuilder.Entity<ModulesDTO>().HasKey(m => m.ModuleID);
            modelBuilder.Entity<OptionsDTO>().HasKey(o => o.OptionID);
            modelBuilder.Entity<OrganisationEventsDTO>().HasKey(o => o.EventID);
            modelBuilder.Entity<PhasesDTO>().HasKey(p => p.PhaseID);
            modelBuilder.Entity<PlatformsDTO>().HasKey(p => p.PlatformID);
            modelBuilder.Entity<ProjectImagesDTO>().HasKey(p => p.ImageID);
            modelBuilder.Entity<ProjectsDTO>().HasKey(p => p.ProjectID);
            modelBuilder.Entity<QuestionnaireQuestionsDTO>().HasKey(q => q.QQuestionID);
            modelBuilder.Entity<UserActivitiesDTO>().HasKey(u => u.ActivityID);
            modelBuilder.Entity<UserDetailsDTO>().HasKey(u => u.UserID);
            modelBuilder.Entity<UsersDTO>().HasKey(u => u.UserID);
            modelBuilder.Entity<VotesDTO>().HasKey(v => v.VoteID);
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
