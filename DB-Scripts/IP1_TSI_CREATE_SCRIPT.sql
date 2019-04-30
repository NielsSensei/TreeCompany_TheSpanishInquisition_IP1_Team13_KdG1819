/*         CREATE SCRIPT IP1 VERSIE 5          */
/*TABLES DROPPEN*/
DROP TABLE IF EXISTS Ideations
DROP TABLE IF EXISTS Answers
DROP TABLE IF EXISTS Ideationquestions
DROP TABLE IF EXISTS Ideas
DROP TABLE IF EXISTS UserDetails
DROP TABLE IF EXISTS Projectimages
DROP TABLE IF EXISTS Modules
DROP TABLE IF EXISTS Questionnairequestions
DROP TABLE IF EXISTS Choices
DROP TABLE IF EXISTS Options
DROP TABLE IF EXISTS Ideafields
DROP TABLE IF EXISTS Organisationevents
DROP TABLE IF EXISTS Useractivities
DROP TABLE IF EXISTS Devices
DROP TABLE IF EXISTS Votes
DROP TABLE IF EXISTS Reports

DROP TABLE IF EXISTS Phases
DROP TABLE IF EXISTS Projects

DROP TABLE IF EXISTS Users
DROP TABLE IF EXISTS Platforms

/*TABELLEN AANMAKEN*/

CREATE TABLE Platforms(
	PlatformID INT IDENTITY,
	Name NVARCHAR(100) NOT NULL,
	SiteUrl VARCHAR(50) NOT NULL,
	IconImage BINARY(255),

	/*Constraints*/
	CONSTRAINT pk_Plaftorms PRIMARY KEY(PlatformID)
)

CREATE TABLE Users(
	UserID INT IDENTITY,
	Name NVARCHAR(100),
	Email NVARCHAR(100),
	Password BINARY(25),
	Role TINYINT NOT NULL,
	PlatformID INT NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_Users PRIMARY KEY(UserID),
	CONSTRAINT fk_Users_Platforms FOREIGN KEY (PlatformID) references Platforms(PlatformID) ON DELETE CASCADE ON UPDATE CASCADE
)

CREATE TABLE UserDetails(
	UserID INT,
	Zipcode VARCHAR(16) NOT NULL,
	Banned BIT NOT NULL,
	Gender TINYINT,
	Active BIT NOT NULL,
	BirthDate DATE,
	OrgName NVARCHAR(100),
	Description NVARCHAR(255),

	/*Constraints*/
	CONSTRAINT pk_UserDetails PRIMARY KEY(UserID),
	CONSTRAINT fk_UserDetails_Users FOREIGN KEY (UserID) references Users(UserID) ON DELETE CASCADE ON UPDATE CASCADE	
)

