using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    /*
     * @author Niels Van Zandbergen
     */
    internal static class ExtensionMethods
    {
        /*
         * @documentation Niels Van Zandbergen
         *
         * @param left: string bijvoorbeeld nieuwe QuestionTitle voor een IdeationQuestion
         * @param right: string bijvoorbeeld andere QuestionTitle waarmee hij vergelijkt (binnen dezelfde Ideation hier)
         *
         * Er gebeurt voor beide strings een splitsing in een HashSet waar alle woorden in zitten. Deze worden met elkaar vergeleken, daarnaast wordt
         * er ook nog gekeken of het een veelgebruikt woord is zoals: de/het/een, .... Als bijvoorbeeld twee titels Groenplaats bevatten dan wordt de
         * count met 1 verhoogd. Normaliter is het de bedoeling dat deze count 0 blijft maar dit is nog instelbaar binnen de repositories zelf hoe
         * streng het moet zijn. In principe is dit een uitgebreidere versie van de Equals methode maar dan op woordbasis ipv objectbasis.
         *
         * @see DAL.ExtensionMethods.CommonWordDictionary
         */
        internal static int HasMatchingWords(string left, string right)
        {
            int count = 0;
            var leftSet = new HashSet<string>(left.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries));
            var rightSet = new HashSet<string>(right.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries));
            
            foreach(String leftString in leftSet){
                if (rightSet.Contains(leftString) && !CommonWordDictionary().Contains(leftString)){
                    count++;
                }
            }
            return count;
        }

        internal static void CheckForNotFound(Object obj, String datatype, int ID)
        {
            if (obj == null)
                throw new KeyNotFoundException("This " + datatype + " can't be found! The ID was: " + ID);
        }

        internal static String ListToString(List<String> myStrings)
        {
            String myString = "";

            if (myStrings != null)
            {
                for(int i = 0; i < myStrings.Count; i++)
                {
                    myString += myStrings[i];
                    if (i != myStrings.Count - 1) myString += ",";
                }
            }

            return myString;
        }

        internal static List<String> StringToList(String myString)
        {
            string[] myArray = myString.Split(",", StringSplitOptions.RemoveEmptyEntries);
            List<String> myList = new List<string>();

            foreach(String s in myArray)
            {
                myList.Add(s);
            }

            return myList;
        }

        internal static bool VerifyOverlap(DateTime firstStart, DateTime firstEnd, DateTime secondStart, DateTime secondEnd)
        {
            return firstStart == secondStart || firstEnd == secondEnd || firstStart == secondEnd ||
                   firstEnd == secondStart;
        }
        
        private static string[] CommonWordDictionary()
        {
            return new[]
            {
                "aan", "af", "al", "alles", "als", "ander", "andere", "beetje", "belangrijk", "beslissen", "bij", "bijna", "bijvoorbeeld", "bijzonder", 
                "binnenkort", "daar", "daarom", "dan", "dat", "de", "deze", "dit", "doen", "door", "dus", "echt", "een", "één", "eerste", "einde", "elk", 
                "en", "enkele", "even", "foto", "gaan", "gebruik", "gebruikelijk", "geen", "gemakkelijk", "genoeg", "geven", "gewoon", "goed", "hebben", 
                "heel", "hem", "hen", "het", "hier", "hij", "hoe", "hoeveel", "hoeveelheid", "hoewel", "hun", "idee", "ieder", "iedereen", "iemand", "iets", 
                "ik", "in", "is", "ja", "jij" , "jouw", "jullie", "jou", "kiezen", "kijken", "kom", "komen", "krijgen", "kunnen", "laat", "langs", "laten", 
                "leeg", "leuk", "maar", "maken", "makkelijk", "me", "meel", "meer", "meerdere", "meest", "mij", "mijn", "minder", "mits", "moeilijk", 
                "moeten", "mogelijk", "mogen", "na", "naar", "natuurlijk", "nee", "niet", "niets", "nieuw", "nieuws", "noch", "nodig", "noemen", "nog", 
                "nooit", "normaal", "nu", "of", "om", "ons", "onze", "ooit", "ook", "op", "orde", "over", "overal", "overeenkomen", "pagina", "populair", 
                "proberen", "probleem", "reeds", "resultaat", "richting", "rond", "samen", "simpel", "sinds", "slechts", "snel", "snelheid", "soort", 
                "speciaal", "stad", "stem", "start","stop", "succes", "te", "tegen", "terug", "terwijl", "tevreden", "toen", "toename", "totaal", "tussen", 
                 "u", "uit", "uitnodigen", "uur", "vaak", "van", "vandaag", "ver", "veranderen", "verandering", "verder", "vergelijkbaar", "vergelijken", 
                 "vergelijking", "verschil", "verwachten", "vinden", "voor", "voorbeeld", "waar", "waarom", "waarschijnlijk", "wachten", "wanneer", 
                 "want", "wat", "we", "weg", "welke", "wie", "wij", "willen", "zeggen", "zeker", "zelfde", "zij", "zijn", "zo", "zoals", "zonder", "zou", 
                 "zulke", "zullen"
            };
        }
    }
}
