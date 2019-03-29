using System;
using System.Collections.Generic;
using Domain;
using Domain.Common;
using Domain.Projects;
using Domain.UserInput;
using Domain.Users;

namespace DAL
{
    public class Seed
    {
        //Added by NG
        // Modified by NVZ & XV
        //Declaration of seed Data
        #region
        public List<PlatformOwner> PlatformOwners { get; set; }
        public List<User> Users { get; set; }
        public List<Phase> Phases { get; set; }
        public List<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }
        public List<Answer> Answers { get; set; }
        public List<IdeationQuestion> IdeationQuestions { get; set; }
        public List<Idea> Ideas { get; set; }
        public List<Field> Fields { get; set; }

        public Project project;
        public Questionnaire questionnaire;
        public Platform platform1;
        public Ideation ideation;
        public Media video;
        public IOT_Device device;
        public Vote vote1;
        public Vote vote2;
        public Vote vote3;
        public Interaction iter1;
        public Interaction iter2;
        public Interaction iter3;
        
        private PlatformOwner platformOwner1;
        private PlatformOwner platformOwner2;
        private PlatformOwner platformOwner3;
        
        private User anonUser1;
        private User anonUser2;
        private User logUser1;
        private User logUser2;
        private User logUser3;
        private User verUser1;
        private User verUser2;
        private Organisation orgUser1;
        private User modUser1;
        private User modUser2;
        private User admin1;
        private User admin2;
        private User superAdmin;
        
        private Phase phase1;
        private Phase phase2;
        
        private QuestionnaireQuestion qQuestion1;
        private Answer q1Answer1;
        private Answer q1Answer2;
        private QuestionnaireQuestion qQuestion2;
        private SingleAnswer q2Answer1;
        private SingleAnswer q2Answer2;
        private QuestionnaireQuestion qQuestion3;
        private SingleAnswer q3Answer1;
        private SingleAnswer q3Answer2;
        private QuestionnaireQuestion qQuestion4;
        private MultipleAnswer q4Answer1;
        private MultipleAnswer q4Answer2;
        private QuestionnaireQuestion qQuestion5;
        private Answer q5Answer1;
        private QuestionnaireQuestion qQuestion6;
       
        private IdeationQuestion iQuestion1;
        private Idea i1Idea1;
        private Field field1;
        private Field field2;
        
        private Idea i1Idea2;
        private Field field3;
        private Field field4;
        
        private IdeationQuestion iQuestion2;
        private Idea i2Idea1;
        private Field field5;
        private Idea i2Idea2;
        private Field field6;
        
        #endregion
        
