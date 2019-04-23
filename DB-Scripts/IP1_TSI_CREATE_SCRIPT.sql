/*         CREATE SCRIPT IP1 VERSIE 4          */

/*TABLES DROPPEN*/
DROP TABLE IF EXISTS ideations
DROP TABLE IF EXISTS answers
DROP TABLE IF EXISTS ideationquestions
DROP TABLE IF EXISTS ideas
DROP TABLE IF EXISTS userDetails
DROP TABLE IF EXISTS projectimages
DROP TABLE IF EXISTS modules
DROP TABLE IF EXISTS questionnairequestions
DROP TABLE IF EXISTS choices
DROP TABLE IF EXISTS options
DROP TABLE IF EXISTS ideafields
DROP TABLE IF EXISTS organisationevents
DROP TABLE IF EXISTS useractivities
DROP TABLE IF EXISTS devices
DROP TABLE IF EXISTS votes

DROP TABLE IF EXISTS phases
DROP TABLE IF EXISTS projects

DROP TABLE IF EXISTS users
DROP TABLE IF EXISTS platforms



/*TABELLEN AANMAKEN*/

CREATE TABLE platforms(
	platformID INT IDENTITY  PRIMARY KEY,
	name VARCHAR(100) NOT NULL,
	siteUrl VARCHAR(50) NOT NULL,
	iconImage VARBINARY(255)
	
)

CREATE TABLE users(
	userID INT IDENTITY  PRIMARY KEY,
	name VARCHAR(100),
	email VARCHAR(100),
	password BINARY(25),
	role TINYINT NOT NULL,
	platformID INT NOT NULL,
	/*Constraints*/
	CONSTRAINT fk_users_platforms FOREIGN KEY (platformID) references platforms(platformID)
	ON DELETE CASCADE
	ON UPDATE CASCADE
)

CREATE TABLE userdetails(
	userID INT PRIMARY KEY,
	zipcode VARCHAR(16) NOT NULL,
	banned BIT NOT NULL,
	gender TINYINT,
	active BIT NOT NULL,
	birthDate DATE,
	orgName VARCHAR(100),
	description VARCHAR(255),
	/*Constraints*/
	CONSTRAINT fk_userdetails_users FOREIGN KEY (userID) references users(userID)
	ON DELETE CASCADE
	ON UPDATE CASCADE
	
)

