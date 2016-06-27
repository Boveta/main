using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace Boveta.Backend.HouseObjects
{
    public class House
    {
        public int houseId = -1;
        public int livingArea = -1;
        public int additionalArea = -1;
        public int listPrice = -1;
        public int soldPrice = -1;
        public DateTime published = DateTime.MinValue;
        public DateTime soldDate = DateTime.MinValue;
        public string objectType = "unknown";
        public int rent = -1;
        public int floor = -1;
        public int rooms = -1;
        public int constructionYear = -1;
        public string url = "";
        public string address = "";
        public string city = "";
        public double latitude = 0;
        public double longitude = 0;
        public bool isSold = false;

        // Used in modelling
        public double distanceToReferencePoint = 0;

        public GeoCoordinate geoCoord 
        {
            get
            {
                return new GeoCoordinate(latitude, longitude);
            }

            set
            {
                this.latitude = value.Latitude;
                this.longitude = value.Longitude;
            }
        }

    }
}