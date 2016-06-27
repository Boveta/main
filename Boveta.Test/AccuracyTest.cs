using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Boveta.Backend.Database;
using Boveta.Backend;
using System.Configuration;
using Boveta.Backend.Analytics;
using System.IO;
using System.Threading;
using System.Linq;

namespace Boveta.Test
{
    internal class AnalysisResult
    {
        public double realPrice;
        public double estimatePrice;
        public double houseSize;
        public double searchRadius;
        public int datapointsUsed;
        public double estimatedErrorAbs
        {
            get { return Math.Abs(realPrice - estimatePrice); }
        }

        public double estimatedRelErrorAbs
        {
            get { return Math.Abs(realPrice - estimatePrice) / realPrice; }
        }

        public double estimatedErrorSquared
        {
            get { return Math.Pow(realPrice - estimatePrice, 2); }
        }

        public override string ToString()
        {
            return estimatePrice + ", " + realPrice + ", " + houseSize + ", " + searchRadius + ", " + datapointsUsed;
        }
    }

    /// <summary>
    /// Summary description for AccuracyTest
    /// </summary>
    [TestClass]
    public class AccuracyTest
    {
        private double MIN_CONF_SCORE = 0.5;

        [TestMethod]
        public void TestAccuracyAppSwe()
        {
            var search = new BovetaSearch()
            {
                ObjectType = "Apartment",
                Country = "Sweden",
                HouseLocation = new System.Device.Location.GeoCoordinate(59, 18),
            };

            var connectionString = ConfigurationManager.ConnectionStrings["BovetaSQLSwe"].ConnectionString;

            var allHouses = DatabaseConnector.GetSurroundingHouses(search, 5000000, connectionString);
            var results = new List<AnalysisResult>();

            Random rnd = new Random();

            for(int i = 0; i < 500; ++i)
            {
                var house = allHouses[rnd.Next(allHouses.Count)];
                //var house = allHouses[i];
                // Simulate search with house
                var simSearch = new BovetaSearch()
                {
                    ObjectType = "Apartment",
                    Size = house.livingArea,
                    AdditionalArea = house.additionalArea,
                    Condition = HouseCondition.Average,
                    HouseLocation = house.geoCoord,
                    Country = "Sweden"
                };
                var estimation = PriceEstimator.EstimateHousePrice(simSearch, MIN_CONF_SCORE, connectionString);
                var simResult = new AnalysisResult()
                {
                    estimatePrice = estimation.estimatedValue,
                    realPrice = house.soldPrice,
                    houseSize = house.livingArea,
                    searchRadius = estimation.searchRadius,
                    datapointsUsed = estimation.datapointsUsed
                };

                results.Add(simResult);
            }

            int successfullSearches = results.Where(w => w.estimatePrice > 0).Count();
            double totalRelErrorAbs = 0;
            foreach (var result in results.Where(w => w.estimatePrice > 0))
            {
                totalRelErrorAbs += result.estimatedRelErrorAbs;
            }

            WriteOutputToFile(results, "Sweden");

            double averageError = totalRelErrorAbs / successfullSearches;
            Assert.IsTrue(averageError < 0.2, "Error over 20%....");
            Assert.IsTrue((double)successfullSearches/(double)results.Count > 0.8, "Less than 75% of searches successful.");
            
        }

        private void WriteOutputToFile(List<AnalysisResult> results, string country)
        {
            using (StreamWriter file = new StreamWriter("Output" + country + ".txt"))
            {
                foreach (var result in results)
                    file.WriteLine(result);
            }
        }
    }
}
