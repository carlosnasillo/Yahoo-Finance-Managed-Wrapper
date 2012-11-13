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
using System.ComponentModel;


namespace MaasOne.Finance.YahooFinance
{
    /// <summary>
    /// Provides methods for downloading market quotes data.
    /// </summary>
    /// <remarks></remarks>
    public partial class MarketQuotesDownload : Base.DownloadClient<MarketQuotesResult>
    {

        public MarketQuotesDownloadSettings Settings { get { return (MarketQuotesDownloadSettings)base.Settings; } set { base.SetSettings(value); } }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public MarketQuotesDownload()
        {
            this.Settings = new MarketQuotesDownloadSettings();
        }

        /// <summary>
        /// Starts an asynchronous download of all available sector market quotes.
        /// </summary>
        /// <param name="rankedBy">The property the list is ranked by</param>
        /// <param name="rankDir">The direction of ranking</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadAllSectorQuotesAysnc(MarketQuoteProperty rankedBy = MarketQuoteProperty.Name, ListSortDirection rankDir = ListSortDirection.Ascending, object userArgs = null)
        {
            base.DownloadAsync(new MarketQuotesDownloadSettings() { RankedBy = rankedBy, RankDirection = rankDir }, userArgs);
        }
        /// <summary>
        /// Starts an asynchronous download of all available industries of a special sector.
        /// </summary>
        /// <param name="sector">The sector of the industries</param>
        /// <param name="rankedBy">The property the list is ranked by</param>
        /// <param name="rankDir">The direction of ranking</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadIndustryQuotesAsync(Sector sector, MarketQuoteProperty rankedBy = MarketQuoteProperty.Name, ListSortDirection rankDir = ListSortDirection.Ascending, object userArgs = null)
        {
            base.DownloadAsync(new MarketQuotesDownloadSettings() { Sector = sector, RankedBy = rankedBy, RankDirection = rankDir }, userArgs);
        }
        /// <summary>
        /// Starts an asynchronous download of all available company quotes of a special industry.
        /// </summary>
        /// <param name="industyID">The ID of the industry</param>
        /// <param name="rankedBy">The property the list is ranked by</param>
        /// <param name="rankDir">The direction of ranking</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadCompanyQuotesAsync(Industry industy, MarketQuoteProperty rankedBy = MarketQuoteProperty.Name, ListSortDirection rankDir = ListSortDirection.Ascending, object userArgs = null)
        {
            base.DownloadAsync(new MarketQuotesDownloadSettings() { Industry = industy, RankedBy = rankedBy, RankDirection = rankDir }, userArgs);
        }

        public void DownloadAsync(MarketQuotesDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        protected override MarketQuotesResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            System.Globalization.CultureInfo ci = FinanceHelper.DefaultYqlCulture;

            string text = MyHelper.StreamToString(stream, ((MarketQuotesDownloadSettings)settings).TextEncoding);
            char delimiter = ',';
            string[][] table = MyHelper.CsvTextToStringTable(text, delimiter);

            List<MarketQuotesData> lst = new List<MarketQuotesData>();

            if (table.Length > 1)
            {
                for (int i = 1; i <= table.Length - 1; i++)
                {
                    if (table[i].Length == 10)
                    {
                        MarketQuotesData quote = new MarketQuotesData();
                        quote.Name = table[i][0];
                        double t1;
                        if (double.TryParse(table[i][1], System.Globalization.NumberStyles.Any, ci, out t1)) quote.OneDayPriceChangePercent = t1;
                        string mktcap = table[i][2];
                        if (mktcap != "NA" & mktcap != string.Empty & mktcap.Length > 1)
                        {
                            double value = 0;
                            double.TryParse(mktcap.Substring(0, mktcap.Length - 1), System.Globalization.NumberStyles.Any, ci, out value);
                            quote.MarketCapitalizationInMillion = value * FinanceHelper.GetStringMillionFactor(mktcap);
                        }
                        double t2;
                        double t3;
                        double t4;
                        double t5;
                        double t6;
                        double t7;
                        double t8;
                        if (double.TryParse(table[i][3], System.Globalization.NumberStyles.Any, ci, out t2)) quote.PriceEarningsRatio = t2;
                        if (double.TryParse(table[i][4], System.Globalization.NumberStyles.Any, ci, out t3)) quote.ReturnOnEquityPercent = t3;
                        if (double.TryParse(table[i][5], System.Globalization.NumberStyles.Any, ci, out t4)) quote.DividendYieldPercent = t4;
                        if (double.TryParse(table[i][6], System.Globalization.NumberStyles.Any, ci, out t5)) quote.LongTermDeptToEquity = t5;
                        if (double.TryParse(table[i][7], System.Globalization.NumberStyles.Any, ci, out t6)) quote.PriceToBookValue = t6;
                        if (double.TryParse(table[i][8], System.Globalization.NumberStyles.Any, ci, out t7)) quote.NetProfitMarginPercent = t7;
                        if (double.TryParse(table[i][9], System.Globalization.NumberStyles.Any, ci, out t8)) quote.PriceToFreeCashFlow = t8;
                        lst.Add(quote);
                    }
                }
            }

            return new MarketQuotesResult(lst.ToArray());
        }

    }




