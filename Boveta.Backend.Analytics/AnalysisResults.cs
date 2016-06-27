using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boveta.Backend.Analytics
{
    public class AnalysisResults
    {
        public double EstimatedValue = -1;
        public double EstimatedStandardDeviation;
        public double ConfidenceScore = 0;
        public int NumDatapointsUsed = 0;
        public double Intercept;
        public double BetaSize;
        public double SearchRadius = -1;
    }
}
