/* Platform */
INSERT INTO Platforms(PlatformID,Name,SiteUrl) 
VALUES(1,'Stad Antwerpen','www.cityofideas.be')
/* Users 
0 = Anonymous
1 = loggedin
2 = loggedinverified
3 = loggedinorg
4 = moderator
5 = admin
6 = superadmin

INSERT INTO Users(UserID,Name,Email,Role,PlatformID)
VALUES(1,'TSI_LoggedIn','niels.vanzandbergen@student.kdg.be',1,1)

INSERT INTO Users(UserID,Name,Email,Role,PlatformID)
VALUES(2,'TSI_BANNED','niels.vanzandbergen@student.kdg.be',1,1)

INSERT INTO Users(UserID,Name,Email,Role,PlatformID)
VALUES(3,'TSI_INACTIVE','niels.vanzandbergen@student.kdg.be',1,1)

INSERT INTO Users(UserID,Name,Email,Role,PlatformID)
VALUES(4,'TSI_LoggedInVerified','niels.vanzandbergen@student.kdg.be',2,1)

INSERT INTO Users(UserID,Name,Email,Role,PlatformID)
VALUES(5,'TSI_LoggedInOrganisation','niels.vanzandbergen@student.kdg.be',3,1)

INSERT INTO Users(UserID,Name,Email,Role,PlatformID)
VALUES(6,'TSI_Moderator','niels.vanzandbergen@student.kdg.be',4,1)

INSERT INTO Users(UserID,Name,Email,Role,PlatformID)
VALUES(7,'TSI_Admin','niels.vanzandbergen@student.kdg.be',5,1)

INSERT INTO Users(UserID,Name,Email,Role,PlatformID)
VALUES(8,'TSI_SuperAdmin','niels.vanzandbergen@student.kdg.be',6,1)
*/

/* UserDetails 
BIT 0 = true | 1 = false 

INSERT INTO UserDetails(UserID,Zipcode,Banned,Gender,Active,BirthDate)
VALUES(1,'9120',0,1,1,'1997-09-08')

INSERT INTO UserDetails(UserID,Zipcode,Banned,Gender,Active,BirthDate)
VALUES(2,'9120',1,2,0,'1997-09-08')

INSERT INTO UserDetails(UserID,Zipcode,Banned,Gender,Active,BirthDate)
VALUES(3,'9120',0,3,0,'1997-09-08')

INSERT INTO UserDetails(UserID,Zipcode,Banned,Gender,Active,BirthDate)
VALUES(4,'9120',0,1,1,'1997-09-08')

INSERT INTO UserDetails(UserID,Zipcode,Banned,Gender,Active,BirthDate,OrgName,Description)
Values(5,'9120',0,0,1,'30-01-2019','The Spanish Inquisition','Een groep programmeurs die klaar is om de wereld over te nemen omdat niemand hen verwacht')

INSERT INTO UserDetails(UserID,Zipcode,Banned,Gender,Active,BirthDate)
VALUES(6,'9120',0,1,1,'1997-09-08')

INSERT INTO UserDetails(UserID,Zipcode,Banned,Gender,Active,BirthDate)
VALUES(7,'9120',0,1,1,'1997-09-08')

INSERT INTO UserDetails(UserID,Zipcode,Banned,Gender,Active,BirthDate)
VALUES(8,'9120',0,1,1,'1997-09-08')
*/

/* Organisationevents */
INSERT INTO OrganisationEvents(EventID,UserID,Name,Description,StartDate,EndDate)
VALUES(1,5,'The Inquisition','Een conferentie over hoe we Antwerpen kunnen introduceren tot de Spaanse cultuur','2019-03-31','2019-04-1')

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
INSERT INTO Projects(ProjectID,CurrentPhaseID,UserID,PlatformID,Title,Goal,Status,Visible,LikeVisibility,ReactionCount,LikeCount,FbLikeCount,TwitterLikeCOunt)
VALUES(1,1,7,1,'GROENplaats','De Antwerpse Groenplaats terug groen maken','NIET GESTART',1,6,0,0,0,0)

/* Phases */
INSERT INTO Phases(PhaseID,ProjectID,Description,StartDate,EndDate)
VALUES(1,1,'Vergroenen van de Groenplaats','2019-03-10','2019-03-30')

INSERT INTO Phases(PhaseID,ProjectID,Description,StartDate,EndDate)
VALUES(2,1,'Gebruik van nieuwe groene ruimte','2019-04-01','2019-04-30')

