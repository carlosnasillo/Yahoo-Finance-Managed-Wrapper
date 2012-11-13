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


namespace MaasOne.Finance.YahooScreener
{

    public partial class BondScreenerInfoDownload : Base.DownloadClient<BondScreenerInfoResult>
    {


        public BondScreenerInfoDownloadSettings Settings { get { return (BondScreenerInfoDownloadSettings)base.Settings; } set { base.SetSettings(value); } }

        public BondScreenerInfoDownload()
        {
            this.Settings = new BondScreenerInfoDownloadSettings();
        }


        public void DownloadAsync(BondScreenerData res, object userArgs = null)
        {
            this.DownloadAsync(new BondScreenerInfoDownloadSettings(res), userArgs);
        }
        public void DownloadAsync(BondScreenerInfoDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        protected override BondScreenerInfoResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            BondScreenerInfoDownloadSettings set = (BondScreenerInfoDownloadSettings)settings;

            BondScreenerInfoData res = new BondScreenerInfoData();
            System.Globalization.CultureInfo convCulture = new System.Globalization.CultureInfo("en-US");
            //XDocument doc = MyHelper.ParseXmlDocument(stream);
            XDocument doc = MyHelper.ParseXmlDocument(stream);
            XElement[] resultsNodes = XPath.GetElements("//results", doc);
            if (resultsNodes.Length > 0)
            {
                XElement resultNode = resultsNodes[0];
                if (MyHelper.EnumToArray(resultNode.Elements()).Length > 0)
                {
                    XElement tdNode = null;// MyHelper.EnumToArray(resultNode.Elements())[0];
                    int tableIndex = 0;
                    foreach (XElement tableNode in tdNode.Elements())
                    {
                        XElement[] tableEnm = MyHelper.EnumToArray(tableNode.Elements());
                        switch (tableIndex)
                        {
                            case 0:
                                XElement trNode = tableEnm[1];
                                //res.Title = MyHelper.EnumToArray<XElement>(MyHelper.EnumToArray<XElement>(trNode.Elements())[0].Elements())[0].Value;
                                DateTime d;
                                //if (System.DateTime.TryParseExact(trNode.LastNode.FirstNode.Value.Replace("As of ", ""), "dd-MMM-yyyy", convCulture, System.Globalization.DateTimeStyles.None, out d)) res.AsOf = d;

                                break;
                            case 3:
                                XElement innerTableNode = null;//tableNode.FirstNode.FirstNode.FirstNode;
                                int innerIndex = 0;
                                foreach (XElement innerTrNode in innerTableNode.Elements())
                                {
                                    switch (innerIndex)
                                    {
                                        case 0:
                                            double t;
                                            //if (double.TryParse(innerTrNode.LastNode.Value, System.Globalization.NumberStyles.Any, convCulture, out t)) res.Price = t;
                                            break;
                                        case 1:
                                            //if (double.TryParse(innerTrNode.LastNode.Value, System.Globalization.NumberStyles.Any, convCulture, out t)) res.CouponInPercent = t;
                                            break;
                                        case 2:
                                            //if (System.DateTime.TryParseExact(innerTrNode.LastNode.Value, "dd-MMM-yyyy", convCulture, System.Globalization.DateTimeStyles.None, out d)) res.Maturity = d;
                                            break;
                                        case 3:
                                            //if (double.TryParse(innerTrNode.LastNode.Value, System.Globalization.NumberStyles.Any, convCulture, out t)) res.YieldToMaturityInPercent = t;
                                            break;
                                        case 4:
                                            //if (double.TryParse(innerTrNode.LastNode.Value, System.Globalization.NumberStyles.Any, convCulture, out t)) res.CurrentYieldInPercent = t;
                                            break;
                                        case 5:
                                            //res.CouponPaymentFrequency = innerTrNode.LastNode.Value;
                                            break;
                                        case 6:
                                            //if (System.DateTime.TryParseExact(innerTrNode.LastNode.Value, "dd-MMM-yyyy", convCulture, System.Globalization.DateTimeStyles.None, out d)) res.FirstCouponDate = d;
                                            break;
                                        case 7:
                                            /* switch (innerTrNode.LastNode.Value)
                                            {
                                                case "Treasury":
                                                    res.Type = BondType.Treasury;
                                                    break;
                                                case "Treasury Zero":
                                                    res.Type = BondType.TreasuryZeroCoupon;
                                                    break;
                                                case "Corporate":
                                                    res.Type = BondType.Corporate;
                                                    break;
                                                case "Municipal":
                                                    res.Type = BondType.Municipal;
                                                    break;
                                            }
                                             */
                                            break;
                                        case 8:
                                            //res.Callable = Convert.ToBoolean((innerTrNode.LastNode.Value == "Yes" ? true : false));
                                            break;
                                    }
                                    innerIndex += 1;
                                }


                                break;
                            case 7:
                                innerTableNode = null; // tableNode.FirstNode.FirstNode.FirstNode;
                                innerIndex = 0;
                                foreach (XElement innerTrNode in innerTableNode.Elements())
                                {
                                    switch (innerIndex)
                                    {
                                        case 0:
                                            int n;
                                            //if (int.TryParse(innerTrNode.LastNode.Value, System.Globalization.NumberStyles.Any, convCulture, out n)) res.AvailableQuantity = n;
                                            break;
                                        case 1:
                                            //if (int.TryParse(innerTrNode.LastNode.Value, System.Globalization.NumberStyles.Any, convCulture, out n)) res.MinimumTradeQuantity = n;
                                            break;
                                        case 2:
                                            //if (System.DateTime.TryParseExact(innerTrNode.LastNode.Value, "dd-MMM-yyyy", convCulture, System.Globalization.DateTimeStyles.None, out d)) res.DatedDate = d;
                                            break;
                                        case 3:
                                            //if (System.DateTime.TryParseExact(innerTrNode.LastNode.Value, "dd-MMM-yyyy", convCulture, System.Globalization.DateTimeStyles.None, out d)) res.SettlementDate = d;
                                            break;
                                    }
                                    innerIndex += 1;
                                }

                                break;
                        }
                        tableIndex += 1;
                    }
                    res.Issue = new Link(set.Data.Issue.Title, set.Data.Issue.Url);
                    res.FitchRating = set.Data.FitchRating;
                }
            }
            return new BondScreenerInfoResult(res);
        }

    }


