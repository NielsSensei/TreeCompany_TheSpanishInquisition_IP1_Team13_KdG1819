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
INSERT INTO dbo.modules(projectID, phaseID, onGoing, tags, isQuestionnaire)
VALUES(1,1,1,'#Questionnaire,#ForTheClimate,#OpinionsAreImportant',1)

INSERT INTO dbo.modules(projectID, phaseID, onGoing, tags, isQuestionnaire)
VALUES(1,2,1,'#CreateIdeas,#ForTheClimate,#NoIdeaIsStupid',0)

/* QuestionnaireQuestions 
0 = open                                                                                                                                                       
1 = single               
2 = multi                                                                                                                                                       
3 = drop                 
4 = mail
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

INSERT INTO dbo.answers(qQuestionID, userID)
VALUES(2,1)

INSERT INTO dbo.answers(qQuestionID, userID)
VALUES(3,1)

INSERT INTO dbo.answers(qQuestionID, userID)
VALUES(4,1)

INSERT INTO dbo.answers(qQuestionID, userID, answerText)
VALUES(5,1,'voorbeeldigeantwerpenaar@nva.be')

/* Options */
INSERT INTO dbo.options(optionText,qQuestionID)
VALUES('Geen tram 4 meer op de groenplaats.',2)

INSERT INTO dbo.options(optionText,qQuestionID)
VALUES('Verkeer afsluiten op de groenplaats.',2)

INSERT INTO dbo.options(optionText,qQuestionID)
VALUES('De groenplaats vervangen door klein bos.',2)

INSERT INTO dbo.options(optionText, qQuestionID)
VALUES('Ja',3)

INSERT INTO dbo.options(optionText, qQuestionID)
VALUES('Nee',3)

INSERT INTO dbo.options(optionText, qQuestionID)
VALUES('10m²',4)

INSERT INTO dbo.options(optionText, qQuestionID)
VALUES('20m²',4)

INSERT INTO dbo.options(optionText, qQuestionID)
VALUES('30m²',4)

INSERT INTO dbo.options(optionText, qQuestionID)
VALUES('40m²',4)

/* Choices */
INSERT INTO dbo.choices(answerID, optionID)
VALUES(2,1)

INSERT INTO dbo.choices(answerID, optionID)
VALUES(3,4)

INSERT INTO dbo.choices(answerID, optionID)
VALUES(4,6)

INSERT INTO dbo.choices(answerID, optionID)
VALUES(4,9)

/* Ideations */
INSERT INTO dbo.ideations(moduleID, userID, organisation, userIdea, requiredFields)
VALUES(2,7,0,1,41)

/* Ideationquestions */
INSERT INTO dbo.ideationquestions(moduleID, questionTitle, description, websiteLink)
VALUES(1,'Hoe maken we de Groenplaats groener?','Sinds 1990 is de Groenplaats niet meer groen zoals je kan zien via de link, dit is zeer jammer.','voorbeeldlink.be')

/* ideas */
INSERT INTO dbo.ideas(iQuestionID, userID, reported, reviewByAdmin, visible, title, status, verifiedUser)
VALUES(1,1,0,0,1,'#MakeGroenplaatsGreenAgain','NIET GESELECTEERD',0)

INSERT INTO dbo.ideas(iQuestionID, userID, reported, reviewByAdmin, visible, title, status, verifiedUser)
VALUES(1,1,0,0,1,'Groenplaats Stadspark','NIET GESELECTEERD',0)

INSERT INTO dbo.ideas(iQuestionID, userID, reported, reviewByAdmin, visible, title, status, verifiedUser)
VALUES(1,1,0,0,1,'Theater','NIET GESELECTEERD',0)

INSERT INTO dbo.ideas(iQuestionID, userID, reported, reviewByAdmin, visible, title, status, verifiedUser)
VALUES(1,1,0,0,1,'Cinema','NIET GESELECTEERD',0)

/* ideafields */
INSERT INTO dbo.ideafields(ideaID, fieldText)
VALUES(1,'We maken een grote haag van bomen en struiken rond de Groenplaats om de grijze beton erbuiten te houden!')

INSERT INTO dbo.ideafields(ideaID, fieldText)
VALUES(2,'Maken een aantal graspleintjes en bloembakken aan met stenen wandelpaden en een pleintje in het midden rond het standbeeld :)')

