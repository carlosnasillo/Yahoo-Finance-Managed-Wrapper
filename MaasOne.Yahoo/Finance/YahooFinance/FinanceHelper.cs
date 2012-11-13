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
    internal abstract class FinanceHelper
    {
        public const string NameOptionSymbol = "symbol";
        public const string NameOptionType = "type";
        public const string NameOptionLastPrice = "lastPrice";
        public const string NameOptionStrikePrice = "strikePrice";
        public const string NameOptionChange = "change";
        public const string NameOptionBid = "bid";
        public const string NameOptionAsk = "ask";
        public const string NameOptionVolume = "vol";
        public const string NameOptionOpenInterest = "openInt";

        public const string NameOptionChangeDir = "changeDir";
        public const string NameQuoteBaseID = "Symbol";
        public const string NameQuoteBaseLastTradePriceOnly = "LastTradePriceOnly";
        public const string NameQuoteBaseChange = "Change";
        public const string NameQuoteBaseOpen = "Open";
        public const string NameQuoteBaseDaysHigh = "DaysHigh";
        public const string NameQuoteBaseDaysLow = "DaysLow";
        public const string NameQuoteBaseVolume = "Volume";
        public const string NameQuoteBaseLastTradeDate = "LastTradeDate";

        public const string NameQuoteBaseLastTradeTime = "LastTradeTime";
        public const string NameHistQuoteDate = "Date";
        public const string NameHistQuoteOpen = "Open";
        public const string NameHistQuoteHigh = "High";
        public const string NameHistQuoteLow = "Low";
        public const string NameHistQuoteClose = "Close";
        public const string NameHistQuoteVolume = "Volume";

        public const string NameHistQuoteAdjClose = "AdjClose";
        public const string NameMarketName = "name";
        public const string NameIndustryID = "id";
        public const string NameCompanySymbol = "symbol";
        public const string NameCompanyCompanyName = "CompanyName";
        public const string NameCompanyStart = "start";
        public const string NameCompanyEnd = "end";
        public const string NameCompanySector = "Sector";
        public const string NameCompanyIndustry = "Industry";
        public const string NameCompanyFullTimeEmployees = "FullTimeEmployees";

        public const string NameCompanyNotAvailable = "NaN";

        private static System.Globalization.CultureInfo mDefaultCulture = new System.Globalization.CultureInfo("en-US");
        public static System.Globalization.CultureInfo DefaultYqlCulture
        {
            get { return mDefaultCulture; }
        }

        public static IEnumerable<string> IIDsToStrings(IEnumerable<IID> idList)
        {
            List<string> lst = new List<string>();
            if (idList != null)
            {
                foreach (IID id in idList)
                {
                    if (id != null && id.ID != string.Empty)
                        lst.Add(id.ID);
                }
            }
            return lst;
        }
        public static Sector[] SectorEnumToArray(IEnumerable<Sector> values)
        {
            List<Sector> lst = new List<Sector>();
            if (values != null)
            {
                lst.AddRange(values);
            }
            return lst.ToArray();
        }
        public static string[] CleanIDfromAT(IEnumerable<string> enm)
        {
            if (enm != null)
            {
                List<string> lst = new List<string>();
                foreach (string id in enm)
                {
                    lst.Add(CleanIndexID(id));
                }
                return lst.ToArray();
            }
            else
            {
                return null;
            }
        }
        public static string CleanIndexID(string id)
        {
            return id.Replace("@", "");
        }
        public static QuoteProperty[] CheckPropertiesOfQuotesData(IEnumerable<QuotesData> quotes, IEnumerable<QuoteProperty> properties)
        {
            List<QuoteProperty> lstProperties = new List<QuoteProperty>();
            if (properties == null)
            {
                return GetAllActiveProperties(quotes);
            }
            else
            {
                lstProperties.AddRange(properties);
                if (lstProperties.Count == 0)
                {
                    return GetAllActiveProperties(quotes);
                }
                else
                {
                    return lstProperties.ToArray();
                }
            }
        }

        public static QuoteProperty[] GetAllActiveProperties(IEnumerable<QuotesData> quotes)
        {
            List<QuoteProperty> lst = new List<QuoteProperty>();
            if (quotes != null)
            {
                foreach (QuoteProperty qp in Enum.GetValues(typeof(QuoteProperty)))
                {
                    bool valueIsNotNull = false;
                    foreach (QuotesData quote in quotes)
                    {
                        if (quote[qp] != null)
                        {
                            valueIsNotNull = true;
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                    if (valueIsNotNull)
                        lst.Add(qp);
                }
            }
            return lst.ToArray();
        }

        public static double GetStringMillionFactor(string s)
        {
            if (s.EndsWith("T") || s.EndsWith("K"))
            {
                return 1.0 / 1000;
            }
            else if (s.EndsWith("M"))
            {
                return 1;
            }
            else if (s.EndsWith("B"))
            {
                return 1000;
            }
            else
            {
                return 0;
            }
        }
        public static double GetMillionValue(string s)
        {
            double v = 0;
            double.TryParse(s.Substring(0, s.Length - 1), System.Globalization.NumberStyles.Any, mDefaultCulture, out v);
            return v * GetStringMillionFactor(s);
        }
        public static string CleanTd(string value)
        {
            List<char> sb = new List<char>();
            if (value.Length > 0)
            {
                bool allowCopy = true;
                for (int i = 0; i <= value.Length - 1; i++)
                {
                    if (value[i] == '<')
                    {
                        allowCopy = false;
                    }
                    else if (value[i] == '>')
                    {
                        allowCopy = true;
                        continue;
                    }
                    if (allowCopy)
                        sb.Add(value[i]);
                }
            }
            return new string(sb.ToArray()).Replace("&nbsp;", "");
        }
        public static double ParseToDouble(string s)
        {
            double v = 0;
            double.TryParse(s.Replace("%", ""), System.Globalization.NumberStyles.Any, mDefaultCulture, out v);
            return v;
        }

        public static DateTime ParseToDateTime(string s)
        {
            DateTime d = new DateTime();
            System.DateTime.TryParse(s, mDefaultCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out d);
            return d;
        }
        public static Support.YCurrencyID YCurrencyIDFromString(string id)
        {
            string idStr = id.ToUpper();
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[A-Z][A-Z][A-Z][A-Z][A-Z][A-Z]=X");
            if (idStr.Length == 8 && regex.Match(idStr).Success)
            {
                Support.CurrencyInfo b = null;
                Support.CurrencyInfo dep = null;
                string baseStr = idStr.Substring(0, 3);
                string depStr = idStr.Substring(3, 3);
                foreach (Support.CurrencyInfo cur in Support.WorldMarket.DefaultCurrencies)
                {
                    if (baseStr == cur.ID)
                    {
                        b = new Support.CurrencyInfo(cur.ID, cur.Description);
                    }
                    else if (depStr == cur.ID)
                    {
                        dep = new Support.CurrencyInfo(cur.ID, cur.Description);
                    }
                    if (b != null & dep != null)
                    {
                        return new Support.YCurrencyID(b, dep);
                    }
                }

                return null;
            }
            else
            {
                return null;
            }
        }

        public static string GetChartImageSize(ChartImageSize value)
        {
            return value.ToString().Substring(0, 1).ToLower();
        }
        public static string GetChartTimeSpan(ChartTimeSpan value)
        {
            if (value == ChartTimeSpan.cMax)
            {
                return "my";
            }
            else
            {
                return value.ToString().Replace("c", "").ToLower();
            }
        }
        public static string GetChartType(ChartType value)
        {
            return value.ToString().Substring(0, 1).ToLower();
        }
        public static string GetChartScale(ChartScale value)
        {
            if (value == ChartScale.Arithmetic)
            {
                return "off";
            }
            else
            {
                return "on";
            }
        }
        public static string GetMovingAverageInterval(MovingAverageInterval value)
        {
            return value.ToString().Replace("m", "");
        }
        public static string GetTechnicalIndicatorsI(TechnicalIndicator value)
        {
            switch (value)
            {
                case TechnicalIndicator.Bollinger_Bands:
                    return value.ToString().Substring(0, 1).ToLower() + ',';
                case TechnicalIndicator.Parabolic_SAR:
                    return value.ToString().Substring(0, 1).ToLower() + ',';
                case TechnicalIndicator.Splits:
                    return value.ToString().Substring(0, 1).ToLower() + ',';
                case TechnicalIndicator.Volume:
                    return value.ToString().Substring(0, 1).ToLower() + ',';
                default:
                    return string.Empty;
            }
        }
        public static string GetTechnicalIndicatorsII(TechnicalIndicator value)
        {
            switch (value)
            {
                case TechnicalIndicator.MACD:
                    return "m26-12-9,";
                case TechnicalIndicator.MFI:
                    return "f14,";
                case TechnicalIndicator.ROC:
                    return "p12,";
                case TechnicalIndicator.RSI:
                    return "r14,";
                case TechnicalIndicator.Slow_Stoch:
                    return "ss,";
                case TechnicalIndicator.Fast_Stoch:
                    return "fs,";
                case TechnicalIndicator.Vol:
                    return "v,";
                case TechnicalIndicator.Vol_MA:
                    return "vm,";
                case TechnicalIndicator.W_R:
                    return "w14,";
                default:
                    return string.Empty;
            }
        }

        public static char GetHistQuotesInterval(HistQuotesInterval item)
        {
            switch (item)
            {
                case HistQuotesInterval.Daily:
                    return 'd';
                case HistQuotesInterval.Weekly:
                    return 'w';
                default:
                    return 'm';
            }
        }

        public static string MarketQuotesRankingTypeString(MarketQuoteProperty rankedBy)
        {
            switch (rankedBy)
            {
                case MarketQuoteProperty.Name:
                    return "coname";
                case MarketQuoteProperty.DividendYieldPercent:
                    return "yie";
                case MarketQuoteProperty.LongTermDeptToEquity:
                    return "qto";
                case MarketQuoteProperty.MarketCapitalizationInMillion:
                    return "mkt";
                case MarketQuoteProperty.NetProfitMarginPercent:
                    return "qpm";
                case MarketQuoteProperty.OneDayPriceChangePercent:
                    return "pr1";
                case MarketQuoteProperty.PriceEarningsRatio:
                    return "pee";
                case MarketQuoteProperty.PriceToBookValue:
                    return "pri";
                case MarketQuoteProperty.PriceToFreeCashFlow:
                    return "prf";
                case MarketQuoteProperty.ReturnOnEquityPercent:
                    return "ttm";
                default:
                    return string.Empty;
            }
        }
        public static string MarketQuotesRankingDirectionString(System.ComponentModel.ListSortDirection dir)
        {
            if (dir == ListSortDirection.Ascending)
            {
                return "u";
            }
            else
            {
                return "d";
            }
        }

        public static string CsvQuotePropertyTags(QuoteProperty[] properties)
        {
            System.Text.StringBuilder symbols = new System.Text.StringBuilder();
            if (properties != null && properties.Length > 0)
            {
                foreach (QuoteProperty qp in properties)
                {
                    switch (qp)
                    {
                        case QuoteProperty.Ask:
                            symbols.Append("a0");
                            break;
                        case QuoteProperty.AverageDailyVolume:
                            symbols.Append("a2");
                            break;
                        case QuoteProperty.AskSize:
                            symbols.Append("a5");
                            break;
                        case QuoteProperty.Bid:
                            symbols.Append("b0");
                            break;
                        case QuoteProperty.AskRealtime:
                            symbols.Append("b2");
                            break;
                        case QuoteProperty.BidRealtime:
                            symbols.Append("b3");
                            break;
                        case QuoteProperty.BookValuePerShare:
                            symbols.Append("b4");
                            break;
                        case QuoteProperty.BidSize:
                            symbols.Append("b6");
                            break;
                        case QuoteProperty.Change_ChangeInPercent:
                            symbols.Append('c');
                            break;
                        case QuoteProperty.Change:
                            symbols.Append("c1");
                            break;
                        case QuoteProperty.Commission:
                            symbols.Append("c3");
                            break;
                        case QuoteProperty.Currency:
                            symbols.Append("c4");
                            break;
                        case QuoteProperty.ChangeRealtime:
                            symbols.Append("c6");
                            break;
                        case QuoteProperty.AfterHoursChangeRealtime:
                            symbols.Append("c8");
                            break;
                        case QuoteProperty.TrailingAnnualDividendYield:
                            symbols.Append("d0");
                            break;
                        case QuoteProperty.LastTradeDate:
                            symbols.Append("d1");
                            break;
                        case QuoteProperty.TradeDate:
                            symbols.Append("d2");
                            break;
                        case QuoteProperty.DilutedEPS:
                            symbols.Append("e0");
                            break;
                        case QuoteProperty.EPSEstimateCurrentYear:
                            symbols.Append("e7");
                            break;
                        case QuoteProperty.EPSEstimateNextYear:
                            symbols.Append("e8");
                            break;
                        case QuoteProperty.EPSEstimateNextQuarter:
                            symbols.Append("e9");
                            break;
                        case QuoteProperty.TradeLinksAdditional:
                            symbols.Append("f0");
                            break;
                        case QuoteProperty.SharesFloat:
                            symbols.Append("f6");
                            break;
                        case QuoteProperty.DaysLow:
                            symbols.Append("g0");
                            break;
                        case QuoteProperty.HoldingsGainPercent:
                            symbols.Append("g1");
                            break;
                        case QuoteProperty.AnnualizedGain:
                            symbols.Append("g3");
                            break;
                        case QuoteProperty.HoldingsGain:
                            symbols.Append("g4");
                            break;
                        case QuoteProperty.HoldingsGainPercentRealtime:
                            symbols.Append("g5");
                            break;
                        case QuoteProperty.HoldingsGainRealtime:
                            symbols.Append("g6");
                            break;
                        case QuoteProperty.DaysHigh:
                            symbols.Append("h0");
                            break;
                        case QuoteProperty.MoreInfo:
                            symbols.Append('i');
                            break;
                        case QuoteProperty.OrderBookRealtime:
                            symbols.Append("i5");
                            break;
                        case QuoteProperty.YearLow:
                            symbols.Append("j0");
                            break;
                        case QuoteProperty.MarketCapitalization:
                            symbols.Append("j1");
                            break;
                        case QuoteProperty.SharesOutstanding:
                            symbols.Append("j2");
                            break;
                        case QuoteProperty.MarketCapRealtime:
                            symbols.Append("j3");
                            break;
                        case QuoteProperty.EBITDA:
                            symbols.Append("j4");
                            break;
                        case QuoteProperty.ChangeFromYearLow:
                            symbols.Append("j5");
                            break;
                        case QuoteProperty.PercentChangeFromYearLow:
                            symbols.Append("j6");
                            break;
                        case QuoteProperty.YearHigh:
                            symbols.Append("k0");
                            break;
                        case QuoteProperty.LastTradeRealtimeWithTime:
                            symbols.Append("k1");
                            break;
                        case QuoteProperty.ChangeInPercentRealtime:
                            symbols.Append("k2");
                            break;
                        case QuoteProperty.LastTradeSize:
                            symbols.Append("k3");
                            break;
                        case QuoteProperty.ChangeFromYearHigh:
                            symbols.Append("k4");
                            break;
                        case QuoteProperty.ChangeInPercentFromYearHigh:
                            symbols.Append("k5");
                            break;
                        case QuoteProperty.LastTradeWithTime:
                            symbols.Append("l0");
                            break;
                        case QuoteProperty.LastTradePriceOnly:
                            symbols.Append("l1");
                            break;
                        case QuoteProperty.HighLimit:
                            symbols.Append("l2");
                            break;
                        case QuoteProperty.LowLimit:
                            symbols.Append("l3");
                            break;
                        case QuoteProperty.DaysRange:
                            symbols.Append('m');
                            break;
                        case QuoteProperty.DaysRangeRealtime:
                            symbols.Append("m2");
                            break;
                        case QuoteProperty.FiftydayMovingAverage:
                            symbols.Append("m3");
                            break;
                        case QuoteProperty.TwoHundreddayMovingAverage:
                            symbols.Append("m4");
                            break;
                        case QuoteProperty.ChangeFromTwoHundreddayMovingAverage:
                            symbols.Append("m5");
                            break;
                        case QuoteProperty.PercentChangeFromTwoHundreddayMovingAverage:
                            symbols.Append("m6");
                            break;
                        case QuoteProperty.ChangeFromFiftydayMovingAverage:
                            symbols.Append("m7");
                            break;
                        case QuoteProperty.PercentChangeFromFiftydayMovingAverage:
                            symbols.Append("m8");
                            break;
                        case QuoteProperty.Name:
                            symbols.Append("n0");
                            break;
                        case QuoteProperty.Notes:
                            symbols.Append("n4");
                            break;
                        case QuoteProperty.Open:
                            symbols.Append("o0");
                            break;
                        case QuoteProperty.PreviousClose:
                            symbols.Append("p0");
                            break;
                        case QuoteProperty.PricePaid:
                            symbols.Append("p1");
                            break;
                        case QuoteProperty.ChangeInPercent:
                            symbols.Append("p2");
                            break;
                        case QuoteProperty.PriceSales:
                            symbols.Append("p5");
                            break;
                        case QuoteProperty.PriceBook:
                            symbols.Append("p6");
                            break;
                        case QuoteProperty.ExDividendDate:
                            symbols.Append("q0");
                            break;
                        case QuoteProperty.PERatio:
                            symbols.Append("r0");
                            break;
                        case QuoteProperty.DividendPayDate:
                            symbols.Append("r1");
                            break;
                        case QuoteProperty.PERatioRealtime:
                            symbols.Append("r2");
                            break;
                        case QuoteProperty.PEGRatio:
                            symbols.Append("r5");
                            break;
                        case QuoteProperty.PriceEPSEstimateCurrentYear:
                            symbols.Append("r6");
                            break;
                        case QuoteProperty.PriceEPSEstimateNextYear:
                            symbols.Append("r7");
                            break;
                        case QuoteProperty.Symbol:
                            symbols.Append("s0");
                            break;
                        case QuoteProperty.SharesOwned:
                            symbols.Append("s1");
                            break;
                        case QuoteProperty.Revenue:
                            symbols.Append("s6");
                            break;
                        case QuoteProperty.ShortRatio:
                            symbols.Append("s7");
                            break;
                        case QuoteProperty.LastTradeTime:
                            symbols.Append("t1");
                            break;
                        case QuoteProperty.TradeLinks:
                            symbols.Append("t6");
                            break;
                        case QuoteProperty.TickerTrend:
                            symbols.Append("t7");
                            break;
                        case QuoteProperty.OneyrTargetPrice:
                            symbols.Append("t8");
                            break;
                        case QuoteProperty.Volume:
                            symbols.Append("v0");
                            break;
                        case QuoteProperty.HoldingsValue:
                            symbols.Append("v1");
                            break;
                        case QuoteProperty.HoldingsValueRealtime:
                            symbols.Append("v7");
                            break;
                        case QuoteProperty.YearRange:
                            symbols.Append("w0");
                            break;
                        case QuoteProperty.DaysValueChange:
                            symbols.Append("w1");
                            break;
                        case QuoteProperty.DaysValueChangeRealtime:
                            symbols.Append("w4");
                            break;
                        case QuoteProperty.StockExchange:
                            symbols.Append("x0");
                            break;
                        case QuoteProperty.TrailingAnnualDividendYieldInPercent:
                            symbols.Append("y0");
                            break;
                    }
                }
            }
            return symbols.ToString();
        }
        public static char ServerToDelimiter(YahooServer server)
        {
            if (server == YahooServer.Australia | server == YahooServer.Canada | server == YahooServer.HongKong | server == YahooServer.India | server == YahooServer.Korea | server == YahooServer.Mexico | server == YahooServer.Singapore | server == YahooServer.UK | server == YahooServer.USA)
            {
                return ',';
            }
            else
            {
                return ';';
            }
        }
        private static string ReplaceDjiID(string id)
        {
            if (id.ToUpper() == "^DJI")
            {
                return "INDU";
            }
            else
            {
                return id;
            }
        }

        private FinanceHelper() { }

    }
}
