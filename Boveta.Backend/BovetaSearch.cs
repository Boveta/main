using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace Boveta.Backend
{
    public class BovetaSearch
    {
        public string Address;
        public string City;
        public string Country;
        public string ObjectType;
        public double Size;
        public double AdditionalArea = -1;
        public GeoCoordinate HouseLocation;
        public HouseCondition Condition = HouseCondition.Average;
    }
}