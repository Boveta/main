using Boveta.Backend.Database;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;
using Boveta.Backend.HouseObjects;
using Boveta.Backend.Analytics;

namespace Boveta.Backend.Analytics
{

    /// <summary>
    /// PriceEstimator module. This will be redone to a more general analytics module later
    /// Version: 0.1
    /// Author: Simon Hall, simphall@gmail.com
    /// Release Date: 2015-04-30
    /// </summary>
    public class PriceEstimator
    {
        public const string versionString = "0.3";
        
        public static PriceEstimation EstimateHousePrice(BovetaSearch search, double minConfScore, string connectionString = "")
        {
            AnalysisResults analysisResults = new AnalysisResults();

            var houses = DatabaseConnector.GetSurroundingHouses(search, 20000, connectionString);
            var results = AnalyticsEngine.AnalyzeSurroundingHouses(houses, search);
            if (results.ConfidenceScore > minConfScore)
            {
                analysisResults = results;
            }

            var priceEstimate = new PriceEstimation()
            {
                searchRadius = analysisResults.SearchRadius,
                estimatedValue = (int)((analysisResults.EstimatedValue + 500)/1000)*1000,
                estimatorVersion = versionString,
                datapointsUsed = analysisResults.NumDatapointsUsed
            };

            // TODO: Add log entry here
            return priceEstimate;
        }
    }
}