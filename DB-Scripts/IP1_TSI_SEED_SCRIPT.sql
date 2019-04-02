/* Platform */
INSERT INTO dbo.platforms(name, siteUrl) 
VALUES('Stad Antwerpen', 'www.cityofideas.be')
/* Users 
0 = Anonymous
1 = loggedin
2 = loggedinverified
3 = loggedinorg
4 = moderator
5 = admin
6 = superadmin
*/
INSERT INTO dbo.users(name, email, password, role, platformID)
VALUES('TSI_LoggedIn', 'niels.vanzandbergen@student.kdg.be',CONVERT(binary,'696969'), 1, 1)

INSERT INTO dbo.users(name, email, password, role, platformID)
VALUES('TSI_BANNED', 'niels.vanzandbergen@student.kdg.be',CONVERT(binary,'696969'), 1, 1)

INSERT INTO dbo.users(name, email, password, role, platformID)
VALUES('TSI_INACTIVE', 'niels.vanzandbergen@student.kdg.be',CONVERT(binary,'696969'), 1, 1)

INSERT INTO dbo.users(name, email, password, role, platformID)
VALUES('TSI_LoggedInVerified', 'niels.vanzandbergen@student.kdg.be',CONVERT(binary,'696969'), 2, 1)

INSERT INTO dbo.users(name, email, password, role, platformID)
VALUES('TSI_LoggedInOrganisation', 'niels.vanzandbergen@student.kdg.be',CONVERT(binary,'696969'), 3, 1)

INSERT INTO dbo.users(name, email, password, role, platformID)
VALUES('TSI_Moderator', 'niels.vanzandbergen@student.kdg.be',CONVERT(binary,'696969'), 4, 1)

INSERT INTO dbo.users(name, email, password, role, platformID)
VALUES('TSI_Admin', 'niels.vanzandbergen@student.kdg.be',CONVERT(binary,'696969'), 5, 1)

INSERT INTO dbo.users(name, email, password, role, platformID)
VALUES('TSI_SuperAdmin', 'niels.vanzandbergen@student.kdg.be',CONVERT(binary,'696969'), 6, 1)

/* UserDetails 
BIT 0 = true | 1 = false 
*/
INSERT INTO dbo.userdetails(userID,zipcode,banned,gender,active,birthDate)
VALUES(1,'9120',0,0,1,'1997-09-08')

INSERT INTO dbo.userdetails(userID,zipcode,banned,gender,active,birthDate)
VALUES(2,'9120',1,1,0,'1997-09-08')

INSERT INTO dbo.userdetails(userID,zipcode,banned,gender,active,birthDate)
VALUES(3,'9120',0,2,0,'1997-09-08')

INSERT INTO dbo.userdetails(userID,zipcode,banned,gender,active,birthDate)
VALUES(4,'9120',0,0,1,'1997-09-08')

INSERT INTO dbo.userdetails(userID,zipcode,banned,active,orgName,description)
Values(5,'9120',0,1,'The Spanish Inquisition', 'Een groep programmeurs die klaar is om de wereld over te nemen omdat niemand hen verwacht')

INSERT INTO dbo.userdetails(userID,zipcode,banned,gender,active,birthDate)
VALUES(6,'9120',0,0,1,'1997-09-08')

INSERT INTO dbo.userdetails(userID,zipcode,banned,gender,active,birthDate)
VALUES(7,'9120',0,0,1,'1997-09-08')

INSERT INTO dbo.userdetails(userID,zipcode,banned,gender,active,birthDate)
VALUES(8,'9120',0,0,1,'1997-09-08')

/* Organisationevents */
INSERT INTO dbo.organisationevents(userID, name, description, startDate, endDate)
VALUES(5,'The Inquisition','Een conferentie over hoe we Antwerpen kunnen introduceren tot de Spaanse cultuur','2019-03-31','2019-04-1')

/* Projects 
Status: Niet gestart/Phase naam/Afgesloten
likeVisbility
0 = enkel likecount 
1 = enkel fblikecount
2 = enkel twitterlikecount
3 = likecount & fblikecount
4 = likecount & twitterlikecount
5 = fblikecount & twitterlikecount
6 = alle 3
*/
INSERT INTO dbo.projects(currentPhaseID,userID,platformID,title,goal,status,visible,likeVisibility)
VALUES(1,7,1,'GROENplaats','De Antwerpse Groenplaats terug groen maken','NIET GESTART',1,6)

/* Phases */
INSERT INTO dbo.phases(projectID, description, startDate, endDate)
VALUES(1,'Vergroenen van de Groenplaats','2019-03-10','2019-03-30')

INSERT INTO dbo.phases(projectID, description, startDate, endDate)
VALUES(1,'Gebruik van nieuwe groene ruimte','2019-04-01','2019-04-30')

/* Modules */
INSERT INTO dbo.modules(projectID, onGoing, tags, isQuestionnaire)
VALUES(1,1,'#Questionnaire,#ForTheClimate,#OpinionsAreImportant',1)

/* QuestionnaireQuestions 
0 = open                                                                                                                                                       
1 = single               
2 = multi                                                                                                                                                       
3 = drop                 
4 = mail
We still need required bit for the table, waiting for Sacha.
*/
INSERT INTO dbo.questionnairequestions(moduleID, questionTEXT, qType, required)
VALUES(1,'Waarom wil je de Groenplaats terug groen?',0,1)

INSERT INTO dbo.questionnairequestions(moduleID, questionTEXT, qType, required)
VALUES(1,'Welke aanpassing wil je het liefst?',1,1)

INSERT INTO dbo.questionnairequestions(moduleID, questionTEXT, qType, required)
VALUES(1,'Vind je dat er ook een bloementuin op de Groenplaats past?',2,1)

INSERT INTO dbo.questionnairequestions(moduleID, questionTEXT, qType, required)
VALUES(1,'Hoeveel m2 aan gras moet er aangelegd worden?',3,1)

INSERT INTO dbo.questionnairequestions(moduleID, questionTEXT, qType, required)
VALUES(1,'Gelieve u email adres achter te halen als u updates wilt over het project',4,0)

/* Answers */
INSERT INTO dbo.answers(qQuestionID, userID, answerText)
VALUES(1,1,'Een grijs stadshart is deprimerend.')

/* Options
Parent tables need updates atm, no inserts yet
*/

/* Choices
Parent tables need updates atm, no inserts yet
*/

/* Ideations  
Parent tables need updates atm, no inserts yet
*/

/* Ideationquestions 
Parent tables need updates atm, no inserts yet
*/

/* ideas
Parent tables need updates atm, no inserts yet
*/

/* ideafields
Parent tables need updates atm, no inserts yet
*/

/* votes
Parent tables need updates atm, no inserts yet
*/

/* useractivities
Parent tables need updates atm, no inserts yet
*/

/* Devices */
INSERT INTO dbo.devices(locationX, locationY) VALUES(55,55)