CREATE TABLE organisationevents(
	eventID INT IDENTITY PRIMARY KEY,
	userID INT NOT NULL,
	name VARCHAR(100) NOT NULL,
	description VARCHAR(255) NOT NULL,
	startDate DATE NOT NULL,
	endDate DATE NOT NULL,

	/*Constraints*/
	CONSTRAINT fk_userevents_users FOREIGN KEY (userID) references users(userID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT chk_userevents_startDate CHECK (startDate <= enddate),
	CONSTRAINT chk_userevents_endDate CHECK (endDate >= startDate)
)

CREATE TABLE projects(
	projectID INT IDENTITY PRIMARY KEY,
	currentPhaseID INT NOT NULL,
	userID INT NOT NULL,
	platformID INT NOT NULL,
	title VARCHAR(50) NOT NULL,
	goal VARCHAR(255) NOT NULL,
	status VARCHAR(25) NOT NULL,
	visible BIT NOT NULL,
	reactionCount INT,
	likeCount INT,
	fbLikeCount INT, 
	twitterLikeCOunt INT,
	likeVisibility TINYINT NOT NULL,

	/*Constraints*/
	CONSTRAINT fk_projects_platforms FOREIGN KEY (platformID) references platforms(platformID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT fk_projects_users FOREIGN KEY (userID) references users(userID) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT ck_projects_status CHECK (status = UPPER(status))

)

CREATE TABLE phases(
	phaseID INT IDENTITY PRIMARY KEY,
	projectID INT NOT NULL,
	description VARCHAR(255) NOT NULL,
	startDate DATE NOT NULL,
	endDate DATE NOT NULL,

	/*Constraints*/
	CONSTRAINT fk_phases_projects FOREIGN KEY (projectID) references projects(projectID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT chk_phases_startDate CHECK (startDate <= endDate),
	CONSTRAINT chk_phases_endDate CHECK (endDate >= startDate)
)

CREATE TABLE modules(
	moduleID INT IDENTITY PRIMARY KEY,
	projectID INT NOT NULL,
	phaseID INT NOT NULL,
	onGoing BIT NOT NULL,
	likeCount INT,
	fbLikeCount INT, 
	twitterLikeCOunt INT,
	shareCount INT,
	retweetCount INT,
	tags VARCHAR(255),
	isQuestionnaire BIT NOT NULL,

	/*Constraints*/
	CONSTRAINT fk_modules_projects FOREIGN KEY (projectID) references projects(projectID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT fk_modules_phases FOREIGN KEY (phaseID) references phases(phaseID) ON DELETE NO ACTION ON UPDATE NO ACTION
)

CREATE TABLE projectimages(
	projectID INT NOT NULL,
	imageID INT IDENTITY PRIMARY KEY,
	projectImage VARBINARY(255),

	/*Constraints*/
	CONSTRAINT fk_projectimages_projects FOREIGN KEY (projectID) references projects(projectID) ON DELETE CASCADE ON UPDATE CASCADE

)

CREATE TABLE devices(
	deviceID INT IDENTITY PRIMARY KEY,
	locationX FLOAT,
	locationY FLOAT
)

CREATE TABLE ideationquestions(
	iQuestionID INT IDENTITY PRIMARY KEY,
	moduleID INT NOT NULL,
	questionTitle VARCHAR(50) NOT NULL,
	description VARCHAR(255) NOT NULL,
	websiteLink VARCHAR(50) NOT NULL,
	deviceID INT,

	/*Constraints*/
	CONSTRAINT fk_ideationquestions_modules FOREIGN KEY (moduleID) references modules(moduleID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT fk_ideationquestions_devices FOREIGN KEY (deviceID) references devices(deviceID) ON DELETE CASCADE ON UPDATE CASCADE

)

CREATE TABLE ideas(
	ideaID INT IDENTITY PRIMARY KEY,
	iQuestionID INT NOT NULL,
	userID INT NOT NULL,
	reported BIT NOT NULL,
	reviewByAdmin BIT NOT NULL,
	visible BIT NOT NULL,
	voteCount INT,
	retweetCount INT,
	shareCount INT,
	title VARCHAR(100) NOT NULL,
	status VARCHAR(100),
	verifiedUser BIT NOT NULL,
	parentID INT,

	CONSTRAINT fk_ideas_ideationquestions FOREIGN KEY (iQuestionID) references ideationquestions(iQuestionID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT fk_ideas_users FOREIGN KEY (userID) references users(userID),
	CONSTRAINT fk_ideas_ideas FOREIGN KEY (parentID) references ideas(ideaID)
)


CREATE TABLE ideations (
	moduleID INT PRIMARY KEY,
	userID INT NOT NULL,
	organisation BIT NOT NULL,
	eventID INT,
	userIdea BIT NOT NULL,
	mediaFile VARBINARY(255),
	requiredFields tinyint NOT NULL,
	extraInfo VARCHAR(100),

	CONSTRAINT fk_ideations_module FOREIGN KEY (moduleID) references modules(moduleID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT fk_ideations_users FOREIGN KEY (userID) references users(userID)



)

CREATE TABLE questionnairequestions(
	qQuestionID INT IDENTITY PRIMARY KEY,
	moduleID INT NOT NULL,
	questionText VARCHAR(100) NOT NULL,
	qType BIT NOT NULL,
	required BIT NOT NULL,

	CONSTRAINT fk_questionnairequestions_modules FOREIGN KEY (moduleID) references modules(moduleID) ON DELETE CASCADE ON UPDATE CASCADE



)

CREATE TABLE answers(
	answerID INT IDENTITY PRIMARY KEY,
	qQuestionID INT NOT NULL,
	userID INT,
	answerText VARCHAR(255),

	CONSTRAINT fk_answers_questionnairequestions FOREIGN KEY (qQuestionID) references questionnairequestions(qQuestionID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT fk_answers_users FOREIGN KEY (userID) references users(userID)
	
)

CREATE TABLE options(
	optionID INT IDENTITY PRIMARY KEY,
	optionText VARCHAR(150) NOT NULL,
	qQuestionID INT NOT NULL,

	CONSTRAINT fk_options_questionnairequestions FOREIGN KEY (qQuestionID) references questionnairequestions(qQuestionID)
)

CREATE TABLE choices(
	choiceID INT IDENTITY PRIMARY KEY,
	answerID INT NOT NULL,
	optionID INT NOT NULL,

	CONSTRAINT fk_choices_answers FOREIGN KEY (answerID) references answers(answerID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT fk_choices_options FOREIGN KEY (optionID) references options(optionID) ON DELETE CASCADE ON UPDATE CASCADE
	
)


CREATE TABLE ideafields(
	fieldID INT IDENTITY PRIMARY KEY,
	ideaID INT NOT NULL,
	fieldText VARCHAR(255),
	fieldStrings varchar(800),
	locationX FLOAT,
	locationY FLOAT,
	searchable BIT,
	url VARCHAR(50),
	uploadedImage VARBINARY(255),
	uploadedMedia VARBINARY(255),

	CONSTRAINT fk_ideafields_ideas FOREIGN KEY (ideaID) references ideas(ideaID) 

)


CREATE TABLE votes(
	voteID INT IDENTITY PRIMARY KEY,
	deviceID INT,
	inputID INT NOT NULL,
	userID INT NOT NULL,
	inputType TINYINT NOT NULL,
	userMail VARCHAR(100),
	locationX FLOAT,
	locationY FLOAT,
	choices VARCHAR(255),

	CONSTRAINT fk_votes_devices FOREIGN KEY (deviceID) references devices(deviceID),
	CONSTRAINT fk_votes_user FOREIGN KEY (userID) references users(userID)
)

CREATE TABLE useractivities(
	activityID INT IDENTITY PRIMARY KEY,
	userID INT NOT NULL,
	platformID INT NOT NULL,
	eventID INT,
	projectID INT,
	phaseID INT,
	moduleID INT,
	iQuestionID INT,
	ideaID INT,
	voteID INT,
	actionDescription VARCHAR(150),

	/*Constraints*/
	CONSTRAINT fk_useractivities_users FOREIGN KEY (userID) references users(userID),
	CONSTRAINT fk_useractivities_platforms FOREIGN KEY (platformID) references platforms(platformID),
	CONSTRAINT fk_useractivities_projects FOREIGN KEY (projectID) references projects(projectID),
	CONSTRAINT fk_useractivities_modules FOREIGN KEY (moduleID) references modules(moduleID),
	CONSTRAINT fk_useractivities_ideationquestions FOREIGN KEY (iQuestionID) references ideationquestions(iQuestionID),
	CONSTRAINT fk_useractivities_ideas FOREIGN KEY (ideaID) references ideas(ideaID),
	CONSTRAINT fk_useractivities_votes FOREIGN KEY (voteID) references votes(voteID),
	CONSTRAINT fk_useractivities_events FOREIGN KEY (eventID) references organisationevents(eventID),
	CONSTRAINT fk_useractivities_phase FOREIGN KEY (phaseID) references phases(phaseID)
)