INSERT INTO Phases(PhaseID,ProjectID,Description,StartDate,EndDate)
VALUES(3,1,'Uitzoeken welke plaatsen uit de stadskern nog Groener kunnen','2019-05-01','2019-05-31')

INSERT INTO Phases(PhaseID,ProjectID,Description,StartDate,EndDate)
VALUES(4,1,'Begin van de vergroening van district Merksem','2019-06-01','2019-06-30')

INSERT INTO Phases(PhaseID,ProjectID,Description,StartDate,EndDate)
VALUES(5,1,'Begin van de vergroening van district Deurne','2019-07-01','2019-07-31')

/* Modules */
INSERT INTO Modules(ModuleID,ProjectID,PhaseID,OnGoing,Tags,IsQuestionnaire,LikeCount,FbLikeCount,TwitterLikeCount,ShareCount,RetweetCount,Title)
VALUES(1,1,1,1,'#Questionnaire,#ForTheClimate,#OpinionsAreImportant',1,0,0,0,0,0,'Hoe pakken we de Groenplaats aan, onze gedachten')

INSERT INTO Modules(ModuleID,ProjectID,PhaseID,OnGoing,Tags,IsQuestionnaire,LikeCount,FbLikeCount,TwitterLikeCount,ShareCount,RetweetCount,Title)
VALUES(2,1,2,1,'#CreateIdeas,#ForTheClimate,#NoIdeaIsStupid',0,0,0,0,0,0,'Geef uw alternatief!')

/* QuestionnaireQuestions 
0 = open                                                                                                                                                       
1 = single               
2 = multi                                                                                                                                                       
3 = drop                 
4 = mail
*/
INSERT INTO QuestionnaireQuestions(QQuestionID,ModuleID,QuestionTEXT,QType,Required)
VALUES(1,1,'Waarom wil je de Groenplaats terug groen?',0,1)

INSERT INTO QuestionnaireQuestions(QQuestionID,ModuleID,QuestionTEXT,QType,Required)
VALUES(2,1,'Welke aanpassing wil je het liefst?',1,1)

INSERT INTO QuestionnaireQuestions(QQuestionID,ModuleID,QuestionTEXT,QType,Required)
VALUES(3,1,'Vind je dat er ook een bloementuin op de Groenplaats past?',2,1)

INSERT INTO QuestionnaireQuestions(QQuestionID,ModuleID,QuestionTEXT,QType,Required)
VALUES(4,1,'Hoeveel m2 aan gras moet er aangelegd worden?',3,1)

INSERT INTO QuestionnaireQuestions(QQuestionID,ModuleID,QuestionTEXT,QType,Required)
VALUES(5,1,'Gelieve u email adres achter te halen als u updates wilt over het project',4,0)

/* Answers */
INSERT INTO Answers(AnswerID,QQuestionID,UserID,AnswerText)
VALUES(1,1,1,'Een grijs stadshart is deprimerend.')

INSERT INTO Answers(AnswerID,QQuestionID,UserID)
VALUES(2,2,1)

INSERT INTO Answers(AnswerID,QQuestionID,UserID)
VALUES(3,3,1)

INSERT INTO Answers(AnswerID,QQuestionID,UserID)
VALUES(4,4,1)

INSERT INTO Answers(AnswerID,QQuestionID,UserID,AnswerText)
VALUES(5,5,1,'voorbeeldigeantwerpenaar@nva.be')

/* Options */
INSERT INTO Options(OptionID,OptionText,QQuestionID)
VALUES(1,'Geen tram 4 meer op de groenplaats.',2)

INSERT INTO Options(OptionID,OptionText,QQuestionID)
VALUES(2,'Verkeer afsluiten op de groenplaats.',2)

INSERT INTO Options(OptionID,OptionText,QQuestionID)
VALUES(3,'De groenplaats vervangen door klein bos.',2)

INSERT INTO Options(OptionID,OptionText,QQuestionID)
VALUES(4,'Ja',3)

INSERT INTO Options(OptionID,OptionText,QQuestionID)
VALUES(5,'Nee',3)

INSERT INTO Options(OptionID,OptionText,QQuestionID)
VALUES(6,'10m�',4)

INSERT INTO Options(OptionID,OptionText,QQuestionID)
VALUES(7,'20m�',4)

INSERT INTO Options(OptionID,OptionText,QQuestionID)
VALUES(8,'30m�',4)

INSERT INTO Options(OptionID,OptionText,QQuestionID)
VALUES(9,'40m�',4)

/* Choices */
INSERT INTO Choices(ChoiceID,AnswerID,OptionID)
VALUES(1,2,1)

