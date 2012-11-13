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
using MaasOne.Xml;
using System.Text.RegularExpressions;
using MaasOne.Finance.YahooFinance;
using System.Xml.Linq;


namespace MaasOne.Finance.YahooFinance
{

    /// <summary>
    /// Provides methods for downloading key statistics of major companies.
    /// </summary>
    /// <remarks></remarks>
    public partial class CompanyStatisticsDownload : Base.DownloadClient<CompanyStatisticsResult>
    {

        public CompanyStatisticsDownloadSettings Settings { get { return (CompanyStatisticsDownloadSettings)base.Settings; } set { base.SetSettings(value); } }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public CompanyStatisticsDownload()
        {
            this.Settings = new CompanyStatisticsDownloadSettings();
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
            this.DownloadAsync(new CompanyStatisticsDownloadSettings(unmanagedID), userArgs);
        }

        public void DownloadAsync(CompanyStatisticsDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        protected override CompanyStatisticsResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            CompanyStatisticsData result = null;
            if (stream != null)
            {
                System.Globalization.CultureInfo convCulture = new System.Globalization.CultureInfo("en-US");
                XDocument doc = MyHelper.ParseXmlDocument(stream);
                XElement resultNode = XPath.GetElement("//table[@id=\"yfncsumtab\"]/tr[2]",doc);

                if (resultNode != null)
                {

                    XElement tempNode = null;
                    XElement vmNode = XPath.GetElement("/td[1]/table[2]/tr/td/table", resultNode);
                    double[] vmValues = new double[9];
                    if (vmNode != null)
                    {
                        tempNode = XPath.GetElement("/tr[1]/td[2]/span", vmNode);
                        if (tempNode != null) vmValues[0] = FinanceHelper.GetMillionValue(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[2]/td[2]", vmNode);
                        if (tempNode != null) vmValues[1] = FinanceHelper.GetMillionValue(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[3]/td[2]", vmNode);
                        if (tempNode != null) vmValues[2] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[4]/td[2]", vmNode);
                        if (tempNode != null) vmValues[3] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[5]/td[2]", vmNode);
                        if (tempNode != null) vmValues[4] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[6]/td[2]", vmNode);
                        if (tempNode != null) vmValues[5] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[7]/td[2]", vmNode);
                        if (tempNode != null) vmValues[6] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[8]/td[2]", vmNode);
                        if (tempNode != null) vmValues[7] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[9]/td[2]", vmNode);
                        if (tempNode != null) vmValues[8] = FinanceHelper.ParseToDouble(tempNode.Value);

                    }

                    CompanyValuationMeasures vm = new CompanyValuationMeasures(vmValues);


                    XElement fyNode = XPath.GetElement("/td[1]/table[4]/tr/td/table",resultNode);
                    XElement profitNode = XPath.GetElement("/td[1]/table[5]/tr/td/table", resultNode);
                    XElement meNode = XPath.GetElement("/td[1]/table[6]/tr/td/table", resultNode);
                    XElement isNode = XPath.GetElement("/td[1]/table[7]/tr/td/table", resultNode);
                    XElement bsNode = XPath.GetElement("/td[1]/table[8]/tr/td/table", resultNode);
                    XElement cfsNode = XPath.GetElement("/td[1]/table[9]/tr/td/table", resultNode);

                    DateTime fiscalYEnds = new DateTime();
                    DateTime mostRecQutr = new DateTime();
                    double[] fhValues = new double[20];

                    if (fyNode != null)
                    {
                        tempNode = XPath.GetElement("/tr[2]/td[2]", fyNode);
                        if (tempNode != null) fiscalYEnds = FinanceHelper.ParseToDateTime(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[3]/td[2]",fyNode);
                        if (tempNode != null) mostRecQutr = FinanceHelper.ParseToDateTime(tempNode.Value);
                    }

                    if (profitNode != null)
                    {
                        tempNode = XPath.GetElement("/tr[2]/td[2]",profitNode);
                        if (tempNode != null) fhValues[0] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[3]/td[2]",profitNode);
                        if (tempNode != null) fhValues[1] = FinanceHelper.ParseToDouble(tempNode.Value);
                    }

                    if (meNode != null)
                    {
                        tempNode = XPath.GetElement("/tr[2]/td[2]",meNode);
                        if (tempNode != null) fhValues[2] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[3]/td[2]",meNode);
                        if (tempNode != null) fhValues[3] = FinanceHelper.ParseToDouble(tempNode.Value);
                    }

                    if (isNode != null)
                    {
                        tempNode = XPath.GetElement("/tr[2]/td[2]", isNode);
                        if (tempNode != null) fhValues[4] = FinanceHelper.GetMillionValue(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[3]/td[2]", isNode);
                        if (tempNode != null) fhValues[5] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[4]/td[2]", isNode);
                        if (tempNode != null) fhValues[6] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[5]/td[2]", isNode);
                        if (tempNode != null) fhValues[7] = FinanceHelper.GetMillionValue(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[6]/td[2]", isNode);
                        if (tempNode != null) fhValues[8] = FinanceHelper.GetMillionValue(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[7]/td[2]", isNode);
                        if (tempNode != null) fhValues[9] = FinanceHelper.GetMillionValue(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[8]/td[2]", isNode);
                        if (tempNode != null) fhValues[10] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[9]/td[2]", isNode);
                        if (tempNode != null) fhValues[11] = FinanceHelper.ParseToDouble(tempNode.Value);
                    }

                    if (bsNode != null)
                    {
                        tempNode = XPath.GetElement("/tr[2]/td[2]",bsNode);
                        if (tempNode != null) fhValues[12] = FinanceHelper.GetMillionValue(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[3]/td[2]",bsNode);
                        if (tempNode != null) fhValues[13] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[4]/td[2]",bsNode);
                        if (tempNode != null) fhValues[14] = FinanceHelper.GetMillionValue(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[5]/td[2]",bsNode);
                        if (tempNode != null) fhValues[15] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[6]/td[2]",bsNode);
                        if (tempNode != null) fhValues[16] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[7]/td[2]",bsNode);
                        if (tempNode != null) fhValues[17] = FinanceHelper.ParseToDouble(tempNode.Value);

                    }

                    if (cfsNode != null)
                    {
                        tempNode = XPath.GetElement("/tr[2]/td[2]", cfsNode);
                        if (tempNode != null) fhValues[18] = FinanceHelper.GetMillionValue(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[3]/td[2]",cfsNode);
                        if (tempNode != null) fhValues[19] = FinanceHelper.GetMillionValue(tempNode.Value);
                    }

                    CompanyFinancialHighlights fh = new CompanyFinancialHighlights(fiscalYEnds, mostRecQutr, fhValues);


                    XElement sphNode = XPath.GetElement("/td[3]/table[2]/tr/td/table", resultNode);
                    XElement stNode = XPath.GetElement("/td[3]/table[3]/tr/td/table", resultNode);
                    XElement dsNode = XPath.GetElement("/td[3]/table[4]/tr/td/table", resultNode);

                    double[] ctiValues = new double[23];
                    DateTime exDivDate = new DateTime();
                    DateTime divDate = new DateTime();
                    DateTime splitDate = new DateTime();
                    SharesSplitFactor sf = null;

                    if (sphNode != null)
                    {

                        tempNode = XPath.GetElement("/tr[2]/td[2]",sphNode);
                        if (tempNode != null) ctiValues[0] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[3]/td[2]",sphNode);
                        if (tempNode != null) ctiValues[1] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[4]/td[2]",sphNode);
                        if (tempNode != null) ctiValues[2] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[5]/td[2]",sphNode);
                        if (tempNode != null) ctiValues[3] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[6]/td[2]",sphNode);
                        if (tempNode != null) ctiValues[4] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[7]/td[2]",sphNode);
                        if (tempNode != null) ctiValues[5] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[8]/td[2]",sphNode);
                        if (tempNode != null) ctiValues[6] = FinanceHelper.ParseToDouble(tempNode.Value);

                    }


                    if (stNode != null)
                    {

                        tempNode = XPath.GetElement("/tr[2]/td[2]",stNode);
                        if (tempNode != null) ctiValues[7] = FinanceHelper.ParseToDouble(tempNode.Value) / 1000;

                        tempNode = XPath.GetElement("/tr[3]/td[2]",stNode);
                        if (tempNode != null) ctiValues[8] = FinanceHelper.ParseToDouble(tempNode.Value) / 1000;

                        tempNode = XPath.GetElement("/tr[4]/td[2]",stNode);
                        if (tempNode != null) ctiValues[9] = FinanceHelper.GetMillionValue(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[5]/td[2]",stNode);
                        if (tempNode != null) ctiValues[10] = FinanceHelper.GetMillionValue(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[6]/td[2]",stNode);
                        if (tempNode != null) ctiValues[11] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[7]/td[2]",stNode);
                        if (tempNode != null) ctiValues[12] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[8]/td[2]",stNode);
                        if (tempNode != null) ctiValues[13] = FinanceHelper.GetMillionValue(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[9]/td[2]",stNode);
                        if (tempNode != null) ctiValues[14] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[10]/td[2]",stNode);
                        if (tempNode != null) ctiValues[15] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[11]/td[2]",stNode);
                        if (tempNode != null) ctiValues[16] = FinanceHelper.GetMillionValue(tempNode.Value);

                    }

                    if (dsNode != null)
                    {

                        tempNode = XPath.GetElement("/tr[2]/td[2]",dsNode);
                        if (tempNode != null) ctiValues[17] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[3]/td[2]",dsNode);
                        if (tempNode != null) ctiValues[18] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[4]/td[2]",dsNode);
                        if (tempNode != null) ctiValues[19] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[5]/td[2]",dsNode);
                        if (tempNode != null) ctiValues[20] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[6]/td[2]",dsNode);
                        if (tempNode != null) ctiValues[21] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[7]/td[2]",dsNode);
                        if (tempNode != null) ctiValues[22] = FinanceHelper.ParseToDouble(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[8]/td[2]",dsNode);
                        if (tempNode != null) divDate = FinanceHelper.ParseToDateTime(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[9]/td[2]",dsNode);
                        if (tempNode != null) exDivDate = FinanceHelper.ParseToDateTime(tempNode.Value);

                        tempNode = XPath.GetElement("/tr[10]/td[2]",dsNode);
                        if (tempNode != null)
                        {
                            string[] txt = tempNode.Value.Split(':');
                            int from, to;
                            if (int.TryParse(txt[0], out to) && int.TryParse(txt[1], out from))
                            {
                                sf = new SharesSplitFactor(to, from);
                            }
                        }

                        tempNode = XPath.GetElement("/tr[11]/td[2]",dsNode);
                        if (tempNode != null) splitDate = FinanceHelper.ParseToDateTime(tempNode.Value);

                    }
                    CompanyTradingInfo cti = new CompanyTradingInfo(ctiValues, divDate, exDivDate, splitDate, sf);

                    result = new CompanyStatisticsData(((CompanyStatisticsDownloadSettings)settings).ID, vm, fh, cti);
                }
            }

            return new CompanyStatisticsResult(result);
        }

    }


    /// <summary>
    /// Stores the result data
    /// </summary>
    public class CompanyStatisticsResult
    {

        private CompanyStatisticsData mItem = null;
        public CompanyStatisticsData Item
        {
            get { return mItem; }
        }

        internal CompanyStatisticsResult(CompanyStatisticsData item)
        {
            mItem = item;
        }

    }


    /// <summary>
    /// CompanyStatisticsData is a conatiner class for several statistics of a single company.
    /// </summary>
    /// <remarks></remarks>
    public class CompanyStatisticsData : IID
    {

        private string mID;
        private CompanyValuationMeasures mValuationMeasures = null;
        private CompanyFinancialHighlights mFinancialHighlights = null;

        private CompanyTradingInfo mTradingInfo = null;
        /// <summary>
        /// The ID of the company.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ID
        {
            get { return mID; }
        }

        public CompanyValuationMeasures ValuationMeasures
        {
            get { return mValuationMeasures; }
        }
        public CompanyFinancialHighlights FinancialHighlights
        {
            get { return mFinancialHighlights; }
        }
        public CompanyTradingInfo TradingInfo
        {
            get { return mTradingInfo; }
        }

        internal CompanyStatisticsData(string id, CompanyValuationMeasures vm, CompanyFinancialHighlights fh, CompanyTradingInfo ti)
        {
            mID = id;
            mValuationMeasures = vm;
            mFinancialHighlights = fh;
            mTradingInfo = ti;
        }

    }

    public class CompanyValuationMeasures
    {

        /// <summary>
        /// The total dollar value of all outstanding shares. Computed as shares times current market price. Capitalization is a measure of corporate size.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Current Market Price Per Share x Number of Shares Outstanding
        /// Intraday Value
        /// Shares outstanding is taken from the most recently filed quarterly or annual report and Market Cap is calculated using shares outstanding.</remarks>
        public double MarketCapitalisationInMillion { get; set; }
        /// <summary>
        /// Enterprise Value is a measure of theoretical takeover price, and is useful in comparisons against income statement line items above the interest expense/income lines such as revenue and EBITDA.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Market Cap + Total Debt - Total Cash &amp; Short Term Investments</remarks>
        public double EnterpriseValueInMillion { get; set; }
        /// <summary>
        /// A popular valuation ratio calculated by dividing the current market price by trailing 12-month (ttm) Earnings Per Share.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Current Market Price / Earnings Per Share
        /// Intraday Value
        /// Trailing Twelve Months</remarks>
        public double TrailingPE { get; set; }
        /// <summary>
        /// A valuation ratio calculated by dividing the current market price by projected 12-month Earnings Per Share.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Current Market Price / Projected Earnings Per Share
        /// Fiscal Year Ending</remarks>
        public double ForwardPE { get; set; }
        /// <summary>
        /// Forward-looking measure rather than typical earnings growth measures, which look eck in time (historical). Used to measure a stock's valuation against its projected 5-yr growth rate.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: P/E Ratio / 5-Yr Expected EPS Growth
        /// 5 years expected</remarks>
        public double PEGRatio { get; set; }
        /// <summary>
        /// A valuation ratio calculated by dividing the current market price by trailing 12-month (ttm) Total Revenues. Often used to value unprofitable companies.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Current Market Price / Total Revenues Per Share
        /// Trailing Twelve Months</remarks>
        public double PriceToSales { get; set; }
        /// <summary>
        /// A valuation ratio calculated by dividing the current market price by the most recent quarter's (mrq) Book Value Per Share.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Current Market Price / Book Value Per Share
        /// Most Recent Quarter</remarks>
        public double PriceToBook { get; set; }
        /// <summary>
        /// Firm value compared against revenue. Provides a more rigorous comparison than the Price/Sales ratio by removing the effects of capitalization from both sides of the ratio. Since revenue is unaffected by the interest income/expense line item, the appropriate value comparison should also remove the effects of capitalization, as EV does.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Enterprise Value / Total Revenues
        /// Trailing Twelve Months</remarks>
        public double EnterpriseValueToRevenue { get; set; }
        /// <summary>
        /// Firm value compared against EBITDA (Earnings before interest, taxes, depreciation, and amortization). See Enterprise Value/Revenue.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Enterprise Value / EBITDA
        /// Trailing Twelve Months</remarks>
        public double EnterpriseValueToEBITDA { get; set; }

        internal CompanyValuationMeasures(double[] values)
        {
            this.MarketCapitalisationInMillion = values[0];
            this.EnterpriseValueInMillion = values[1];
            this.TrailingPE = values[2];
            this.ForwardPE = values[3];
            this.PEGRatio = values[4];
            this.PriceToSales = values[5];
            this.PriceToBook = values[6];
            this.EnterpriseValueToRevenue = values[7];
            this.EnterpriseValueToEBITDA = values[8];
        }

    }

    public class CompanyFinancialHighlights
    {

        //Fiscal Year
        /// <summary>
        /// The date of the end of the firm's accounting year.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.DateTime FiscalYearEnds { get; set; }
        /// <summary>
        /// Date for the most recent quarter end for which data is available on the Key Statistics page. This period is often abbreviated as "MRQ."
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.DateTime MostRecentQuarter { get; set; }

        //Profitability and Management Effectiveness
        /// <summary>
        /// Also known as Return on Sales, this value is the Net Income After Taxes for the trailing 12 months divided by Total Revenue for the same period and is expressed as a percentage.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: (Net Income / Total Revenues) x 100
        /// Trailing Twelve Months</remarks>
        public double ProfitMarginPercent { get; set; }
        /// <summary>
        /// This item represents the difference between the Total Revenues and the Total Operating Costs divided by Total Revenues, and is expressed as a percentage. Total Operating Costs consist of: (a) Cost of Goods Sold (b) Total (c) Selling, General &amp; Administrative Expenses (d) Total R &amp; D Expenses (e) Depreciation &amp; Amortization and (f) Total Other Operating Expenses, Total. A ratio used to measure a company's operating efficiency.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: [(Total Revenues - Total Operating Costs) / (Total Revenues)] x 100
        /// Trailing Twelve Months</remarks>
        public double OperatingMarginPercent { get; set; }
        /// <summary>
        /// This ratio shows percentage of Returns to Total Assets of the company. This is a useful measure in analyzing how well a company uses its assets to produce earnings.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Earnings from Continuing Operations / Average Total Equity
        /// Trailing Twelve Months</remarks>
        public double ReturnOnAssetsPercent { get; set; }
        /// <summary>
        /// This is a measure of the return on money provided by the firms' owners. This ratio represents Earnings from Continuing Operations divided by average Total Equity and is expressed as a percentage.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: [(Earnings from Continuing Operations) / Total Common Equity] x 100
        /// Trailing Twelve Months</remarks>
        public double ReturnOnEquityPercent { get; set; }

        //Income Statement
        /// <summary>
        /// The amount of money generated by a company's business activities. Also known as Sales.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Trailing Twelve Months</remarks>
        public double RevenueInMillion { get; set; }
        /// <summary>
        /// Revenue in relation to shares.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Total Revenues / Weighted Average Shares Outstanding
        /// Trailing Twelve Months</remarks>
        public double RevenuePerShare { get; set; }
        /// <summary>
        /// The growth of Quarterly Total Revenues from the same quarter a year ago.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: [(Qtrly Total Revenues x Qtrly Total Revenues (yr ago)) / Qtrly Total Revenues (yr ago)] x 100
        /// Year Over Year</remarks>
        public double QuarterlyRevenueGrowthPercent { get; set; }
        /// <summary>
        /// This item represents Total Revenues minus Cost Of Goods Sold, Total.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Trailing Twelve Months</remarks>
        public double GrossProfitInMillion { get; set; }
        /// <summary>
        /// The accounting acronym EBITDA stands for "Earnings Before Interest, Tax, Depreciation, and Amortization."
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Trailing Twelve Months</remarks>
        public double EBITDAInMillion { get; set; }
        /// <summary>
        /// This ratio shows percentage of Net Income to Common Excluding Extra Items less Earnings Of Discontinued Operations to Total Revenues. This is the dollar amount accruing to common shareholders for dividends and retained earnings.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Net Income - Preferred Dividend and Other Adjustments - Earnings Of Discontinued Operations - Extraordinary Item &amp; Accounting Change
        /// Trailing Twelve Months</remarks>
        public double NetIncomeAvlToCommonInMillion { get; set; }
        /// <summary>
        /// This is the Adjusted Income Available to Common Stockholders (based on Generally Accepted Accounting Principles, GAAP) for the trailing 12 months divided by the trailing 12 month weighted average shares outstanding. Diluted EPS uses diluted weighted average shares in the calculation, or the weighted average shares assuming all convertible securities are exercised.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: (Net Income - Preferred Dividend and Other Adjustments)/ Weighted Average Diluted Shares Outstanding
        /// Trailing Twelve Months</remarks>
        public double DilutedEPS { get; set; }
        /// <summary>
        /// The growth of Quarterly Net Income from the same quarter a year ago.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: [(Qtrly Net Income x Qtrly Net Income (yr ago)) / Qtrly Net Income (yr ago)] x 100
        /// Year Over Year</remarks>
        public double QuaterlyEarningsGrowthPercent { get; set; }

        //Balance Sheet and CashFlowStatement
        /// <summary>
        /// The Total Cash and Short-term Investments on the elance sheet as of the most recent quarter.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Most Recent Quarter</remarks>
        public double TotalCashInMillion { get; set; }
        /// <summary>
        /// This is the Total Cash plus Short Term Investments divided by the Shares Outstanding at the end of the most recent fiscal quarter.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Most Recent Quarter</remarks>
        public double TotalCashPerShare { get; set; }
        /// <summary>
        /// The Total Debt on the elance sheet as of the most recent quarter.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Short Term Borrowings + Current Portion of Long Term Debt + Current Portion of Capital Lease + Long Term Debt + Long Term Capital Lease + Finance Division Debt Current + Finance Division Debt Non Current
        /// Most Recent Quarter</remarks>
        public double TotalDeptInMillion { get; set; }
        /// <summary>
        /// This ratio is Total Debt for the most recent fiscal quarter divided by Total Shareholder Equity for the same period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: [(Long-term Debt + Capital Leases + Finance Division Debt Non-Current + Short-term Borrowings + Current Portion of Long-term Debt + Current Portion of Capital Lease Obligation + Finance Division Debt Current) / (Total Common Equity + Total Preferred Equity)] x 100
        /// Most Recent Quarter</remarks>
        public double TotalDeptPerEquity { get; set; }
        /// <summary>
        /// This is the ratio of Total Current Assets for the most recent quarter divided by Total Current Liabilities for the same period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Total Current Assets / Total Current Liabilities
        /// Most Recent Quarter</remarks>
        public double CurrentRatio { get; set; }
        /// <summary>
        /// This is defined as the Common Shareholder's Equity divided by the Shares Outstanding at the end of the most recent fiscal quarter.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Total Common Equity / Total Common Shares Outstanding
        /// Most Recent Quarter</remarks>
        public double BookValuePerShare { get; set; }
        /// <summary>
        /// Net cash used or generated in operating activities during the stated period of time. It reflects net impact of all operating activity transactions on the cash flow of the entity. This GAAP figure is taken directly from the company's Cash Flow Statement and might include significant non-recurring items.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: Net Income + Depreciation and Amortization, Total + Other Amortization + Other Non-Cash Items, Total + Change in Working Capital
        /// Trailing Twelve Months</remarks>
        public double OperatingCashFlowInMillion { get; set; }
        /// <summary>
        /// This figure is a normalized item that excludes non-recurring items and also takes into consideration cash inflows from financing activities such as debt or preferred stock issuances.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Formula: (EBIT + Interest Expense) x (1 x Tax Rate) + Depreciation &amp; Amort., Total + Other Amortization + Capital Expenditure + Sale (Purchase) of Intangible assets - Change in Net Working Capital + Pref. Dividends Paid + Total Debt Repaid + Total Debt Issued + Repurchase of Preferred + Issuance of Preferred Stock   -- [Where: Tax Rate = 0.375]
        /// Trailing Twelve Months</remarks>
        public double LeveredFreeCashFlowInMillion { get; set; }


        internal CompanyFinancialHighlights(System.DateTime fiscalYEnds, System.DateTime mostRecentQtr, double[] values)
        {
            this.FiscalYearEnds = fiscalYEnds;
            this.MostRecentQuarter = mostRecentQtr;

            this.ProfitMarginPercent = values[0];
            this.OperatingMarginPercent = values[1];

            this.ReturnOnAssetsPercent = values[2];
            this.ReturnOnEquityPercent = values[3];

            this.RevenueInMillion = values[4];
            this.RevenuePerShare = values[5];
            this.QuarterlyRevenueGrowthPercent = values[6];
            this.GrossProfitInMillion = values[7];
            this.EBITDAInMillion = values[8];
            this.NetIncomeAvlToCommonInMillion = values[9];
            this.DilutedEPS = values[10];
            this.QuaterlyEarningsGrowthPercent = values[11];

            this.TotalCashInMillion = values[12];
            this.TotalCashPerShare = values[13];
            this.TotalDeptInMillion = values[14];
            this.TotalDeptPerEquity = values[15];
            this.CurrentRatio = values[16];
            this.BookValuePerShare = values[17];

            this.OperatingCashFlowInMillion = values[18];
            this.LeveredFreeCashFlowInMillion = values[19];
        }

    }


    public class CompanyTradingInfo
    {

        //StockPriceHistory
        /// <summary>
        /// The Beta used is Beta of Equity. Beta is the monthly price change of a particular company relative to the monthly price change of the S&amp;P500. The time period for Beta is 3 years (36 months) when available.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Beta { get; set; }
        /// <summary>
        /// The percentage change in price from 52 weeks ago.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double OneYearChangePercent { get; set; }
        /// <summary>
        /// The S&amp;P 500 Index's percentage change in price from 52 weeks ago.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double SP500OneYearChangePercent { get; set; }
        /// <summary>
        /// This price is the highest Price the stock traded at in the last 12 months. This could be an intraday high.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double OneYearHigh { get; set; }
        /// <summary>
        /// This price is the lowest Price the stock traded at in the last 12 months. This could be an intraday low.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double OneYearLow { get; set; }
        /// <summary>
        /// A simple moving average that is calculated by dividing the sum of the closing prices in the last 50 trading days by 50.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double FiftyDayMovingAverage { get; set; }
        /// <summary>
        /// A simple moving average that is calculated by dividing the sum of the closing prices in the last 200 trading days by 200.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double TwoHundredDayMovingAverage { get; set; }

        //Share Statistics
        /// <summary>
        /// This is the average daily trading volume during the last 3 months.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double AverageVolumeThreeMonthInThousand { get; set; }
        /// <summary>
        /// This is the average daily trading volume during the last 10 days.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double AverageVolumeTenDaysInThousand { get; set; }
        /// <summary>
        /// This is the number of shares of common stock currently outstanding—the number of shares issued minus the shares held in treasury. This field reflects all offerings and acquisitions for stock made after the end of the previous fiscal period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double SharesOutstandingInMillion { get; set; }
        /// <summary>
        /// This is the number of freely traded shares in the hands of the public. Float is calculated as Shares Outstanding minus Shares Owned by Insiders, 5% Owners, and Rule 144 Shares.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double FloatInMillion { get; set; }
        /// <summary>
        /// This is the number of shares currently borrowed by investors for sale, but not yet returned to the owner (lender).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double PercentHeldByInsiders { get; set; }
        public double PercentHeldByInstitutions { get; set; }
        public double SharesShortInMillion { get; set; }
        /// <summary>
        /// This represents the number of days it would take to cover the Short Interest if trading continued at the average daily volume for the month. It is calculated as the Short Interest for the Current Month divided by the Average Daily Volume.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ShortRatio { get; set; }
        /// <summary>
        /// Number of shares short divided by float.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ShortPercentOfFloat { get; set; }
        /// <summary>
        /// Shares Short in the prior month. See Shares Short.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double SharesShortPriorMonthInMillion { get; set; }


        //Dividends and Splits
        /// <summary>
        /// The annualized amount of dividends expected to be paid in the current fiscal year.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ForwardAnnualDividendRate { get; set; }
        /// <summary>
        /// Formula: (Forward Annual Dividend Rate / Current Market Price) x 100
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double ForwardAnnualDividendYieldPercent { get; set; }
        /// <summary>
        /// The sum of all dividends paid out in the trailing 12-month period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double TrailingAnnualDividendYield { get; set; }
        /// <summary>
        /// Formula: (Trailing Annual Dividend Rate / Current Market Price) x 100
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double TrailingAnnualDividendYieldPercent { get; set; }
        /// <summary>
        /// The average Forward Annual Dividend Yield in the past 5 years.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double FiveYearAverageDividendYieldPercent { get; set; }
        /// <summary>
        /// The ratio of Earnings paid out in Dividends, expressed as a percentage.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double PayoutRatio { get; set; }
        /// <summary>
        /// The payment date for a declared dividend.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.DateTime DividendDate { get; set; }
        /// <summary>
        /// The first day of trading when the seller, rather than the buyer, of a stock is entitled to the most recently announced dividend payment. The date set by the NYSE (and generally followed on other U.S. exchanges) is currently two business days before the record date. A stock that has gone ex-dividend is denoted by an x in newspaper listings on that date.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.DateTime ExDividendDate { get; set; }
        public System.DateTime LastSplitDate { get; set; }
        public SharesSplitFactor LastSplitFactor { get; set; }



        internal CompanyTradingInfo(double[] values, System.DateTime dividendDate, System.DateTime exDividendDate, System.DateTime lastSplitDate, SharesSplitFactor lastSplitFactor)
        {
            this.Beta = values[0];
            this.OneYearChangePercent = values[1];
            this.SP500OneYearChangePercent = values[2];
            this.OneYearHigh = values[3];
            this.OneYearLow = values[4];
            this.FiftyDayMovingAverage = values[5];
            this.TwoHundredDayMovingAverage = values[6];

            this.AverageVolumeThreeMonthInThousand = values[7];
            this.AverageVolumeTenDaysInThousand = values[8];
            this.SharesOutstandingInMillion = values[9];
            this.FloatInMillion = values[10];
            this.PercentHeldByInsiders = values[11];
            this.PercentHeldByInstitutions = values[12];
            this.SharesShortInMillion = values[13];
            this.ShortRatio = values[14];
            this.ShortPercentOfFloat = values[15];
            this.SharesShortPriorMonthInMillion = values[16];

            this.ForwardAnnualDividendRate = values[17];
            this.ForwardAnnualDividendYieldPercent = values[18];
            this.TrailingAnnualDividendYield = values[19];
            this.TrailingAnnualDividendYieldPercent = values[20];
            this.FiveYearAverageDividendYieldPercent = values[21];
            this.PayoutRatio = values[22];
            this.DividendDate = dividendDate;
            this.ExDividendDate = exDividendDate;
            this.LastSplitFactor = lastSplitFactor;
            this.LastSplitDate = lastSplitDate;
        }

    }

    /// <summary>
    /// Provides properties for a stock split relation.
    /// </summary>
    /// <remarks></remarks>
    public class SharesSplitFactor
    {

        /// <summary>
        /// Old relational value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int OldShares { get; set; }
        /// <summary>
        /// New relational value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int NewShares { get; set; }


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="newShares">The new number of shares after splitting (relative)</param>
        /// <param name="forOldShares">The old number of shares before splitting (relative)</param>
        /// <remarks></remarks>
        public SharesSplitFactor(int newShares, int forOldShares)
        {
            this.OldShares = forOldShares;
            this.NewShares = newShares;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", this.NewShares, this.OldShares);
        }

    }



    public class CompanyStatisticsDownloadSettings : Base.SettingsBase
    {


        public string ID { get; set; }

        public CompanyStatisticsDownloadSettings()
        {
            this.ID = string.Empty;
        }
        public CompanyStatisticsDownloadSettings(string id)
        {
            this.ID = id;
        }


        protected override string GetUrl()
        {
            if (this.ID == string.Empty) { throw new ArgumentException("ID is empty.", "ID"); }
            return string.Format("http://finance.yahoo.com/q/ks?s={0}", this.ID);
        }

        public override object Clone()
        {
            return new CompanyStatisticsDownloadSettings(this.ID);
        }
    }

}
