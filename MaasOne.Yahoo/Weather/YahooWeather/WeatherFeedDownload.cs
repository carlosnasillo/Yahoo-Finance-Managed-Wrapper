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


namespace MaasOne.Weather.YahooWeather
{


    public partial class WeatherFeedDownload : Base.DownloadClient<WeatherFeedResult>
    {


        public WeatherFeedDownloadSettings Settings { get { return (WeatherFeedDownloadSettings)base.Settings; } set { base.SetSettings(value); } }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public WeatherFeedDownload()
        {
        }




        public void DownloadAsync(IEnumerable<long> woeids, bool metricValues, object userArgs = null)
        {
            if (woeids == null)
            {
                throw new ArgumentNullException("woeids", "The passed list is null.");
            }
            else
            {
                base.DownloadAsync(new WeatherFeedWOEIDDownloadSettings() { IsMetric = metricValues, WOEIDS = MyHelper.EnumToArray(woeids) }, userArgs);
            }
        }
        public void DownloadAsync(IEnumerable<LocationIDData> locations, bool metricValues, object userArgs = null)
        {
            if (locations == null)
            {
                throw new ArgumentNullException("locations", "The passed list is null.");
            }
            else
            {
                base.DownloadAsync(new WeatherFeedLocationIDDownloadSettings() { IsMetric = metricValues, Locations = MyHelper.EnumToArray(locations) }, userArgs);
            }
        }
        public void DownloadAsync(string keyword, bool metricValues, IResultIndexSettings opt, object userArgs = null)
        {
            this.DownloadAsync(new string[] { keyword }, metricValues, opt);
        }
        public void DownloadAsync(IEnumerable<string> keywords, bool metricValues, IResultIndexSettings opt, object userArgs = null)
        {
            if (keywords == null)
            {
                throw new ArgumentNullException("keywords", "The passed list is null.");
            }
            else
            {
                base.DownloadAsync(new WeatherFeedKeywordDownloadSettings() { Count = opt.Count, Index = opt.Index, IsMetric = metricValues, Keywords = MyHelper.EnumToArray(keywords) }, userArgs);
            }
        }

        protected override WeatherFeedResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            List<WeatherFeed> feeds = new List<WeatherFeed>();
            XDocument xmlDoc = MyHelper.ParseXmlDocument(stream);
            if (xmlDoc != null)
            {
                foreach (XElement feedNode in XPath.GetElements("//channel",xmlDoc))
                {
                    WeatherFeed feed = this.ToWeatherFeed(feedNode);
                    if (feed != null)
                        feeds.Add(feed);
                }
            }
            return new WeatherFeedResult(feeds.ToArray());
        }



