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


namespace MaasOne.Search.BOSS
{



    public partial class SuggestionDownload : Base.DownloadClient<SuggestionResult>
    {


        public void DownloadAsync(string query, YahooServer searchSvr, object userArgs)
        {
            this.DownloadAsync(new SuggestionDownloadSettings() { Query = query, Server = searchSvr }, userArgs);
        }
        public void DownloadAsync(SuggestionDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public SuggestionDownload()
        {
        }

        protected override SuggestionResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            List<string> lst = new List<string>();
            XDocument doc = MyHelper.ParseXmlDocument(stream);
            XElement[] results = XPath.GetElements("//s",doc);
            foreach (XElement resultNode in results)
            {
                string att = MyHelper.GetXmlAttributeValue(resultNode, "k");
                if (att != string.Empty)
                    lst.Add(att);
            }
            return new SuggestionResult(lst.ToArray(), (SuggestionDownloadSettings)settings);
        }

    }



    public class SuggestionResult
    {

        private SuggestionDownloadSettings mSettings = null;
        public SuggestionDownloadSettings Settings { get { return mSettings; } }

        private string[] mItems = null;
        public string[] Items { get { return mItems; } }

        internal SuggestionResult(string[] items, SuggestionDownloadSettings settings)
        {
            mSettings = settings;
            mItems = items;
        }
    }

    public class SuggestionDownloadSettings : Base.SettingsBase
    {


        public YahooServer Server { get; set; }
        public string Query { get; set; }

        public SuggestionDownloadSettings()
        {
            this.Query = string.Empty;
            this.Server = YahooServer.USA;
        }

        protected override string GetUrl()
        {
            if (this.Server == YahooServer.YQL)
            {
                return MyHelper.YqlUrl("*", "yahoo.search.suggestions", "command=\"" + this.Query + '"', null, false);
            }
            else
            {
                System.Text.StringBuilder url = new System.Text.StringBuilder();
                url.Append("http://sugg.");
                url.Append(YahooHelper.ServerString(this.Server));
                url.Append("search.yahoo.com/gossip-");
                if (this.Server == YahooServer.USA)
                {
                    url.Append("us");
                }
                else
                {
                    url.Append(YahooHelper.ServerString(this.Server).Replace(".", ""));
                }
                url.Append("-sayt/?output=xml&nresults=10");
                url.Append("&command=");
                url.Append(Uri.EscapeDataString(this.Query));
                return url.ToString();
            }
        }

        public override object Clone()
        {
            return new SuggestionDownloadSettings() { Server = this.Server, Query = this.Query };
        }

    }

}
