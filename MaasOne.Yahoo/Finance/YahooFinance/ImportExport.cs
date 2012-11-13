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
using MaasOne.Finance.YahooFinance;
using MaasOne.Xml;
using System.Xml.Linq;


namespace MaasOne.Finance.YahooFinance
{
    public class ImportExport
    {
        #region CSV


        /// <summary>
        /// Converts a list of quote data to a CSV formatted text
        /// </summary>
        /// <param name="quotes">The list of quote values</param>
        /// <param name="delimiter">The delimiter of the CSV text</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <returns>The converted data string in CSV format</returns>
        /// <remarks></remarks>
        public static string FromQuotesData(IEnumerable<QuotesData> quotes, char delimiter, System.Globalization.CultureInfo culture = null)
        {
            return FromQuotesData(quotes, delimiter, null, culture);
        }
        /// <summary>
        /// Converts a list of quote data to a CSV formatted text
        /// </summary>
        /// <param name="quotes">The list of quote values</param>
        /// <param name="delimiter">The delimiter of the CSV text</param>
        /// <param name="properties">The used properties of the items</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <returns>The converted data string in CSV format</returns>
        /// <remarks></remarks>
        public static string FromQuotesData(IEnumerable<QuotesData> quotes, char delimiter, IEnumerable<QuoteProperty> properties, System.Globalization.CultureInfo culture = null)
        {
            if (quotes != null)
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
                if (culture != null)
                    ci = culture;

                QuoteProperty[] prpts = FinanceHelper.CheckPropertiesOfQuotesData(quotes, properties);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                foreach (QuoteProperty qp in prpts)
                {
                    sb.Append(qp.ToString());
                    sb.Append(delimiter);
                }
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();

                foreach (QuotesData q in quotes)
                {
                    if (q != null)
                    {
                        System.Text.StringBuilder sbQ = new System.Text.StringBuilder();
                        foreach (QuoteProperty qp in prpts)
                        {
                            object o = MyHelper.ObjectToString(q[qp], ci);
                            if (o is string)
                            {
                                if (o.ToString() == string.Empty)
                                {
                                    sbQ.Append("\"N/A\"");
                                }
                                else
                                {
                                    sbQ.Append("\"");
                                    sbQ.Append(q[qp].ToString().Replace("\"", "\"\""));
                                    sbQ.Append("\"");
                                }
                            }
                            else
                            {
                                sbQ.Append(MyHelper.ObjectToString(q[qp], ci));
                            }
                            sbQ.Append(delimiter);
                        }
                        if (sbQ.Length > 0)
                            sbQ.Remove(sbQ.Length - 1, 1);
                        sb.AppendLine(sbQ.ToString());
                    }
                }
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Tries to read a list of quote data from a CSV formatted text (incl. Header)
        /// </summary>
        /// <param name="csvText">The CSV formatted text</param>
        /// <param name="delimiter">The delimiter of the CSV text</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <returns>The converted quote values or Nothing</returns>
        /// <remarks></remarks>
        public static QuotesData[] ToQuotesData(string csvText, char delimiter, System.Globalization.CultureInfo culture = null)
        {
            List<QuoteProperty> properties = new List<QuoteProperty>();
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
            if (culture != null)
                ci = culture;

            if (csvText != string.Empty)
            {
                string[] rows = csvText.Split(new string[] {
				"\r",
                "\n"
			}, StringSplitOptions.RemoveEmptyEntries);
                string[] headerParts = MyHelper.CsvRowToStringArray(rows[0], delimiter);
                foreach (string part in headerParts)
                {
                    foreach (QuoteProperty qp in Enum.GetValues(typeof(QuoteProperty)))
                    {
                        if (qp.ToString() == part.Trim())
                        {
                            properties.Add(qp);
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                }
                if (properties.Count != headerParts.Length)
                    return null;
            }

            return ToQuotesData(csvText, delimiter, properties.ToArray(), ci, true);
        }
        internal static QuotesData[] ToQuotesData(string csvText, char delimiter, QuoteProperty[] properties, System.Globalization.CultureInfo culture, bool hasHeader = false)
        {
            List<QuotesData> quotes = new List<QuotesData>();
            if (csvText != string.Empty & culture != null & (properties != null && properties.Length > 0))
            {
                if (properties.Length > 0)
                {
                    string[][] table = MyHelper.CsvTextToStringTable(csvText, delimiter);
                    int start = 0;
                    if (hasHeader)
                        start = 1;

                    if (table.Length > 0)
                    {
                        if (!(table[0].Length == properties.Length))
                        {
                            String[][] semicolTable = MyHelper.CsvTextToStringTable(csvText, Convert.ToChar((delimiter == ';' ? ',' : ';')));
                            if (semicolTable[0].Length == properties.Length)
                            {
                                table = semicolTable;
                            }
                        }
                        if (table.Length > 0 && table.Length > start)
                        {
                            for (int i = start; i <= table.Length - 1; i++)
                            {
                                QuotesData qd = CsvArrayToQuoteData(table[i], properties, culture);
                                if (qd != null)
                                    quotes.Add(qd);
                            }
                        }
                    }
                }
            }
            return quotes.ToArray();
        }

        /// <summary>
        /// Converts a list of quote options to a CSV formatted text
        /// </summary>
        /// <param name="quoteOptions">The list of quote option values</param>
        /// <param name="delimiter">The delimiter of the CSV text</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <returns>The converted data string in CSV format</returns>
        /// <remarks></remarks>
        public static string FromQuoteOptions(IEnumerable<QuoteOptionsData> quoteOptions, char delimiter, System.Globalization.CultureInfo culture = null)
        {
            System.Text.StringBuilder csv = new System.Text.StringBuilder();
            if (quoteOptions != null)
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
                if (culture != null)
                    ci = culture;
                foreach (QuoteOptionsData qbd in quoteOptions)
                {
                    csv.Append("\"");
                    csv.Append(qbd.Symbol);
                    csv.Append("\"");
                    csv.Append("\"");
                    csv.Append((qbd.Type == QuoteOptionType.Call ? "C" : "P").ToString());
                    csv.Append("\"");
                    csv.Append(delimiter);
                    csv.Append(MyHelper.ObjectToString(qbd.LastPrice, ci));
                    csv.Append(delimiter);
                    csv.Append(MyHelper.ObjectToString(qbd.StrikePrice, ci));
                    csv.Append(delimiter);
                    csv.Append(MyHelper.ObjectToString(Math.Abs(qbd.Change), ci));
                    csv.Append(delimiter);
                    csv.Append("\"");
                    csv.Append((qbd.Change >= 0 ? "Up" : "Down").ToString());
                    csv.Append("\"");
                    csv.Append(MyHelper.ObjectToString(qbd.Bid, ci));
                    csv.Append(delimiter);
                    csv.Append(MyHelper.ObjectToString(qbd.Ask, ci));
                    csv.Append(delimiter);
                    csv.Append(MyHelper.ObjectToString(qbd.Volume, ci));
                    csv.Append(delimiter);
                    csv.Append(MyHelper.ObjectToString(qbd.OpenInterest, ci));
                }
            }
            return csv.ToString();
        }
        /// <summary>
        /// Tries to read a list of quote options from a CSV formatted text
        /// </summary>
        /// <param name="csvText">The CSV formatted text</param>
        /// <param name="delimiter">The delimiter of the CSV text</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <returns>The converted quote values or Nothing</returns>
        /// <remarks></remarks>
        public static QuoteOptionsData[] ToQuoteOptions(string csvText, char delimiter, System.Globalization.CultureInfo culture = null)
        {
            List<QuoteOptionsData> lst = new List<QuoteOptionsData>();
            if (csvText != string.Empty)
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
                if (culture != null)
                    ci = culture;

                string[][] table = MyHelper.CsvTextToStringTable(csvText, delimiter);
                if (table.Length > 1)
                {
                    for (int i = 0; i <= table.Length - 1; i++)
                    {
                        if (table[i].Length == 10)
                        {
                            QuoteOptionsData qd = new QuoteOptionsData();
                            qd.Symbol = table[i][0];
                            qd.Type = (QuoteOptionType)(table[i][1].ToLower() == "p" ? QuoteOptionType.Put : QuoteOptionType.Call);
                            double t1;
                            double t2;
                            double t3;
                            double t4;
                            double t5;
                            int t6;
                            int t7;
                            if (double.TryParse(table[i][2], System.Globalization.NumberStyles.Currency, ci, out t1) &&
                                double.TryParse(table[i][3], System.Globalization.NumberStyles.Currency, ci, out t2) &&
                                double.TryParse(table[i][4], System.Globalization.NumberStyles.Currency, ci, out t3) &&
                                double.TryParse(table[i][6], System.Globalization.NumberStyles.Currency, ci, out t4) &&
                                double.TryParse(table[i][7], System.Globalization.NumberStyles.Currency, ci, out t5) &&
                                int.TryParse(table[i][8], System.Globalization.NumberStyles.Integer, ci, out t6) &&
                                int.TryParse(table[i][9], System.Globalization.NumberStyles.Integer, ci, out t7))
                            {
                                qd.LastPrice = t1;
                                qd.StrikePrice = t2;
                                qd.Change = t3;
                                qd.Bid = t4;
                                qd.Ask = t5;
                                qd.Volume = t6;
                                qd.OpenInterest = t7;

                                qd.Change *= Convert.ToInt32((table[i][5].ToLower() == "down" ? -1 : 1));
                                lst.Add(qd);
                            }
                        }
                    }
                }
            }
            return lst.ToArray();
        }

        /// <summary>
        /// Converts a list of historic quote periods to a CSV formatted text
        /// </summary>
        /// <param name="quotes">The list of historic quote periods</param>
        /// <param name="delimiter">The delimiter of the CSV text</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <returns>The converted data string in CSV format</returns>
        /// <remarks></remarks>
        public static string FromHistQuotesData(IEnumerable<HistQuotesData> quotes, char delimiter, System.Globalization.CultureInfo culture = null)
        {
            System.Text.StringBuilder csv = new System.Text.StringBuilder();
            if (quotes != null)
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
                if (culture != null)
                    ci = culture;
                csv.AppendLine(HistQuotesCSVHeadline(delimiter));
                foreach (HistQuotesData hq in quotes)
                {
                    csv.Append(MyHelper.ObjectToString(hq.TradingDate, ci));
                    csv.Append(delimiter);
                    csv.Append(MyHelper.ObjectToString(hq.Open, ci));
                    csv.Append(delimiter);
                    csv.Append(MyHelper.ObjectToString(hq.High, ci));
                    csv.Append(delimiter);
                    csv.Append(MyHelper.ObjectToString(hq.Low, ci));
                    csv.Append(delimiter);
                    csv.Append(MyHelper.ObjectToString(hq.Close, ci));
                    csv.Append(delimiter);
                    csv.Append(MyHelper.ObjectToString(hq.Volume, ci));
                    csv.Append(delimiter);
                    csv.AppendLine(MyHelper.ObjectToString(hq.CloseAdjusted, ci));
                }
            }
            return csv.ToString();
        }
        /// <summary>
        /// Tries to read a list of historic quote periods from a CSV formatted text
        /// </summary>
        /// <param name="csvText">The CSV formatted text</param>
        /// <param name="delimiter">The delimiter of the CSV text</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <returns>The converted historic quote periods or Nothing</returns>
        /// <remarks></remarks>
        public static HistQuotesData[] ToHistQuotesData(string csvText, char delimiter, System.Globalization.CultureInfo culture = null)
        {
            List<HistQuotesData> lst = new List<HistQuotesData>();
            if (csvText != string.Empty)
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
                if (culture != null)
                    ci = culture;
                string[][] table = MyHelper.CsvTextToStringTable(csvText, delimiter);
                if (table.Length > 1)
                {
                    for (int i = 0; i <= table.Length - 1; i++)
                    {
                        if (table[i].Length == 7)
                        {
                            HistQuotesData qd = new HistQuotesData();
                            System.DateTime t1;
                            double t2;
                            double t3;
                            double t4;
                            double t5;
                            double t6;
                            long t7;

                            if (System.DateTime.TryParse(table[i][0], culture, System.Globalization.DateTimeStyles.None, out t1) &&
                                double.TryParse(table[i][1], System.Globalization.NumberStyles.Currency, culture, out t2) &&
                                double.TryParse(table[i][2], System.Globalization.NumberStyles.Currency, culture, out t3) &&
                                double.TryParse(table[i][3], System.Globalization.NumberStyles.Currency, culture, out t4) &&
                                double.TryParse(table[i][4], System.Globalization.NumberStyles.Currency, culture, out t5) &&
                                double.TryParse(table[i][6], System.Globalization.NumberStyles.Currency, culture, out t6) &&
                                long.TryParse(table[i][5], System.Globalization.NumberStyles.Integer, culture, out t7))
                            {
                                qd.TradingDate = t1;
                                qd.Open = t2;
                                qd.High = t3;
                                qd.Low = t4;
                                qd.Close = t5;
                                qd.CloseAdjusted = t6;
                                qd.Volume = t7;

                                lst.Add(qd);
                            }
                        }
                    }
                }
            }
            return lst.ToArray();
        }

