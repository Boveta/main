using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Device.Location;

namespace Boveta.Backend
{
    public static class GoogleGeocoder
    {
        public static string getGeocoderURL(string address, string zipcode, string city, string country)
        {
            return String.Format("http://maps.google.com/maps/api/geocode/json?address={0},{1}+{2},+{3}&sensor=false",
                address, zipcode, city, country);
        }

        public static string getGeocodeJSON(string geocoderURL)
        {
            string result = "";

            WebRequest myWebRequest = WebRequest.Create(geocoderURL);
            WebResponse myWebResponse = myWebRequest.GetResponse();
            Stream ReceiveStream = myWebResponse.GetResponseStream();
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader readStream = new StreamReader(ReceiveStream, encode);
            Console.WriteLine("\nResponse stream received");
            Char[] read = new Char[256];

            // Read 256 charcters at a time.     
            int count = readStream.Read(read, 0, 256);
            Console.WriteLine("HTML...\r\n");

            while (count > 0)
            {
                // Dump the 256 characters on a string and display the string onto the console.
                String str = new String(read, 0, count);
                result += str;
                count = readStream.Read(read, 0, 256);
            }
            result = result.Replace(" ", "");
            return result;
        }

        public static GeoCoordinate getLocationFromJSON(string JSON)
        {
            double latitude = getCoordinateFromJSON(JSON, "\"lat\":");
            double longitude = getCoordinateFromJSON(JSON, "\"lng\":");

            return new GeoCoordinate(latitude, longitude);
        }

        public static GeoCoordinate getLocationFromAddress(string address, string zipcode, string city, string country)
        {
            // Clean Address
            address = address.Replace(" ", "+").ToUpper();
            zipcode = zipcode.Replace(" ", "").ToUpper();
            country = country.Replace(" ", "+").ToUpper();
            city = city.Replace(" ", "+").ToUpper();
            try
            {
                string geocoderURL = getGeocoderURL(address, zipcode, city, country);
                string geocoderJSON = getGeocodeJSON(geocoderURL);
                return getLocationFromJSON(geocoderJSON);
            }
            catch(Exception ex)
            {
                // Add logging here
                return null;
            }
            
        }

        private static double getCoordinateFromJSON(string JSON, string identifier)
        {
            int identifierIndex = JSON.IndexOf(identifier);

            //Identify the substring that contains the number
            string numberSubst = JSON.Substring(identifierIndex + identifier.Length, 20);

            var allowedChars = "0123456789.".ToCharArray();
            int numberStartInd = numberSubst.IndexOfAny(allowedChars);
            int numberEndInd = numberStartInd;
            while (allowedChars.Contains(numberSubst[numberEndInd]))
            {
                // Will go over index by one!
                numberEndInd++;
            }
            int length = numberEndInd - numberStartInd - 1;
            string finalNumberString = numberSubst.Substring(numberStartInd,length);
            try
            {
                return Double.Parse(finalNumberString);
            }
            catch
            {
                return Double.Parse(finalNumberString.Replace(".",","));
            }
        }
    }
}