    /// <summary>
    /// Stores the result data
    /// </summary>
    public class MarketQuotesResult
    {
        private MarketQuotesData[] mItems = null;
        public MarketQuotesData[] Items
        {
            get { return mItems; }
        }
        internal MarketQuotesResult(MarketQuotesData[] items)
        {
            mItems = items;
        }
    }



    /// <summary>
    /// Stores market quote informations
    /// </summary>
    /// <remarks></remarks>
    public class MarketQuotesData
    {

        /// <summary>
        /// The name of the sector, industry or company.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name { get; set; }
        public double OneDayPriceChangePercent { get; set; }
        public double MarketCapitalizationInMillion { get; set; }
        public double PriceEarningsRatio { get; set; }
        public double ReturnOnEquityPercent { get; set; }
        public double DividendYieldPercent { get; set; }
        public double LongTermDeptToEquity { get; set; }
        public double PriceToBookValue { get; set; }
        public double NetProfitMarginPercent { get; set; }
        public double PriceToFreeCashFlow { get; set; }

        /// <summary>
        /// All market quote properties.
        /// </summary>
        /// <param name="prp">The market quote property you want to get or set.</param>
        /// <value></value>
        /// <returns>A value representing and depending on the passed property.</returns>
        /// <remarks></remarks>
        public object this[MarketQuoteProperty prp]
        {
            get
            {
                switch (prp)
                {
                    case MarketQuoteProperty.Name:
                        return this.Name;
                    case MarketQuoteProperty.DividendYieldPercent:
                        return this.DividendYieldPercent;
                    case MarketQuoteProperty.LongTermDeptToEquity:
                        return this.LongTermDeptToEquity;
                    case MarketQuoteProperty.MarketCapitalizationInMillion:
                        return this.MarketCapitalizationInMillion;
                    case MarketQuoteProperty.NetProfitMarginPercent:
                        return this.NetProfitMarginPercent;
                    case MarketQuoteProperty.OneDayPriceChangePercent:
                        return this.OneDayPriceChangePercent;
                    case MarketQuoteProperty.PriceEarningsRatio:
                        return this.PriceEarningsRatio;
                    case MarketQuoteProperty.PriceToBookValue:
                        return this.PriceToBookValue;
                    case MarketQuoteProperty.PriceToFreeCashFlow:
                        return this.PriceToFreeCashFlow;
                    case MarketQuoteProperty.ReturnOnEquityPercent:
                        return this.ReturnOnEquityPercent;
                    default:
                        return null;
                }
            }
            set
            {
                switch (prp)
                {
                    case MarketQuoteProperty.Name:
                        this.Name = value.ToString();
                        break;
                    case MarketQuoteProperty.DividendYieldPercent:
                        double t1;
                        if (double.TryParse(value.ToString(), out t1))
                            this.DividendYieldPercent = t1;
                        break;
                    case MarketQuoteProperty.LongTermDeptToEquity:
                        double t2;
                        if (double.TryParse(value.ToString(), out t2))
                            this.LongTermDeptToEquity = t2;
                        break;
                    case MarketQuoteProperty.MarketCapitalizationInMillion:
                        double t3;
                        if (double.TryParse(value.ToString(), out t3))
                            this.MarketCapitalizationInMillion = t3;
                        break;
                    case MarketQuoteProperty.NetProfitMarginPercent:
                        double t4;
                        if (double.TryParse(value.ToString(), out t4))
                            this.NetProfitMarginPercent = t4;
                        break;
                    case MarketQuoteProperty.OneDayPriceChangePercent:
                        double t5;
                        if (double.TryParse(value.ToString(), out t5))
                            this.OneDayPriceChangePercent = t5;
                        break;
                    case MarketQuoteProperty.PriceEarningsRatio:
                        double t6;
                        if (double.TryParse(value.ToString(), out t6))
                            this.PriceEarningsRatio = t6;
                        break;
                    case MarketQuoteProperty.PriceToBookValue:
                        double t7;
                        if (double.TryParse(value.ToString(), out t7))
                            this.PriceToBookValue = t7;
                        break;
                    case MarketQuoteProperty.PriceToFreeCashFlow:
                        double t8;
                        if (double.TryParse(value.ToString(), out t8))
                            this.PriceToFreeCashFlow = t8;
                        break;
                    case MarketQuoteProperty.ReturnOnEquityPercent:
                        double t9;
                        if (double.TryParse(value.ToString(), out t9))
                            this.ReturnOnEquityPercent = t9;
                        break;
                }
            }
        }

    }


