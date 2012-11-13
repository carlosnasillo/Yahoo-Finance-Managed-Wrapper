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
using MaasOne.Base;
using System.Xml.Linq;


namespace MaasOne.Finance.YahooScreener
{


    public partial class BondScreenerDownload : Base.DownloadClient<BondScreenerResult>
    {
        public BondScreenerDownloadSettings Settings { get { return (BondScreenerDownloadSettings)base.Settings; } set { base.SetSettings(value); } }

        public BondScreenerDownload()
        {
            this.Settings = new BondScreenerDownloadSettings();
        }


        protected override BondScreenerResult ConvertResult(ConnectionInfo connInfo, System.IO.Stream stream, SettingsBase settings)
        {
            List<BondScreenerData> results = new List<BondScreenerData>();
            System.Globalization.CultureInfo convCulture = new System.Globalization.CultureInfo("en-US");
            XDocument doc = MyHelper.ParseXmlDocument(stream);
            XElement[] resultsNodes = XPath.GetElements("//results", doc);
            if (resultsNodes.Length == 1)
            {
                XElement resultsNode = resultsNodes[0];
                foreach (XElement trNode in resultsNode.Elements())
                {
                    XAttribute classAtt = trNode.Attribute(XName.Get("class"));
                    if (classAtt != null)
                    {
                        if (classAtt.Value == "yfnc_tabledata1")
                        {
                            BondScreenerData res = new BondScreenerData();
                            int index = 0;
                            foreach (XElement tdNode in trNode.Elements())
                            {

                                switch (index)
                                {
                                    case 0:
                                        XElement fontNode = null;//tdNode.FirstNode;
                                        switch (fontNode.Value)
                                        {
                                            case "Treas":
                                                res.Type = BondType.Treasury;
                                                break;
                                            case "Zero":
                                                res.Type = BondType.TreasuryZeroCoupon;
                                                break;
                                            case "Corp":
                                                res.Type = BondType.Corporate;
                                                break;
                                            case "Muni":
                                                res.Type = BondType.Municipal;
                                                break;
                                        }
                                        break;
                                    case 1:
                                        XElement aNode = null; // tdNode.FirstNode.FirstNode;
                                        fontNode = null;    
                                    //fontNode = tdNode.LastNode.FirstNode;
                                        res.Issue = new Link(fontNode.Value.Replace("\n", ""), new Uri("http://reports.finance.yahoo.com" + aNode.Attribute(XName.Get("href")).Value));
                                        break;
                                    case 2:
                                            fontNode = null;    
                                   //fontNode = tdNode.FirstNode.FirstNode;
                                        double t;
                                        if (double.TryParse(fontNode.Value, System.Globalization.NumberStyles.Any, convCulture, out t)) res.Price = t;
                                        break;
                                    case 3:
                                           fontNode = null;    
                                    //fontNode = tdNode.FirstNode.FirstNode;
                                        if (double.TryParse(fontNode.Value, System.Globalization.NumberStyles.Any, convCulture, out t)) res.CouponInPercent = t;
                                        break;
                                    case 4:
                                        XElement nobrNode = null;// tdNode.FirstNode.FirstNode.FirstNode;
                                        DateTime d;
                                        if (System.DateTime.TryParseExact(nobrNode.Value, "dd-MMM-yyyy", convCulture, System.Globalization.DateTimeStyles.None, out d)) res.Maturity = d;
                                        break;
                                    case 5:
                                            fontNode = null;    
                                   //fontNode = tdNode.FirstNode.FirstNode;
                                        if (double.TryParse(fontNode.Value, System.Globalization.NumberStyles.Any, convCulture, out t)) res.YieldToMaturityInPercent = t;
                                        break;
                                    case 6:
                                            fontNode = null;    
                                   //fontNode = tdNode.FirstNode.FirstNode;
                                        if (double.TryParse(fontNode.Value, System.Globalization.NumberStyles.Any, convCulture, out t)) res.CurrentYieldInPercent = t;
                                        break;
                                    case 7:
                                           fontNode = null;    
                                    //fontNode = tdNode.FirstNode;
                                        foreach (Rating r in Enum.GetValues(typeof(Rating)))
                                        {
                                            if (r.ToString() == fontNode.Value)
                                            {
                                                res.FitchRating = r;
                                                break; // TODO: might not be correct. Was : Exit For
                                            }
                                        }

                                        break;
                                    case 8:
                                        fontNode = null;    
                                   //fontNode = tdNode.FirstNode;
                                        res.Callable = Convert.ToBoolean((fontNode.Value == "Yes" ? true : false));
                                        break;
                                }
                                index += 1;
                            }
                            results.Add(res);
                        }
                    }
                }
            }
            return new BondScreenerResult(results.ToArray());
        }
    }


