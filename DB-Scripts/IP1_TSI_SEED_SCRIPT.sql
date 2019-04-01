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
VALUES('TSI_LoggedIn', 'niels.vanzandbergen@student.kdg.be','696969', 1, 1)

INSERT INTO dbo.users(name, email, password, role, platformID)
VALUES('TSI_BANNED', 'niels.vanzandbergen@student.kdg.be','696969', 1, 1)

INSERT INTO dbo.users(name, email, password, role, platformID)
VALUES('TSI_INACTIVE', 'niels.vanzandbergen@student.kdg.be','696969', 1, 1)

INSERT INTO dbo.users(name, email, password, role, platformID)
VALUES('TSI_LoggedInVerified', 'niels.vanzandbergen@student.kdg.be','696969', 2, 1)

INSERT INTO dbo.users(name, email, password, role, platformID)
VALUES('TSI_LoggedInOrganisation', 'niels.vanzandbergen@student.kdg.be','696969', 3, 1)

INSERT INTO dbo.users(name, email, password, role, platformID)
VALUES('TSI_Moderator', 'niels.vanzandbergen@student.kdg.be','696969', 4, 1)

INSERT INTO dbo.users(name, email, password, role, platformID)
VALUES('TSI_Admin', 'niels.vanzandbergen@student.kdg.be','696969', 5, 1)

INSERT INTO dbo.users(name, email, password, role, platformID)
VALUES('TSI_SuperAdmin', 'niels.vanzandbergen@student.kdg.be','696969', 6, 1)

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
Values(5,'9120',0,1,'The Spanish Inquisition', 'Een groep programmeurs dat klaar is voor de
wereld overnemen omdat niemand hen verwacht')

INSERT INTO dbo.userdetails(userID,zipcode,banned,gender,active,birthDate)
VALUES(6,'9120',0,0,1,'1997-09-08')

INSERT INTO dbo.userdetails(userID,zipcode,banned,gender,active,birthDate)
VALUES(7,'9120',0,0,1,'1997-09-08')

INSERT INTO dbo.userdetails(userID,zipcode,banned,gender,active,birthDate)
VALUES(8,'9120',0,0,1,'1997-09-08')

/* Organisationevents */
INSERT INTO dbo.organisationevents(userID, name, description, startDate, endDate)
VALUES(5,'The Inquisition','Een conferentie over hoe we Antwerpen kunnen introduceren tot de
Spaanse cultuur','2019-03-31','2019-04-1')

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
NOTE: Dit werkt atm ff niet. Sacha moet stuff updaten
*/
INSERT INTO dbo.projects(currentPhaseID,userID,platformID,title,goal,status,visible,likeVisibility)
VALUES(1,7,1,'GROENplaats','De Antwerpse Groenplaats terug groen maken','Niet gestart',1,0)

/* Phases 
Veranderingen door Sacha nodig.
*/

/* Modules
Veranderingen door Sacha nodig.
*/

/* QuestionnaireQuestions
Parent tables need updates atm, no inserts yet
*/

/* Answers
Parent tables need updates atm, no inserts yet
*/

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