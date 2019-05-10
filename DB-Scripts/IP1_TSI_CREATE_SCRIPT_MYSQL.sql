/*         CREATE SCRIPT IP1 VERSIE 1.MySQL          */

/*TABELLEN AANMAKEN*/

CREATE TABLE Platforms(
	PlatformID INT AUTO_INCREMENT,
	Name NVARCHAR(100) NOT NULL,
	SiteUrl VARCHAR(50) NOT NULL,
	IconImage VARBINARY(255),

	/*Constraints*/
	CONSTRAINT pk_Plaftorms PRIMARY KEY(PlatformID)
)
;

CREATE TABLE OrganisationEvents(
	EventID INT AUTO_INCREMENT,
	UserID NVARCHAR(255) NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	Description NVARCHAR(255) NOT NULL,
	StartDate DATE NOT NULL CHECK (StartDate <= Enddate),
	EndDate DATE NOT NULL CHECK (EndDate >= StartDate),

	/*Constraints*/
	CONSTRAINT pk_OrganisationEvents PRIMARY KEY(EventID)
)
;

CREATE TABLE Projects(
	ProjectID INT AUTO_INCREMENT,
	CurrentPhaseID INT NOT NULL,
	UserID NVARCHAR(255) NOT NULL,
	PlatformID INT NOT NULL,
	Title NVARCHAR(50) NOT NULL,
	Goal NVARCHAR(255) NOT NULL,
	Status VARCHAR(25) NOT NULL CHECK (Status = UPPER(Status)),
	StartDate DATE,
	EndDate DATE,
	Visible BOOL NOT NULL,
	ReactionCount INT,
	LikeCount INT,
	FbLikeCount INT, 
	TwitterLikeCOunt INT,
	LikeVisibility TINYINT NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_Projects PRIMARY KEY(ProjectID)
)
;

CREATE TABLE Phases(
	PhaseID INT AUTO_INCREMENT,
	ProjectID INT NOT NULL,
	Description NVARCHAR(255) NOT NULL,
	StartDate DATE NOT NULL CHECK (StartDate <= EndDate),
	EndDate DATE NOT NULL CHECK (EndDate >= StartDate),
	IsCurrent BOOL NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_Phases PRIMARY KEY(PhaseID)
)
;

CREATE TABLE Modules(
	ModuleID INT AUTO_INCREMENT,
	ProjectID INT NOT NULL,
	PhaseID INT NOT NULL,
	OnGoing BOOL NOT NULL,
	Title NVARCHAR(100),
	LikeCount INT,
	FbLikeCount INT, 
	TwitterLikeCount INT,
	ShareCount INT,
	RetweetCount INT,
	Tags VARCHAR(255),
	IsQuestionnaire BOOL NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_Modules PRIMARY KEY(ModuleID)
)
;

CREATE TABLE Projectimages(
	ProjectID INT NOT NULL,
	ImageID TINYINT AUTO_INCREMENT,
	ProjectImage VARBINARY(255),

	/*Constraints*/
	CONSTRAINT pk_ProjectImages PRIMARY KEY(ProjectID, ImageID)
)
;

CREATE TABLE Devices(
	DeviceID INT AUTO_INCREMENT,
	LocationX FLOAT,
	LocationY FLOAT,

	/*Constraints*/
	CONSTRAINT pk_Devices PRIMARY KEY(DeviceID)
)
;

CREATE TABLE IdeationQuestions(
	IQuestionID INT AUTO_INCREMENT,
	ModuleID INT NOT NULL,
	QuestionTitle NVARCHAR(50) NOT NULL,
	Description NVARCHAR(255) NOT NULL,
	WebsiteLink NVARCHAR(50) NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_IdeationQuestions PRIMARY KEY(IQuestionID)
)
;

