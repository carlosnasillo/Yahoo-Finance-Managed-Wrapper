// ******************************************************************************
// ** 
// **  Yahoo! Managed
// **  Written by Marius Häusler 2012
// **  It would be pleasant, if you contact me when you are using this code.
// **  Contact: YahooFinanceManaged@gmail.com
// **  Project Home: http://code.google.com/p/yahoo-finance-managed/
// **  
// ******************************************************************************
// **  
// **  Copyright 2012 Marius Häusler
// **  
// **  Licensed under the Apache License, Version 2.0 (the "License");
// **  you may not use this file except in compliance with the License.
// **  You may obtain a copy of the License at
// **  
// **    http://www.apache.org/licenses/LICENSE-2.0
// **  
// **  Unless required by applicable law or agreed to in writing, software
// **  distributed under the License is distributed on an "AS IS" BASIS,
// **  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// **  See the License for the specific language governing permissions and
// **  limitations under the License.
// ** 
// ******************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using MaasOne.Xml;
using MaasOne;


namespace MaasOne.Geo.PlaceFinder
{


    public class PlaceFinderDownloadSettings : Base.SettingsBase, IResultIndexSettings
    {

        public int Index { get; set; }
        public int Count { get; set; }
        /// <summary>
        /// Location setback in meters, intended to approximate a building location offset from the road center-line.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Offset { get; set; }
        public Culture Culture { get; set; }

        public PlacesFinderQuery Query { get; set; }
        public bool CoordinatesOnly { get; set; }
        public bool NoWOEID { get; set; }
        public bool UsSpecificElements { get; set; }
        public bool IncludeNearestAirportCode { get; set; }
        public bool IncludeTelephoneAreaCode { get; set; }
        public bool IncludeDetailedStreetAttributes { get; set; }
        public bool IncludeTimezoneInfo { get; set; }
        public bool IncludeBoundingBox { get; set; }
        public bool IncludeNeighborhoodNames { get; set; }
        public bool IncludeCrossStreets { get; set; }
        public bool LimitResultsToCulture { get; set; }
        /// <summary>
        /// Enables exact matches.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool QuickMode { get; set; }
        /// <summary>
        /// Needs longitude/latitude value in LocationSearchOptions
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ReverseGeoCoding { get; set; }

        public PlaceFinderDownloadSettings()
        {
            this.Count = 10;
            this.LimitResultsToCulture = false;
            this.Query = null;
            this.CoordinatesOnly = false;
            this.NoWOEID = false;
            this.UsSpecificElements = false;
            this.IncludeNearestAirportCode = false;
            this.IncludeTelephoneAreaCode = false;
            this.IncludeDetailedStreetAttributes = true;
            this.IncludeTimezoneInfo = false;
            this.IncludeBoundingBox = true;
            this.IncludeNeighborhoodNames = false;
            this.IncludeCrossStreets = false;
            this.LimitResultsToCulture = false;           
            
        }

        protected override string GetUrl()
        {
            if (this.Query == null)
                throw new ArgumentNullException("query", "The query options object is null");
            System.Text.StringBuilder url = new System.Text.StringBuilder();

            url.Append("http://where.yahooapis.com/geocode?");
            if (this.Index > 0 & this.Index <= 99)
                url.Append("&start=" + this.Index);
            if (this.Count != 10 & this.Count > 0 & this.Count <= 100)
                url.Append("&count=" + this.Count);
            if (this.Offset != 15 & this.Offset >= 0 & this.Offset <= 100)
                url.Append("&offset=" + this.Offset);

            System.Text.StringBuilder flags = new System.Text.StringBuilder();
            if (this.CoordinatesOnly)
                flags.Append("C");
            if (this.NoWOEID)
                flags.Append("E");
            if (this.UsSpecificElements)
                flags.Append("G");
            if (this.IncludeNearestAirportCode)
                flags.Append("Q");
            if (this.IncludeTelephoneAreaCode)
                flags.Append("R");
            if (this.IncludeDetailedStreetAttributes)
                flags.Append("S");
            if (this.IncludeTimezoneInfo)
                flags.Append("T");
            if (this.IncludeBoundingBox)
                flags.Append("X");
            if (flags.Length > 0)
            {
                url.Append("&flags=" + flags.ToString());
            }
            System.Text.StringBuilder gflags = new System.Text.StringBuilder();
            if (this.IncludeNeighborhoodNames)
                gflags.Append("A");
            if (this.IncludeCrossStreets)
                gflags.Append("C");
            if (this.LimitResultsToCulture)
                gflags.Append("L");
            if (this.QuickMode)
                gflags.Append("Q");
            if (this.ReverseGeoCoding)
                gflags.Append("R");
            if (gflags.Length > 0)
            {
                url.Append("&gflags=" + gflags.ToString());
            }
            if (this.Culture != null && !(this.Culture.Language == Language.en & this.Culture.Country == Country.US))
            {
                url.AppendFormat("&locale={0}_{1}", this.Culture.Language, this.Culture.Country);
            }

            url.Append(this.Query.QueryString());

            return url.ToString();
        }