INSERT INTO Choices(ChoiceID,AnswerID,OptionID)
VALUES(2,3,4)

INSERT INTO Choices(ChoiceID,AnswerID,OptionID)
VALUES(3,4,6)

INSERT INTO Choices(ChoiceID,AnswerID,OptionID)
VALUES(4,4,9)

/* Ideations */
INSERT INTO Ideations(ModuleID,UserID,Organisation,UserIdea,RequiredFields,EventID,extraInfo)
VALUES(2,7,0,1,41,0,'We hebben input nodig van de lokale Antwerpenaars over de Groenplaats. We gaan ook verschillende insteken proberen te geven zodat elke user wel iets te zeggen heeft.')

/* Ideationquestions */
INSERT INTO IdeationQuestions(IQuestionID,ModuleID,QuestionTitle,Description,WebsiteLink)
VALUES(1,2,'Hoe maken we de Groenplaats groener?','Sinds 1990 is de Groenplaats niet meer groen zoals je kan zien via de link, dit is zeer jammer.','voorbeeldlink.be')

/* ideas */
INSERT INTO Ideas(IdeaID,IQuestionID,UserID,Reported,ReviewByAdmin,Visible,Title,Status,VerifiedUser,VoteCount,RetweetCount,ShareCount,ParentID,DeviceID)
VALUES(1,1,1,0,0,1,'#MakeGroenplaatsGreenAgain','NIET GESELECTEERD',0,0,0,0,0,0)

INSERT INTO Ideas(IdeaID,IQuestionID,UserID,Reported,ReviewByAdmin,Visible,Title,Status,VerifiedUser,VoteCount,RetweetCount,ShareCount,ParentID,DeviceID)
VALUES(2,1,1,0,0,1,'Groenplaats Stadspark','NIET GESELECTEERD',0,0,0,0,0,0)

INSERT INTO Ideas(IdeaID,IQuestionID,UserID,Reported,ReviewByAdmin,Visible,Title,Status,VerifiedUser,VoteCount,RetweetCount,ShareCount,ParentID,DeviceID)
VALUES(3,1,1,0,0,1,'Theater','NIET GESELECTEERD',0,0,0,0,0,0)

INSERT INTO Ideas(IdeaID,IQuestionID,UserID,Reported,ReviewByAdmin,Visible,Title,Status,VerifiedUser,VoteCount,RetweetCount,ShareCount,ParentID,DeviceID)
VALUES(4,1,1,1,0,1,'Cinema','NIET GESELECTEERD',0,0,0,0,0,0)

INSERT INTO Ideas(IdeaID,IQuestionID,UserID,Reported,ReviewByAdmin,Visible,Title,Status,VerifiedUser,VoteCount,RetweetCount,ShareCount,ParentID,DeviceID)
VALUES(5,1,1,1,0,1,'SPA','NIET GESELECTEERD',0,0,0,0,0,0)

/* ideafields */
INSERT INTO IdeaFields(FieldID,IdeaID,FieldText,LocationX,LocationY)
VALUES(1,1,'We maken een grote haag van bomen en struiken rond de Groenplaats om de grijze beton erbuiten te houden!',0,0)

INSERT INTO IdeaFields(FieldID,IdeaID,FieldText,LocationX,LocationY)
VALUES(2,2,'Maken een aantal graspleintjes en bloembakken aan met stenen wandelpaden en een pleintje in het midden rond het standbeeld :)',0,0)

INSERT INTO IdeaFields(FieldID,IdeaID,fieldStrings,LocationX,LocationY)
VALUES(3,2,'I see a gray square and I want to paint it green - Rolling Stoned',0,0)

INSERT INTO IdeaFields(FieldID,IdeaID,FieldText,LocationX,LocationY)
VALUES(4,3,'Een locatie zo nabij het oude centrum moet evenveel cultuur hebben als het centrum zelf. Dus stel ik voor om hier regelmatige theater voorstelling te houden, zodat we de jongeren echte cultuur kunnen aanleren.',0,0)

INSERT INTO IdeaFields(FieldID,IdeaID,FieldText,LocationX,LocationY)
VALUES(5,4,'Nope, dom idee. Wij wille gewoon goeie films kunne zien, buiten op de Groenplaats. Ff pintje op caf�, laatste nieve film om middernacht opt gras buite. Der woont tog niemand, dus ook geen lawaaid overlast.',0,0)

INSERT INTO IdeaFields(FieldID,IdeaID,FieldText,LocationX,LocationY)
VALUES(6,5,'30 jaar geleden was de groenplaats nog groen toen dat SPA aan het roer hing in antwerpen. Sinds de NVA zich kwam moeien is er een echte vergrijzing in de stad!',0,0)