CREATE TABLE Ideas(
	IdeaID INT AUTO_INCREMENT,
	IQuestionID INT NOT NULL,
	UserID NVARCHAR(255) NOT NULL,
	Reported BOOL NOT NULL,
	ReviewByAdmin BOOL NOT NULL,
	Visible BOOL NOT NULL,
	VoteCount INT,
	RetweetCount INT,
	ShareCount INT,
	Title NVARCHAR(100) NOT NULL,
	Status VARCHAR(100),
	VerifiedUser BOOL NOT NULL,
	IsDeleted BOOL NOT NULL,
	ParentID INT,
	DeviceID INT,

	/*Constraints*/
	CONSTRAINT pk_Ideas PRIMARY KEY(IdeaID)
)
;


CREATE TABLE Ideations (
	ModuleID INT NOT NULL,
	UserID NVARCHAR(255) NOT NULL,
	Organisation BOOL NOT NULL,
	EventID INT,
	UserIdea BOOL NOT NULL,
	MediaFile BINARY(255),
	RequiredFields tinyint NOT NULL,
	ExtraInfo NVARCHAR(100),

	/*Constraints*/
	CONSTRAINT pk_Ideations PRIMARY KEY(ModuleID)
);

CREATE TABLE QuestionnaireQuestions(
	QQuestionID INT AUTO_INCREMENT,
	ModuleID INT NOT NULL,
	QuestionText NVARCHAR(100) NOT NULL,
	QType BOOL NOT NULL,
	Required BOOL NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_QuestionnaireQuestions PRIMARY KEY(QQuestionID)
)
;

CREATE TABLE Answers(
	AnswerID INT AUTO_INCREMENT
	QQuestionID INT NOT NULL,
	UserID NVARCHAR(255),
	AnswerText NVARCHAR(255),

	/*Constraints*/
	CONSTRAINT pk_Answers PRIMARY KEY(AnswerID)
);

CREATE TABLE Options(
	OptionID INT AUTO_INCREMENT,
	OptionText NVARCHAR(150) NOT NULL,
	QQuestionID INT NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_Options PRIMARY KEY(OptionID)
);

CREATE TABLE Choices(
	ChoiceID INT AUTO_INCREMENT,
	AnswerID INT NOT NULL,
	OptionID INT NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_Choices PRIMARY KEY(ChoiceID, AnswerID, OptionID)
)
;


CREATE TABLE IdeaFields(
	FieldID INT AUTO_INCREMENT,
	IdeaID INT NOT NULL,
	FieldText NVARCHAR(255),
	FieldStrings NVARCHAR(800),
	LocationX FLOAT,
	LocationY FLOAT,
	Url VARCHAR(50),
	UploadedImage VARBINARY(255),
	UploadedMedia VARBINARY(255),

	/*Constraints*/
	CONSTRAINT pk_IdeaFields PRIMARY KEY(FieldID,IdeaID)
)
;


CREATE TABLE Votes(
	VoteID INT AUTO_INCREMENT,
	DeviceID INT,
	InputID INT NOT NULL,
	UserID NVARCHAR(255) NOT NULL,
	InputType TINYINT NOT NULL,
	UserMail NVARCHAR(100),
	LocationX FLOAT,
	LocationY FLOAT,
	Choices NVARCHAR(255),

	/*Constraints*/
	CONSTRAINT pk_Votes PRIMARY KEY(VoteID)
)
;

CREATE TABLE UserActivities(
	ActivityID INT AUTO_INCREMENT,
	UserID NVARCHAR(255) NOT NULL,
	PlatformID INT NOT NULL,
	EventID INT,
	ProjectID INT,
	ModuleID INT,
	IQuestionID INT,
	IdeaID INT,
	VoteID INT,
	ActionDescription VARCHAR(150),

	/*Constraints*/
	CONSTRAINT pk_UserAcivities PRIMARY KEY(ActivityID)
)
;

CREATE TABLE Reports(
	ReportID       INT AUTO_INCREMENT,
	IdeaID         INT NOT NULL,
	FlaggerID      NVARCHAR(255) NOT NULL,
	ReporteeID     NVARCHAR(255) NOT NULL,
	Reason         NVARCHAR(255),
	ReportApproved TINYINT NOT NULL,

	/*Constraints*/
	CONSTRAINT pk_Reports PRIMARY KEY(ReportID)
)
;