    public class BondScreenerInfoResult
    {
        private BondScreenerInfoData mItem = null;
        public BondScreenerInfoData Item
        {
            get { return mItem; }
        }
        internal BondScreenerInfoResult(BondScreenerInfoData item)
        {
            mItem = item;
        }
    }




    public class BondScreenerInfoData : BondScreenerData
    {

        public string Title { get; set; }
        public string CouponPaymentFrequency { get; set; }
        public System.DateTime FirstCouponDate { get; set; }
        public int AvailableQuantity { get; set; }
        public int MinimumTradeQuantity { get; set; }
        public System.DateTime DatedDate { get; set; }
        public System.DateTime SettlementDate { get; set; }
        public System.DateTime AsOf { get; set; }

    }



    public class BondScreenerInfoDownloadSettings : Base.SettingsBase
    {

        public BondScreenerData Data { get; set; }

        public BondScreenerInfoDownloadSettings()
        {
            this.Data = null;
        }
        public BondScreenerInfoDownloadSettings(BondScreenerData data)
        {
            this.Data = data;
        }

        protected override string GetUrl()
        {
            System.Text.StringBuilder whereClause = new System.Text.StringBuilder();
            whereClause.Append("url=\"");
            whereClause.Append(this.Data.Issue.Url.ToString());
            whereClause.Append("\" AND xpath='/html/body/table/tr/td/table[3]/tr/td[1]'");
            return MyHelper.YqlUrl("*", "html", whereClause.ToString(), null, false);
        }

        public override object Clone()
        {
            return new BondScreenerInfoDownloadSettings(this.Data);
        }

    }

}
