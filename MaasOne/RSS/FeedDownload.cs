// ******************************************************************************
// ** 
// **  MaasOne WebServices
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


namespace MaasOne.RSS
{


    /// <summary>
    /// Class for downloading RSS feeds
    /// </summary>
    /// <remarks></remarks>
    public partial class FeedDownload : Base.DownloadClient<FeedResult>
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public FeedDownload()
        {
            this.SetSettings(new FeedDownloadSettings());
        }

        public void DownloadAsync(string url, object userArgs)
        {
            if (url == string.Empty)
                throw new ArgumentNullException("url", "The url is empty.");
            this.DownloadAsync(new Uri(url), userArgs);
        }
        public void DownloadAsync(Uri url, object userArgs)
        {
            if (url == null)
                throw new ArgumentNullException("url", "The url is null.");
            this.DownloadAsync(new Uri[] { url }, userArgs);
        }
        public void DownloadAsync(IEnumerable<string> urls, object userArgs)
        {
            if (urls == null)
                throw new ArgumentNullException("urls", "The list is null.");
            List<Uri> lst = new List<Uri>();
            foreach (string url in urls)
            {
                lst.Add(new Uri(url));
            }
            this.DownloadAsync(lst, userArgs);
        }
        public void DownloadAsync(IEnumerable<Uri> urls, object userArgs)
        {
            if (urls == null)
                throw new ArgumentNullException("urls", "The list is null.");
            base.DownloadAsync(new FeedDownloadSettings(urls), userArgs);
        }

        protected override FeedResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            List<Feed> feeds = new List<Feed>();
            XDocument xmlDoc = MyHelper.ParseXmlDocument(stream);
            if (xmlDoc != null)
            {
                foreach (XElement f in XPath.GetElements("//channel",xmlDoc))
                {
                    feeds.Add(ImportExport.XML.ToFeed(f));
                }
            }
            return new FeedResult(feeds.ToArray(), (FeedDownloadSettings)settings);
        }

    }



    public class FeedResult 
    {
        private FeedDownloadSettings mSettings = null;
        public FeedDownloadSettings Settings { get { return mSettings; } }

        private Feed[] mFeeds = null;
        public Feed[] Feeds
        {
            get { return mFeeds; }
        }

        public FeedResult(Feed[] feeds, FeedDownloadSettings settings)
        {
            mFeeds = feeds;
            mSettings = settings;
        }

       
    }



    public class FeedDownloadSettings : Base.SettingsBase
    {

        public Uri[] URLs { get; set; }

        public FeedDownloadSettings()
        {
            this.URLs = new Uri[] { };
        }
        public FeedDownloadSettings(Uri url)
        {
            this.URLs = new Uri[] { url };
        }
        public FeedDownloadSettings(IEnumerable<Uri> urls)
        {
            this.URLs = MyHelper.EnumToArray(urls);
        }

        protected override string GetUrl()
        {
            Uri[] arr = MyHelper.EnumToArray(this.URLs);
            if (arr.Length > 0)
            {
                if (arr.Length == 1)
                {
                    return arr[0].ToString();
                }
                else
                {
                    System.Text.StringBuilder whereClause = new System.Text.StringBuilder("url in (");
                    for (int i = 0; i <= arr.Length - 1; i++)
                    {
                        whereClause.Append("'");
                        whereClause.Append(arr[i].ToString());
                        whereClause.Append("'");
                        if (i < arr.Length - 1)
                            whereClause.Append(",");
                    }
                    whereClause.Append(")");
                    return MyHelper.YqlUrl("*", "xml", whereClause.ToString(), null, false);
                }
            }
            else
            {
                throw new ArgumentException("The list of urls is empty", "urls");
            }
        }

        public override object Clone()
        {
            return new FeedDownloadSettings((Uri[])this.URLs.Clone()); ;
        }

    }




}
