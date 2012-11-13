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
using System.Xml.Linq;


namespace MaasOne.Weather.YahooWeather
{

    public partial class LocationIDSearchDownload : Base.DownloadClient<LocationIDSearchResult>
    {

        public LocationIDSearchDownloadSettings Settings { get { return (LocationIDSearchDownloadSettings)base.Settings; } set { base.SetSettings(value); } }

        public LocationIDSearchDownload()
        {
        }


        public void DownloadAsync(string keyword, MaasOne.IResultIndexSettings opt, object userArgs)
        {
            this.DownloadAsync(new LocationIDSearchDownloadSettings() { Keywords = new string[] { keyword }, Count = opt.Count, Index = opt.Index }, userArgs);
        }
        public void DownloadAsync(LocationIDSearchDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        protected override LocationIDSearchResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            List<LocationIDData> lst = new List<LocationIDData>();
            XDocument doc = MyHelper.ParseXmlDocument(stream);
            XElement[] results = XPath.GetElements("//loc",doc);
            foreach (XElement locNode in results)
            {
                LocationIDData loc = new LocationIDData();
                loc.Name = locNode.Value;
                XAttribute att = locNode.Attribute(XName.Get("id"));
                if (att != null)
                    loc.ID = att.Value;
                lst.Add(loc);
            }
            return new LocationIDSearchResult(lst.ToArray());
        }

    }




    public class LocationIDSearchResult
    {
        private LocationIDData[] mItems = null;
        public LocationIDData[] Items
        {
            get { return mItems; }
        }
        internal LocationIDSearchResult(LocationIDData[] items)
        {
            mItems = items;
        }
    }


    


    public class LocationIDSearchDownloadSettings : Base.SettingsBase, IResultIndexSettings
    {

        public int Index { get; set; }
        public int Count { get; set; }
        public string[] Keywords { get; set; }

        public LocationIDSearchDownloadSettings()
        {
            this.Keywords = new string[] { };
            this.Count = 10;
        }

        protected override string GetUrl()
        {
            System.Text.StringBuilder whereClause = new System.Text.StringBuilder();
            whereClause.Append("query in (");
            List<string> lst = new List<string>();
            if (this.Keywords != null)
                lst.AddRange(this.Keywords);
            if (lst.Count > 0)
            {
                for (int i = 0; i <= lst.Count - 1; i++)
                {
                    whereClause.Append('"');
                    whereClause.Append(lst[i].ToString());
                    whereClause.Append('"');
                    if (i != lst.Count - 1)
                        whereClause.Append(',');
                }
            }
            whereClause.Append(')');

            return MyHelper.YqlUrl("*", "weather.search", whereClause.ToString(), this, false);
        }

        public override object Clone()
        {
            return new LocationIDSearchDownloadSettings() { Index = this.Index, Count = this.Count, Keywords = (string[])this.Keywords.Clone() };
        }

    }



}
