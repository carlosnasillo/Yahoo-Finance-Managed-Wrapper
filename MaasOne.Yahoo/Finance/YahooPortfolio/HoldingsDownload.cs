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

    public partial class HoldingsDownload : DownloadClient<HoldingsResult>
    {
        public HoldingsDownloadSettings Settings { get { return (HoldingsDownloadSettings)base.Settings; } set { base.SetSettings(value); } }

        public HoldingsDownload()
        {
            this.Settings = new HoldingsDownloadSettings();
        }

        protected override HoldingsResult ConvertResult(ConnectionInfo connInfo, System.IO.Stream stream, SettingsBase settings)
        {
            HoldingsDownloadSettings set = (HoldingsDownloadSettings)settings;
            XDocument doc = MyHelper.ParseXmlDocument(stream);
            Holding[] result = this.ConvertHtmlDoc(doc, set.PortfolioID);
            return new HoldingsResult(result);
        }


        internal Holding[] ConvertHtmlDoc(XDocument doc, string portfolioID)
        {
            if (doc != null)
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
                List<Holding> lst = new List<Holding>();
                XElement[] rowNodes = XPath.GetElements("//form[@action=\"/portfolio/" + portfolioID + "/save-sort\"]/div/table/tbody/tr", doc);
                if (rowNodes != null)
                {
                    XPath inputPath = new XPath("//input");
                    foreach (XElement trNode in rowNodes)
                    {
                        Holding h = new Holding();
                        XName valueName = XName.Get("value");
                        XName nmeName = XName.Get("name");
                        XElement[] inputs = inputPath.GetElements(trNode);
                        int i; double d;
                        int dd = 0, md = 0, yd = 0;
                        foreach (XElement inputNode in inputs)
                        {
                            XAttribute valueAtt = inputNode.Attribute(valueName);
                            XAttribute nameAtt = inputNode.Attribute(nmeName);
                            if (valueAtt != null && nameAtt != null)
                            {
                                switch (nameAtt.Value)
                                {
                                    case "yfi_pf_order[]":
                                        if (int.TryParse(valueAtt.Value, System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out i)) { h.Order = i; }
                                        break;
                                    case "yfi_pf_lot[]":
                                        if (int.TryParse(valueAtt.Value, System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out i)) { h.Lot = i; }
                                        break;
                                    case "yfi_pf_symbol[]":
                                        h.SetID(valueAtt.Value);
                                        break;
                                    case "yfi_pf_price_paid[]":
                                        if (double.TryParse(valueAtt.Value, System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out d)) { h.PricePaid = d; }
                                        break;
                                    case "yfi_pf_shares_owned[]":
                                        if (int.TryParse(valueAtt.Value, System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out i)) { h.Shares = i; }
                                        break;
                                    case "yfi_pf_commission[]":
                                        if (double.TryParse(valueAtt.Value, System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out d)) { h.Commission = d; }
                                        break;
                                    case "yfi_pf_comment[]":
                                        h.Notes = valueAtt.Value;
                                        break;
                                    case "yfi_pf_lowlimit[]":
                                        if (double.TryParse(valueAtt.Value, System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out d)) { h.LowLimit = d; }
                                        break;
                                    case "yfi_pf_highlimit[]":
                                        if (double.TryParse(valueAtt.Value, System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out d)) { h.HighLimit = d; }
                                        break;
                                    case "yfi_pf_trade_date_day[]":
                                        if (int.TryParse(valueAtt.Value, System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out i)) { dd = i; }
                                        break;
                                    case "yfi_pf_trade_date_month[]":
                                        if (int.TryParse(valueAtt.Value, System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out i)) { md = i; }
                                        break;
                                    case "yfi_pf_trade_date_year[]":
                                        if (int.TryParse(valueAtt.Value, System.Globalization.NumberStyles.Any, MyHelper.ConverterCulture, out i)) { yd = i; }
                                        break;
                                }

                            }
                            if ((dd != 0) && (md != 0) && (yd != 0) && !(dd == DateTime.Today.Day && md == DateTime.Today.Month && yd == DateTime.Today.Year)) h.TradeDate = new DateTime(yd, md, dd);

                        }
                        lst.Add(h);
                    }
                }
                return lst.ToArray();
            }
            else { return null; }
        }


    }


    /// <summary>
    /// Stores the result data
    /// </summary>
    public class HoldingsResult
    {
        private Holding[] mHoldings = null;
        public Holding[] Items { get { return mHoldings; } }

        internal HoldingsResult(Holding[] holdings)
        {
            mHoldings = holdings;
        }
    }

    public class Holding : IID
    {
        private string mID = string.Empty;
        public string ID { get { return mID; } }

        public int Order { get; set; }
        public int Lot { get; set; }
        public Nullable<DateTime> TradeDate { get; set; }
        public int Shares { get; set; }
        public double PricePaid { get; set; }
        public double Commission { get; set; }
        public double LowLimit { get; set; }
        public double HighLimit { get; set; }
        public string Notes { get; set; }
        internal Holding()
        {
            mID = string.Empty;
            this.TradeDate = DateTime.Today;
            this.Notes = string.Empty;
        }
        internal void SetID(string value) { mID = value; }
        public Holding(string id)
        {
            mID = id;
            this.TradeDate = DateTime.Today;
            this.Notes = string.Empty;
        }
    }


    public class HoldingsDownloadSettings : SettingsBase
    {
        public YAccountManager Account { get; set; }
        public string PortfolioID { get; set; }

        protected override System.Net.CookieContainer Cookies { get { return this.Account != null ? this.Account.Cookies : null; } }


        public HoldingsDownloadSettings() { }

        protected override string GetUrl()
        {
            return "http://finance.yahoo.com/portfolio/" + this.PortfolioID + "/sort";
        }

        public override object Clone()
        {
            return new HoldingsDownloadSettings() { Account = this.Account, PortfolioID = this.PortfolioID };
        }
    }

}
