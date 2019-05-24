using System;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    /*
     * @author Xander Veldeman
     * @documentation Niels Van Zandbergen
     *
     * Doordat we gebruik maken van Identity voor onze Authenticatie hebben we een eigen gebruikerklasse nodig. Dit is de klasse die wordt aangesproken binnen Identity voor
     * alles en hierdoor wordt het ook gepersisteerd via de Identity Migration. De PersonalData annotatie staat voor de properties die we meegeven in een JsonFile als de
     * gebruiker wenst zijn persoonlijke data op te halen als hij/zij/x dit wil. Dit is om conform te zijn met de GDPR.
     *
     * @see https://gdpr-eu.be/wat-is-gdpr/
     * 
     */
    public class UimvcUser : IdentityUser
    {
        [PersonalData] public string Name { get; set; }

        [PersonalData] public string Zipcode { get; set; }

        [PersonalData] public bool Gender { get; set; }

        [PersonalData] public DateTime DateOfBirth { get; set; }

        [PersonalData] public int PlatformDetails { get; set; }

        [PersonalData] public string OrgName { get; set; }

        [PersonalData] public string Description { get; set; }

        public bool Banned { get; set; }

        public bool Active { get; set; }
    }
}