        //Added by NG
        //Initialization of seed Data
        internal Seed()
        {
            platform1 = new Platform(1);

            //INIT PLATFORMOWNERS
            
            platformOwner1 = new PlatformOwner(1, "Guy Fawkes");
            platformOwner1.PlatformID = platform1.Id;
            
            platformOwner2 = new PlatformOwner(2, "Steve Jobs");
            platformOwner2.PlatformID = platform1.Id;
            
            platformOwner3 = new PlatformOwner(3, "Bill Gates");
            platformOwner3.PlatformID = platform1.Id;

            //INIT COLLECTION FOR PLATFORM
            platform1.Owners = new List<PlatformOwner>();
            //ADD OWNERS TO PLATFORM
            platform1.AddOwner(platformOwner1);
            platform1.AddOwner(platformOwner2);
            platform1.AddOwner(platformOwner3);

            //INIT USERS

            #region

            //Anonymous Users

            #region

            anonUser1 = new User();
            anonUser1.Id = 1;
            anonUser1.Role = Role.ANONYMOUS;

            anonUser2 = new User();
            anonUser2.Id = 2;
            anonUser2.Role = Role.ANONYMOUS;

            #endregion

            //Logged-In Users

            #region

            logUser1 = new User();
            logUser1.Id = 3;
            logUser1.Role = Role.LOGGEDIN;
            logUser1.Email = "person@coldmail.com";

            logUser2 = new User();
            logUser2.Id = 4;
            logUser2.Role = Role.LOGGEDIN;
            logUser2.Name = "Jane Doe";
            logUser2.Email = "unknown1.blindspot@fbi.gov";
            logUser2.Gender = false;

            logUser3 = new User();
            logUser3.Id = 5;
            logUser3.Role = Role.LOGGEDIN;
            logUser3.Name = "Frodo Baggins";
            logUser3.Email = "frodo.baggins@the.shire.ar";
            logUser3.Gender = true;
            logUser3.BirthDate = DateTime.Parse("22/09/1368");

            #endregion

            //Verified Users

            #region

            verUser1 = new User();
            verUser1.Id = 6;
            verUser1.Role = Role.LOGGEDINVERIFIED;
            verUser1.Name = "Matthew Mercer";
            verUser1.Gender = true;
            verUser1.BirthDate = Convert.ToDateTime("29/05/1982");

            verUser2 = new User();
            verUser2.Id = 7;
            verUser2.Role = Role.LOGGEDINVERIFIED;
            verUser2.Name = "God";

            #endregion

            //Organisation Users
            orgUser1 = new Organisation();
            orgUser1.Id = 8;
            orgUser1.Role = Role.LOGGEDINORG;
            orgUser1.Name = "Galactic Entertainment Studios";
            orgUser1.Email = "nathan@gestudios.eu";
            //INIT COLLECTION FOR EVENTS
            orgUser1.Events = new List<Event>();
            orgUser1.AddEvent(new Event(){Id = 1, organiserID = orgUser1.Id});

            //Moderator Users

            #region

            modUser1 = new User();
            modUser1.Id = 9;
            modUser1.Name = "Jan Van Overvelt";
            modUser1.Role = Role.MODERATOR;
            modUser1.Email = "jan.van.overvelt@kdg.be";

            modUser2 = new User();
            modUser2.Id = 10;
            modUser2.Role = Role.MODERATOR;
            modUser2.Name = "Eddy Pijl";
            modUser2.Email = "eddy.pijl@kdg.be";

            #endregion

            //Admin Users

            #region

            admin1 = new User();
            admin1.Id = 11;
            admin1.Role = Role.ADMIN;
            admin1.Name = "";

            admin2 = new User();
            admin2.Id = 12;
            admin2.Role = Role.ADMIN;
            admin2.Name = "";

            #endregion

            //SuperAdmin Users
            superAdmin = new User();
            superAdmin.Id = 13;
            superAdmin.Role = Role.SUPERADMIN;

            #endregion

            //ADD USERS TO PLATFORM

            #region
            // INIT LIST FOR USERS
            platform1.Users = new List<User>();
            platform1.AddUser(anonUser1);
            platform1.AddUser(anonUser2);
            platform1.AddUser(logUser1);
            platform1.AddUser(logUser2);
            platform1.AddUser(logUser3);
            platform1.AddUser(verUser1);
            platform1.AddUser(verUser2);
            platform1.AddUser(orgUser1);
            platform1.AddUser(modUser1);
            platform1.AddUser(modUser2);
            platform1.AddUser(admin1);
            platform1.AddUser(admin2);
            platform1.AddUser(superAdmin);

            #endregion

            //ADD ADMINS TO OWNERS

            #region
            //INIT LIST PLATFORMOWNERS
            platformOwner1.Admins = new List<User>();
            platformOwner1.addAdmin(admin1);
            
            platformOwner2.Admins = new List<User>();
            platformOwner2.addAdmin(admin2);
            
            platformOwner3.Admins = new List<User>();
            platformOwner3.addAdmin(superAdmin);

            #endregion

            //INIT PROJECT
            project = new Project();
            project.Id = 1;
            project.Title = "GROENplaats";
            project.Goal = "De Antwerpse Groenplaats terug groen maken";
            project.Open = true;
            project.PreviewImages = new List<Image>()
            {
                new Image() {Width = 259, Height = 194, SavedName = ".\\DAL\\Media & Image files\\grasplein.jpg", UserImage = false},
                new Image() {Width = 318, Height = 159, SavedName = ".\\DAL\\Media & Image files\\park.jpg", UserImage = false}
            };
            

            //INIT PHASES
            #region
            phase1 = new Phase();
            phase1.Id = 1;
            phase1.Project = project;
            phase1.Description = "Vergroenen van de Groenplaats";
            phase1.StartDate = Convert.ToDateTime("10/03/2019");
            phase1.EndDate = Convert.ToDateTime("30/03/2019");

            phase2 = new Phase();
            phase2.Id = 2;
            phase2.Project = project;
            phase2.Description = "Gebruik van nieuwe groene ruimte";
            phase2.StartDate = Convert.ToDateTime("1/04/2019");
            phase2.EndDate = Convert.ToDateTime("30/04/2019");
            #endregion
            
            //ADD PHASES TO PROJECT
            //INIT PROJECT PHASES COLLECTION
            project.Phases = new List<Phase>();
            project.Phases.Add(phase1);
            project.Phases.Add(phase2);
            project.CurrentPhase = phase1;
            
            //INIT MODULES
            #region
            //INIT QUESTIONNAIRE COLLECTION
            questionnaire = new Questionnaire();
            questionnaire.Phases = new List<Phase>();
            
            // questionnaire = new Questionnaire();
            questionnaire.Id = 1;
            questionnaire.Phases.Add(phase1);
            questionnaire.Phases.Add(phase2);
            questionnaire.Tags = new List<string>() {"#Questionnaire", "#ForTheClimate", "#OpinionsAreImportant"};
            
            ideation = new Ideation();
            ideation.Phases = new List<Phase>();
            ideation.Id = 1;
            ideation.Phases.Add(phase1);
            ideation.Phases.Add(phase2);
            ideation.Tags = new List<string>() {"#CreateIdeas", "#ForTheClimate", "#NoIdeaIsStupid"};
            ideation.Media = new Media(){Extension = ".mp4", LengthInSeconds = 138, UserVideo = false, SavedName = ".\\DAL\\Media & Image files\\Groenplaats in Antwerpen.mp4"};
            #endregion
            
            //ADD MODULES TO PROJECT
            project.Modules = new List<Module>();
            project.Modules.Add(questionnaire);
            project.Modules.Add(ideation);
            
            //ADD MODULES TO PHASES
            phase1.Module = ideation;
            phase2.Module = questionnaire;
            
            //QUESTIONNAIRE STUFF
            #region
            
            questionnaire.Questions = new List<QuestionnaireQuestion>();
            //INIT QUESTIONS
            #region
            qQuestion1 = new QuestionnaireQuestion();
            qQuestion1.Id = 1;
            qQuestion1.Type = QuestionType.OPEN;
            qQuestion1.QuestionText = "Waarom wil je de Groenplaats terug groen?";
            
            qQuestion2 = new QuestionnaireQuestion();
            qQuestion2.Id = 2;
            qQuestion2.Type = QuestionType.DROP;
            qQuestion2.QuestionText = "Welke aanpassing wil je het liefst?";
            
            qQuestion3 = new QuestionnaireQuestion();
            qQuestion3.Id = 3;
            qQuestion3.Type = QuestionType.SINGLE;
            qQuestion3.QuestionText = "Vind je dat er ook een bloementuin op de Groenplaats past?";
            
            qQuestion4 = new QuestionnaireQuestion();
            qQuestion4.Id = 4;
            qQuestion4.Type = QuestionType.MULTI;
            qQuestion4.QuestionText = "Hoeveel m2 aan gras moet er aangelegd worden?";
            
            qQuestion5 = new QuestionnaireQuestion();
            qQuestion5.Id = 5;
            qQuestion5.Type = QuestionType.MAIL;
            qQuestion5.QuestionText = "Gelieve u email adres achter te halen als u updates wilt over het project";
            qQuestion5.Optional = true;
            #endregion
            
            //ADD QUESTIONS TO QUESTIONNAIRE
            questionnaire.Questions.Add(qQuestion1);
            questionnaire.Questions.Add(qQuestion2);
            questionnaire.Questions.Add(qQuestion3);
            questionnaire.Questions.Add(qQuestion4);
            questionnaire.Questions.Add(qQuestion5);
          

            //INIT QUESTIONNAIRE ANSWERS
            #region
            q1Answer1 = new Answer();
            q1Answer1.Id = 1;
            q1Answer1.OpenAnswer = "Een grijs stadshart is deprimerend.";
            q1Answer1.Completed = true;
            
            q1Answer2 = new Answer();
            q1Answer2.Id = 2;
            q1Answer2.Completed = false;
            
            q2Answer1 = new SingleAnswer();
            q2Answer1.Id = 3;
            q2Answer1.DropDownList = true;
            q2Answer1.Options = new List<string>(){"Geen tram 4 meer op de groenplaats","Verkeer afsluiten op de groenplaats", "De groenplaats vervangen door klein bos."};
            q2Answer1.IsUserEmail = false;
            q2Answer1.Completed = false;
            
            q2Answer2 = new SingleAnswer();
            q2Answer2.Id = 4;
            q2Answer2.DropDownList = true;
            q2Answer2.Options = new List<string>(){"Geen tram 4 meer op de groenplaats","Verkeer afsluiten op de groenplaats", "De groenplaats vervangen door klein bos."};
            q2Answer2.IsUserEmail = false;
            q2Answer2.Choice = q2Answer1.Options[0];
            q2Answer2.Completed = true;
            
            q3Answer1 = new SingleAnswer();
            q3Answer1.Id = 5;
            q3Answer1.DropDownList = false;
            q3Answer1.Options = new List<string>(){"Ja","Nee"};
            q3Answer1.IsUserEmail = false;
            q3Answer1.Completed = false;
            
            
            q3Answer2 = new SingleAnswer();
            q3Answer2.Id = 6;
            q3Answer2.DropDownList = false;
            q3Answer2.Options = new List<string>(){"Ja","Nee"};
            q3Answer2.IsUserEmail = false;
            q3Answer2.Choice = q2Answer1.Options[1];
            q3Answer2.Completed = true;
            
            q4Answer1 = new MultipleAnswer();
            q4Answer1.Id = 7;
            q4Answer1.RegularAnswers = new List<string>() {"10m²", "20m²", "30m²"};
            q4Answer1.ExtraAnswers = new List<string>() {"40m²"};
            q4Answer1.Completed = false;
            
            q4Answer2 = new MultipleAnswer();
            q4Answer2.Id = 8;
            q4Answer2.RegularAnswers = new List<string>() {"10m²", "20m²", "30m²"};
            q4Answer2.OpenAnswer = String.Concat(q4Answer2.RegularAnswers[1], ",", q4Answer2.RegularAnswers[2]);
            q4Answer2.Completed = true;
            
            q5Answer1 = new Answer();
            q5Answer1.Id = 9;
            q5Answer1.IsUserEmail = true;
            q5Answer1.OpenAnswer = "voorbeeldigeantwerpenaar@nva.be";
            q5Answer1.Completed = true;
            
            
            #endregion

            //ADD ANSWERS TO QUESTIONS
            q1Answer1.questionID = qQuestion1.Id;
            q1Answer2.questionID = qQuestion1.Id;
            q2Answer1.questionID = qQuestion2.Id;
            q2Answer2.questionID = qQuestion2.Id;
            q3Answer1.questionID = qQuestion3.Id;
            q3Answer2.questionID = qQuestion3.Id;
            q4Answer1.questionID = qQuestion4.Id;
            q4Answer2.questionID = qQuestion4.Id;
            q5Answer1.questionID = qQuestion5.Id;
            
            qQuestion1.Answers = new List<Answer>();
            qQuestion1.Answers.Add(q1Answer1);
            qQuestion1.Answers.Add(q1Answer2);
            
            qQuestion2.Answers = new List<Answer>();
            qQuestion2.Answers.Add(q2Answer1);
            qQuestion2.Answers.Add(q2Answer2);
            
            qQuestion3.Answers = new List<Answer>();
            qQuestion3.Answers.Add(q3Answer1);
            qQuestion3.Answers.Add(q3Answer2);
            
            qQuestion4.Answers = new List<Answer>();
            qQuestion4.Answers.Add(q4Answer1);
            qQuestion4.Answers.Add(q4Answer2);
            
            qQuestion5.Answers = new List<Answer>();
            qQuestion5.Answers.Add(q5Answer1);
            
            
            #endregion
            
            //IDEATION STUFF
            //INIT IDEATIONQUESTION
            #region
            iQuestion1 = new IdeationQuestion();
            iQuestion1.Id = 1;
            iQuestion1.QuestionText = "Hoe maken we de Groenplaats groener?";
            iQuestion1.IdeationId = ideation.Id;
            #endregion
            ideation.CentralQuestion = iQuestion1;
            //idea 1
            #region
            i1Idea1 = new Idea();
            i1Idea1.Id = 1;
            i1Idea1.Title = "#MakeGroenplaatsGreenAgain";
            field1 = new Field();
            field1.Id = 1;
            field1.Text =
                "We maken een grote haag van bomen en struiken rond de Groenplaats om de grijze beton erbuiten te houden!";
            i1Idea1.Fields = new List<Field>();
            i1Idea1.Fields.Add(field1);
            field2 = new Field();
            field2.Id = 2;
            field2.Text = "http://groenplaats.be/wp-content/uploads/sites/111/2016/06/5.jpg";
            i1Idea1.Fields.Add(field2);
            i1Idea1.Reported = true;
            #endregion
            i1Idea1.questionID = iQuestion1.Id;
            iQuestion1.Ideas = new List<Idea>();
            iQuestion1.Ideas.Add(i1Idea1);
            
            //idea 2
            #region
            i1Idea2 = new Idea();
            i1Idea2.Id = 2;
            i1Idea2.Title = "Groenplaats Stadspark";
            field3 = new Field();
            field3.Id = 3;
            field3.Text = 
                "Maken een aantal graspleintjes en bloembakken aan met stenen wandelpaden en een pleintje in het midden rond het standbeeld :)";
            i1Idea2.Fields = new List<Field>();
            i1Idea2.Fields.Add(field3);
            field4 = new Field();
            field4.Id = 4;
            field4.Text = "I see a gray square and I want to 'paint' it green - Rolling Stoned";
            i1Idea2.Fields.Add(field4);
            
            device = new IOT_Device(){Id = 1, LocationX = 55.00, LocationY = 55.00};
            vote1 = new Vote(){Id = 1, IdeaID = 2, Positive = true, deviceID = 1};
            vote2 = new Vote(){Id = 2, IdeaID = 2, Positive = false, deviceID = 1 };
            vote3 = new Vote(){Id = 3, IdeaID = 2, Positive = true, deviceID = 1 };
            iter1 = new Interaction(){DeviceId = 1, UserId = 1};
            iter2 = new Interaction(){DeviceId = 1, UserId = 3};
            iter3 = new Interaction(){DeviceId = 1, UserId = 6};
            i1Idea2.VoteCount += 3;
            
            #endregion
            i1Idea2.questionID = iQuestion1.Id;
            iQuestion1.Ideas.Add(i1Idea2);
            
            //idea 3
            #region
            i2Idea1 = new Idea();
            i2Idea1.Id = 5;
            i2Idea1.Title = "Theater";
            field5 = new Field();
            field5.Id = 5;
            field5.Text = "Een locatie zo nabij het oude centrum moet evenveel cultuur hebben als het centrum zelf." +
                          " Dus stel ik voor om hier regelmatige theater voorstelling te houden, zodat we de jongeren " +
                          "echte cultuur kunnen aanleren.";
            i2Idea1.Fields = new List<Field>();
            i2Idea1.Fields.Add(field5);
            #endregion
            i2Idea1.questionID = iQuestion1.Id;
            
            
            //idea 4
            #region
            i2Idea2 = new Idea();
            i2Idea2.Id = 6;
            i2Idea2.Title = "Cinema";
            field6 = new Field();
            field6.Id = 6;
            field6.Text = "Nope, dom idee. Wij wille gewoon goeie films kunne zien, buiten op de Groenplaats. " +
                          "Ff pintje op café, laatste nieve film om middernacht opt gras buite. " +
                          "Der woont tog niemand, dus ook geen lawaaid overlast.";
            i2Idea2.Fields = new List<Field>();
            i2Idea2.Fields.Add(field6);
            #endregion
            i2Idea2.questionID = iQuestion1.Id;
            
            
            //INIT COLLECTIONS
            #region
            PlatformOwners = new List<PlatformOwner>();
            PlatformOwners.Add(platformOwner1);
            PlatformOwners.Add(platformOwner2);
            PlatformOwners.Add(platformOwner3);
            project.myPlatformOwner = platformOwner1.Id;
            platformOwner1.Projects = new List<Project>();
            platformOwner1.addProject(project);
            
            
            Users = new List<User>();
            Users.Add(anonUser1);
            Users.Add(anonUser2);
            Users.Add(logUser1);
            Users.Add(logUser2);
            Users.Add(logUser3);
            Users.Add(verUser1);
            Users.Add(verUser2);
            Users.Add(orgUser1);
            Users.Add(modUser1);
            Users.Add(modUser2);
            Users.Add(admin1);
            Users.Add(admin2);
            Users.Add(superAdmin);
            
            Phases = new List<Phase>();
            Phases.Add(phase1);
            Phases.Add(phase2);

            QuestionnaireQuestions = new List<QuestionnaireQuestion>();
            QuestionnaireQuestions.Add(qQuestion1);
            QuestionnaireQuestions.Add(qQuestion2);

            Answers = new List<Answer>();
            Answers.Add(q1Answer1);
            Answers.Add(q1Answer2);
            Answers.Add(q2Answer1);
            Answers.Add(q2Answer2);

            IdeationQuestions = new List<IdeationQuestion>();
            IdeationQuestions.Add(iQuestion1);
            IdeationQuestions.Add(iQuestion2);

            Ideas = new List<Idea>();
            Ideas.Add(i1Idea1);
            Ideas.Add(i1Idea2);
            Ideas.Add(i2Idea1);
            Ideas.Add(i2Idea2);
            
            Fields = new List<Field>();
            Fields.Add(field1);
            Fields.Add(field2);
            Fields.Add(field3);
            Fields.Add(field4);
            Fields.Add(field5);
            Fields.Add(field6);
            
            #endregion
        }
    }
}