        public static MarketQuotesData[] ToMarketQuotesData(string csv, System.Globalization.CultureInfo culture = null)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
            if (culture != null)
                ci = culture;
            char delimiter = ',';
            string[][] table = MyHelper.CsvTextToStringTable(csv, delimiter);
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
                        double t2;
                        double t3;
                        double t4;
                        double t5;
                        double t6;
                        double t7;
                        double t8;

                        if (double.TryParse(table[i][1], System.Globalization.NumberStyles.Any, ci, out t1))
                            quote.OneDayPriceChangePercent = t1;
                        string mktcap = table[i][2];
                        if (mktcap != "NA" & mktcap != string.Empty & mktcap.Length > 1)
                        {
                            double value = 0;
                            double.TryParse(mktcap.Substring(0, mktcap.Length - 1), System.Globalization.NumberStyles.Any, ci, out value);
                            quote.MarketCapitalizationInMillion = value * FinanceHelper.GetStringMillionFactor(mktcap);
                        }
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
            return lst.ToArray();
        }


        private static string HistQuotesCSVHeadline(char delimiter)
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}Adj Close", delimiter, FinanceHelper.NameHistQuoteDate, FinanceHelper.NameHistQuoteOpen, FinanceHelper.NameHistQuoteHigh, FinanceHelper.NameHistQuoteLow, FinanceHelper.NameHistQuoteClose, FinanceHelper.NameHistQuoteVolume);
        }


        private static object QuoteStringToObject(string value, QuoteProperty prp, System.Globalization.CultureInfo culture)
        {
            object o = MyHelper.StringToObject(value, culture);
            if (prp == QuoteProperty.Name && o is Array && ((Array)o).Length > 0)
            {
                Array arr = (Array)o;
                string s = "";
                for (int i = 0; i < arr.Length; i++)
                {
                    s += arr.GetValue(i).ToString();
                    if (i != arr.Length - 1)
                    {
                        s += " - ";
                    }
                }
                return s;
            }
            else
            {
                return o;
            }
        }


        private static QuotesData CsvArrayToQuoteData(string[] rowItems, QuoteProperty[] properties, System.Globalization.CultureInfo culture)
        {
            if (rowItems.Length > 0)
            {
                QuotesData quote = null;
                if (rowItems.Length == properties.Length)
                {
                    quote = new QuotesData();
                    for (int i = 0; i <= properties.Length - 1; i++)
                    {
                        quote[properties[i]] = QuoteStringToObject(rowItems[i], properties[i], culture);
                    }
                }
                else
                {

                    if (rowItems.Length > 1)
                    {
                        List<QuoteProperty> alternateProperties = new List<QuoteProperty>();
                        foreach (QuoteProperty qp in properties)
                        {
                            foreach (QuoteProperty q in mAlternateQuoteProperties)
                            {
                                if (qp == q)
                                {
                                    alternateProperties.Add(qp);
                                    break;
                                }
                            }
                        }


                        if (alternateProperties.Count > 0)
                        {
                            List<KeyValuePair<QuoteProperty, int>[]> lst = new List<KeyValuePair<QuoteProperty, int>[]>();
                            int[][] permutations = MaxThreePerm(alternateProperties.Count, Math.Min(rowItems.Length - properties.Length + 1, 3));
                            foreach (int[] perm in permutations)
                            {
                                List<KeyValuePair<QuoteProperty, int>> lst2 = new List<KeyValuePair<QuoteProperty, int>>();
                                for (int i = 0; i <= alternateProperties.Count - 1; i++)
                                {
                                    lst2.Add(new KeyValuePair<QuoteProperty, int>(alternateProperties[i], perm[i]));
                                }
                                lst.Add(lst2.ToArray());
                            }

                            foreach (KeyValuePair<QuoteProperty, int>[] combination in lst)
                            {
                                String[] newRowItems = CsvNewRowItems(rowItems, properties, combination);

                                try
                                {
                                    if (newRowItems.Length > 0)
                                    {
                                        quote = new QuotesData();
                                        for (int i = 0; i <= properties.Length - 1; i++)
                                        {
                                            quote[properties[i]] = QuoteStringToObject(rowItems[i], properties[i], culture);
                                        }
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex.Message);
                                }
                            }

                        }
                    }
                }
                return quote;
            }
            else
            {
                return null;
            }
        }


        public static int[][] MaxThreePerm(int propertyCount, int maxCount)
        {
            List<int[]> lst = new List<int[]>();
            for (int i = 1; i <= maxCount; i++)
            {
                if (propertyCount > 1)
                {
                    for (int n = 1; n <= maxCount; n++)
                    {
                        if (propertyCount > 2)
                        {
                            for (int m = 1; m <= maxCount; m++)
                            {
                                lst.Add(new int[] {
								i,
								n,
								m
							});
                            }
                        }
                        else
                        {
                            lst.Add(new int[] {
							i,
							n
						});
                        }
                    }
                }
                else
                {
                    lst.Add(new int[] { i });
                }
            }
            return lst.ToArray();
        }


        private static QuoteProperty[] mAlternateQuoteProperties = new QuoteProperty[] {
		QuoteProperty.LastTradeSize,
		QuoteProperty.BidSize,
		QuoteProperty.AskSize

	};

        private static string[] CsvNewRowItems(string[] oldItems, QuoteProperty[] properties, KeyValuePair<QuoteProperty, int>[] multipleItemProperties)
        {
            System.Globalization.CultureInfo convCulture = new System.Globalization.CultureInfo("en-US");
            List<string> newRowItems = new List<string>();
            int itemsCount = properties.Length;
            foreach (KeyValuePair<QuoteProperty, int> q in multipleItemProperties)
            {
                itemsCount += q.Value - 1;
            }
            if (itemsCount == oldItems.Length)
            {
                int actualIndex = 0;
                foreach (QuoteProperty qp in properties)
                {
                    Nullable<KeyValuePair<QuoteProperty, int>> alternatProperty = null;
                    foreach (KeyValuePair<QuoteProperty, int> q in multipleItemProperties)
                    {
                        if (q.Key == qp)
                        {
                            alternatProperty = q;
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                    if (!alternatProperty.HasValue)
                    {
                        newRowItems.Add(oldItems[actualIndex]);
                    }
                    else
                    {
                        string newRowItem = string.Empty;

                        for (int i = actualIndex; i <= (actualIndex + alternatProperty.Value.Value - 1); i++)
                        {
                            int @int = 0;
                            if (int.TryParse(oldItems[i], System.Globalization.NumberStyles.Integer, convCulture, out @int) && (oldItems[i] == @int.ToString() || oldItems[i] == "000"))
                            {
                                newRowItem += oldItems[i];
                            }
                            else
                            {
                                newRowItem = string.Empty;
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                        if (newRowItem != string.Empty)
                        {
                            newRowItems.Add(newRowItem);
                            actualIndex += alternatProperty.Value.Value - 1;
                        }
                    }
                    actualIndex += 1;
                }
            }
            return newRowItems.ToArray();
        }



        #endregion





        #region XML

        /// <summary>
        /// Writes a QuoteData to XML format
        /// </summary>
        /// <param name="writer">The used XML writer</param>
        /// <param name="quote">The used QuoteData</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <remarks></remarks>
        public static void FromQuoteData(System.Xml.XmlWriter writer, QuotesData quote, System.Globalization.CultureInfo culture = null)
        {
            FromQuoteData(writer, quote, null, culture);
        }
        /// <summary>
        /// Writes a QuoteData to XML format
        /// </summary>
        /// <param name="writer">The used XML writer</param>
        /// <param name="quote">The used QuoteData</param>
        /// <param name="properties">The used properties of the quotes</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <remarks></remarks>
        public static void FromQuoteData(System.Xml.XmlWriter writer, QuotesData quote, IEnumerable<QuoteProperty> properties, System.Globalization.CultureInfo culture = null)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
            if (culture != null)
                ci = culture;
            writer.WriteStartElement("Quote");
            if (quote[QuoteProperty.Symbol] != null)
                writer.WriteAttributeString("ID", quote[QuoteProperty.Symbol].ToString());
            QuoteProperty[] prps = FinanceHelper.CheckPropertiesOfQuotesData(new QuotesData[] { quote }, properties);
            foreach (QuoteProperty qp in prps)
            {
                writer.WriteStartElement(qp.ToString());
                writer.WriteValue(MyHelper.ObjectToString(quote[qp], ci));
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        /// <summary>
        /// Tries to read a QuoteData from XML
        /// </summary>
        /// <param name="node">The XML node of a QuoteData</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <returns>The converted quote data or Nothing</returns>
        /// <remarks></remarks>
        public static QuotesData ToQuoteData(XElement node, System.Globalization.CultureInfo culture = null)
        {
            if (node != null && node.Name.LocalName.ToLower() == "quote")
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
                if (culture != null)
                    ci = culture;
                QuotesData quote = new QuotesData();
                foreach (XElement propertyNode in node.Elements())
                {
                    foreach (QuoteProperty qp in Enum.GetValues(typeof(QuoteProperty)))
                    {
                        if (propertyNode.Name.LocalName == qp.ToString())
                        {
                            quote[qp] = MyHelper.StringToObject(propertyNode.Value, ci);
                            break; // TODO: might not be correct. Was : Exit For
                        }
                    }
                }
                return quote;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Writes a QuoteOption to XML format
        /// </summary>
        /// <param name="writer">The used XML writer</param>
        /// <param name="quoteOption">The used QuoteOption</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <remarks></remarks>
        public static void FromQuoteOption(System.Xml.XmlWriter writer, QuoteOptionsData quoteOption, System.Globalization.CultureInfo culture = null)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
            if (culture != null)
                ci = culture;
            writer.WriteStartElement("Option");

            writer.WriteAttributeString(FinanceHelper.NameOptionSymbol, quoteOption.Symbol);
            writer.WriteAttributeString(FinanceHelper.NameOptionType, (quoteOption.Type == QuoteOptionType.Call ? "C" : "P").ToString());

            writer.WriteStartElement(FinanceHelper.NameOptionLastPrice);
            writer.WriteValue(MyHelper.ObjectToString(quoteOption.LastPrice, ci));
            writer.WriteEndElement();

            writer.WriteStartElement(FinanceHelper.NameOptionStrikePrice);
            writer.WriteValue(MyHelper.ObjectToString(quoteOption.StrikePrice, ci));
            writer.WriteEndElement();

            writer.WriteStartElement(FinanceHelper.NameOptionChange);
            writer.WriteValue(MyHelper.ObjectToString(Math.Abs(quoteOption.Change), ci));
            writer.WriteEndElement();

            writer.WriteStartElement(FinanceHelper.NameOptionChangeDir);
            writer.WriteValue((quoteOption.Change >= 0 ? "Up" : "Down").ToString());
            writer.WriteEndElement();

            writer.WriteStartElement(FinanceHelper.NameOptionBid);
            writer.WriteValue(MyHelper.ObjectToString(quoteOption.Bid, ci));
            writer.WriteEndElement();

            writer.WriteStartElement(FinanceHelper.NameOptionAsk);
            writer.WriteValue(MyHelper.ObjectToString(quoteOption.Ask, ci));
            writer.WriteEndElement();

            writer.WriteStartElement(FinanceHelper.NameOptionVolume);
            writer.WriteValue(MyHelper.ObjectToString(quoteOption.Volume, ci));
            writer.WriteEndElement();

            writer.WriteStartElement(FinanceHelper.NameOptionOpenInterest);
            writer.WriteValue(MyHelper.ObjectToString(quoteOption.OpenInterest, ci));
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
        /// <summary>
        /// Tries to read a QuoteOption from XML
        /// </summary>
        /// <param name="node">The XML node of QuoteOption</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <returns>The converted quote data or Nothing</returns>
        /// <remarks></remarks>
        public static QuoteOptionsData ToQuoteOption(XElement node, System.Globalization.CultureInfo culture = null)
        {
            if (node != null && node.Name.LocalName.ToLower() == "option")
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
                if (culture != null)
                    ci = culture;
                string symbol = string.Empty;
                string t = string.Empty;
                foreach (XAttribute att in node.Attributes())
                {
                    switch (att.Name.LocalName)
                    {
                        case FinanceHelper.NameOptionSymbol:
                            symbol = att.Value;
                            break;
                        case "type":
                            t = att.Value;
                            break;
                    }
                }
                QuoteOptionType type = QuoteOptionType.Call;
                if (t.ToLower() == "p")
                    type = QuoteOptionType.Put;
                double strikePrice = 0;
                double lastPrice = 0;
                double change = 0;
                double bid = 0;
                double ask = 0;
                int volume = 0;
                int openInt = 0;
                foreach (XElement propertyNode in node.Elements())
                {
                    switch (propertyNode.Name.LocalName)
                    {
                        case FinanceHelper.NameOptionStrikePrice:
                            double.TryParse(propertyNode.Value, System.Globalization.NumberStyles.Currency, ci, out strikePrice);
                            break;
                        case FinanceHelper.NameOptionLastPrice:
                            double.TryParse(propertyNode.Value, System.Globalization.NumberStyles.Currency, ci, out lastPrice);
                            break;
                        case FinanceHelper.NameOptionChange:
                            double.TryParse(propertyNode.Value, System.Globalization.NumberStyles.Currency, ci, out change);
                            break;
                        case FinanceHelper.NameOptionChangeDir:
                            if (propertyNode.Value == "Down")
                                change *= -1;
                            break;
                        case FinanceHelper.NameOptionBid:
                            double.TryParse(propertyNode.Value, System.Globalization.NumberStyles.Currency, ci, out bid);
                            break;
                        case FinanceHelper.NameOptionAsk:
                            double.TryParse(propertyNode.Value, System.Globalization.NumberStyles.Currency, ci, out ask);
                            break;
                        case FinanceHelper.NameOptionVolume:
                            int.TryParse(propertyNode.Value, System.Globalization.NumberStyles.Integer, ci, out volume);
                            break;
                        case FinanceHelper.NameOptionOpenInterest:
                            int.TryParse(propertyNode.Value, System.Globalization.NumberStyles.Integer, ci, out openInt);
                            break;
                    }
                }
                return new QuoteOptionsData(symbol, type, strikePrice, lastPrice, change, bid, ask, volume, openInt);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Writes HistQuoteData to XML format
        /// </summary>
        /// <param name="writer">The used XML writer</param>
        /// <param name="quote">The used HistQuoteData</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <remarks>Creates a main node for all periods</remarks>
        public static void FromHistQuoteData(System.Xml.XmlWriter writer, HistQuotesData quote, System.Globalization.CultureInfo culture = null)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
            if (culture != null)
                ci = culture;
            writer.WriteStartElement("HistQuote");

            writer.WriteStartElement(FinanceHelper.NameHistQuoteDate);
            writer.WriteValue(quote.TradingDate.ToShortDateString());
            writer.WriteEndElement();

            writer.WriteStartElement(FinanceHelper.NameHistQuoteOpen);
            writer.WriteValue(MyHelper.ObjectToString(quote.Open, ci));
            writer.WriteEndElement();

            writer.WriteStartElement(FinanceHelper.NameHistQuoteHigh);
            writer.WriteValue(MyHelper.ObjectToString(quote.High, ci));
            writer.WriteEndElement();

            writer.WriteStartElement(FinanceHelper.NameHistQuoteLow);
            writer.WriteValue(MyHelper.ObjectToString(quote.Low, ci));
            writer.WriteEndElement();

            writer.WriteStartElement(FinanceHelper.NameHistQuoteClose);
            writer.WriteValue(MyHelper.ObjectToString(quote.Close, ci));
            writer.WriteEndElement();

            writer.WriteStartElement(FinanceHelper.NameHistQuoteVolume);
            writer.WriteValue(MyHelper.ObjectToString(quote.Volume, ci));
            writer.WriteEndElement();

            writer.WriteStartElement(FinanceHelper.NameHistQuoteAdjClose);
            writer.WriteValue(MyHelper.ObjectToString(quote.CloseAdjusted, ci));
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
        /// <summary>
        /// Tries to read a HistQuoteData from XML
        /// </summary>
        /// <param name="node">The XML node of HistQuoteData</param>
        /// <param name="culture">The used culture for formating dates and numbers. If parameter value is null/Nothing, default Culture will be used.</param>
        /// <returns>The converted historic quote data or Nothing</returns>
        /// <remarks></remarks>
        public static HistQuotesData ToHistQuoteData(XElement node, System.Globalization.CultureInfo culture = null)
        {
            if (node != null && node.Name.LocalName.ToLower() == "histquote" && MyHelper.EnumToArray(node.Elements()).Length > 0)
            {
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
                if (culture != null)
                    ci = culture;
                XElement[] elm = MyHelper.EnumToArray(node.Elements());
                HistQuotesData qd = new HistQuotesData();
                foreach (XElement cNode in node.Elements())
                {
                    switch (cNode.Name.LocalName)
                    {
                        case FinanceHelper.NameHistQuoteDate:
                            System.DateTime t1;
                            if (System.DateTime.TryParse(elm[0].Value, out t1)) qd.TradingDate = t1;
                            break;
                        case FinanceHelper.NameHistQuoteOpen:
                            double t2;
                            if (double.TryParse(elm[1].Value, System.Globalization.NumberStyles.Currency, ci, out t2)) qd.Open = t2;
                            break;
                        case FinanceHelper.NameHistQuoteHigh:
                            double t3;
                            if (double.TryParse(elm[2].Value, System.Globalization.NumberStyles.Currency, ci, out t3)) qd.High = t3;
                            break;
                        case FinanceHelper.NameHistQuoteLow:
                            double t4;
                            if (double.TryParse(elm[3].Value, System.Globalization.NumberStyles.Currency, ci, out t4)) qd.Low = t4;
                            break;
                        case FinanceHelper.NameHistQuoteClose:
                            double t5;
                            if (double.TryParse(elm[4].Value, System.Globalization.NumberStyles.Currency, ci, out t5)) qd.Close = t5;
                            break;
                        case FinanceHelper.NameHistQuoteAdjClose:
                            double t6;
                            if (double.TryParse(elm[6].Value, System.Globalization.NumberStyles.Currency, ci, out t6)) qd.CloseAdjusted = t6;
                            break;
                        case FinanceHelper.NameHistQuoteVolume:
                            long t7;
                            if (long.TryParse(elm[5].Value, System.Globalization.NumberStyles.Integer, ci, out t7)) qd.Volume = t7;
                            break;
                    }
                }
                return qd;
            }
            else
            {
                return null;
            }
        }




        #endregion

    }
}


