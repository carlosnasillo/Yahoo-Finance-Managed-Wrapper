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


namespace MaasOne.Geo.GeoPlanet
{



    public partial class PlacesDownload : Base.DownloadClient<PlacesResult>
    {

        public PlacesDownloadSettings Settings { get { return (PlacesDownloadSettings)base.Settings; } set { base.SetSettings(value); } }


        public PlacesDownload()
        {
            this.SetSettings(new PlacesDownloadSettings());
        }


        public void DownloadAsync(string keyword, object userArgs)
        {
            this.DownloadAsync(keyword, PlaceType.Undefined, userArgs);
        }
        public void DownloadAsync(string keyword, PlaceType type, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "places";
            settings.Query = this.KeywordQuery(keyword, type);
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadAsync(IEnumerable<long> woeids, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "places";
            settings.Query = this.WoeidQuery(woeids);
            base.DownloadAsync(settings, userArgs);
        }       
        public void DownloadParentAsync(long woeid, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "place/";
            settings.Query = woeid.ToString() + "/parent";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadChildrenAsync(long woeid, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "place/";
            settings.Query = woeid.ToString() + "/children";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadChildrenAsync(long woeid, PlaceType type, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "place/";
            settings.Query = woeid.ToString() + "/children.type(" + Convert.ToInt32(type).ToString() + ")";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadChildrenAsync(long woeid, int degree, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "place/";
            settings.Query = woeid.ToString() + "/children.degree(" + degree.ToString() + ")";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadAncestorsAsync(long woeid, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "place/";
            settings.Query = woeid.ToString() + "/ancestors";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadCommonAncestorAsync(int first, IEnumerable<long> woeids, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "place/";
            settings.Query = this.ComAncestorQuery(first, woeids);
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadBelongtosAsync(long woeid, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "place/";
            settings.Query = woeid.ToString() + "/belongtos";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadBelongtosAsync(long woeid, PlaceType type, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "place/";
            settings.Query = woeid.ToString() + "/belongtos.type(" + Convert.ToInt32(type).ToString() + ")";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadDescendantsAsync(long woeid, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "place/";
            settings.Query = woeid.ToString() + "/descendants";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadDescendantsAsync(long woeid, PlaceType type, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "place/";
            settings.Query = woeid.ToString() + "/descendants.type(" + Convert.ToInt32(type).ToString() + ")";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadNeighborsAsync(long woeid, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "place/";
            settings.Query = woeid.ToString() + "/neighbors";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadNeighborsAsync(long woeid, int degree, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "place/";
            settings.Query = woeid.ToString() + "/descendants.degree(" + degree.ToString() + ")";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadSiblingsAsync(long woeid, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "place/";
            settings.Query = woeid.ToString() + "/siblings";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadContinentsAsync(object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "continents";
            settings.Query = "";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadOceansAsync(object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "oceans";
            settings.Query = "";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadSeasAsync(object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "seas";
            settings.Query = "";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadCountriesAsync(object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "countries";
            settings.Query = "";
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadStatesAsync(PlacesData country, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "states/";
            settings.Query = country.WOEID.ToString();
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadCountyAsync(PlacesData state, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "counties/";
            settings.Query = state.WOEID.ToString();
            base.DownloadAsync(settings, userArgs);
        }
        public void DownloadDistrictsAsync(PlacesData county, object userArgs)
        {
            PlacesDownloadSettings settings = (PlacesDownloadSettings)this.Settings.Clone();
            settings.Collection = "districts/";
            settings.Query = county.WOEID.ToString();
            base.DownloadAsync(settings, userArgs);
        }

        protected override PlacesResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            if (settings is PlacesDownloadSettings)
            {
                List<PlacesData> places = new List<PlacesData>();
                XDocument doc = MyHelper.ParseXmlDocument(stream);
                XElement[] results = XPath.GetElements("//place",doc);
                foreach (XElement node in results)
                {
                    places.Add(this.ToPlace(node));
                }
                return new PlacesResult(places.ToArray());
            }
            else
            {
                return null;
            }
        }



        private string KeywordQuery(string keyword, PlaceType type)
        {
            string query = string.Empty;
            string q = string.Format(".q({0})", Uri.EscapeDataString(keyword));
            if (type != PlaceType.Undefined)
            {
                string t = string.Format(".type({0})", Convert.ToInt32(type).ToString());
                query = string.Format("$and({0},{1})", q, t);
            }
            else
            {
                query = q;
            }
            return query;
        }
        private string WoeidQuery(IEnumerable<long> woeids)
        {
            System.Text.StringBuilder query = new System.Text.StringBuilder(".woeid(");
            List<long> lst = new List<long>();
            if (woeids != null)
                lst.AddRange(woeids);
            if (lst.Count == 1)
            {
                query.Append(lst[0].ToString());
            }
            else if (lst.Count > 1)
            {
                for (int i = 0; i <= lst.Count - 1; i++)
                {
                    query.Append(lst[i].ToString());
                    if (i != lst.Count - 1)
                        query.Append(',');
                }
            }
            query.Append(")");
            return query.ToString();
        }
        private string ComAncestorQuery(int first, IEnumerable<long> woeids)
        {
            List<long> lst = new List<long>();
            if (woeids != null)
                lst.AddRange(woeids);
            System.Text.StringBuilder query = new System.Text.StringBuilder();
            query.Append(first.ToString());
            query.Append("/common/");
            if (lst.Count == 1)
            {
                query.Append(lst[0].ToString());
            }
            else if (lst.Count > 1)
            {
                for (int i = 1; i <= lst.Count - 1; i++)
                {
                    query.Append(lst[i].ToString());
                    if (i != lst.Count - 1)
                        query.Append("/");
                }
            }
            return query.ToString();
        }


        private GeoPlanet.PlacesData ToPlace(XElement node)
        {
            if (node != null && node.Name.LocalName.ToLower() == "place")
            {
                GeoPlanet.PlacesData p = new GeoPlanet.PlacesData();
                foreach (XElement prpNode in node.Elements())
                {
                    switch (prpNode.Name.LocalName.ToLower())
                    {
                        case "woeid":
                            long l;
                            if (long.TryParse(prpNode.Value, out l)) p.WOEID = l;
                            break;
                        case "placetypename":
                            string code = MyHelper.GetXmlAttributeValue(prpNode, "code");
                            if (code != string.Empty)
                                p.Type = (PlaceType)Convert.ToInt32(code);
                            break;
                        case "name":
                            p.Name = prpNode.Value;
                            break;
                        case "country":
                            GeoPlanet.AdminArea ctr = new GeoPlanet.AdminArea();
                            ctr.Code = MyHelper.GetXmlAttributeValue(prpNode, "code");
                            ctr.Name = prpNode.Value;
                            break;
                        case "admin1":
                            if (prpNode.Value != string.Empty)
                            {
                                GeoPlanet.FederalAdminArea admin1 = new GeoPlanet.FederalAdminArea();
                                admin1.Code = MyHelper.GetXmlAttributeValue(prpNode, "code");
                                admin1.Name = prpNode.Value;
                                string type = MyHelper.GetXmlAttributeValue(prpNode, "type").ToLower();
                                if (type != string.Empty)
                                {
                                    for (GeoPlanet.FederalAdminAreaType i = 0; i <= MaasOne.Geo.GeoPlanet.FederalAdminAreaType.State; i++)
                                    {
                                        if (i.ToString().ToLower() == type)
                                        {
                                            admin1.AdminType = i;
                                            break; // TODO: might not be correct. Was : Exit For
                                        }
                                    }
                                }
                                p.FederalAdmin = admin1;
                            }
                            break;
                        case "admin2":
                            if (prpNode.Value != string.Empty)
                            {
                                GeoPlanet.RegionalAdminArea admin2 = new GeoPlanet.RegionalAdminArea();
                                admin2.Code = MyHelper.GetXmlAttributeValue(prpNode, "code");
                                admin2.Name = prpNode.Value;
                                string type = MyHelper.GetXmlAttributeValue(prpNode, "type").ToLower();
                                if (type != string.Empty)
                                {
                                    for (GeoPlanet.RegionalAdminAreaType i = 0; i <= MaasOne.Geo.GeoPlanet.RegionalAdminAreaType.Province; i++)
                                    {
                                        if (i.ToString().ToLower() == type)
                                        {
                                            admin2.AdminType = i;
                                            break; // TODO: might not be correct. Was : Exit For
                                        }
                                    }
                                }
                                p.RegionalAdmin = admin2;
                            }
                            break;
                        case "admin3":
                            if (prpNode.Value != string.Empty)
                            {
                                p.LocalAdmin = new GeoPlanet.LocalAdminArea();
                                p.LocalAdmin.Code = MyHelper.GetXmlAttributeValue(prpNode, "code");
                                p.LocalAdmin.Name = prpNode.Value;
                                string type = MyHelper.GetXmlAttributeValue(prpNode, "type").ToLower();
                                if (type != string.Empty)
                                {
                                    for (GeoPlanet.LocalAdminAreaType i = 0; i <= MaasOne.Geo.GeoPlanet.LocalAdminAreaType.Ward; i++)
                                    {
                                        if (i.ToString().ToLower() == type)
                                        {
                                            p.LocalAdmin.AdminType = i;
                                            break; // TODO: might not be correct. Was : Exit For
                                        }
                                    }
                                }
                            }
                            break;
                        case "locality1":
                            if (prpNode.Value != string.Empty)
                            {
                                p.Locality1 = new GeoPlanet.Locality();
                                p.Locality1.Type = MyHelper.GetXmlAttributeValue(prpNode, "type").ToLower();
                                p.Locality1.Name = prpNode.Value;
                            }
                            break;
                        case "locality2":
                            if (prpNode.Value != string.Empty)
                            {
                                p.Locality2 = new GeoPlanet.Locality();
                                p.Locality2.Type = MyHelper.GetXmlAttributeValue(prpNode, "type").ToLower();
                                p.Locality2.Name = prpNode.Value;
                            }
                            break;
                        case "postal":
                            if (prpNode.Value != string.Empty)
                                p.PostalCode = prpNode.Value;
                            break;
                        case "centroid":
                            p.Center = GetCoordinates(prpNode);
                            break;
                        case "boundingbox":
                            CoordinatesRectangle b = new CoordinatesRectangle();
                            foreach (XElement directionNode in prpNode.Elements())
                            {
                                switch (directionNode.Name.LocalName.ToLower())
                                {
                                    case "southwest":
                                        b.SouthWest = GetCoordinates(directionNode);
                                        break;
                                    case "northeast":
                                        b.NorthEast = GetCoordinates(directionNode);
                                        break;
                                }
                            }

                            p.BoundingBox = b;
                            break;
                        case "arearank":
                            int i1 = 0;
                            int.TryParse(prpNode.Value, out i1);
                            p.AreaInSquareKilometers = GetAreaRank(i1);
                            break;
                        case "poprank":
                            int.TryParse(prpNode.Value, out i1);
                            p.PopulationCount = GetPopRank(i1);
                            break;
                    }
                }
                return p;
            }
            else
            {
                return null;
            }
        }


        private Coordinates GetCoordinates(XElement node)
        {
            double lat = 0;
            double lng = 0;
            foreach (XElement coordNode in node.Elements())
            {
                switch (coordNode.Name.LocalName.ToLower())
                {
                    case "latitude":
                        double.TryParse(coordNode.Value, System.Globalization.NumberStyles.Any, new System.Globalization.CultureInfo("en-US"), out lat);
                        break;
                    case "longitude":
                        double.TryParse(coordNode.Value, System.Globalization.NumberStyles.Any, new System.Globalization.CultureInfo("en-US"), out lng);
                        break;
                }
            }
            return new Coordinates(lng, lat);
        }
        private GeoPlanet.ValueRange GetPopRank(int type)
        {
            switch (type)
            {
                case 19:
                    return new GeoPlanet.ValueRange(type, 1000000000, 3000000000L);
                case 20:
                    return new GeoPlanet.ValueRange(type, 3000000000L, 10000000000L);
                default:
                    return GetAreaRank(type);
            }
        }
        private GeoPlanet.ValueRange GetAreaRank(int type)
        {
            switch (type)
            {
                case 1:
                    return new GeoPlanet.ValueRange(type, 1, 3);
                case 2:
                    return new GeoPlanet.ValueRange(type, 3, 10);
                case 3:
                    return new GeoPlanet.ValueRange(type, 10, 30);
                case 4:
                    return new GeoPlanet.ValueRange(type, 30, 100);
                case 5:
                    return new GeoPlanet.ValueRange(type, 100, 300);
                case 6:
                    return new GeoPlanet.ValueRange(type, 300, 1000);
                case 7:
                    return new GeoPlanet.ValueRange(type, 1000, 3000);
                case 8:
                    return new GeoPlanet.ValueRange(type, 3000, 10000);
                case 9:
                    return new GeoPlanet.ValueRange(type, 10000, 30000);
                case 10:
                    return new GeoPlanet.ValueRange(type, 30000, 100000);
                case 11:
                    return new GeoPlanet.ValueRange(type, 100000, 300000);
                case 12:
                    return new GeoPlanet.ValueRange(type, 300000, 1000000);
                case 13:
                    return new GeoPlanet.ValueRange(type, 1000000, 3000000);
                case 14:
                    return new GeoPlanet.ValueRange(type, 3000000, 10000000);
                case 15:
                    return new GeoPlanet.ValueRange(type, 10000000, 30000000);
                case 16:
                    return new GeoPlanet.ValueRange(type, 30000000, 100000000);
                case 17:
                    return new GeoPlanet.ValueRange(type, 100000000, 300000000);
                case 18:
                    return new GeoPlanet.ValueRange(type, 300000000, 1000000000);
                default:
                    return new GeoPlanet.ValueRange();
            }
        }


    }




    public class PlacesResult
    {

        private PlacesData[] mItems = null;
        public PlacesData[] Items { get { return mItems; } }

        internal PlacesResult(PlacesData[] items)
        {
            mItems = items;
        }
    }




    public class PlacesData : IPlace
    {

        public string Address
        {
            get { return this.Name; }
        }
        public int Radius
        {
            get
            {
                double a = Math.Abs(this.BoundingBox.NorthWest.Latitude - this.BoundingBox.SouthWest.Latitude) / 180 * 42000;
                double b = Math.Abs(this.BoundingBox.SouthWest.Longitude - this.BoundingBox.SouthEast.Longitude) / 360 * 42000;
                double c = (Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)) / 2) * 1000;
                return Convert.ToInt32(c);
            }
            set
            {
                throw new NotSupportedException("Setting a value is not supported");
            }
        }
        public string Name { get; set; }
        public PlaceType Type { get; set; }
        PlaceType IPlace.WOEType
        {
            get { return Type; }
            set { Type = value; }
        }
        public long WOEID { get; set; }
        public Coordinates Center { get; set; }
        Coordinates IPlace.Position
        {
            get { return Center; }
            set { Center = value; }
        }
        public CoordinatesRectangle BoundingBox { get; set; }
        public string PostalCode { get; set; }
        string IPlace.ZipCode
        {
            get { return PostalCode; }
            set { PostalCode = value; }
        }
        public Locality Locality1 { get; set; }
        public Locality Locality2 { get; set; }
        public AdminArea Country { get; set; }
        public LocalAdminArea LocalAdmin { get; set; }
        public RegionalAdminArea RegionalAdmin { get; set; }
        public FederalAdminArea FederalAdmin { get; set; }
        public ValueRange AreaInSquareKilometers { get; set; }
        public ValueRange PopulationCount { get; set; }

        public override string ToString()
        {
            string res = string.Empty;

            if (this.Address != string.Empty)
            {
                foreach (string s in this.Address.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    res += s.Trim() + " ";
                }
            }
            if (this.PostalCode != string.Empty)
            {
                res += this.PostalCode + ", ";
            }
            if (this.Locality1 != null)
            {
                res += this.Locality1.ToString() + ", ";
            }
            if (this.Locality2 != null)
            {
                res += this.Locality2.ToString() + ", ";
            }
            if (this.LocalAdmin != null)
            {
                res += this.LocalAdmin.ToString() + ", ";
            }
            if (this.RegionalAdmin != null)
            {
                res += this.RegionalAdmin.ToString() + ", ";
            }
            if (this.FederalAdmin != null)
            {
                res += this.FederalAdmin.ToString() + ", ";
            }
            if (this.Country != null)
            {
                res += this.Country.ToString() + ", ";
            }

            if (res == string.Empty)
            {
                if (this.Name != string.Empty)
                {
                    res = this.Name;

                }
            }
            else
            {
                res = res.Substring(0, res.Length - 1);
            }

            return res;
        }

    }


    public class AdminArea
    {
        private string mCode = string.Empty;
        private string mName = string.Empty;
        public string Code
        {
            get { return mCode; }
            set { mCode = value; }
        }
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        public override string ToString()
        {
            return this.Name;
        }
    }


    public class FederalAdminArea : AdminArea
    {
        private FederalAdminAreaType mAdminType = FederalAdminAreaType.Canton;
        public FederalAdminAreaType AdminType
        {
            get { return mAdminType; }
            set { mAdminType = value; }
        }
    }


    public class RegionalAdminArea : AdminArea
    {
        private RegionalAdminAreaType mAdminType = RegionalAdminAreaType.County;
        public RegionalAdminAreaType AdminType
        {
            get { return mAdminType; }
            set { mAdminType = value; }
        }
    }


    public class LocalAdminArea : AdminArea
    {
        private LocalAdminAreaType mAdminType = LocalAdminAreaType.Commune;
        public LocalAdminAreaType AdminType
        {
            get { return mAdminType; }
            set { mAdminType = value; }
        }
    }


    public class Locality
    {

        private string mName = string.Empty;

        private string mType = string.Empty;
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        public string Type
        {
            get { return mType; }
            set { mType = value; }
        }

        public override string ToString()
        {
            return this.Name;
        }

    }


    public class ValueRange : Range<long>
    {


        private int mType;
        public int Type
        {
            get { return mType; }
            set { mType = value; }
        }
        public ValueRange()
        {
        }
        public ValueRange(int type, long fromVal, long toVal)
        {
            mType = type;
            this.Minimum = fromVal;
            this.Maximum = toVal;
        }


    }




    public class PlacesDownloadSettings : Base.SettingsBase, IResultIndexSettings
    {
        public int Index { get; set; }
        public int Count { get; set; }
        public string ApplicationID { get; set; }
        public Culture Culture { get; set; }
        internal string Collection { get; set; }
        internal string Query { get; set; }

        public PlacesDownloadSettings()
        {
            this.Culture = new Culture(Language.en, Country.US);
            this.Collection = string.Empty;
            this.Query = string.Empty;
            this.Count = 10;
            this.ApplicationID = string.Empty;
        }
        public PlacesDownloadSettings(IResultIndexSettings resultsCount)
        {
            this.Index = resultsCount.Index;
            this.Count = resultsCount.Count;
            this.Culture = new Culture(Language.en, Country.US);
            this.Collection = string.Empty;
            this.Query = string.Empty;
        }
        public PlacesDownloadSettings(IResultIndexSettings resultsCount, string appID)
        {
            this.Index = resultsCount.Index;
            this.Count = resultsCount.Count;
            this.ApplicationID = appID;
            this.Culture = new Culture(Language.en, Country.US);
            this.Collection = string.Empty;
            this.Query = string.Empty;
        }


        protected override string GetUrl()
        {
            if (this.ApplicationID == null || this.ApplicationID.Trim() == string.Empty)
                throw new ArgumentNullException("appID", "The Yahoo! Application ID is empty. You need to pass a valid Application ID to use Yahoo! Web Services. For more information go to: http://developer.yahoo.com/faq/#appid");
            string startCount = string.Empty;
            string cult = string.Empty;
            if (this.Culture != null & this.Collection.StartsWith("place"))
            {
                cult = string.Format("&lang={0}-{1}", this.Culture.Language.ToString(), this.Culture.Country.ToString());
            }
            if (this != null && (this.Index >= 0 & this.Count > 0))
                startCount = ";start=" + this.Index.ToString() + ";count=" + this.Count.ToString();
            string url = string.Format("http://where.yahooapis.com/v1/{0}{1}{2}?appid={3}&format=xml&view=long{4}", this.Collection, this.Query, startCount, this.ApplicationID, cult);
            return url;
        }


        public override object Clone()
        {
            return new PlacesDownloadSettings() { Index = this.Index, Count = this.Count, ApplicationID = this.ApplicationID, Collection = this.Collection, Culture = this.Culture, Query = this.Query };
        }


        public PlacesDownloadSettings NewQuery(string collection, string query)
        {
            this.Collection = collection;
            this.Query = query;
            return this;
        }
    }





}
