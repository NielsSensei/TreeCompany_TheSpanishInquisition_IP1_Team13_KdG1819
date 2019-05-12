using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    internal static class ExtensionMethods
    {
        internal static int HasMatchingWords(string left, string right)
        {
            int count = 0;
            var leftSet = new HashSet<string>(left.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries));
            var rightSet = new HashSet<string>(right.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries));

            /*TODO: (FOR LATER) Test deze methode ies uit. Op de huidige data gaat hem enkel flippen als er al groenplaats in staat.
             * Het kan misschien nog zijn dat ik deze ga moeten uitbreiden wanneer gebruikers DE/HET/EEN ofzo gebruiken in hun titles door
             * een soort van ignoreLijst. -NVZ
            */
            foreach (String leftString in leftSet)
            {
                if (rightSet.Contains(leftString))
                {
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
                for (int i = 0; i < myStrings.Count; i++)
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

            foreach (String s in myArray)
            {
                myList.Add(s);
            }

            return myList;
        }
    }
}