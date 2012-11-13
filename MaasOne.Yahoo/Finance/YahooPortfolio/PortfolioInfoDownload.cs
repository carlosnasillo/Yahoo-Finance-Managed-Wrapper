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
using MaasOne.Base;
using MaasOne.Xml;
using System.Xml.Linq;


namespace MaasOne.Finance.YahooPortfolio
{



    public class PortfolioInfoDownload : DownloadClient<PortfolioInfoResult>
    {
        public PortfolioInfoDownloadSettings Settings { get { return (PortfolioInfoDownloadSettings)base.Settings; } set { base.SetSettings(value); } }

        public PortfolioInfoDownload()
        {
            this.Settings = new PortfolioInfoDownloadSettings();
        }

        protected override PortfolioInfoResult ConvertResult(ConnectionInfo connInfo, System.IO.Stream stream, SettingsBase settings)
        {
            XDocument doc = MyHelper.ParseXmlDocument(stream);
                return this.ConvertHtml(doc);           
        }


        internal PortfolioInfoResult ConvertHtml(XDocument doc)
        {
            XElement resultsNode = XPath.GetElement("//div[@id=\"yfi-main\"]/div/div[2]/form/div/table/tbody",doc);
            List<PortfolioInfo> lst = new List<PortfolioInfo>();
            if (resultsNode != null)
            {
                foreach (XElement trNode in resultsNode.Elements())
                {
                    XElement a = XPath.GetElement("/td[2]/a",trNode);
                    if (a != null)
                    {
                        string name = a.Value;
                        XAttribute att = a.Attribute(XName.Get("href"));
                        if (att != null)
                        {
                            string id = att.Value.Split(';')[0].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1];
                            lst.Add(new PortfolioInfo(name, id));
                        }
                    }
                }
            }
            return new PortfolioInfoResult(lst.ToArray());
        }

    }


    public class PortfolioInfoResult
    {
        private PortfolioInfo[] mItems = null;
        public PortfolioInfo[] Items { get { return mItems; } }
        internal PortfolioInfoResult(PortfolioInfo[] items)
        {
            mItems = items;
        }
    }

    public class PortfolioInfo
    {
        private string mName = string.Empty;
        public string Name { get { return mName; } }
        private string mID = string.Empty;
        public string ID { get { return mID; } }
        public PortfolioInfo(string name, string id)
        {
            mName = name;
            mID = id;
        }
        public override string ToString()
        {
            return mName;
        }
    }


    public class PortfolioInfoDownloadSettings : SettingsBase
    {
        public YAccountManager Account { get; set; }

        protected override System.Net.CookieContainer Cookies { get { return this.Account != null ? this.Account.Cookies : null; } }

        protected override string GetUrl()
        {
            return "http://finance.yahoo.com/portfolios/manage";
        }

        public override object Clone()
        {
            return new PortfolioInfoDownloadSettings() { Account = this.Account };
        }
    }


}
