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
using MaasOne.Finance.YahooScreener;
using MaasOne.Finance.YahooScreener.Criterias;
using MaasOne.Finance.YahooFinance;


namespace MaasOne.Finance.YahooScreener
{


    public partial class StockScreenerDownload : Base.DownloadClient<StockScreenerResult>
    {

        public StockScreenerDownloadSettings Settings { get { return (StockScreenerDownloadSettings)base.Settings; } set { base.SetSettings(value); } }
        public StockScreenerDownload()
        {
            this.Settings = new StockScreenerDownloadSettings();
        }

        public void DownloadAsync(IEnumerable<StockCriteriaDefinition> criterias, object userArgs)
        {
            if (criterias == null)
                throw new ArgumentNullException("criterias", "The criterias enumerable is null.");
            base.DownloadAsync(new StockScreenerDownloadSettings() { Criterias = MyHelper.EnumToArray(criterias), Comparing = false }, userArgs);
        }
        public void DownloadAsync(IEnumerable<IID> ids, IEnumerable<StockCriteriaDefinition> criterias, object userArgs)
        {
            this.DownloadAsync(FinanceHelper.IIDsToStrings(ids), MyHelper.EnumToArray(criterias), userArgs);
        }
        public void DownloadAsync(IEnumerable<string> ids, IEnumerable<StockCriteriaDefinition> criterias, object userArgs)
        {
            if (criterias == null)
                throw new ArgumentNullException("criterias", "The criterias enumerable is null.");
            if (ids == null)
                throw new ArgumentNullException("ids", "The ID Enumerable is null.");
            List<string> strIDs = new List<string>(ids);
            if (strIDs.Count == 0)
                throw new ArgumentException("ids", "There must be minimum one ID available for downloading.");
            base.DownloadAsync(new StockScreenerDownloadSettings() { IDs = strIDs.ToArray(), Criterias = MyHelper.EnumToArray(criterias), Comparing = true }, userArgs);
        }

