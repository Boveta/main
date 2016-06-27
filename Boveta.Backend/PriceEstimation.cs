using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boveta.Backend
{
    public class PriceEstimation
    {
        public int estimatedValue;
        public int datapointsUsed;
        public double searchRadius; 

        // Use version numbering of the analytics engine done for all estimations
        public string estimatorVersion;

        public override string ToString()
        {
            string valueString = this.estimatedValue.ToString();


            if (valueString.Length > 6)
            {
                valueString = valueString.Insert(valueString.Length - 3, ",");
                valueString = valueString.Insert(valueString.Length - 7, ",");
            }
            else if (valueString.Length > 3)
            {
                valueString = valueString.Insert(valueString.Length - 3, ",");
            }
            return valueString;
        }
    }
}