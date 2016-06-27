using Boveta.Backend.HouseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics;

namespace Boveta.Backend.Analytics
{
    public static class AnalyticsEngine
    {
        private const double LOG_BASE = 100;
        private const double RADIUS_W = 5000;
        private const double COUNT_W = 5000;
        private const double COUNT_THRESH = 5000;
        private const double POL_WEIGHT_NUM = 2;
        private const double POL_WEIGHT_DENOM = 14.14;
        private const int MIN_RESULTS = 5;

        public static AnalysisResults AnalyzeSurroundingHouses(List<House> houses, BovetaSearch search)
        {
            var results = new AnalysisResults();

            // Should always be the case. Extra check just to make sure.
            houses = houses.Where(house => house.livingArea > 0).ToList();

            // Should we use listing price or sold price? Better if possible.
            bool useSoldPrice = false;

            if(houses.Where(house => house.soldPrice > 0).Count() >= 
               houses.Where(house => house.listPrice > 0).Count())
            {
                useSoldPrice = true;
                houses = houses.Where(house => house.soldPrice > 0).ToList();
            }
            else
            {
                houses = houses.Where(house => house.listPrice > 0).ToList();
            }

            // For convenience
            int numHouses = houses.Count();

            // At least MIN_RESULTS houses in big initial radius
            if (numHouses < MIN_RESULTS) return results;

            // Optimize the number of houses/search radius
            houses = houses.OrderBy(w => w.distanceToReferencePoint).ToList();

            double bestRadius = 0;
            double bestScore = Double.MinValue;
            for(double radius = 100; radius < 10000; radius = radius * 2)
            {
                int s = houses.Where(w => w.distanceToReferencePoint <= radius).Count();
                double order = Math.Log(s, LOG_BASE);
                double score = order - radius / RADIUS_W;
                Console.WriteLine(" Radius: " + radius + "Score: " + score);
                if (score > bestScore & s > MIN_RESULTS)
                {
                    bestRadius = radius;
                    bestScore = score;
                }
            }
            results.SearchRadius = bestRadius;

            houses = houses.Where(w => w.distanceToReferencePoint < bestRadius).ToList();

            if (houses.Count < MIN_RESULTS) return results;

            List<double> xdata = new List<double>();
            List<double> ydata = new List<double>();
            List<double> weights = new List<double>();

            foreach (var house in houses)
            {
                int housePrice = house.listPrice;
                if (useSoldPrice)
                {
                    housePrice = house.soldPrice;
                }

                weights.Add(1 + POL_WEIGHT_NUM / (POL_WEIGHT_DENOM + house.distanceToReferencePoint));
                xdata.Add(house.livingArea);
                ydata.Add(housePrice);
            }

            var p = Fit.PolynomialWeighted(xdata.ToArray(), ydata.ToArray(),weights.ToArray(),1);
            double estimation = p[0] + p[1] * search.Size;

            if(search.Condition != HouseCondition.Average)
            {
                // Adjust weights to accomodate for condition

                List<double> modelerr = new List<double>();

                // 1. Find error in current model. Use this as measure for over/under valuation
                for(int i = 0; i < ydata.Count; ++i)
                {
                    double modelError = ydata[i] - (p[0] + p[1] * xdata[i]);
                    modelerr.Add(modelError);
                }

                // 2. Adjust weights so that overvalued houses are worth more.
                for(int i = 0; i < modelerr.Count; i++)
                {
                    if(modelerr[i] < 0) // Model price > real price. Probably bad condition
                    {
                        if (search.Condition == HouseCondition.AboveAverage)
                        {
                            weights[i] = weights[i] * 0.5; // Less representive of true house price
                        }
                        else if (search.Condition == HouseCondition.BelowAverage)
                        {
                            weights[i] = weights[i] * 2; // More representive of true house price
                        }
                    }
                    else  // Model price < real price. Probably good condition
                    {
                        if (search.Condition == HouseCondition.AboveAverage)
                        {
                            weights[i] = weights[i] * 2; // More representive of true house price
                        }
                        else if (search.Condition == HouseCondition.BelowAverage)
                        {
                            weights[i] = weights[i] * 0.5; // Less representive of true house price
                        }
                    }
                }

                // Recalculate polynomial
                p = Fit.PolynomialWeighted(xdata.ToArray(), ydata.ToArray(), weights.ToArray(), 1);
            }

            

            results.Intercept = p[0];
            results.BetaSize = p[1];

            results.NumDatapointsUsed = houses.Count();
            results.EstimatedValue = (int)(results.Intercept + results.BetaSize * search.Size);

            if (search.Condition == HouseCondition.BelowAverage & results.EstimatedValue > estimation)
            {
                // Oops. Our advanced searched failed. Reduce cost by 10% for now.....
                results.EstimatedValue = results.EstimatedValue * 0.9;
            }

            if (search.Condition == HouseCondition.AboveAverage & results.EstimatedValue < estimation)
            {
                // Oops. Our advanced searched failed. Increase cost by 10% for now.....
                results.EstimatedValue = results.EstimatedValue * 1.1;
            }

            results.ConfidenceScore = 0.6;

            return results;
        }
    }
}