INSERT INTO dbo.ideafields(ideaID, fieldStrings)
VALUES(2,'I see a gray square and I want to ' + 'paint' + ' it green - Rolling Stoned')

INSERT INTO dbo.ideafields(ideaID, fieldText)
VALUES(3, 'Een locatie zo nabij het oude centrum moet evenveel cultuur hebben als het centrum zelf. Dus stel ik voor om hier regelmatige theater voorstelling te houden, zodat we de jongeren echte cultuur kunnen aanleren.')

INSERT INTO dbo.ideafields(ideaID, fieldText)
VALUES(4,'Nope, dom idee. Wij wille gewoon goeie films kunne zien, buiten op de Groenplaats. Ff pintje op café, laatste nieve film om middernacht opt gras buite. Der woont tog niemand, dus ook geen lawaaid overlast.')

/* Devices */
INSERT INTO dbo.devices(locationX, locationY) VALUES(55,55)

/* votes */
INSERT INTO dbo.votes(deviceID, inputID, userID, inputType, userMail, choices)
VALUES(1,2,1,2,'niels.vanzandbergen@student.kdg.be','Yes')

INSERT INTO dbo.votes(deviceID, inputID, userID, inputType, userMail, choices)
VALUES(1,2,3,2,'niels.vanzandbergen@student.kdg.be','No')

INSERT INTO dbo.votes(deviceID, inputID, userID, inputType, userMail, choices)
VALUES(1,2,6,2,'niels.vanzandbergen@student.kdg.be','Yes')

/* useractivities 
Note: de bedoeling van de keywords hier is dat ze vervangen worden door obj.
*/
INSERT INTO dbo.useractivities(userID, platformID, eventID, actionDescription)
VALUES(5,1,1,'username heeft een event georganiseerd.')

INSERT INTO dbo.useractivities(userID, platformID, projectID, actionDescription)
VALUES(7,1,1, 'platform heeft een nieuw project geintroduceerd.')

INSERT INTO dbo.useractivities(userID, platformID, projectID, phaseID, actionDescription)
VALUES(7,1,1,2,'Nieuwe phase gestart binnen project.')

INSERT INTO dbo.useractivities(userID, platformID, projectID, moduleID, actionDescription)
VALUES(7,1,1,1,'Nieuwe questionnaire toegevoegd aan project.')

INSERT INTO dbo.useractivities(userID, platformID, projectID, moduleID, actionDescription)
VALUES(7,1,1,2,'Nieuwe ideation toegevoegd aan project.')

INSERT INTO dbo.useractivities(userID, platformID, projectID, moduleID, iQuestionID, actionDescription)
VALUES(7,1,1,2,1,'Nieuwe discussie gestart binnen ideation.')

INSERT INTO dbo.useractivities(userID, platformID, projectID, moduleID, iQuestionID, ideaID, actionDescription)
VALUES(1,1,1,2,1,1,'User heeft een nieuw idee gegeven binnen een ideation.')

INSERT INTO dbo.useractivities(userID, platformID, projectID, moduleID, iQuestionID, ideaID, actionDescription)
VALUES(1,1,1,2,1,2,'User heeft een nieuw idee gegeven binnen een ideation.')

INSERT INTO dbo.useractivities(userID, platformID, projectID, moduleID, iQuestionID, ideaID, actionDescription)
VALUES(1,1,1,2,1,3,'User heeft een nieuw idee gegeven binnen een ideation.')

INSERT INTO dbo.useractivities(userID, platformID, projectID, moduleID, iQuestionID, ideaID, actionDescription)
VALUES(1,1,1,2,1,4,'User heeft een nieuw idee gegeven binnen een ideation.')

INSERT INTO dbo.useractivities(userID, platformID, projectID, moduleID, iQuestionID, ideaID, voteID, actionDescription)
VALUES(1,1,1,2,1,2,1,'User heeft gestemd op een idee.')

INSERT INTO dbo.useractivities(userID, platformID, projectID, moduleID, iQuestionID, ideaID, voteID, actionDescription)
VALUES(3,1,1,2,1,2,1,'User heeft gestemd op een idee.')

INSERT INTO dbo.useractivities(userID, platformID, projectID, moduleID, iQuestionID, ideaID, voteID, actionDescription)
VALUES(6,1,1,2,1,2,1,'User heeft gestemd op een idee.')