    public class MarketQuotesDownloadSettings : Base.SettingsBase, ITextEncodingSettings
    {
        /// <summary>
        /// The text encoding for downloading quotes NOT from YQL server.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.Text.Encoding TextEncoding { get; set; }

        private Nullable<Sector> mSector = null;
        /// <summary>
        /// Gets the downloaded sectors.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Nullable<Sector> Sector
        {
            get { return mSector; }
            set
            {
                mSector = value;
                mIndustry = null;
            }
        }
        private Nullable<Industry> mIndustry = null;
        /// <summary>
        /// Gets the IDs of the downloaded industries.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Nullable<Industry> Industry
        {
            get { return mIndustry; }
            set
            {
                mIndustry = value;
                mSector = null;
            }
        }

        public MarketQuoteProperty RankedBy = MarketQuoteProperty.Name;
        public ListSortDirection RankDirection;

        public MarketQuotesDownloadSettings()
        {
            this.TextEncoding = System.Text.Encoding.UTF8;
        }

        protected override string GetUrl()
        {
            if (mSector.HasValue)
            {
                return "http://biz.yahoo.com/p/csv/" + ((int)mSector.Value).ToString() + FinanceHelper.MarketQuotesRankingTypeString(this.RankedBy) + FinanceHelper.MarketQuotesRankingDirectionString(this.RankDirection) + ".csv";
            }
            else if (mIndustry.HasValue)
            {
                return "http://biz.yahoo.com/p/csv/" + ((int)mIndustry.Value).ToString() + FinanceHelper.MarketQuotesRankingTypeString(this.RankedBy) + FinanceHelper.MarketQuotesRankingDirectionString(this.RankDirection) + ".csv";
            }
            else
            {
                return "http://biz.yahoo.com/p/csv/" + "s_" + FinanceHelper.MarketQuotesRankingTypeString(this.RankedBy) + FinanceHelper.MarketQuotesRankingDirectionString(this.RankDirection) + ".csv";
            }
        }

        public override object Clone()
        {
            MarketQuotesDownloadSettings cln = new MarketQuotesDownloadSettings();
            if (this.Sector.HasValue) { cln.Sector = this.Sector; }
            if (this.Industry.HasValue) { cln.Industry = this.Industry; }
            cln.RankDirection = this.RankDirection;
            cln.RankedBy = this.RankedBy;
            return cln;
        }
    }


}