        private WeatherFeed ToWeatherFeed(XElement channelNode)
        {
            System.Globalization.CultureInfo convCulture = new System.Globalization.CultureInfo("en-US");

            if (channelNode != null)
            {
                RSS.Feed defaultFeed = RSS.ImportExport.XML.ToFeed(channelNode);
                if (defaultFeed != null)
                {
                    defaultFeed.Items.Clear();
                    WeatherFeed feed = new WeatherFeed();
                    feed.CopyValues(defaultFeed);

                    foreach (XElement prpNode in channelNode.Elements())
                    {
                        if (prpNode.Name.NamespaceName.ToLower() == "yweather")
                        {
                            switch (prpNode.Name.LocalName.ToLower())
                            {
                                case "location":
                                    feed.Location = new LocationInfo();
                                    foreach (XAttribute att in prpNode.Attributes())
                                    {
                                        switch (att.Name.LocalName)
                                        {
                                            case "city":
                                                feed.Location.City = att.Value;
                                                break;
                                            case "region":
                                                feed.Location.Region = att.Value;
                                                break;
                                            case "country":
                                                feed.Location.Country = att.Value;
                                                break;
                                        }
                                    }

                                    break;
                                case "units":
                                    foreach (XAttribute att in prpNode.Attributes())
                                    {
                                        switch (att.Name.LocalName)
                                        {
                                            case "temperature":
                                                if (att.Value.ToLower() == "f")
                                                {
                                                    feed.TemperatureUnit = DegreesUnit.Fahrenheit;
                                                }
                                                else
                                                {
                                                    feed.TemperatureUnit = DegreesUnit.Celsius;
                                                }
                                                break;
                                            case "distance":
                                                if (att.Value.ToLower() == "mi")
                                                {
                                                    feed.DistanceUnit = DistanceUnit.Miles;
                                                }
                                                else
                                                {
                                                    feed.DistanceUnit = DistanceUnit.Kilometer;
                                                }
                                                break;
                                            case "pressure":
                                                if (att.Value.ToLower() == "in")
                                                {
                                                    feed.PressureUnit = PressureUnit.PoundsPerSquareInch;
                                                }
                                                else
                                                {
                                                    feed.PressureUnit = PressureUnit.Milibars;
                                                }
                                                break;
                                            case "speed":
                                                if (att.Value.ToLower() == "mph")
                                                {
                                                    feed.SpeedUnit = DistanceUnit.Miles;
                                                }
                                                else
                                                {
                                                    feed.SpeedUnit = DistanceUnit.Kilometer;
                                                }
                                                break;
                                        }
                                    }

                                    break;
                                case "wind":
                                    feed.Wind = new WindInfo();
                                    foreach (XAttribute att in prpNode.Attributes())
                                    {
                                        switch (att.Name.LocalName)
                                        {
                                            case "chill":
                                                int n;
                                                if (int.TryParse(att.Value, out n)) feed.Wind.Chill = n;
                                                break;
                                            case "direction":
                                                if (int.TryParse(att.Value, out n)) feed.Wind.Direction = n;
                                                break;
                                            case "speed":
                                                double t;
                                                if (double.TryParse(att.Value, System.Globalization.NumberStyles.Any, convCulture, out t)) feed.Wind.Speed = t;
                                                break;
                                        }
                                    }

                                    break;
                                case "atmosphere":
                                    feed.Atmosphere = new AtmosphereInfo();
                                    foreach (XAttribute att in prpNode.Attributes())
                                    {
                                        switch (att.Name.LocalName)
                                        {
                                            case "humidity":
                                                int n;
                                                if (int.TryParse(att.Value, out n)) feed.Atmosphere.HumidityInPercent = n;
                                                break;
                                            case "visibility":
                                                double t;
                                                if (double.TryParse(att.Value, System.Globalization.NumberStyles.Any, convCulture, out t)) feed.Atmosphere.VisibilityDistance = t;
                                                break;
                                            case "pressure":
                                                if (double.TryParse(att.Value, System.Globalization.NumberStyles.Any, convCulture, out t)) feed.Atmosphere.Pressure = t;
                                                break;
                                            case "rising":
                                                if (att.Value == "0")
                                                {
                                                    feed.Atmosphere.StateOfBarometricPressure = PressureState.Steady;
                                                }
                                                else if (att.Value == "1")
                                                {
                                                    feed.Atmosphere.StateOfBarometricPressure = PressureState.Rising;
                                                }
                                                else if (att.Value == "2")
                                                {
                                                    feed.Atmosphere.StateOfBarometricPressure = PressureState.Falling;
                                                }
                                                break;
                                        }
                                    }

                                    break;
                                case "astronomy":
                                    feed.Astronomy = new AstronomyInfo();
                                    foreach (XAttribute att in prpNode.Attributes())
                                    {
                                        switch (att.Name.LocalName)
                                        {
                                            case "sunrise":
                                                DateTime d;
                                                if (System.DateTime.TryParse(att.Value, convCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out d)) feed.Astronomy.Sunrise = d;
                                                break;
                                            case "sunset":
                                                if (System.DateTime.TryParse(att.Value, convCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out d)) feed.Astronomy.Sunset = d;
                                                break;
                                        }
                                    }

                                    break;
                            }


                        }
                        else if (prpNode.Name.LocalName.ToLower() == "item")
                        {
                            RSS.FeedItem defaultItem = RSS.ImportExport.XML.ToFeedItem(prpNode);
                            if (defaultItem != null)
                            {
                                WeatherFeedItem newItem = new WeatherFeedItem();
                                newItem.CopyValues(defaultItem);

                                foreach (XElement itemNode in prpNode.Elements())
                                {
                                    if (itemNode.Name.NamespaceName == "yweather")
                                    {
                                        switch (itemNode.Name.LocalName.ToLower())
                                        {
                                            case "condition":
                                                newItem.ActualCondition = new ActualCondition();
                                                foreach (XAttribute att in itemNode.Attributes())
                                                {
                                                    switch (att.Name.LocalName.ToLower())
                                                    {
                                                        case "text":
                                                            newItem.ActualCondition.Description = att.Value;
                                                            break;
                                                        case "code":
                                                            int i = 0;
                                                            if (int.TryParse(att.Value, out i) && (i <= Convert.ToInt32(ConditionType.Isolated_Thundershowers) | i == Convert.ToInt32(ConditionType.Not_Available)))
                                                            {
                                                                newItem.ActualCondition.Type = (ConditionType)i;
                                                            }
                                                            break;
                                                        case "temp":
                                                            int n;
                                                            if (int.TryParse(att.Value, out n)) newItem.ActualCondition.Temperature = n;
                                                            break;
                                                        case "date":
                                                            //Wed, 17 Aug 2011 10:18 pm CEST

                                                            string dateStr = att.Value;
                                                            int index = Math.Max(dateStr.LastIndexOf("am"), dateStr.LastIndexOf("pm"));
                                                            if (index > 0)
                                                            {
                                                                System.DateTime d = default(System.DateTime);
                                                                if (System.DateTime.TryParseExact(att.Value.Substring(0, index + 2), "ddd, dd MMM yyyy hh:mm tt", convCulture, System.Globalization.DateTimeStyles.None, out d))
                                                                {
                                                                    newItem.ActualCondition.ForecastDate = d;
                                                                }
                                                            }
                                                            break;
                                                    }
                                                }

                                                break;
                                            case "forecast":
                                                Forecast cond = new Forecast();
                                                foreach (XAttribute att in itemNode.Attributes())
                                                {
                                                    switch (att.Name.LocalName.ToLower())
                                                    {
                                                        case "text":
                                                            cond.Description = att.Value;
                                                            break;
                                                        case "code":
                                                            int i = 0;
                                                            if (int.TryParse(att.Value, out i) && (i <= Convert.ToInt32(ConditionType.Isolated_Thundershowers) | i == Convert.ToInt32(ConditionType.Not_Available)))
                                                            {
                                                                cond.Type = (ConditionType)i;
                                                            }
                                                            break;
                                                        case "day":
                                                            switch (att.Value.ToLower())
                                                            {
                                                                case "mon":
                                                                    cond.Day = DayOfWeek.Monday;
                                                                    break;
                                                                case "tue":
                                                                    cond.Day = DayOfWeek.Tuesday;
                                                                    break;
                                                                case "wed":
                                                                    cond.Day = DayOfWeek.Wednesday;
                                                                    break;
                                                                case "thu":
                                                                    cond.Day = DayOfWeek.Thursday;
                                                                    break;
                                                                case "fri":
                                                                    cond.Day = DayOfWeek.Friday;
                                                                    break;
                                                                case "sat":
                                                                    cond.Day = DayOfWeek.Saturday;
                                                                    break;
                                                                case "sun":
                                                                    cond.Day = DayOfWeek.Sunday;
                                                                    break;
                                                            }
                                                            break;
                                                        case "date":
                                                            DateTime d;
                                                            if (System.DateTime.TryParse(att.Value, convCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out d)) cond.ForecastDate = d;
                                                            break;
                                                        case "low":
                                                            int n;
                                                            if (int.TryParse(att.Value, out n)) cond.LowTemperature = n;
                                                            break;
                                                        case "high":
                                                            if (int.TryParse(att.Value, out n)) cond.HighTemperature = n;
                                                            break;
                                                    }
                                                }

                                                newItem.ForecastConditions.Add(cond);
                                                break;
                                        }
                                    }
                                    else if (itemNode.Name.NamespaceName == "geo")
                                    {
                                        switch (itemNode.Name.LocalName.ToLower())
                                        {
                                            case "lat":
                                                double d = 0;
                                                if (double.TryParse(itemNode.Value, System.Globalization.NumberStyles.Any, convCulture, out d))
                                                {
                                                    newItem.Coordinates = new Geo.Coordinates(newItem.Coordinates.Longitude, d);
                                                }
                                                break;
                                            case ("long"):
                                                if (double.TryParse(itemNode.Value, System.Globalization.NumberStyles.Any, convCulture, out d))
                                                {
                                                    newItem.Coordinates = new Geo.Coordinates(d, newItem.Coordinates.Latitude);
                                                }
                                                break;
                                        }
                                    }
                                }
                                feed.Items.Add(newItem);
                            }
                        }

                    }
                    return feed;
                }
            }
            return null;

        }





