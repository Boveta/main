using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;
using Boveta.Backend.HouseObjects;

namespace Boveta.Backend.Database
{
    public static class DatabaseConnector
    {
        public static List<House> GetSurroundingHouses(BovetaSearch search, double radiusMeter, string connectionString = "")
        {
            var ret = new List<House>();
            MySqlConnection conn = new MySqlConnection();
            MySqlDataReader rdr;

            if (connectionString == "")
            {
                System.Configuration.Configuration rootWebConfig =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/");

                connectionString = rootWebConfig.ConnectionStrings.ConnectionStrings[GetConnectionString(search.Country)].ConnectionString;
            }
            
            try
            {
                conn.ConnectionString = connectionString;
                conn.Open();

                string stm = GetSQLString(search, radiusMeter);

                MySqlCommand cmd = new MySqlCommand(stm, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    House newHouse = null;
                    if(search.Country.ToLower() == "sweden")
                    { 
                        /*
                        booliId	int(11)	
                        livingArea	int(11)	
                        additionalArea	int(11)	
                        listPrice	int(11)
                        soldPrice	int(11)	
                        published	datetime
                        soldDate	date
                        objectType	varchar(20)
                        rent	int(11)
                        floor	int(11)	
                        rooms	int(11)	
                        constructionYear	int(11)	
                        url	varchar(200)
                        address	varchar(100)
                        latitude	float
                        longitude   float
                        */

                        newHouse = new House()
                        {
                            houseId = rdr.GetInt32(0),
                            livingArea = rdr.GetInt32(1),
                            additionalArea = rdr.GetInt32(2),
                            listPrice = rdr.GetInt32(3),
                            soldPrice = rdr.GetInt32(4),
                            published = rdr.GetDateTime(5),
                            soldDate = rdr.GetDateTime(6),
                            objectType = rdr.GetString(7),
                            rent = rdr.GetInt32(8),
                            floor = rdr.GetInt32(9),
                            rooms = rdr.GetInt32(10),
                            constructionYear = rdr.GetInt32(11),
                            url = rdr.GetString(12),
                            address = rdr.GetString(13),
                            latitude = rdr.GetFloat(14),
                            longitude = rdr.GetFloat(15)
                        };
                    }
                    else if (search.Country.ToLower() == "netherlands")
                    {
                        /*
                        globalId	int(11)	
                        livingArea	int(11)	
                        additionalArea	int(11)	
                        listPrice	int(11)
                        -- soldPrice	int(11)	
                        published	datetime
                        -- soldDate	date
                        objectType	varchar(20)
                        -- rent	int(11)
                        -- floor	int(11)	
                        rooms	int(11)	
                        --constructionYear	int(11)	
                        url	varchar(200)
                        address	varchar(100)
                        latitude	float
                        longitude   float
                        isSold      boolean
                        */
                        DateTime published = DateTime.MinValue;
                        try
                        {
                            string dateTimeStr = rdr.GetString(4);
                            published = DateTime.Parse(dateTimeStr);
                        }
                        catch { }

                        newHouse = new House();
                        newHouse.houseId = rdr.GetInt32(0);
                        newHouse.livingArea = rdr.GetInt32(1);
                        newHouse.additionalArea = rdr.GetInt32(2);
                        newHouse.listPrice = rdr.GetInt32(3);
                        newHouse.soldPrice = -1; //rdr.GetInt32(4),
                        newHouse.published = published;
                        newHouse.soldDate = DateTime.MinValue; //rdr.GetDateTime(6),
                        newHouse.objectType = rdr.GetString(5);
                        newHouse.rent = -1; //rdr.GetInt32(8),
                        newHouse.floor = -1; //rdr.GetInt32(9),
                        newHouse.rooms = rdr.GetInt32(6);
                        newHouse.constructionYear = -1; //rdr.GetInt32(11),
                        newHouse.url = rdr.GetString(7);
                        newHouse.address = rdr.GetString(8);
                        newHouse.city = rdr.GetString(9);
                        string latStr = rdr.GetString(10);
                        try
                        {
                            newHouse.latitude = Double.Parse(latStr);
                        }
                        catch { }
                        string lngStr = rdr.GetString(11);
                        try
                        {
                            newHouse.longitude = Double.Parse(lngStr);
                        }
                        catch { }
                        newHouse.isSold = rdr.GetBoolean(12);

                    }
                    double distance = newHouse.geoCoord.GetDistanceTo(search.HouseLocation);
                    newHouse.distanceToReferencePoint = distance;

                    // Do the final distance check with true cirle
                    if (distance <= radiusMeter && distance > 0)
                    {
                        ret.Add(newHouse);
                    }
                }

                return ret;
            }
            catch (MySqlException ex)
            {
                return ret;
            }
            finally
            {
                conn.Close();
            }
        }