        public void DownloadAsync(StockScreenerDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        protected override StockScreenerResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            StockScreenerDownloadSettings set = (StockScreenerDownloadSettings)settings;
            List<StockScreenerData> results = new List<StockScreenerData>();
            string result = MyHelper.StreamToString(stream, set.TextEncoding);

            List<string> lines = new List<string>(result.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

            if (lines.Count > 0)
            {
                System.Globalization.CultureInfo convCulture = new System.Globalization.CultureInfo("en-US");

                List<QuoteProperty> quoteProps = new List<QuoteProperty>();
                List<StockScreenerProperty> screenerProps = new List<StockScreenerProperty>();
                foreach (StockCriteriaDefinition crit in set.Criterias)
                {
                    foreach (QuoteProperty qp in crit.ProvidedQuoteProperties)
                    {
                        if (!quoteProps.Contains(qp))
                            quoteProps.Add(qp);
                    }
                    foreach (StockScreenerProperty sp in crit.ProvidedScreenerProperties)
                    {
                        if (!screenerProps.Contains(sp))
                            screenerProps.Add(sp);
                    }
                }

                string[] propertySymbols = new string[-1 + 1];
                string[] propertyNames = new string[-1 + 1];
                int startIndex = 0;

                if (!set.Comparing)
                {
                    startIndex = 2;
                    propertySymbols = lines[0].Split('|');
                    propertyNames = lines[1].Split('|');
                }
                else
                {
                    startIndex = 0;
                    List<string> lstSymbols = new List<string>();
                    lstSymbols.Add("");
                    lstSymbols.Add("");
                    lstSymbols.Add("b");
                    lstSymbols.Add("");
                    lstSymbols.Add("c");
                    lstSymbols.Add("8o");
                    lstSymbols.Add("9c");
                    lstSymbols.Add("9t");
                    foreach (StockCriteriaDefinition crt in set.Criterias)
                    {
                        if (crt.ProvidedQuoteProperties.Length > 5 | crt.ProvidedScreenerProperties.Length > 3)
                        {
                            lstSymbols.Add(crt.CriteriaTag);
                        }
                    }
                    propertySymbols = lstSymbols.ToArray();

                    List<string> lstNames = new List<string>();
                    lstNames.Add("Ticker");
                    lstNames.Add("Company Name");
                    lstNames.Add("Last Trade");
                    lstNames.Add("Trade Time");
                    lstNames.Add("Mkt Cap");
                    lstNames.Add("Return On Equity");
                    lstNames.Add("Return On Assets");
                    lstNames.Add("Forward PE");
                    foreach (StockCriteriaDefinition crt in set.Criterias)
                    {
                        if (crt.ProvidedQuoteProperties.Length > 5 | crt.ProvidedScreenerProperties.Length > 3)
                        {
                            switch (crt.CriteriaTag)
                            {
                                case "f":
                                case "g":
                                    if (crt is PriceGainerLosersCriteria)
                                    {
                                        PriceGainerLosersCriteria mngCrt = (PriceGainerLosersCriteria)crt;
                                        if (mngCrt.ValueRelativeTo == StockTradingAbsoluteTimePoint.TodaysOpen)
                                        {
                                            lstNames.Add("(open)");
                                        }
                                        else
                                        {
                                            lstNames.Add("(close)");
                                        }
                                    }
                                    break;
                                case "h":
                                case "i":
                                    if (crt is PriceMomentumCriteria)
                                    {
                                        PriceMomentumCriteria mngCrt = (PriceMomentumCriteria)crt;
                                        lstNames.Add("(" + mngCrt.RelativeTimeSpanInMinutes.ToString().Replace("_", "") + "m)");
                                    }
                                    break;
                                default:
                                    lstNames.Add("");
                                    break;
                            }
                        }
                    }
                    propertyNames = lstNames.ToArray();

                }

                for (int i = startIndex; i <= lines.Count - 1; i++)
                {
                    string[] values = lines[i].Split('|');


                    if (propertySymbols.Length >= 4 & values.Length == propertySymbols.Length & values.Length == propertyNames.Length)
                    {
                        string id = values[0];
                        string name = values[1];
                        double lastTradePriceOnly = 0;
                        double.TryParse(values[2], System.Globalization.NumberStyles.Any, convCulture, out lastTradePriceOnly);
                        DateTime tradeTime = new DateTime();
                        DateTime.TryParse(values[3], convCulture, System.Globalization.DateTimeStyles.None, out tradeTime);
                        tradeTime = tradeTime.AddHours(tradeTime.Hour).AddMinutes(tradeTime.Minute);

                        StockScreenerData res = new StockScreenerData(id, name, lastTradePriceOnly, tradeTime, quoteProps.ToArray(), screenerProps.ToArray());

                        if (values.Length >= 5)
                        {
                            for (int p = 4; p <= values.Length - 1; p++)
                            {
                                if (values[p] != string.Empty & values[p] != "N/A")
                                {
                                    double dblValue = 0;

                                    if (double.TryParse(values[p], System.Globalization.NumberStyles.Any, convCulture, out dblValue) || FinanceHelper.GetMillionValue(values[p]) != 0)
                                    {
                                        switch (propertySymbols[p])
                                        {
                                            case "c":
                                                res[QuoteProperty.MarketCapitalization] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            //case "c":
                                            //res.AdditionalValues[(int)StockScreenerProperty.RevenueEstimate_ThisYear] = dblValue;
                                            //break;
                                            case "8o":
                                                res.AdditionalValues[(int)StockScreenerProperty.ReturnOnEquity] = dblValue;
                                                break;
                                            case "9c":
                                                res.AdditionalValues[(int)StockScreenerProperty.ReturnOnAssets] = dblValue;
                                                break;
                                            case "9t":
                                                res.AdditionalValues[(int)StockScreenerProperty.ForwardPriceToEarningsRatio] = dblValue;
                                                break;
                                            case "9o":
                                                res.AdditionalValues[(int)StockScreenerProperty.NumberOfEmployees] = dblValue;
                                                break;
                                            case "f":
                                                double absoluteChange = dblValue;
                                                double absolutePreviousValue = res.LastTradePriceOnly - absoluteChange;
                                                double changeInPercent = absoluteChange / absolutePreviousValue;

                                                res[QuoteProperty.ChangeInPercent] = changeInPercent * 100;
                                                res[QuoteProperty.Change] = absoluteChange;

                                                if (propertyNames[p].EndsWith("(open)"))
                                                {
                                                    res[QuoteProperty.Open] = absolutePreviousValue;
                                                }
                                                else
                                                {
                                                    res[QuoteProperty.PreviousClose] = absolutePreviousValue;
                                                }

                                                break;
                                            case "g":
                                                changeInPercent = dblValue;
                                                absolutePreviousValue = (res.LastTradePriceOnly / (100 + changeInPercent)) * 100;
                                                absoluteChange = res.LastTradePriceOnly - absolutePreviousValue;

                                                res[QuoteProperty.ChangeInPercent] = changeInPercent;
                                                res[QuoteProperty.Change] = absoluteChange;

                                                if (propertyNames[p].EndsWith("(open)"))
                                                {
                                                    res[QuoteProperty.Open] = absolutePreviousValue;
                                                }
                                                else
                                                {
                                                    res[QuoteProperty.PreviousClose] = absolutePreviousValue;
                                                }

                                                break;
                                            case "h":
                                                if (set.Criterias != null)
                                                {
                                                    PriceMomentumCriteria context = null;

                                                    foreach (StockCriteriaDefinition crit in set.Criterias)
                                                    {
                                                        if (crit != null && crit is PriceMomentumCriteria && ((PriceMomentumCriteria)crit).PercentValues == false)
                                                        {
                                                            context = (PriceMomentumCriteria)crit;
                                                            break; // TODO: might not be correct. Was : Exit For
                                                        }
                                                    }
                                                    if (context != null)
                                                    {
                                                        if (propertyNames[p].EndsWith("(" + context.RelativeTimeSpanInMinutes.ToString().Replace("_", "") + "m)"))
                                                        {
                                                            TemporaryPriceChangeInfo info = new TemporaryPriceChangeInfo();
                                                            info.ChangeRelativeTimePoint = context.TimeSpanRelativeTo;
                                                            info.ChangeTimeSpan = context.RelativeTimeSpanInMinutes;
                                                            info.Change = dblValue * Convert.ToInt32((context.GainOrLoss == StockPriceChangeDirection.Gain ? 1 : -1));
                                                            info.ChangeInPercent = info.Change / (res.LastTradePriceOnly - info.Change);
                                                            res.TemporaryLimitedChange = info;
                                                        }
                                                    }
                                                }

                                                break;
                                            case "i":
                                                if (set.Criterias != null)
                                                {
                                                    PriceMomentumCriteria context = null;
                                                    foreach (StockCriteriaDefinition crit in set.Criterias)
                                                    {
                                                        if (crit != null && crit is PriceMomentumCriteria && ((PriceMomentumCriteria)crit).PercentValues == true)
                                                        {
                                                            context = (PriceMomentumCriteria)crit;
                                                            break; // TODO: might not be correct. Was : Exit For
                                                        }
                                                    }
                                                    if (context != null)
                                                    {
                                                        if (propertyNames[p].EndsWith("(" + context.RelativeTimeSpanInMinutes.ToString().Replace("_", "") + "m)"))
                                                        {
                                                            TemporaryPriceChangeInfo info = new TemporaryPriceChangeInfo();
                                                            info.ChangeRelativeTimePoint = context.TimeSpanRelativeTo;
                                                            info.ChangeTimeSpan = context.RelativeTimeSpanInMinutes;
                                                            info.ChangeInPercent = dblValue * Convert.ToInt32((context.GainOrLoss == StockPriceChangeDirection.Gain ? 1 : -1));
                                                            info.Change = res.LastTradePriceOnly - ((res.LastTradePriceOnly / (100 + info.ChangeInPercent)) * 100);
                                                            res.TemporaryLimitedChange = info;
                                                        }
                                                    }
                                                }

                                                break;
                                            case "j":
                                                if (set.Criterias != null)
                                                {
                                                    ExtremePriceCriteria context = null;
                                                    foreach (StockCriteriaDefinition crit in set.Criterias)
                                                    {
                                                        if (crit != null && crit is ExtremePriceCriteria && ((ExtremePriceCriteria)crit).PercentValues == false)
                                                        {
                                                            context = (ExtremePriceCriteria)crit;
                                                            break; // TODO: might not be correct. Was : Exit For
                                                        }
                                                    }
                                                    if (context != null)
                                                    {
                                                        absoluteChange = dblValue * Convert.ToInt32((context.LessGreater == LessGreater.Greater ? 1 : -1));
                                                        absolutePreviousValue = res.LastTradePriceOnly - absoluteChange;
                                                        changeInPercent = absoluteChange / absolutePreviousValue;
                                                        switch (context.ExtremeParameter)
                                                        {
                                                            case StockExtremeParameter.TodaysHigh:
                                                                res[QuoteProperty.DaysHigh] = absolutePreviousValue;
                                                                break;
                                                            case StockExtremeParameter.TodaysLow:
                                                                res[QuoteProperty.DaysLow] = absolutePreviousValue;
                                                                break;
                                                            case StockExtremeParameter.YearsHigh:
                                                                res[QuoteProperty.YearHigh] = absolutePreviousValue;
                                                                res[QuoteProperty.ChangeInPercentFromYearHigh] = changeInPercent;
                                                                res[QuoteProperty.ChangeFromYearHigh] = absoluteChange;
                                                                break;
                                                            case StockExtremeParameter.YearsLow:
                                                                res[QuoteProperty.YearLow] = absolutePreviousValue;
                                                                res[QuoteProperty.PercentChangeFromYearLow] = changeInPercent;
                                                                res[QuoteProperty.ChangeFromYearLow] = absoluteChange;
                                                                break;
                                                        }
                                                    }
                                                }

                                                break;
                                            case "k":
                                                if (set.Criterias != null)
                                                {
                                                    ExtremePriceCriteria context = null;
                                                    foreach (StockCriteriaDefinition crit in set.Criterias)
                                                    {
                                                        if (crit != null && crit is ExtremePriceCriteria && ((ExtremePriceCriteria)crit).PercentValues == true)
                                                        {
                                                            context = (ExtremePriceCriteria)crit;
                                                            break; // TODO: might not be correct. Was : Exit For
                                                        }
                                                    }
                                                    if (context != null)
                                                    {
                                                        changeInPercent = dblValue * Convert.ToInt32((context.LessGreater == LessGreater.Greater ? 1 : -1));
                                                        absolutePreviousValue = (res.LastTradePriceOnly / (100 + changeInPercent)) * 100;
                                                        absoluteChange = res.LastTradePriceOnly - absolutePreviousValue;
                                                        switch (context.ExtremeParameter)
                                                        {
                                                            case StockExtremeParameter.TodaysHigh:
                                                                res[QuoteProperty.DaysHigh] = absolutePreviousValue;
                                                                break;
                                                            case StockExtremeParameter.TodaysLow:
                                                                res[QuoteProperty.DaysLow] = absolutePreviousValue;
                                                                break;
                                                            case StockExtremeParameter.YearsHigh:
                                                                res[QuoteProperty.YearHigh] = absolutePreviousValue;
                                                                res[QuoteProperty.ChangeInPercentFromYearHigh] = changeInPercent;
                                                                res[QuoteProperty.ChangeFromYearHigh] = absoluteChange;
                                                                break;
                                                            case StockExtremeParameter.YearsLow:
                                                                res[QuoteProperty.YearLow] = absolutePreviousValue;
                                                                res[QuoteProperty.PercentChangeFromYearLow] = changeInPercent;
                                                                res[QuoteProperty.ChangeFromYearLow] = absoluteChange;
                                                                break;
                                                        }
                                                    }
                                                }

                                                break;
                                            case "l":
                                                if (set.Criterias != null)
                                                {
                                                    GapVsPreviousClose context = null;
                                                    foreach (StockCriteriaDefinition crit in set.Criterias)
                                                    {
                                                        if (crit != null && crit is GapVsPreviousClose && ((GapVsPreviousClose)crit).PercentValues == false)
                                                        {
                                                            context = (GapVsPreviousClose)crit;
                                                            break; // TODO: might not be correct. Was : Exit For
                                                        }
                                                    }
                                                    if (context != null)
                                                    {
                                                        res.AdditionalValues[(int)StockScreenerProperty.Gap] = dblValue;
                                                    }
                                                }

                                                break;
                                            case "m":
                                                if (set.Criterias != null)
                                                {
                                                    GapVsPreviousClose context = null;
                                                    foreach (StockCriteriaDefinition crit in set.Criterias)
                                                    {
                                                        if (crit != null && crit is GapVsPreviousClose && ((GapVsPreviousClose)crit).PercentValues == true)
                                                        {
                                                            context = (GapVsPreviousClose)crit;
                                                            break; // TODO: might not be correct. Was : Exit For
                                                        }
                                                    }
                                                    if (context != null)
                                                    {
                                                        res.AdditionalValues[(int)StockScreenerProperty.GapInPercent] = dblValue;
                                                    }
                                                }

                                                break;
                                            case "o":
                                                if (set.Criterias != null)
                                                {
                                                    PriceToMovingAverageRatioCriteria context = null;
                                                    foreach (StockCriteriaDefinition crit in set.Criterias)
                                                    {
                                                        if (crit != null && crit is PriceToMovingAverageRatioCriteria)
                                                        {
                                                            context = (PriceToMovingAverageRatioCriteria)crit;
                                                            break; // TODO: might not be correct. Was : Exit For
                                                        }
                                                    }
                                                    if (context != null)
                                                    {
                                                        changeInPercent = dblValue;
                                                        double maValue = (dblValue / (100 + changeInPercent)) * 100;
                                                        absoluteChange = res.LastTradePriceOnly - maValue;

                                                        if (context.MovingAverage == MovingAverageType.FiftyDays)
                                                        {
                                                            res[QuoteProperty.FiftydayMovingAverage] = maValue;
                                                            res[QuoteProperty.ChangeFromFiftydayMovingAverage] = absoluteChange;
                                                            res[QuoteProperty.PercentChangeFromFiftydayMovingAverage] = changeInPercent;
                                                        }
                                                        else
                                                        {
                                                            res[QuoteProperty.TwoHundreddayMovingAverage] = maValue;
                                                            res[QuoteProperty.ChangeFromTwoHundreddayMovingAverage] = absoluteChange;
                                                            res[QuoteProperty.PercentChangeFromTwoHundreddayMovingAverage] = changeInPercent;
                                                        }
                                                    }
                                                }

                                                break;
                                            case "7":
                                                res.AdditionalValues[(int)StockScreenerProperty.Beta] = dblValue;
                                                break;
                                            case "v":
                                                res[QuoteProperty.PriceSales] = dblValue;
                                                break;
                                            case "e":
                                                res.AdditionalValues[(int)StockScreenerProperty.PriceEarningsRatio] = dblValue;
                                                break;
                                            case "u":
                                                res[QuoteProperty.PEGRatio] = dblValue;
                                                break;
                                            case "9p":
                                                res.AdditionalValues[(int)StockScreenerProperty.EntityValue] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            case "9q":
                                                res.AdditionalValues[(int)StockScreenerProperty.EntityValueToRevenueRatio] = dblValue;
                                                break;
                                            case "9r":
                                                res.AdditionalValues[(int)StockScreenerProperty.EntityValueToOperatingCashFlowRatio] = dblValue;
                                                break;
                                            case "9s":
                                                res.AdditionalValues[(int)StockScreenerProperty.EntityValueToFreeCashFlowRatio] = dblValue;
                                                break;
                                            case "x":
                                                res[QuoteProperty.EPSEstimateNextQuarter] = dblValue;
                                                break;
                                            case "y":
                                                res[QuoteProperty.EPSEstimateCurrentYear] = dblValue;
                                                break;
                                            case "z":
                                                res[QuoteProperty.EPSEstimateNextYear] = dblValue;
                                                break;
                                            case "8e":
                                                res.AdditionalValues[(int)StockScreenerProperty.EPS_NYCE] = dblValue;
                                                break;
                                            case "9v":
                                                res.AdditionalValues[(int)StockScreenerProperty.SalesGrowthEstimate_ThisQuarter] = dblValue;
                                                break;
                                            case "8h":
                                                res.AdditionalValues[(int)StockScreenerProperty.EarningsGrowthEstimate_ThisYear] = dblValue;
                                                break;
                                            case "9b":
                                                res.AdditionalValues[(int)StockScreenerProperty.EarningsGrowthEstimate_NextYear] = dblValue;
                                                break;
                                            case "9u":
                                                res.AdditionalValues[(int)StockScreenerProperty.EarningsGrowthEstimate_Next5Years] = dblValue;
                                                break;
                                            case "1":
                                                res.AdditionalValues[(int)StockScreenerProperty.SharesOutstanding] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            case "2":
                                                res[QuoteProperty.SharesFloat] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            case "3":
                                                res[QuoteProperty.ShortRatio] = dblValue;
                                                break;
                                            case "8g":
                                                res.AdditionalValues[(int)StockScreenerProperty.SharesShortPriorMonth] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            case "8m":
                                                res.AdditionalValues[(int)StockScreenerProperty.SharesShort] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            case "9d":
                                                res.AdditionalValues[(int)StockScreenerProperty.HeldByInsiders] = dblValue;
                                                break;
                                            case "9n":
                                                res.AdditionalValues[(int)StockScreenerProperty.HeldByInstitutions] = dblValue;
                                                break;
                                            case "4":
                                                res[QuoteProperty.TrailingAnnualDividendYield] = dblValue;
                                                break;
                                            case "5":
                                                res[QuoteProperty.TrailingAnnualDividendYieldInPercent] = dblValue;
                                                break;
                                            case "8a":
                                                res.AdditionalValues[(int)StockScreenerProperty.OperatingMargin] = dblValue;
                                                break;
                                            case "8r":
                                                res.AdditionalValues[(int)StockScreenerProperty.ProfitMargin_ttm] = dblValue;
                                                break;
                                            case "9f":
                                                res.AdditionalValues[(int)StockScreenerProperty.EBITDAMargin_ttm] = dblValue;
                                                break;
                                            case "9k":
                                                res.AdditionalValues[(int)StockScreenerProperty.GrossMargin_ttm] = dblValue;
                                                break;
                                            case "8f":
                                                res[QuoteProperty.PriceBook] = dblValue;
                                                break;
                                            //case "8f":
                                            //res.AdditionalValues[(int)StockScreenerProperty.CashPerShare] = dblValue;
                                            //break;
                                            case "8l":
                                                res.AdditionalValues[(int)StockScreenerProperty.TotalCash] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            case "6":
                                                res[QuoteProperty.BookValuePerShare] = dblValue;
                                                break;
                                            case "9e":
                                                res.AdditionalValues[(int)StockScreenerProperty.TotalDebt] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            case "9g":
                                                res.AdditionalValues[(int)StockScreenerProperty.TotalDebtToEquityRatio] = dblValue;
                                                break;
                                            case "9h":
                                                res.AdditionalValues[(int)StockScreenerProperty.CurrentRatio] = dblValue;
                                                break;
                                            case "9i":
                                                res.AdditionalValues[(int)StockScreenerProperty.LongTermDebtToEquityRatio] = dblValue;
                                                break;
                                            case "9l":
                                                res.AdditionalValues[(int)StockScreenerProperty.QuickRatio] = dblValue;
                                                break;
                                            case "w":
                                                res[QuoteProperty.DilutedEPS] = dblValue;
                                                break;
                                            case "8i":
                                                res.AdditionalValues[(int)StockScreenerProperty.EPS_mrq] = dblValue;
                                                break;
                                            case "0":
                                                res.AdditionalValues[(int)StockScreenerProperty.Sales_ttm] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            case "t":
                                                res[QuoteProperty.EBITDA] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            case "8n":
                                                res.AdditionalValues[(int)StockScreenerProperty.GrossProfit] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            case "8p":
                                                res.AdditionalValues[(int)StockScreenerProperty.NetIncome] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            case "9j":
                                                res.AdditionalValues[(int)StockScreenerProperty.OperatingIncome] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            case "8v":
                                                res.AdditionalValues[(int)StockScreenerProperty.EarningsGrowth_Past5Years] = dblValue;
                                                break;
                                            case "9a":
                                                res.AdditionalValues[(int)StockScreenerProperty.RevenueEstimate_ThisQuarter] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            case "8q":
                                                res.AdditionalValues[(int)StockScreenerProperty.RevenueEstimate_NextQuarter] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            case "8s":
                                                res.AdditionalValues[(int)StockScreenerProperty.SalesGrowthEstimate_NextQuarter] = dblValue;
                                                break;
                                            case "8t":
                                                res.AdditionalValues[(int)StockScreenerProperty.SalesGrowthEstimate_ThisYear] = dblValue;
                                                break;
                                            case "8k":
                                                res.AdditionalValues[(int)StockScreenerProperty.SalesGrowthEstimate_NextYear] = dblValue;
                                                break;
                                            case "8y":
                                                res.AdditionalValues[(int)StockScreenerProperty.FreeCashFlow] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));
                                                break;
                                            case "8z":
                                                res.AdditionalValues[(int)StockScreenerProperty.OperatingCashFlow] = Convert.ToInt64(FinanceHelper.GetMillionValue(values[p]) * (Math.Pow(10, 6)));

                                                break;
                                        }

                                    }
                                }
                            }
                        }

                        results.Add(res);

                    }
                }

            }
            return new StockScreenerResult(results.ToArray());
        }

    }



    public class StockScreenerResult
    {
        private StockScreenerData[] mItems = null;
        public StockScreenerData[] Items
        {
            get { return mItems; }
        }
        internal StockScreenerResult(StockScreenerData[] items)
        {
            mItems = items;
        }

    }


    public class StockScreenerData : QuotesData
    {
        private QuoteProperty[] mProvidedQuoteProperties = null;

        private StockScreenerProperty[] mProvidedScreenerProperties = null;
        public TemporaryPriceChangeInfo TemporaryLimitedChange { get; set; }
        public QuoteProperty[] ProvidedQuoteProperties
        {
            get { return mProvidedQuoteProperties; }
        }
        public StockScreenerProperty[] ProvidedScreenerProperties
        {
            get { return mProvidedScreenerProperties; }
        }
        private Nullable<double>[] mAdditionalValues = new Nullable<double>[48];
        /// <summary>
        /// All additional values for StockScreenerData
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Nullable<double>[] AdditionalValues
        {
            get { return mAdditionalValues; }
        }
        /// <summary>
        /// A specific additional value for StockScreenerData
        /// </summary>
        /// <param name="prp"></param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Nullable<double> GetAdditionalValues(StockScreenerProperty prp)
        {
            return mAdditionalValues[Convert.ToInt32(prp)];
        }
        public void SetAdditionalValues(StockScreenerProperty prp, Nullable<double> value)
        {
            mAdditionalValues[Convert.ToInt32(prp)] = value;
        }
        
        public StockScreenerData(QuoteProperty[] qProps, StockScreenerProperty[] sProps)
        {
            mProvidedQuoteProperties = qProps;
            mProvidedScreenerProperties = sProps;
        }
        public StockScreenerData(string id, string name, double lastTradePriceOnly, DateTime lastTradeTime, QuoteProperty[] qProps, StockScreenerProperty[] sProps)
            : this(qProps, sProps)
        {
            base[QuoteProperty.Symbol] = id;
            base[QuoteProperty.Name] = name;
            base[QuoteProperty.LastTradePriceOnly] = lastTradePriceOnly;
            base[QuoteProperty.LastTradeTime] = lastTradeTime;
        }

    }


    /// <summary>
    /// Class for describing a value change in a specific time span relative to a specific time point
    /// </summary>
    /// <remarks></remarks>
    public class TemporaryPriceChangeInfo
    {
        public double Change { get; set; }
        public double ChangeInPercent { get; set; }
        public StockTradingTimeSpan ChangeTimeSpan { get; set; }
        public StockTradingRelativeTimePoint ChangeRelativeTimePoint { get; set; }
    }




    public class StockScreenerDownloadSettings : Base.SettingsBase, ITextEncodingSettings
    {
        public System.Text.Encoding TextEncoding { get; set; }

        public bool IgnoreInvalidCriterias { get; set; }

        public StockCriteriaDefinition[] Criterias { get; set; }
        public string[] IDs { get; set; }
        internal bool Comparing = false;

        public StockScreenerDownloadSettings()
        {
            this.TextEncoding = System.Text.Encoding.UTF8;
            this.Criterias = new StockCriteriaDefinition[] { };
            this.IDs = new string[] { };
        }


        protected override string GetUrl()
        {
            if (this.IDs != null && this.IDs.Length > 0)
            {
                return this.DownloadUrl(this.IDs, this.Criterias);
            }
            else
            {
                return this.DownloadUrl(this.Criterias);
            }
        }
        private string DownloadUrl(IEnumerable<StockCriteriaDefinition> criterias)
        {
            List<string> tags = new List<string>();
            foreach (StockCriteriaDefinition crit in criterias)
            {
                if (tags.Contains(crit.CriteriaTag))
                {
                    throw new NotSupportedException("Multiple criterias of same type (except percent/no percent difference) are not supported.");
                }
                else
                {
                    tags.Add(crit.CriteriaTag);
                }
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("http://reports.finance.yahoo.com/wr?a0=2.0");
            foreach (StockCriteriaDefinition crit in criterias)
            {
                if (crit != null && crit.IsValid)
                {
                    sb.Append(crit.CriteriaParameter());
                }
                else
                {
                    if (!this.IgnoreInvalidCriterias)
                    {
                        throw new NotSupportedException("Invalid criterias will not be supported. Check out the values of your passed criterias for null values.");
                    }
                }
            }
            sb.Append("&ln=-1");
            return sb.ToString();
        }
        private string DownloadUrl(string[] ids, IEnumerable<StockCriteriaDefinition> criterias)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(this.DownloadUrl(criterias));
            sb.Replace("&ln=-1", "");
            sb.Append("&9w=");
            foreach (string id in ids)
            {
                sb.Append(FinanceHelper.CleanIndexID(id).ToUpper() + "_");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public override object Clone()
        {
            return new StockScreenerDownloadSettings() { Criterias = (StockCriteriaDefinition[])this.Criterias.Clone(), IDs = (string[])this.IDs.Clone() };
        }

    }
}




