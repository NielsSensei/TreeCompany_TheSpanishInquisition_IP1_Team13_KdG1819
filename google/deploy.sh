#!/bin/bash


#const
RED="\e[31m"
GREEN="\e[32m"
BBLUE="\e[94m"
BWHITE="\e[97m"
BOLD=$(tput bold)
NBOLD=$(tput sgr0)
COUNTFILE="./deploy.count"

#variables
EXTERNALIP="0"
COUNTNR="0"
ACCOUNT=""

#functions
function get-ip {
	EXTERNALIP=$(gcloud compute addresses list | grep -o '[0-9]\{1,3\}\.[0-9]\{1,3\}\.[0-9]\{1,3\}\.[0-9]\{1,3\}')
	printf "${BBLUE}${BOLD}Getting external-ip... \n${NBOLD}${BWHITE}"
	if [ "$EXTERNALIP" == "0" ] || [ "$EXTERNALIP" == "" ]
	then
		printf "${RED}No external-ip found! \n"
		printf "${RED}Creating new... ${BWHITE}\n"
		gcloud -q compute addresses create "inquisitor-ip" --region "europe-west1"

		get-ip #custom function

		printf "${BWHITE}${GREEN}${BOLD}$EXTERNALIP\n${NBOLD}${BWHITE}"
	else
		printf "${BWHITE}External-ip found: ${GREEN}${BOLD}$EXTERNALIP\n\n${NBOLD}${BWHITE}"
	fi
}

function init {
	printf "${BBLUE}${BOLD}Creating new VM-Instance.. ${BWHITE}${NBOLD}\n"
	gcloud -q compute instances create "inquisitor-instance" --address "$EXTERNALIP" --tags http-server,https-server

	printf "${BBLUE}${BOLD}Creating Firewall rules... ${BWHITE}\n${NBOLD}"
	#gcloud compute firewall-rules create

	printf "${BBLUE}${BOLD}Creating SQL Server... ${BWHITE}\n${NBOLD}"
	gcloud -q sql instances create "inquisitor-sql${COUNTNR}" --region "europe-west1"

	printf "${BBLUE}${BOLD}Connecting SQL Server... ${BWHITE}${NBOLD}\n"
	gcloud -q sql connect "inquisitor-sql${COUNTNR}"
}

function deploy {
	#gcloud auth login "956325227789-compute@developer.gserviceaccount.com"
	cd ~/repo/UI-MVC/
	gcloud -q app deploy --verbosity=debug bin/Release/netcoreapp2.2/publish/app.yaml
	cd ~/
	#gcloud auth revoke "956325227789-compute@developer.gserviceaccount.com"
}

function delete {
	printf "${BBLUE}${BOLD}Removing SQL Server... ${BWHITE}${NBOLD}\n"
	gcloud -q sql instances delete "inquisitor-sql${COUNTNR}"
	update-sql-rnd #custom function

	printf "${BBLUE}${BOLD}Removing all Firewall rules... ${BWHITE}${NBOLD}\n"
	#gcloud compute firewall-rules delete

	printf "${BBLUE}${BOLD}Removing VM-Instance... ${BWHITE}${NBOLD}\n"
	gcloud -q compute instances delete "inquisitor-instance"
}

function export-sql {
	get-account
	gcloud -q sql instances describe "inquisitor-sql${COUNTNR}"
	gsutil -q acl ch -u "$ACCOUNT":W gs://inquisitor-bucket/
	gsutil -q acl ch -u "$ACCOUNT":W gs://inquisitor-bucket/sqldumpfile.gz
	gcloud -q sql export sql "inquisitor-sql${COUNTNR}" gs://inquisitor-bucket/sqldumpfile.gz
	gsutil -q acl ch -d "$ACCOUNT":W gs://inquisitor-bucket/
	gsutil -q acl ch -d "$ACCOUNT":W gs://inquisitor-bucket/sqldumpfile.gz
}

function sql-rnd {
	if [ -e "$COUNTFILE" ]
	then
		COUNTNR=$(cat "$COUNTFILE")
	else
		touch "$COUNTFILE"
		echo "$COUNTNR" > "$COUNTFILE"
	fi
}

function update-sql-rnd {
	COUNTNR=$((COUNTNR+1))
	echo "$COUNTNR" > "$COUNTFILE"
}

function get-account {
	ACCOUNT=$(gcloud sql instances describe "inquisitor-sql${COUNTNR}" | grep serviceAccountEmailAddress | cut -d " " -f 2)
	printf "${BBLUE}${BOLD}Current account: ${GREEN}${ACCOUNT}${BWHITE}${NBOLD}\n"
	gcloud auth list
}

gcloud config set account "nathan.gijselings@student.kdg.be"
gcloud config set project "cs2-nathan-gijselings"
gcloud auth activate-service-account "inquisitor-service@cs2-nathan-gijselings.iam.gserviceaccount.com" --key-file=~/cs2-nathan-gijselings-4e4ed785dd23.json
get-ip #custom function
sql-rnd #custom function

#default: deploy
if [ -z "$@" ]
then
	init #custom function
	deploy #custom function
fi

for i in "$@"
do
	case $i in
		-d | --delete )
			#delete
			export-sql

			delete #custom function
			;;
		-D | --deleteall )
			#deleteall
			delete #custom function

			printf "${BBLUE}${BOLD}Removing Bucket... ${BWHITE}${NBOLD}\n"
			gsutil -q rm -r gs://inquisitor-bucket
			printf "${BBLUE}${BOLD}Removing External-ip...${BWHITE}${NBOLD}\n"
			gcloud -q compute addresses delete "inquisitor-ip"
			;;
		-i | --import )
			#import
			init #custom function

			get-account

			printf "${BBLUE}Importing Bucket to SQL... ${BWHITE}\n"
			gcloud -q sql instances describe "inquisitor-sql${COUNTNR}"
			gsutil acl ch -u "$ACCOUNT":W gs://inquisitor-bucket/sqldumpfile.gz
			gsutil acl ch -u "$ACCOUNT":R gs://inquisitor-bucket/sqldumpfile.gz
			gcloud -q sql import sql "inquisitor-sql${COUNTNR}" gs://inquisitor-bucket/sqldumpfile.gz

			gsutil acl ch -d "$ACCOUNT":W gs://inquisitor-bucket/
			gsutil acl ch -d "$ACCOUNT":R gs://inquisitor-bucket/
			;;
		-b | --bucket )
			gsutil mb -p "cs2-nathan-gijselings" -l "europe-west1" gs://inquisitor-bucket/
			touch sqldumpfile.gz

			gsutil -q cp ~/sqldumpfile.gz gs://inquisitor-bucket/
			;;
		-e | --export )
			get-account
			export-sql
			;;
	esac
done
exit 0