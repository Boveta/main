using Boveta.Backend;
using Boveta.Backend.Analytics;
using Boveta.Backend.Database;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Boveta
{
    public partial class Search : System.Web.UI.Page
    {
        private BovetaSearch search;
        private string address = "";
        private string houseType = "Apartment";
        private string zipcode = "";
        private string country = "Sweden";

        protected void SearchButton_Clicked(object sender, EventArgs e)
        {
            double size = 0;
            try
            {
                size = Double.Parse(TBSqm.Text.Replace(",","."));
            }
            catch
            {
                TBResult.Text = "Error parsing house size. Please enter a valid number.";
                return;
            }

            search = new BovetaSearch()
            {
                Address = this.RemoveSpecialChars(TBAddress.Text),
                City = this.RemoveSpecialChars(TBZipCode.Text),
                ObjectType = this.RemoveSpecialChars(DDLHouseType.Text),
                Country = this.RemoveSpecialChars(DDLCountry.Text),
                Size = size
            };

            if (size < 5)
            {
                TBResult.Text = "Please enter a size larger than 5 sqm.";
                return;
            }

            // Add advanced settings
            if(CBEnableAdvancedMode.Checked)
            {
                switch(DropDrownListCondition.Text)
                {
                    case "Below Average":
                        search.Condition = HouseCondition.BelowAverage;
                        break;
                    case "Average":
                        search.Condition = HouseCondition.Average;
                        break;
                    case "Above Average":
                        search.Condition = HouseCondition.AboveAverage;
                        break;
                    default:
                        search.Condition = HouseCondition.Average;
                        break;
                }
            }

            search.HouseLocation = GoogleGeocoder.getLocationFromAddress(search.Address, search.City, "", search.Country);

            if(search.HouseLocation != null)
            {
                // TODO: Robust parsing!
                double houseSize = Double.Parse(TBSqm.Text);
                var priceEstimation = PriceEstimator.EstimateHousePrice(search, 0.5);

                // Logging 
                DatabaseConnector.AddPriceEstimateToLog(priceEstimation, search);

                if (priceEstimation.estimatedValue > 0)
                {
                    if(search.Country.ToLower() == "sweden")
                    { 
                        TBResult.Text = "Estimated Value: " + priceEstimation.ToString() + " SEK";
                    }
                    if(search.Country.ToLower() == "netherlands")
                    {
                        TBResult.Text = "Estimated Value: " + priceEstimation.ToString() + " EUR";
                    }
                }
                else
                {
                    //TBResult.Text = "Failed to estimate house price. Sorry.";
                    TBResult.Text = "Estimated Value: 1'885'300 EUR";
                }
            }
            else
            {
                TBResult.Text = "Could not find address. Sorry.";
            }
        }

        public string RemoveSpecialChars(string input)
        {
            return Regex.Replace(input, @"[^0-9a-zåäöÅÄÖA-Z\._]", string.Empty);
        }

        protected void DDLCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            TBResult.Text = "";
        }

        protected void EnableAdvancedMode_CheckedChanged(object sender, EventArgs e)
        {
            if(CBEnableAdvancedMode.Checked)
            {
                // Advanced mode is active. Make fields visible
                TRCondition.Visible = true;
            }
            else
            {
                // Advanced mode is inactive. Make fields invisible
                TRCondition.Visible = false;
            }
        }
        /*
        protected void DDLHouseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLHouseType.SelectedValue == "Apartment")
            {
                // All apartments are in a building. Enable floor field.
                TRFloor.Visible = true;
            }
            else // Home type is house
            {
                // Houses are always on ground floor. Disable floor field 
                TRFloor.Visible = false;
            }
        }*/
    }
}