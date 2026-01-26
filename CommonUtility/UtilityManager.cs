using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility
{
    public static class UtilityManager
    {
        public static string LoginId(string prefix, int size, string suffix)
        {
            string allowedChars = "";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";

            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string randomString = "";
            string temp = "";

            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                randomString += temp;
            }
            randomString = prefix + randomString + suffix;
            return randomString;
        }

        public static string UserPassword(string prefix, int size, string suffix)
        {
            string allowedChars = "";
            allowedChars += "A,B,E,F,G,H,K,M,N,P,R,S,T,W,X,Y,Z,";
            allowedChars += "1,2,3,4,5,6,7,8,9";

            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string randomString = "";
            string temp = "";

            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                randomString += temp;
            }
            randomString = prefix + randomString + suffix;
            return randomString;
        }
    }
}