    public class BondScreenerResult
    {
        private BondScreenerData[] mItems = null;
        public BondScreenerData[] Items
        {
            get { return mItems; }
        }
        internal BondScreenerResult(BondScreenerData[] items)
        {
            mItems = items;
        }
    }




    public class BondScreenerData
    {

        public YahooScreener.BondType Type { get; set; }
        public Link Issue { get; set; }
        public double Price { get; set; }
        public double CouponInPercent { get; set; }
        public System.DateTime Maturity { get; set; }
        public double YieldToMaturityInPercent { get; set; }
        public double CurrentYieldInPercent { get; set; }
        public YahooScreener.Rating FitchRating { get; set; }
        public bool Callable { get; set; }

    }





    public class BondScreenerDownloadSettings : Base.SettingsBase, IResultIndexSettings
    {
        public int Index { get; set; }
        public int Count { get; set; }
        public List<BondType> Types { get; set; }
        public UsState Municipal_State { get; set; }
        public PriceType Price { get; set; }
        public Range<double> CouponRange { get; set; }
        public Range<double> CurrentYieldRange { get; set; }
        public Range<double> YTMRange { get; set; }
        public Range<int> MaturityRangeInMonths { get; set; }
        public Range<Rating> RatingRange { get; set; }
        public Nullable<bool> Callable { get; set; }

        public BondProperty RankFor { get; set; }
        public ListSortDirection RankDirection { get; set; }

        public BondScreenerDownloadSettings()
        {
            this.Count = 15;
        }
        internal BondScreenerDownloadSettings(BondScreenerDownloadSettings original)
        {
            this.Index = original.Index;
            this.Count = original.Count;
            foreach (BondType t in original.Types)
            {
                this.Types.Add(t);
            }
            this.Municipal_State = original.Municipal_State;
            this.Price = original.Price;
            this.CouponRange = new Range<double>
            {
                Maximum = original.CouponRange.Maximum,
                Minimum = original.CouponRange.Minimum
            };
            this.CurrentYieldRange = new Range<double>
            {
                Maximum = original.CurrentYieldRange.Maximum,
                Minimum = original.CurrentYieldRange.Minimum
            };
            this.YTMRange = new Range<double>
            {
                Maximum = original.YTMRange.Maximum,
                Minimum = original.YTMRange.Minimum
            };
            this.MaturityRangeInMonths = new Range<int>
            {
                Maximum = original.MaturityRangeInMonths.Maximum,
                Minimum = original.MaturityRangeInMonths.Minimum
            };
            this.RatingRange = new Range<Rating>
            {
                Maximum = original.RatingRange.Maximum,
                Minimum = original.RatingRange.Minimum
            };
            this.Callable = original.Callable;
            this.RankFor = original.RankFor;
            this.RankDirection = original.RankDirection;
        }

