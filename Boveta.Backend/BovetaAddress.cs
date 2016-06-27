using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Boveta.Backend
{
    public class BovetaAddress
    {
        public string StreetName = "";
        public int HouseNumber = 0;
        public int Floor = 0;

        public BovetaAddress(string streetName = "", int houseNumber = -1, int floor = -1)
        {
            this.StreetName = streetName;
            this.HouseNumber = houseNumber;
            this.Floor = floor;
        }

        public static BovetaAddress ParseAddressString(string addressString)
        {
            BovetaAddress ret = new BovetaAddress();
            var cleanedAddress = RemoveSpecialChars(addressString);
            var splitAddress = cleanedAddress.Split(' ');
            return ret;
        }

        private static string RemoveSpecialChars(string input)
        {
            return Regex.Replace(input, @"[^0-9a-zåäöÅÄÖA-Z\._ ]", string.Empty);
        }
    }
}
