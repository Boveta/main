using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Boveta.Backend;
using System.Collections.Generic;

namespace Boveta.Test
{
    [TestClass]
    public class BovetaAddressTest
    {
        [TestMethod]
        public void ParseAddresses()
        {
            string address1 = "Frans Hallstraat 55 2";
            string address2 = "Frans Hallstraat 55 II";
            string address3 = "Frans Hallstraat 55-2";
            string address4 = "Frans Hallstraat 55-II";

            List<BovetaAddress> parsedAddresses = new List<BovetaAddress>();
            parsedAddresses.Add(BovetaAddress.ParseAddressString(address1));
            parsedAddresses.Add(BovetaAddress.ParseAddressString(address2));
            parsedAddresses.Add(BovetaAddress.ParseAddressString(address3));
            parsedAddresses.Add(BovetaAddress.ParseAddressString(address4));

            foreach(var parsedAddress in parsedAddresses)
            {
                Assert.AreEqual(parsedAddress.StreetName, "Frans Hallstraat");
                Assert.AreEqual(parsedAddress.HouseNumber, 55);
                Assert.AreEqual(parsedAddress.Floor, 2);
            }

        }
    }
}
