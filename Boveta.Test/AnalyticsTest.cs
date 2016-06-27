using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Boveta.Backend.HouseObjects;

namespace Boveta.Test
{
    /// <summary>
    /// Summary description for AnalyticsTest
    /// </summary>
    [TestClass]
    public class AnalyticsTest
    {
        [TestMethod]
        public void EstimateHousePrice()
        {
            var referenceHouses = GetRandomListOfHouses(100, 1000);
            if (referenceHouses.Count == 0)
            {
                Assert.Fail("Failed to generate random house list");
            }
        }

        private static List<House> GetRandomListOfHouses(int numHouses, double maxDistance)
        {
            var houseList = new List<House>();
            var rng = new Random();

            for (int i = 0; i < numHouses; ++i)
            {
                var house = new House();
                house.livingArea = (int)(10 + rng.NextDouble() * 200);
                house.listPrice = (int)(house.livingArea * 10 * (3 + 2 * rng.NextDouble()));
                house.soldPrice = (int)(house.livingArea * 10 * (4 + 1 * rng.NextDouble()));
                house.objectType = "apartment";
                if (rng.NextDouble() > 0.5)
                {
                    house.objectType = "house";
                    house.additionalArea = (int)(10 + rng.NextDouble() * 200);
                }
                house.distanceToReferencePoint = rng.NextDouble() * maxDistance;

                houseList.Add(house);
            }
            return houseList;
        }
    }
}
