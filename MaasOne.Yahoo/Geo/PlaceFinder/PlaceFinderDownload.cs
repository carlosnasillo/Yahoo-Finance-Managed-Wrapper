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
using System.Xml.Linq;


namespace MaasOne.Geo.PlaceFinder
{


    public partial class PlaceFinderDownload : Base.DownloadClient<PlaceFinderResult>
    {



        public PlaceFinderDownloadSettings Settings { get { return (PlaceFinderDownloadSettings)base.Settings; } set { base.SetSettings(value); } }

        public PlaceFinderDownload()
        {
            this.Settings = new PlaceFinderDownloadSettings();
        }

        public void DownloadAsync(PlacesFinderQuery query, object userArgs)
        {
            PlaceFinderDownloadSettings settings = ((PlaceFinderDownloadSettings)this.Settings.Clone());
            settings.Query = query;
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadAsync(PlaceFinderDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        protected override PlaceFinderResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            List<PlaceFinderData> results = new List<PlaceFinderData>();
            System.Globalization.CultureInfo convCulture = new System.Globalization.CultureInfo("en-US");
            PlaceFinderError errorCode = PlaceFinderError.NoError;
            AddressQualitiy bestQuality = AddressQualitiy.NotAnAddress;

            XDocument doc = MyHelper.ParseXmlDocument(stream);
            XElement[] resultSet = XPath.GetElements("//ResultSet",doc);
            if (resultSet.Length == 1)
            {
                XElement resultSetNode = resultSet[0];

                foreach (XElement resultSetElementNode in resultSetNode.Elements())
                {
                    switch (resultSetElementNode.Name.LocalName)
                    {
                        case "Error":
                            errorCode = (PlaceFinderError)Convert.ToInt32(resultSetElementNode.Value.Replace("NN", ""));
                            break;
                        case "ErrorMessage":
                            if (errorCode > PlaceFinderError.NoError)
                            {

                                connInfo = this.GetConnectionInfo(new System.Net.WebException("An internal Yahoo! error occured. Look at InnerException for more details.", new PlaceFinderException(errorCode, resultSetElementNode.Value)),connInfo);
                                break; 
                            }
                            break;
                        case "Locale":
                            string[] codes = resultSetElementNode.Value.Split(new string[] {
								"_",
								"-"
							}, StringSplitOptions.None);
                            Language language = Language.en;
                            Country country = Country.US;
                            foreach (Language lang in Enum.GetValues(typeof(Language)))
                            {
                                if (lang.ToString() == codes[0])
                                {
                                    language = lang;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }

                            foreach (Country cnt in Enum.GetValues(typeof(Country)))
                            {
                                if (cnt.ToString() == codes[1])
                                {
                                    country = cnt;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }

                            break;
                        case "Quality":
                            bestQuality = (AddressQualitiy)Convert.ToInt32(resultSetElementNode.Value);
                            break;
                        case "Result":
                            PlaceFinderData res = new PlaceFinderData();
                            double lat = 0;
                            double lon = 0;
                            double latOff = 0;
                            double lonOff = 0;
                            foreach (XElement resultItemNode in resultSetElementNode.Elements())
                            {
                                switch (resultItemNode.Name.LocalName)
                                {
                                    case "quality":
                                        res.Quality = (AddressQualitiy)Convert.ToInt32(resultItemNode.Value);
                                        break;
                                    case "latitude":
                                        double.TryParse(resultItemNode.Value, System.Globalization.NumberStyles.Any, convCulture, out lat);
                                        break;
                                    case "longitude":
                                        double.TryParse(resultItemNode.Value, System.Globalization.NumberStyles.Any, convCulture, out lon);
                                        break;
                                    case "offsetlat":
                                        double.TryParse(resultItemNode.Value, System.Globalization.NumberStyles.Any, convCulture, out latOff);
                                        break;
                                    case "offsetlon":
                                        double.TryParse(resultItemNode.Value, System.Globalization.NumberStyles.Any, convCulture, out lonOff);
                                        break;
                                    case "radius":
                                        int t;
                                        if (int.TryParse(resultItemNode.Value, System.Globalization.NumberStyles.Any, convCulture, out t)) res.Radius = t;
                                        break;
                                    case "boundingbox":
                                        double n = 0;
                                        double s = 0;
                                        double e = 0;
                                        double w = 0;
                                        foreach (XElement bbItemNode in resultItemNode.Elements())
                                        {
                                            switch (bbItemNode.Name.LocalName)
                                            {
                                                case "north":
                                                    double.TryParse(resultItemNode.Value, System.Globalization.NumberStyles.Any, convCulture, out n);
                                                    break;
                                                case "south":
                                                    double.TryParse(resultItemNode.Value, System.Globalization.NumberStyles.Any, convCulture, out s);
                                                    break;
                                                case "east":
                                                    double.TryParse(resultItemNode.Value, System.Globalization.NumberStyles.Any, convCulture, out e);
                                                    break;
                                                case "west":
                                                    double.TryParse(resultItemNode.Value, System.Globalization.NumberStyles.Any, convCulture, out w);
                                                    break;
                                            }
                                        }

                                        Coordinates ne = new Coordinates(e, n);
                                        Coordinates sw = new Coordinates(w, s);
                                        res.BoundingBox = new CoordinatesRectangle(sw, ne);
                                        break;
                                    case "name":
                                        res.PoiAoiName = resultItemNode.Value;
                                        break;
                                    case "line1":
                                        if (res.DefaultAddress == null)
                                            res.DefaultAddress = new Address();
                                        res.DefaultAddress.StreetAddressOrIntersection = resultItemNode.Value;
                                        break;
                                    case "line2":
                                        if (res.DefaultAddress == null)
                                            res.DefaultAddress = new Address();
                                        res.DefaultAddress.CityOrStateOrZipCode = resultItemNode.Value;
                                        break;
                                    case ("line3"):
                                        if (res.DefaultAddress == null)
                                            res.DefaultAddress = new Address();
                                        res.DefaultAddress.PostalCode = resultItemNode.Value;
                                        break;
                                    case "line4":
                                        if (res.DefaultAddress == null)
                                            res.DefaultAddress = new Address();
                                        res.DefaultAddress.Country = resultItemNode.Value;
                                        break;
                                    case "cross":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.CrossStreets = resultItemNode.Value;
                                        break;
                                    case "house":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.House = resultItemNode.Value;
                                        break;
                                    case "street":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        if (resultItemNode.HasElements)
                                        {
                                            res.ExtendedAddress.Street = this.GetStreetContainer(resultItemNode);
                                        }
                                        else
                                        {
                                            res.ExtendedAddress.Street = new SimpleStreetDescription { FullName = resultItemNode.Value };
                                        }
                                        break;
                                    case "xstreet":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        if (resultItemNode.HasElements)
                                        {
                                            res.ExtendedAddress.CrossStreet = this.GetStreetContainer(resultItemNode);
                                        }
                                        else
                                        {
                                            res.ExtendedAddress.CrossStreet = new SimpleStreetDescription { FullName = resultItemNode.Value };
                                        }
                                        break;
                                    case "unittype":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.UnitType = resultItemNode.Value;
                                        break;
                                    case "unit":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.Unit = resultItemNode.Value;
                                        break;
                                    case "postal":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.PostalCode = resultItemNode.Value;
                                        break;
                                    case "neighborhood":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.Neighborhoods = resultItemNode.Value.Split('/');
                                        break;
                                    case "city":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.City = resultItemNode.Value;
                                        break;
                                    case "county":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.County = resultItemNode.Value;
                                        break;
                                    case "state":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.State = resultItemNode.Value;
                                        break;
                                    case "country":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.Country = resultItemNode.Value;
                                        break;
                                    case "level4":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.Neighborhoods = resultItemNode.Value.Split('/');
                                        break;
                                    case "level3":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.City = resultItemNode.Value;
                                        break;
                                    case "level2":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.County = resultItemNode.Value;
                                        break;
                                    case "level1":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.State = resultItemNode.Value;
                                        break;
                                    case "level0":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.Country = resultItemNode.Value;
                                        break;
                                    case "countycode":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.CountyCode = resultItemNode.Value;
                                        break;
                                    case "statecode":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.StateCode = resultItemNode.Value;
                                        break;
                                    case "countrycode":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.CountryCode = resultItemNode.Value;
                                        break;
                                    case "level2code":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.CountyCode = resultItemNode.Value;
                                        break;
                                    case "level1code":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.StateCode = resultItemNode.Value;
                                        break;
                                    case "level0code":
                                        if (res.ExtendedAddress == null)
                                            res.ExtendedAddress = new ExtendedAddress();
                                        res.ExtendedAddress.CountryCode = resultItemNode.Value;
                                        break;
                                    case "timezone":
                                        res.TimeZone = resultItemNode.Value;
                                        break;
                                    case "areacode":
                                        res.TelephoneAreaCode = resultItemNode.Value;
                                        break;
                                    case "uzip":
                                        res.UniqueZipCode = resultItemNode.Value;
                                        break;
                                    case "hash":
                                        res.Hash = resultItemNode.Value;
                                        break;
                                    case "woeid":
                                        long l;
                                        if (long.TryParse(resultItemNode.Value, System.Globalization.NumberStyles.Any, convCulture, out l)) res.WOEID = l;
                                        break;
                                    case "woetype":
                                        int tInt = 0;
                                        if (int.TryParse(resultItemNode.Value, System.Globalization.NumberStyles.Any, convCulture, out tInt))
                                        {
                                            res.WOEType = (PlaceType)tInt;
                                        }
                                        break;
                                }
                            }

                            res.Position = new Coordinates(lon, lat);
                            res.PositionOffSet = new CoordinatesOffSet
                            {
                                Latitude = latOff,
                                LongitudeOffSet = lonOff
                            };
                            results.Add(res);
                            break;
                    }
                }
            }
            return new PlaceFinderResult(results.ToArray(), bestQuality, (PlaceFinderDownloadSettings)settings);
        }


        private DetailedStreetDescription GetStreetContainer(XElement node)
        {
            DetailedStreetDescription res = null;
            if (node.Name.LocalName == "street" | node.Name.LocalName == "xstreet" & node.HasElements)
            {
                res = new DetailedStreetDescription();
                foreach (XElement subNode in node.Elements())
                {
                    switch (subNode.Name.LocalName)
                    {
                        case "stfull":
                            res.FullName = subNode.Value;
                            break;
                        case "stpredir":
                            res.PrefixDirectional = subNode.Value;
                            break;
                        case "stprefix":
                            res.PrefixType = subNode.Value;
                            break;
                        case "stbody":
                            res.Body = subNode.Value;
                            break;
                        case "stsuffix":
                            res.Suffix = subNode.Value;
                            break;
                        case "stsufdir":
                            res.SuffixDirectional = subNode.Value;
                            break;
                    }
                }
            }
            return res;
        }

    }

    public class PlaceFinderResult
    {
        private PlaceFinderDownloadSettings mSettings = null;
        public PlaceFinderDownloadSettings Settings { get { return mSettings; } }

        private AddressQualitiy mBestQuality;
        public AddressQualitiy BestQuality
        {
            get { return mBestQuality; }
        }
        private PlaceFinderData[] mItems = null;
        public PlaceFinderData[] Items { get { return mItems; } }

        internal PlaceFinderResult(PlaceFinderData[] items, AddressQualitiy bestQual, PlaceFinderDownloadSettings settings)
        {
            mItems = items;
            mBestQuality = bestQual;
            mSettings = settings;
        }
    }





    public class PlaceFinderData : IPlace
    {

        public string Address
        {
            get
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (this.PoiAoiName != string.Empty)
                    sb.AppendLine(this.PoiAoiName);
                if (this.DefaultAddress != null)
                {
                    sb.AppendLine(this.DefaultAddress.ToString());
                }

                return sb.ToString();
            }
        }
        public AddressQualitiy Quality { get; set; }
        public Coordinates Position { get; set; }
        public CoordinatesOffSet PositionOffSet { get; set; }
        public int Radius { get; set; }
        public CoordinatesRectangle BoundingBox { get; set; }
        public string PoiAoiName { get; set; }
        public ExtendedAddress ExtendedAddress { get; set; }
        public Address DefaultAddress { get; set; }

        public string TimeZone { get; set; }
        public string TelephoneAreaCode { get; set; }
        public string UniqueZipCode { get; set; }
        string IPlace.ZipCode
        {
            get { return UniqueZipCode; }
            set { UniqueZipCode = value; }
        }
        public string Hash { get; set; }
        public long WOEID { get; set; }
        public PlaceType WOEType { get; set; }

        public override string ToString()
        {
            string res = string.Empty;
            if (this.ExtendedAddress != null)
            {
                res = this.ExtendedAddress.ToString();
            }
            else if (this.DefaultAddress != null)
            {
                res = this.DefaultAddress.ToString();
            }
            else if (this.Address != string.Empty)
            {
                foreach (string s in this.Address.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    res += s.Trim() + " ";
                }
            }
            else if (this.Position.Latitude != 0 && this.Position.Longitude != 0)
            {
                res = this.Position.ToString();
            }
            else if (this.WOEID != 0)
            {
                res = "WOEID: " + this.WOEID.ToString();
            }

            return res;
        }
    }



    public class Address
    {

        public string StreetAddressOrIntersection { get; set; }
        public string CityOrStateOrZipCode { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public override string ToString()
        {
            string @add = string.Empty;
            if (this.StreetAddressOrIntersection != string.Empty)
                @add = this.StreetAddressOrIntersection + "\n";
            if (this.PostalCode != string.Empty)
                @add += this.PostalCode;
            if (this.CityOrStateOrZipCode != string.Empty)
                @add += this.CityOrStateOrZipCode;
            if (this.Country != string.Empty)
                @add += "\n" + this.Country;
            return @add;
        }
    }

    public class ExtendedAddress
    {

        /// <summary>
        /// House number
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string House { get; set; }
        public IStreetDescription Street { get; set; }
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
        public string CrossStreets { get; set; }
        public IStreetDescription CrossStreet { get; set; }
        public string PostalCode { get; set; }
        public string[] Neighborhoods { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public string CountyCode { get; set; }
        public string StateCode { get; set; }
        public string CountryCode { get; set; }

        public override string ToString()
        {
            string res = string.Empty;
            if (this.Street != null)
            {
                res += this.Street.ToString();
                if (!string.IsNullOrEmpty(this.House))
                {
                    res += " " + this.House;
                    if (!string.IsNullOrEmpty(this.Unit))
                    {
                        res += " " + this.Unit;
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.PostalCode))
            {
                if (res != string.Empty) res += ",";
                res += " " + this.PostalCode;
            }

            if (!string.IsNullOrEmpty(this.City))
            {
                if (res != string.Empty) res += ",";
                res += " " + this.City;
            }

            if (!string.IsNullOrEmpty(this.State))
            {
                if (res != string.Empty) res += ",";
                res += " " + this.State;
            }

            if (!string.IsNullOrEmpty(this.Country))
            {
                if (res != string.Empty) res += ",";
                res += " " + this.Country;
            }

            return res;
        }
    }

    public interface IStreetDescription
    {
        string FullName { get; set; }
    }

    public class SimpleStreetDescription : IStreetDescription
    {
        public string FullName { get; set; }
        public override string ToString()
        {
            return this.FullName;
        }
    }

    public class DetailedStreetDescription : IStreetDescription
    {
        public string FullName { get; set; }
        public string PrefixDirectional { get; set; }
        public string PrefixType { get; set; }
        public string Body { get; set; }
        public string Suffix { get; set; }
        public string SuffixDirectional { get; set; }
        public override string ToString()
        {
            return this.FullName;
        }
    }











}
