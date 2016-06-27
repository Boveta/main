using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Boveta.Backend;
using System.Device.Location;

namespace Boveta.Test
{
    /// <summary>
    /// Summary description for GoogleGeocodeTest
    /// </summary>
    [TestClass]
    public class GoogleGeocodeTest
    {
        [TestMethod]
        public void TestValidAddress()
        {
            string address = "Krukmakargatan 9";
            string city = "";
            string zipcode = "118 51";
            string country = "Sweden";

            GeoCoordinate geoCoordinate = GoogleGeocoder.getLocationFromAddress(address,zipcode,city,country);
            Assert.IsNotNull(geoCoordinate);
        }

        [TestMethod]
        public void TestSwedishLetters()
        {
            string address = "Hässlingby gård 1";
            string city = "Haninge";
            string zipcode = "136 91";
            string country = "Sweden";

            GeoCoordinate geoCoordinate = GoogleGeocoder.getLocationFromAddress(address, zipcode, city, country);
            Assert.IsNotNull(geoCoordinate);
        }

        [TestMethod]
        public void TestIncorrectAddress()
        {
            string address = "awfoiewamf 9";
            string city = "awfawfe";
            string zipcode = "16 898";
            string country = "Sweden";

            GeoCoordinate geoCoordinate = GoogleGeocoder.getLocationFromAddress(address, zipcode, city, country);
            if(geoCoordinate == null)
            {
                // This should happen
                return;
            }
            Assert.Fail();
        }
    }
}