create table AspNetRoles
(
	Id TEXT not null
		constraint PK_AspNetRoles
			primary key,
	Name TEXT,
	NormalizedName TEXT,
	ConcurrencyStamp TEXT
)
;

create table AspNetRoleClaims
(
	Id INTEGER not null
		constraint PK_AspNetRoleClaims
			primary key
			 autoincrement,
	RoleId TEXT not null
		constraint FK_AspNetRoleClaims_AspNetRoles_RoleId
			references AspNetRoles
				on delete cascade,
	ClaimType TEXT,
	ClaimValue TEXT
)
;

create index IX_AspNetRoleClaims_RoleId
	on AspNetRoleClaims (RoleId)
;

create unique index RoleNameIndex
	on AspNetRoles (NormalizedName)
;

create table AspNetUsers
(
	Id TEXT not null
		constraint PK_AspNetUsers
			primary key,
	UserName TEXT,
	NormalizedUserName TEXT,
	Email TEXT,
	NormalizedEmail TEXT,
	EmailConfirmed INTEGER not null,
	PasswordHash TEXT,
	SecurityStamp TEXT,
	ConcurrencyStamp TEXT,
	PhoneNumber TEXT,
	PhoneNumberConfirmed INTEGER not null,
	TwoFactorEnabled INTEGER not null,
	LockoutEnd TEXT,
	LockoutEnabled INTEGER not null,
	AccessFailedCount INTEGER not null,
	Name TEXT,
	Zipcode TEXT,
	Gender INTEGER not null,
	DateOfBirth TEXT not null,
	PlatformDetails INTEGER not null,
	OrgName TEXT,
	Description TEXT,
	Banned INTEGER not null,
	Active INTEGER not null
)
;

create table AspNetUserClaims
(
	Id INTEGER not null
		constraint PK_AspNetUserClaims
			primary key
			 autoincrement,
	UserId TEXT not null
		constraint FK_AspNetUserClaims_AspNetUsers_UserId
			references AspNetUsers
				on delete cascade,
	ClaimType TEXT,
	ClaimValue TEXT
)
;

create index IX_AspNetUserClaims_UserId
	on AspNetUserClaims (UserId)
;

create table AspNetUserLogins
(
	LoginProvider TEXT not null,
	ProviderKey TEXT not null,
	ProviderDisplayName TEXT,
	UserId TEXT not null
		constraint FK_AspNetUserLogins_AspNetUsers_UserId
			references AspNetUsers
				on delete cascade,
	constraint PK_AspNetUserLogins
		primary key (LoginProvider, ProviderKey)
)
;

create index IX_AspNetUserLogins_UserId
	on AspNetUserLogins (UserId)
;

create table AspNetUserRoles
(
	UserId TEXT not null
		constraint FK_AspNetUserRoles_AspNetUsers_UserId
			references AspNetUsers
				on delete cascade,
	RoleId TEXT not null
		constraint FK_AspNetUserRoles_AspNetRoles_RoleId
			references AspNetRoles
				on delete cascade,
	constraint PK_AspNetUserRoles
		primary key (UserId, RoleId)
)
;

create index IX_AspNetUserRoles_RoleId
	on AspNetUserRoles (RoleId)
;

create table AspNetUserTokens
(
	UserId TEXT not null
		constraint FK_AspNetUserTokens_AspNetUsers_UserId
			references AspNetUsers
				on delete cascade,
	LoginProvider TEXT not null,
	Name TEXT not null,
	Value TEXT,
	constraint PK_AspNetUserTokens
		primary key (UserId, LoginProvider, Name)
)
;

create index EmailIndex
	on AspNetUsers (NormalizedEmail)
;

create unique index UserNameIndex
	on AspNetUsers (NormalizedUserName)
;