        internal static void MetricOption(System.Text.StringBuilder sb, bool metric)
        {
            sb.Append("u=\"");
            if (metric)
            {
                sb.Append('c');
            }
            else
            {
                sb.Append('f');
            }
            sb.Append('"');
        }
    }



    public class WeatherFeedResult
    {
        private WeatherFeed[] mItems = null;
        public WeatherFeed[] Items
        {
            get { return mItems; }
        }
        internal WeatherFeedResult(WeatherFeed[] items)
        {
            mItems = items;
        }
    }



    public abstract class WeatherFeedDownloadSettings : Base.SettingsBase
    {

        public bool IsMetric { get; set; }

    }

    public class WeatherFeedWOEIDDownloadSettings : WeatherFeedDownloadSettings
    {

        public long[] WOEIDS { get; set; }

        public WeatherFeedWOEIDDownloadSettings()
        {
            this.WOEIDS = new long[] { };
        }

        protected override string GetUrl()
        {
            List<long> lst = new List<long>();
            if (this.WOEIDS != null)
                lst.AddRange(this.WOEIDS);
            if (lst.Count > 0)
            {
                System.Text.StringBuilder whereClause = new System.Text.StringBuilder();
                whereClause.Append("w in (");
                for (int i = 0; i <= lst.Count - 1; i++)
                {
                    whereClause.Append('"');
                    whereClause.Append(lst[i].ToString());
                    whereClause.Append('"');
                    if (i != lst.Count - 1)
                        whereClause.Append(',');
                }
                whereClause.Append(") and ");
                WeatherFeedDownload.MetricOption(whereClause, this.IsMetric);

                return MyHelper.YqlUrl("*", "weather.woeid", whereClause.ToString(), null, false);
            }
            else
            {
                throw new ArgumentException("There must be minimum one WOEID.", "woeids");
            }
        }


        public override object Clone()
        {
            return new WeatherFeedWOEIDDownloadSettings() { WOEIDS = (long[])this.WOEIDS.Clone(), IsMetric = this.IsMetric };
        }
    }

    public class WeatherFeedLocationIDDownloadSettings : WeatherFeedDownloadSettings
    {

        public LocationIDData[] Locations { get; set; }

        public WeatherFeedLocationIDDownloadSettings()
        {
            this.Locations = new LocationIDData[] { };
        }

        protected override string GetUrl()
        {
            List<LocationIDData> lst = new List<LocationIDData>();
            if (this.Locations != null)
                lst.AddRange(this.Locations);
            if (lst.Count > 0)
            {
                System.Text.StringBuilder whereClause = new System.Text.StringBuilder();
                whereClause.Append("location in (");
                for (int i = 0; i <= lst.Count - 1; i++)
                {
                    whereClause.Append('"');
                    whereClause.Append(lst[i].ID);
                    whereClause.Append('"');
                    if (i != lst.Count - 1)
                        whereClause.Append(',');
                }
                whereClause.Append(") and ");
                WeatherFeedDownload.MetricOption(whereClause, this.IsMetric);

                return MyHelper.YqlUrl("*", "weather.forecast", whereClause.ToString(), null, false);
            }
            else
            {
                throw new ArgumentException("There must be minimum one location.", "locations");
            }
        }

        public override object Clone()
        {
            return new WeatherFeedLocationIDDownloadSettings() { Locations = (LocationIDData[])this.Locations.Clone(), IsMetric = this.IsMetric };
        }
    }

    public class WeatherFeedKeywordDownloadSettings : WeatherFeedDownloadSettings, MaasOne.IResultIndexSettings
    {

        public int Index { get; set; }
        public int Count { get; set; }
        public string[] Keywords { get; set; }

        public WeatherFeedKeywordDownloadSettings()
        {
            this.Keywords = new string[] { };
            this.Count = 10;
        }


        protected override string GetUrl()
        {

            List<string> lst = new List<string>();
            if (this.Keywords != null)
                lst.AddRange(this.Keywords);
            if (lst.Count > 0)
            {
                System.Text.StringBuilder idSearch = new System.Text.StringBuilder("query in (");
                for (int i = 0; i <= lst.Count - 1; i++)
                {
                    idSearch.Append('"');
                    idSearch.Append(lst[i]);
                    idSearch.Append('"');
                    if (i != lst.Count - 1)
                        idSearch.Append(',');
                }
                idSearch.Append(")");

                System.Text.StringBuilder feedSearch = new System.Text.StringBuilder("location in (");
                feedSearch.Append(MyHelper.YqlStatement("id", "weather.search", idSearch.ToString(), null));
                feedSearch.Append(") and ");
                WeatherFeedDownload.MetricOption(feedSearch, this.IsMetric);

                return MyHelper.YqlUrl("*", "weather.forecast", feedSearch.ToString(), this, false);
            }
            else
            {
                throw new ArgumentException("There must be minimum one keyword.", "keywords");
            }
        }

        public override object Clone()
        {
            return new WeatherFeedKeywordDownloadSettings() { Keywords = this.Keywords, IsMetric = this.IsMetric, Index = this.Index, Count = this.Count };
        }
    }


}