/* Devices */
INSERT INTO Devices(DeviceID,LocationX,LocationY) 
VALUES(1,55,55)

/* votes */
INSERT INTO Votes(VoteID,DeviceID,InputID,InputType,UserMail,Choices,UserID)
VALUES(1,1,2,2,'niels.vanzandbergen@student.kdg.be','Yes',1)

INSERT INTO Votes(VoteID,DeviceID,InputID,InputType,UserMail,Choices,UserID)
VALUES(2,1,2,2,'niels.vanzandbergen@student.kdg.be','No',2)

INSERT INTO Votes(VoteID,DeviceID,InputID,InputType,UserMail,Choices,UserID)
VALUES(3,1,2,2,'niels.vanzandbergen@student.kdg.be','Yes',3)

/* useractivities 
Note: de bedoeling van de keywords hier is dat ze vervangen worden door obj.
*/
INSERT INTO UserActivities(ActivityID,UserID,PlatformID,ProjectID,ActionDescription)
VALUES(1,7,1,1,'platform heeft een nieuw project geintroduceerd.')

INSERT INTO UserActivities(ActivityID,UserID,PlatformID,ProjectID,ModuleID,ActionDescription)
VALUES(2,7,1,1,1,'Nieuwe questionnaire toegevoegd aan project.')

INSERT INTO UserActivities(ActivityID,UserID,PlatformID,ProjectID,ModuleID,ActionDescription)
VALUES(3,7,1,1,2,'Nieuwe ideation toegevoegd aan project.')

INSERT INTO UserActivities(ActivityID,UserID,PlatformID,ProjectID,ModuleID,IQuestionID,ActionDescription)
VALUES(4,7,1,1,2,1,'Nieuwe discussie gestart binnen ideation.')

INSERT INTO UserActivities(ActivityID,UserID,PlatformID,ProjectID,ModuleID,IQuestionID,IdeaID,ActionDescription)
VALUES(5,1,1,1,2,1,1,'User heeft een nieuw idee gegeven binnen een ideation.')

INSERT INTO UserActivities(ActivityID,UserID,PlatformID,ProjectID,ModuleID,IQuestionID,IdeaID,ActionDescription)
VALUES(6,1,1,1,2,1,2,'User heeft een nieuw idee gegeven binnen een ideation.')

INSERT INTO UserActivities(ActivityID,UserID,PlatformID,ProjectID,ModuleID,IQuestionID,IdeaID,ActionDescription)
VALUES(7,1,1,1,2,1,3,'User heeft een nieuw idee gegeven binnen een ideation.')

INSERT INTO UserActivities(ActivityID,UserID,PlatformID,ProjectID,ModuleID,IQuestionID,IdeaID,ActionDescription)
VALUES(8,1,1,1,2,1,4,'User heeft een nieuw idee gegeven binnen een ideation.')

INSERT INTO UserActivities(ActivityID,UserID,PlatformID,ProjectID,ModuleID,IQuestionID,IdeaID,ActionDescription)
VALUES(9,1,1,1,2,1,5,'User heeft een nieuw idee gegeven binnen een ideation.')

INSERT INTO UserActivities(ActivityID,UserID,PlatformID,ProjectID,ModuleID,IQuestionID,IdeaID,VoteID,ActionDescription)
VALUES(10,1,1,1,2,1,2,1,'User heeft gestemd op een idee.')

INSERT INTO UserActivities(ActivityID,UserID,PlatformID,ProjectID,ModuleID,IQuestionID,IdeaID,VoteID,ActionDescription)
VALUES(11,3,1,1,2,1,2,1,'User heeft gestemd op een idee.')

INSERT INTO UserActivities(ActivityID,UserID,PlatformID,ProjectID,ModuleID,IQuestionID,IdeaID,VoteID,ActionDescription)
VALUES(12,6,1,1,2,1,2,1,'User heeft gestemd op een idee.')

/*Reports*/
INSERT INTO Reports(ReportID,IdeaID,FlaggerID,ReporteeID,Reason,ReportApproved)
VALUES(1,4,5,1,'Dit idee is niet serieus en niet open op de andere idee�n omdat hij/zij/x de spot drijft met de andere idee�n.',0)

INSERT INTO Reports(ReportID,IdeaID,FlaggerID,ReporteeID,Reason,ReportApproved)
VALUES(2,5,5,1,'Dit idee is te politiek getint en probeert NVA stemmers te provoceren door ware leugens te verkondigen.',0)
