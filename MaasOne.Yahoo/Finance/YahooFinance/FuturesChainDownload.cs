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
using System.Collections.Generic;
using System.Text;
using MaasOne.Xml;
using System.Xml.Linq;


namespace MaasOne.Finance.YahooFinance
{



    /// <summary>
    /// Provides methods for downloading key statistics of major companies.
    /// </summary>
    /// <remarks></remarks>
    public partial class FuturesChainDownload : Base.DownloadClient<FuturesResult>
    {

        public FuturesChainDownloadSettings Settings { get { return (FuturesChainDownloadSettings)base.Settings; } set { base.SetSettings(value); } }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public FuturesChainDownload()
        {
            this.Settings = new FuturesChainDownloadSettings();
        }


        /// <summary>
        /// Starts an asynchronous download of company statistic data.
        /// </summary>
        /// <param name="unmanagedID">The unmanaged ID</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadAsync(IID managedID, object userArgs = null)
        {
            if (managedID == null)
                throw new ArgumentNullException("unmanagedID", "The passed ID is empty.");
            this.DownloadAsync(managedID.ID, userArgs);
        }
        /// <summary>
        /// Starts an asynchronous download of company statistic data.
        /// </summary>
        /// <param name="unmanagedID">The unmanaged ID</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadAsync(string unmanagedID, object userArgs = null)
        {
            if (unmanagedID == string.Empty)
                throw new ArgumentNullException("unmanagedID", "The passed ID is empty.");
            this.DownloadAsync(new FuturesChainDownloadSettings(unmanagedID), userArgs);
        }

        public void DownloadAsync(FuturesChainDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        protected override FuturesResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            FutureData[] result = null;
            if (stream != null)
            {
                System.Globalization.CultureInfo convCulture = new System.Globalization.CultureInfo("en-US");
                XDocument doc = MyHelper.ParseXmlDocument(stream);
                XElement resultNode = XPath.GetElement("//table[@id=\"yfncsumtab\"]/table/tr/td/table[2]/tr/td/table",doc);

                if (resultNode != null)
                {
                    System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
                    List<FutureData> lst = new List<FutureData>();
                    int cnt = 0;
                    foreach (XElement node in resultNode.Elements())
                    {
                        if (node.Name.LocalName == "tr")
                        {
                            cnt++;
                            if (cnt > 1)
                            {
                                try
                                {
                                    FutureData data = new FutureData();
                                    double d; XElement tempNode = null;

                                    tempNode = XPath.GetElement("/td[1]", node);
                                    if (tempNode != null) data.SetID(tempNode.Value);

                                    tempNode = XPath.GetElement("/td[2]", node);
                                    if (tempNode != null) data.Name = tempNode.Value;

                                    tempNode = XPath.GetElement("/td[3]/b", node);
                                    if (tempNode != null && double.TryParse(tempNode.Value, System.Globalization.NumberStyles.Any, ci, out d)) data.LastTradePriceOnly = d;

                                    tempNode = XPath.GetElement("/td[3]/nobr/small", node);
                                    if (tempNode != null) data.LastTradeTime = tempNode.Value;

                                    tempNode = XPath.GetElement("/td[4]/b[1]", node);
                                    if (tempNode != null && double.TryParse(tempNode.Value, System.Globalization.NumberStyles.Any, ci, out d)) data.Change = d;

                                    tempNode = XPath.GetElement("/td[4]/b[2]", node);
                                    if (tempNode != null && double.TryParse(tempNode.Value.Replace("(", "").Replace(")", "").Replace("%", ""), System.Globalization.NumberStyles.Any, ci, out d)) data.ChangeInPercent = d;
                                    
                                    lst.Add(data);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                    result = lst.ToArray();
                }
            }
            return new FuturesResult(((FuturesChainDownloadSettings)settings).ID, result);
        }

    }


    /// <summary>
    /// Stores the result data
    /// </summary>
    public class FuturesResult
    {

        private string mID = string.Empty;
        public string ID { get { return mID; } }

        private FutureData[] mItems = null;
        public FutureData[] Items { get { return mItems; } }

        internal FuturesResult(string id, FutureData[] items)
        {
            mID = id;
            mItems = items;
        }

    }







    public class FutureData : ISettableID
    {
        private string mID = string.Empty;
        public string ID { get { return mID; } }
        public void SetID(string value)
        {
            mID = value;
        }
        public string Name { get; set; }
        public double LastTradePriceOnly { get; set; }
        public string LastTradeTime { get; set; }
        public double Change { get; set; }
        public double ChangeInPercent { get; set; }

    }






    public class FuturesChainDownloadSettings : Base.SettingsBase
    {


        public string ID { get; set; }

        public FuturesChainDownloadSettings()
        {
            this.ID = string.Empty;
        }
        public FuturesChainDownloadSettings(string id)
        {
            this.ID = id;
        }


        protected override string GetUrl()
        {
            if (this.ID == string.Empty) { throw new ArgumentException("ID is empty.", "ID"); }
            return string.Format("http://finance.yahoo.com/q/fc?s={0}", this.ID);
        }

        public override object Clone()
        {
            return new FuturesChainDownloadSettings(this.ID);
        }
    }
}