        public override object Clone()
        {
            return new PlaceFinderDownloadSettings() { 
                CoordinatesOnly = this.CoordinatesOnly,
                Count = this.Count,
                Culture = this.Culture,
                IncludeBoundingBox = this.IncludeBoundingBox,
                IncludeCrossStreets = this.IncludeCrossStreets,
                IncludeDetailedStreetAttributes = this.IncludeDetailedStreetAttributes,
                IncludeNearestAirportCode = this.IncludeNearestAirportCode,
                IncludeNeighborhoodNames = this.IncludeNeighborhoodNames,
                IncludeTelephoneAreaCode = this.IncludeTelephoneAreaCode,
                IncludeTimezoneInfo = this.IncludeTimezoneInfo,
                Index = this.Index,
                LimitResultsToCulture = this.LimitResultsToCulture,
                NoWOEID = this.NoWOEID,
                Offset = this.Offset,
                Query = this.Query,
                QuickMode = this.QuickMode,
                ReverseGeoCoding = this.ReverseGeoCoding,
                UsSpecificElements = this.UsSpecificElements
            };
        }
    }


    public abstract class PlacesFinderQuery
    {
        internal abstract string QueryString();
    }


    public class FreeFormQuery : PlacesFinderQuery
    {

        /// <summary>
        /// A location query string in free format.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Location { get; set; }

        public FreeFormQuery() { }
        public FreeFormQuery(string location) { this.Location = location; }

        internal override string QueryString()
        {
            if (this.Location == string.Empty)
                throw new ArgumentException("The location is empty", "Location");
            return "&location=" + this.Location;
        }
    }

    public class PointOfInterestQuery : PlacesFinderQuery
    {

        public string PointOfInterest { get; set; }

        internal override string QueryString()
        {
            if (this.PointOfInterest == string.Empty)
                throw new ArgumentException("The Point of Interest is empty", "PointOfInterest");
            return "&name=" + this.PointOfInterest;
        }
    }


    public class WoeidQuery : PlacesFinderQuery
    {

        public int WOEID { get; set; }

        internal override string QueryString()
        {
            if (this.WOEID == 0)
                throw new ArgumentException("The WOEID is invalid", "WOEID");
            return "&woeid=" + this.WOEID.ToString();
        }
    }




    public class ThreeLineAddressQuery : PlacesFinderQuery
    {

        public string StreetAddressOrIntersection { get; set; }
        public string CityOrStateOrZipCode { get; set; }
        public string PostalCode { get; set; }

        internal override string QueryString()
        {
            string @params = string.Empty;
            if (this.StreetAddressOrIntersection != string.Empty)
            {
                @params += "&line1=" + this.StreetAddressOrIntersection;
            }
            if (this.CityOrStateOrZipCode != string.Empty)
            {
                @params += "&line2=" + this.CityOrStateOrZipCode;
            }
            if (this.PostalCode != string.Empty)
            {
                @params += "&line3=" + this.PostalCode;
            }
            if (@params.Length == 0)
                throw new ArgumentException("All Properties are empty", "");
            return @params;
        }
    }


    public class GeneralAddressQuery : PlacesFinderQuery
    {

        /// <summary>
        /// House number
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string House { get; set; }
        public string Street { get; set; }
        /// <summary>
        /// Unit type such as Apartment(Apt) or Suite(Ste)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string UnitType { get; set; }
        /// <summary>
        /// Unit decription i.e. appartment number
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Unit { get; set; }
        public string CrossStreet { get; set; }
        public string PostalCode { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        internal override string QueryString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (this.House != string.Empty)
                sb.Append("&house=" + this.House);
            if (this.Street != string.Empty)
                sb.Append("&street=" + this.Street);
            if (this.UnitType != string.Empty)
                sb.Append("&untittype=" + this.UnitType);
            if (this.Unit != string.Empty)
                sb.Append("&unit=" + this.Unit);
            if (this.CrossStreet != string.Empty)
                sb.Append("&xstreet=" + this.CrossStreet);
            if (this.PostalCode != string.Empty)
                sb.Append("&postal=" + this.PostalCode);
            if (this.Neighborhood != string.Empty)
                sb.Append("&level4=" + this.Neighborhood);
            if (this.City != string.Empty)
                sb.Append("&level3=" + this.City);
            if (this.County != string.Empty)
                sb.Append("&level2=" + this.County);
            if (this.State != string.Empty)
                sb.Append("&level1=" + this.State);
            if (this.Country != string.Empty)
                sb.Append("&level0=" + this.Country);
            if (sb.Length == 0)
                throw new ArgumentException("All Properties are empty", "");
            return sb.ToString();
        }

    }



}