CREATE TABLE OrganisationEvents(
	EventID INT IDENTITY,
	UserID INT NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	Description NVARCHAR(255) NOT NULL,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_OrganisationEvents PRIMARY KEY(EventID),
	CONSTRAINT fk_OrganisationEvents_Users FOREIGN KEY (UserID) references Users(UserID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT chk_OrganisationEvents_StartDate CHECK (StartDate <= Enddate),
	CONSTRAINT chk_OrganisationEvents_EndDate CHECK (EndDate >= StartDate)
)

CREATE TABLE Projects(
	ProjectID INT IDENTITY,
	CurrentPhaseID INT NOT NULL,
	UserID INT NOT NULL,
	PlatformID INT NOT NULL,
	Title NVARCHAR(50) NOT NULL,
	Goal NVARCHAR(255) NOT NULL,
	Status VARCHAR(25) NOT NULL,
	Visible BIT NOT NULL,
	ReactionCount INT,
	LikeCount INT,
	FbLikeCount INT, 
	TwitterLikeCOunt INT,
	LikeVisibility TINYINT NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_Projects PRIMARY KEY(ProjectID),
	CONSTRAINT fk_Projects_Platforms FOREIGN KEY (PlatformID) references Platforms(PlatformID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT fk_Projects_Users FOREIGN KEY (UserID) references Users(UserID) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT ck_Projects_Status CHECK (Status = UPPER(Status))

)

CREATE TABLE Phases(
	PhaseID INT IDENTITY,
	ProjectID INT NOT NULL,
	Description NVARCHAR(255) NOT NULL,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_Phases PRIMARY KEY(PhaseID),
	CONSTRAINT fk_Phases_Projects FOREIGN KEY (ProjectID) references Projects(ProjectID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT chk_Phases_StartDate CHECK (StartDate <= EndDate),
	CONSTRAINT chk_Phases_EndDate CHECK (EndDate >= StartDate)
)

CREATE TABLE Modules(
	ModuleID INT IDENTITY,
	ProjectID INT NOT NULL,
	PhaseID INT NOT NULL,
	OnGoing BIT NOT NULL,
	Title NVARCHAR(100),
	LikeCount INT,
	FbLikeCount INT, 
	TwitterLikeCount INT,
	ShareCount INT,
	RetweetCount INT,
	Tags VARCHAR(255),
	IsQuestionnaire BIT NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_Modules PRIMARY KEY(ModuleID),
	CONSTRAINT fk_Modules_Projects FOREIGN KEY (ProjectID) references Projects(ProjectID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT fk_Modules_Phases FOREIGN KEY (PhaseID) references Phases(PhaseID) ON DELETE NO ACTION ON UPDATE NO ACTION
)

CREATE TABLE Projectimages(
	ProjectID INT NOT NULL,
	ImageID TINYINT IDENTITY,
	ProjectImage BINARY(255),

	/*Constraints*/
	CONSTRAINT pk_ProjectImages PRIMARY KEY(ProjectID, ImageID),
	CONSTRAINT fk_Projectimages_Projects FOREIGN KEY (ProjectID) references Projects(ProjectID) ON DELETE CASCADE ON UPDATE CASCADE
)

CREATE TABLE Devices(
	DeviceID INT IDENTITY,
	LocationX FLOAT,
	LocationY FLOAT,

	/*Constraints*/
	CONSTRAINT pk_Devices PRIMARY KEY(DeviceID)
)

CREATE TABLE IdeationQuestions(
	IQuestionID INT IDENTITY,
	ModuleID INT NOT NULL,
	QuestionTitle NVARCHAR(50) NOT NULL,
	Description NVARCHAR(255) NOT NULL,
	WebsiteLink NVARCHAR(50) NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_IdeationQuestions PRIMARY KEY(IQuestionID)
)

CREATE TABLE Ideas(
	IdeaID INT IDENTITY,
	IQuestionID INT NOT NULL,
	UserID INT NOT NULL,
	Reported BIT NOT NULL,
	ReviewByAdmin BIT NOT NULL,
	Visible BIT NOT NULL,
	VoteCount INT,
	RetweetCount INT,
	ShareCount INT,
	Title NVARCHAR(100) NOT NULL,
	Status VARCHAR(100),
	VerifiedUser BIT NOT NULL,
	ParentID INT,
	DeviceID INT,

	/*Constraints*/
	CONSTRAINT pk_Ideas PRIMARY KEY(IdeaID),
	CONSTRAINT fk_Ideas_Ideationquestions FOREIGN KEY (IQuestionID) references IdeationQuestions(IQuestionID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT fk_Ideas_Users FOREIGN KEY (UserID) references Users(UserID)
)


CREATE TABLE Ideations (
	ModuleID INT,
	UserID INT NOT NULL,
	Organisation BIT NOT NULL,
	EventID INT,
	UserIdea BIT NOT NULL,
	MediaFile BINARY(255),
	RequiredFields tinyint NOT NULL,
	ExtraInfo NVARCHAR(100),

	/*Constraints*/
	CONSTRAINT pk_Ideations PRIMARY KEY(ModuleID),
	CONSTRAINT fk_Ideations_Module FOREIGN KEY (ModuleID) references Modules(ModuleID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT fk_Ideations_Users FOREIGN KEY (UserID) references Users(UserID),
	CONSTRAINT fk_Ideations_OrganisationEvents FOREIGN KEY (EventID) references OrganisationEvents(EventID)
)

CREATE TABLE QuestionnaireQuestions(
	QQuestionID INT IDENTITY,
	ModuleID INT NOT NULL,
	QuestionText NVARCHAR(100) NOT NULL,
	QType BIT NOT NULL,
	Required BIT NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_QuestionnaireQuestions PRIMARY KEY(QQuestionID),
	CONSTRAINT fk_QuestionnaireQuestions_Modules FOREIGN KEY (ModuleID) references Modules(ModuleID) ON DELETE CASCADE ON UPDATE CASCADE
)

CREATE TABLE Answers(
	AnswerID INT IDENTITY,
	QQuestionID INT NOT NULL,
	UserID INT,
	AnswerText NVARCHAR(255),

	/*Constraints*/
	CONSTRAINT pk_Answers PRIMARY KEY(AnswerID),
	CONSTRAINT fk_Answers_QuestionnaireQuestions FOREIGN KEY (QQuestionID) references QuestionnaireQuestions(QQuestionID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT fk_Answers_Users FOREIGN KEY (UserID) references Users(UserID)
)

CREATE TABLE Options(
	OptionID INT IDENTITY,
	OptionText NVARCHAR(150) NOT NULL,
	QQuestionID INT NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_Options PRIMARY KEY(OptionID),
	CONSTRAINT fk_Options_QuestionnaireQuestions FOREIGN KEY (QQuestionID) references QuestionnaireQuestions(QQuestionID)
)

CREATE TABLE Choices(
	ChoiceID INT IDENTITY,
	AnswerID INT NOT NULL,
	OptionID INT NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_Choices PRIMARY KEY(ChoiceID, AnswerID, OptionID),
	CONSTRAINT fk_Choices_Answers FOREIGN KEY (AnswerID) references Answers(AnswerID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT fk_Choices_Options FOREIGN KEY (OptionID) references Options(OptionID) ON DELETE CASCADE ON UPDATE CASCADE
)


CREATE TABLE IdeaFields(
	FieldID INT IDENTITY,
	IdeaID INT NOT NULL,
	FieldText NVARCHAR(255),
	FieldStrings NVARCHAR(800),
	LocationX FLOAT,
	LocationY FLOAT,
	Url VARCHAR(50),
	UploadedImage VARBINARY(255),
	UploadedMedia VARBINARY(255),

	/*Constraints*/
	CONSTRAINT pk_IdeaFields PRIMARY KEY(FieldID,IdeaID),
	CONSTRAINT fk_IdeaFields_Ideas FOREIGN KEY (IdeaID) references Ideas(IdeaID) 
)


CREATE TABLE Votes(
	VoteID INT IDENTITY,
	DeviceID INT,
	InputID INT NOT NULL,
	UserID INT NOT NULL,
	InputType TINYINT NOT NULL,
	UserMail NVARCHAR(100),
	LocationX FLOAT,
	LocationY FLOAT,
	Choices NVARCHAR(255),

	/*Constraints*/
	CONSTRAINT pk_Votes PRIMARY KEY(VoteID),
	CONSTRAINT fk_Votes_Devices FOREIGN KEY (DeviceID) references Devices(DeviceID),
	CONSTRAINT fk_Votes_User FOREIGN KEY (UserID) references Users(UserID)
)

CREATE TABLE UserActivities(
	ActivityID INT IDENTITY,
	UserID INT NOT NULL,
	PlatformID INT NOT NULL,
	EventID INT,
	ProjectID INT,
	ModuleID INT,
	IQuestionID INT,
	IdeaID INT,
	VoteID INT,
	ActionDescription VARCHAR(150),

	/*Constraints*/
	CONSTRAINT pk_UserAcivities PRIMARY KEY(ActivityID),
	CONSTRAINT fk_UserActivities_Users FOREIGN KEY (UserID) references Users(UserID),
	CONSTRAINT fk_UserActivities_Platforms FOREIGN KEY (PlatformID) references Platforms(PlatformID),
	CONSTRAINT fk_UserActivities_Projects FOREIGN KEY (ProjectID) references Projects(ProjectID),
	CONSTRAINT fk_UserActivities_Modules FOREIGN KEY (ModuleID) references Modules(ModuleID),
	CONSTRAINT fk_UserActivities_IdeationQuestions FOREIGN KEY (IQuestionID) references IdeationQuestions(IQuestionID),
	CONSTRAINT fk_UserActivities_Ideas FOREIGN KEY (IdeaID) references Ideas(IdeaID),
	CONSTRAINT fk_UserActivities_Votes FOREIGN KEY (VoteID) references Votes(VoteID),
	CONSTRAINT fk_UserActivities_Events FOREIGN KEY (EventID) references OrganisationEvents(EventID)
)

CREATE TABLE Reports(
	ReportID       INT IDENTITY,
	IdeaID         INT NOT NULL,
	FlaggerID      INT NOT NULL,
	ReporteeID     INT NOT NULL,
	Reason         NVARCHAR(255),
	ReportApproved TINYINT NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_Reports PRIMARY KEY(ReportID),
	CONSTRAINT fk_Reports_Ideas FOREIGN KEY (IdeaID) references Ideas(IdeaID),
	CONSTRAINT fk_Reports_FlaggingUsers FOREIGN KEY (FlaggerID) references Users(UserID),
	CONSTRAINT fk_Reports_ReportedUsers FOREIGN KEY (ReporteeID) references Users(UserID)
)