        public static void AddPriceEstimateToLog(PriceEstimation priceEstimation, BovetaSearch search)
        {
            MySqlConnection conn;
            System.Configuration.Configuration rootWebConfig =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/");

            var myConnectionString = rootWebConfig.ConnectionStrings.ConnectionStrings[GetConnectionString(search.Country)];

            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = myConnectionString.ConnectionString;
                conn.Open();

                // Prepare statement. Make rough sort on geographical region to speed things up
                string stm = "INSERT INTO search_logs (SearchTime, SearchRadius, PriceEstimatorVersion, Address, City, ObjectType, SizeSQM, EstimateValue)";
                stm += " VALUES ('" + DateTime.Now + "', ";
                stm += priceEstimation.searchRadius + ", ";
                stm += "'" + priceEstimation.estimatorVersion + "', ";
                stm += "'" + search.Address + "', ";
                stm += "'" + search.City + "', ";
                stm += "'" + search.ObjectType + "', ";
                stm += search.Size.ToString().Replace(",",".")  + ", ";
                stm += priceEstimation.estimatedValue + ")";

                MySqlCommand cmd = new MySqlCommand(stm, conn);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {

            }
            catch (Exception ex)
            {
 
            }
        }

        private static string GetConnectionString(string country)
        {
            if(country.ToLower() == "sweden")
            {
                return "BovetaSQLSwe";
            }
            else if(country.ToLower() == "netherlands")
            {
                return "BovetaSQLNL";
            }
            else
            {
                throw new Exception("Could not match country to connection string");
            }
        }

        private static string GetSQLString(BovetaSearch search, double radiusMeter)
        {
            if (search.Country.ToLower() == "sweden")
            {
                // Prepare statement. Make rough sort on geographical region to speed things up
                string stm = "SELECT * FROM xxx";

                // 69 Miles = 111045 ~ 120000 meter
                // Make rough sort on latitude. One degree is almost always the same distance in meters
                string latstr = search.HouseLocation.Latitude.ToString().Replace(",", ".");
                stm += " WHERE ABS(latitude -" + latstr + ")*120000 <= " + radiusMeter;
                // Make rough sort on longitude. Here we need to use cosine
                string lngstr = search.HouseLocation.Longitude.ToString().Replace(",", ".");
                string lngstrad = (search.HouseLocation.Longitude * 0.0174532925).ToString().Replace(",", ".");
                stm += " AND ABS(longitude -" + lngstr + ")*120000*COS(" + lngstrad + ") <= " + radiusMeter;

                if (search.ObjectType == "House")
                {
                    stm += " AND (objectType = 'Villa' OR objectType = 'Fritidshus' OR objectType = 'Radhus' OR objectType = 'Kjedjehus')";
                }
                else if (search.ObjectType == "Apartment")
                {
                    stm += " AND (objectType = 'Lagenhet')";
                }
                stm += " AND (livingArea > 0)";

                return stm;
            }
            else if (search.Country.ToLower() == "netherlands")
            {
                string stm = "SELECT * FROM xxx";
                string latstr = search.HouseLocation.Latitude.ToString().Replace(",", ".");
                stm += " WHERE ABS(latitude -" + latstr + ")*120000 <= " + radiusMeter;
                // Make rough sort on longitude. Here we need to use cosine
                string lngstr = search.HouseLocation.Longitude.ToString().Replace(",", ".");
                string lngstrad = (search.HouseLocation.Longitude * 0.0174532925).ToString().Replace(",", ".");
                stm += " AND ABS(longitude -" + lngstr + ")*120000*COS(" + lngstrad + ") <= " + radiusMeter;

                if (search.ObjectType == "House")
                {
                    stm += " AND (objectType = 'woonhuis')";
                }
                else if (search.ObjectType == "Apartment")
                {
                    stm += " AND (objectType = 'appartement')";
                }
                stm += " AND (livingArea > 0)";
                return stm;
            }
            else
            {
                throw new Exception("Could not match country to connection string");
            }
        }
    }
}