        protected override string GetUrl()
        {
            System.Text.StringBuilder whereClause = new System.Text.StringBuilder();
            System.Globalization.CultureInfo convCulture = new System.Globalization.CultureInfo("en-US");
            whereClause.Append("url=\"http://reports.finance.yahoo.com/z1?");

            double index = this.Index - (this.Index % 15);
            if (index > 0)
                index /= 15;
            index += 1;
            whereClause.AppendFormat("&b={0}", index);

            if (this.Types.Contains(BondType.Treasury))
                whereClause.Append("&tt=1");
            if (this.Types.Contains(BondType.TreasuryZeroCoupon))
                whereClause.Append("&tz=1");
            if (this.Types.Contains(BondType.Corporate))
                whereClause.Append("&tc=1");
            if (this.Types.Contains(BondType.Municipal))
            {
                whereClause.Append("&tz=1");
                if (this.Municipal_State != UsState.Any)
                    whereClause.AppendFormat("&stt={0}", this.Municipal_State.ToString());
            }
            else
            {
                whereClause.Append("&stt=-");
            }
            switch (this.Price)
            {
                case PriceType.Any:
                    whereClause.Append("&pr=0");
                    break;
                case PriceType.Premium:
                    whereClause.Append("&pr=1");
                    break;
                case PriceType.Par:
                    whereClause.Append("&pr=2");
                    break;
                case PriceType.Discount:
                    whereClause.Append("&pr=3");
                    break;
            }
            if (this.CouponRange.Minimum >= 0)
            {
                whereClause.AppendFormat("&cpl={0}", this.CouponRange.Minimum.ToString(convCulture));
            }
            else
            {
                whereClause.Append("&cpl=-1");
            }
            if (this.CouponRange.Maximum >= 0)
            {
                whereClause.AppendFormat("&cpu={0}", this.CouponRange.Maximum.ToString(convCulture));
            }
            else
            {
                whereClause.Append("&cpu=-1");
            }

            if (this.CurrentYieldRange.Minimum >= 0)
            {
                whereClause.AppendFormat("&yl={0}", this.CurrentYieldRange.Minimum.ToString(convCulture));
            }
            else
            {
                whereClause.Append("&yl=-1");
            }
            if (this.CurrentYieldRange.Maximum >= 0)
            {
                whereClause.AppendFormat("&yu={0}", this.CurrentYieldRange.Maximum.ToString(convCulture));
            }
            else
            {
                whereClause.Append("&yu=-1");
            }

            if (this.YTMRange.Minimum >= 0)
            {
                whereClause.AppendFormat("&ytl={0}", this.YTMRange.Minimum.ToString(convCulture));
            }
            else
            {
                whereClause.Append("&ytl=-1");
            }
            if (this.YTMRange.Maximum >= 0)
            {
                whereClause.AppendFormat("&ytu={0}", this.YTMRange.Maximum.ToString(convCulture));
            }
            else
            {
                whereClause.Append("&ytu=-1");
            }

            if (this.MaturityRangeInMonths.Minimum >= 0)
            {
                whereClause.AppendFormat("&mtl={0}", this.MaturityRangeInMonths.Minimum.ToString());
            }
            else
            {
                whereClause.Append("&mtl=-1");
            }
            if (this.MaturityRangeInMonths.Maximum >= 0)
            {
                whereClause.AppendFormat("&mtu={0}", this.MaturityRangeInMonths.Maximum.ToString());
            }
            else
            {
                whereClause.Append("&mtu=-1");
            }

            if (this.RatingRange.Minimum != Rating.Any)
            {
                whereClause.AppendFormat("&rl={0}", Convert.ToInt32(this.RatingRange.Minimum).ToString());
            }
            else
            {
                whereClause.Append("&rl=-1");
            }
            if (this.RatingRange.Maximum != Rating.Any)
            {
                whereClause.AppendFormat("&ru={0}", Convert.ToInt32(this.RatingRange.Maximum).ToString());
            }
            else
            {
                whereClause.Append("&ru=-1");
            }

            if (this.Callable.HasValue)
            {
                whereClause.AppendFormat("&cll={0}", Convert.ToInt32((this.Callable.Value == true ? 1 : 0)).ToString());
            }
            else
            {
                whereClause.Append("&cll=-1");
            }

            switch (this.RankFor)
            {
                case BondProperty.Type:
                    whereClause.Append("&sf=t");
                    break;
                case BondProperty.Issue:
                    whereClause.Append("&sf=i");
                    break;
                case BondProperty.Price:
                    whereClause.Append("&sf=p");
                    break;
                case BondProperty.CouponInPercent:
                    whereClause.Append("&sf=c");
                    break;
                case BondProperty.Maturity:
                    whereClause.Append("&sf=m");
                    break;
                case BondProperty.YtmInPercent:
                    whereClause.Append("&sf=y");
                    break;
                case BondProperty.CurrentYieldInPercent:
                    whereClause.Append("&sf=Y");
                    break;
                case BondProperty.FitchRatings:
                    whereClause.Append("&sf=r");
                    break;
                case BondProperty.Callable:
                    whereClause.Append("&sf=l");
                    break;
            }
            switch (this.RankDirection)
            {
                case ListSortDirection.Ascending:
                    whereClause.Append("&so=a");
                    break;
                case ListSortDirection.Descending:
                    whereClause.Append("&so=d");
                    break;
            }

            whereClause.Append("\" AND xpath='//table[@class=\"yfnc_tableout1\"]/tr/td/table/tr'");

            return MyHelper.YqlUrl("*", "html", whereClause.ToString(), null, false);
        }

        public override object Clone()
        {
            return new BondScreenerDownloadSettings(this);
        }
    }

}
