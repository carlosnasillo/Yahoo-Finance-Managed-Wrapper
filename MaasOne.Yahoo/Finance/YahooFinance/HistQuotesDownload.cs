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
using System.Text.RegularExpressions;


namespace MaasOne.Finance.YahooFinance
{

    /// <summary>
    /// Provides methods for downloading historic quote data.
    /// </summary>
    /// <remarks></remarks>
    public partial class HistQuotesDownload : Base.DownloadClient<HistQuotesResult>
    {

        public HistQuotesDownloadSettings Settings { get { return (HistQuotesDownloadSettings)base.Settings; } set { base.SetSettings(value); } }


        public HistQuotesDownload()
        {
            this.Settings = new HistQuotesDownloadSettings();
        }

        /// <summary>
        /// Starts an asynchronous download of historic quotes data.
        /// </summary>
        /// <param name="unmanagedID">The unmanaged ID</param>
        /// <param name="fromDate">The startdate of the reviewed period</param>
        /// <param name="todate">The enddate of the reviewed period</param>
        /// <param name="interval">The trading period interval</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadAsync(string unmanagedID, System.DateTime fromDate, System.DateTime toDate, HistQuotesInterval interval, object userArgs)
        {
            if (unmanagedID == string.Empty)
                throw new ArgumentNullException("unmanagedID", "The passed ID is empty.");
            this.DownloadAsync(new string[] { unmanagedID }, fromDate, toDate, interval, userArgs);
        }
        /// <summary>
        /// Starts an asynchronous download of historic quotes data.
        /// </summary>
        /// <param name="unmanagedIDs">The unmanaged list of IDs</param>
        /// <param name="fromDate">The startdate of the reviewed period</param>
        /// <param name="todate">The enddate of the reviewed period</param>
        /// <param name="interval">The trading period interval</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadAsync(IEnumerable<string> unmanagedIDs, System.DateTime fromDate, System.DateTime toDate, HistQuotesInterval interval, object userArgs)
        {
            if (unmanagedIDs == null)
                throw new ArgumentNullException("unmanagedIDs", "The passed list is null.");
            this.CheckDates(fromDate, toDate);
            this.DownloadAsync(new HistQuotesDownloadSettings(unmanagedIDs, fromDate, toDate, interval), userArgs);
        }
        public void DownloadAsync(HistQuotesDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        protected override HistQuotesResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            HistQuotesDownloadSettings s = (HistQuotesDownloadSettings)settings;
            string text = MyHelper.StreamToString(stream, s.TextEncoding);
            HistQuotesDataChain[] quotes = new HistQuotesDataChain[-1 + 1];
            if (connInfo.State == Base.ConnectionState.Success)
            {
                if (s.JSON)
                {
                    quotes = this.ConvertJSON(text, s.IDs);
                }
                else
                {
                    quotes = this.ConvertCSV(text, s.IDs[0]);
                }
            }
            return new HistQuotesResult(quotes, s);
        }

        private HistQuotesDataChain[] ConvertJSON(string text, string[] ids)
        {
            List<HistQuotesDataChain> quotes = new List<HistQuotesDataChain>();
            HistQuotesDataChain chain = new HistQuotesDataChain();
            Regex reg = new Regex("{\"col0\":\".*?\",\"col1\":\".*?\",\"col2\":\".*?\",\"col3\":\".*?\",\"col4\":\".*?\",\"col5\":\".*?\",\"col6\":\".*?\"}");
            MatchCollection matches = reg.Matches(text);

            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    if (m.Success)
                    {
                        string[] columns = m.Value.Replace("{", "").Replace("}", "").Split(new char[] { ',' });

                        if (columns.Length == 7)
                        {
                            if (columns[0] == "\"col0\":\"Date\"")
                            {
                                if (chain.Count > 0)
                                {
                                    string id = string.Empty;
                                    if (quotes.Count <= ids.Length - 1)
                                        id = ids[quotes.Count];
                                    chain.SetID(id);
                                    quotes.Add(chain);
                                    chain = new HistQuotesDataChain();
                                }
                            }
                            else
                            {
                                HistQuotesData hq = new HistQuotesData();
                                foreach (string col in columns)
                                {
                                    string[] values = col.Replace("\"", "").Split(':');
                                    if (values.Length == 2)
                                    {
                                        switch (values[0])
                                        {
                                            case "col0":
                                                DateTime t1;
                                                if (System.DateTime.TryParse(values[1], FinanceHelper.DefaultYqlCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out t1)) hq.TradingDate = t1;
                                                break;
                                            case "col1":
                                                double t2;
                                                if (double.TryParse(values[1], System.Globalization.NumberStyles.Any, FinanceHelper.DefaultYqlCulture, out t2)) hq.Open = t2;
                                                break;
                                            case "col2":
                                                double t3;
                                                if (double.TryParse(values[1], System.Globalization.NumberStyles.Any, FinanceHelper.DefaultYqlCulture, out t3)) hq.High = t3;
                                                break;
                                            case "col3":
                                                double t4;
                                                if (double.TryParse(values[1], System.Globalization.NumberStyles.Any, FinanceHelper.DefaultYqlCulture, out t4)) hq.Low = t4;
                                                break;
                                            case "col4":
                                                double t5;
                                                if (double.TryParse(values[1], System.Globalization.NumberStyles.Any, FinanceHelper.DefaultYqlCulture, out t5)) hq.Close = t5;
                                                break;
                                            case "col5":
                                                long t6;
                                                if (long.TryParse(values[1], System.Globalization.NumberStyles.Any, FinanceHelper.DefaultYqlCulture, out t6)) hq.Volume = t6;
                                                break;
                                            case "col6":
                                                double t7;
                                                if (double.TryParse(values[1], System.Globalization.NumberStyles.Any, FinanceHelper.DefaultYqlCulture, out t7)) hq.CloseAdjusted = t7;
                                                break;
                                        }
                                    }
                                }
                                chain.Add(hq);
                            }

                        }
                    }
                }

                if (chain.Count > 0)
                {
                    string id = string.Empty;
                    if (quotes.Count <= ids.Length - 1)
                        id = ids[quotes.Count];
                    chain.SetID(id);
                    quotes.Add(chain);
                }
                chain = null;
            }
            return quotes.ToArray();
        }
        private HistQuotesDataChain[] ConvertCSV(string text, string id)
        {
            List<HistQuotesData> lst = new List<HistQuotesData>();
            if (text != string.Empty)
            {
                System.Globalization.CultureInfo ci = FinanceHelper.DefaultYqlCulture;

                string[][] table = MyHelper.CsvTextToStringTable(text, ',');
                if (table.Length > 1)
                {
                    for (int i = 0; i <= table.Length - 1; i++)
                    {
                        if (table[i].Length == 7)
                        {
                            HistQuotesData qd = new HistQuotesData();
                            DateTime t1;
                            double t2;
                            double t3;
                            double t4;
                            double t5;
                            double t6;
                            long t7;
                            if (System.DateTime.TryParse(table[i][0], ci, System.Globalization.DateTimeStyles.None, out t1) &&
                                double.TryParse(table[i][1], System.Globalization.NumberStyles.Currency, ci, out t2) &&
                                double.TryParse(table[i][2], System.Globalization.NumberStyles.Currency, ci, out t3) &&
                                double.TryParse(table[i][3], System.Globalization.NumberStyles.Currency, ci, out t4) &&
                                double.TryParse(table[i][4], System.Globalization.NumberStyles.Currency, ci, out t5) &&
                                double.TryParse(table[i][6], System.Globalization.NumberStyles.Currency, ci, out t6) &&
                                long.TryParse(table[i][5], System.Globalization.NumberStyles.Integer, ci, out t7))
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
            return new HistQuotesDataChain[] { new HistQuotesDataChain(id, lst.ToArray()) };
        }
        private void CheckDates(System.DateTime fromDate, System.DateTime toDate)
        {
            if (fromDate > toDate)
                throw new ArgumentNullException("date", "The start date is later than the end date.");
        }

    }


    /// <summary>
    /// Stores the result data
    /// </summary>
    public class HistQuotesResult : HistQuotesBaseResult
    {
        private HistQuotesDownloadSettings mSettings = null;
        public HistQuotesDownloadSettings Settings { get { return mSettings; } }
        private HistQuotesDataChain[] mChains = null;
        public HistQuotesDataChain[] Chains { get { return mChains; } }

        internal HistQuotesResult(HistQuotesDataChain[] chains, HistQuotesDownloadSettings settings)
            : base((chains != null && chains.Length > 0) ? chains[0] : new HistQuotesDataChain())
        {
            mSettings = settings;
            mChains = chains;
        }
    }
    
   
    public class HistQuotesDownloadSettings : HistQuotesBaseDownloadSettings<HistQuotesResult>, ITextEncodingSettings
    {

        /// <summary>
        /// The text encoding for downloading quotes NOT from YQL server.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.Text.Encoding TextEncoding { get; set; }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
                if (value != string.Empty) { this.IDs = new string[] { value }; }
                else { this.IDs = new string[] { }; }
            }
        }
        public string[] IDs { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public HistQuotesInterval Interval { get; set; }
        internal bool JSON { get { return this.IDs != null && this.IDs.Length > 1; } }

        public HistQuotesDownloadSettings()
        {
            this.ID = string.Empty;
            this.FromDate = new DateTime(2010, 1, 1);
            this.ToDate = DateTime.Today;
            this.Interval = HistQuotesInterval.Monthly;
        }
        public HistQuotesDownloadSettings(string id, System.DateTime fromDate, System.DateTime toDate, HistQuotesInterval interval)
        {
            this.IDs = new string[] { id };
            this.TextEncoding = System.Text.Encoding.UTF8;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.Interval = interval;
        }
        public HistQuotesDownloadSettings(IEnumerable<string> ids, System.DateTime fromDate, System.DateTime toDate, HistQuotesInterval interval)
        {
            if (ids == null)
                throw new ArgumentNullException("unmanagedIDs", "The passed list is null.");
            this.IDs = MyHelper.EnumToArray(ids);
            this.TextEncoding = System.Text.Encoding.UTF8;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.Interval = interval;
        }


        protected override string GetUrl()
        {
            if (this.IDs == null || this.IDs.Length == 0) { throw new ArgumentException("No ID available.", "IDs"); }
            if (this.FromDate > this.ToDate) { throw new ArgumentException("FromDate must be smaller than ToDate."); }
            return this.DownloadURL(this.IDs, this.FromDate, this.ToDate, this.Interval);
        }
        private string DownloadURL(IEnumerable<string> ids, System.DateTime fromDate, System.DateTime toDate, HistQuotesInterval interval)
        {
            string[] idArr = MyHelper.EnumToArray(ids);
            if (idArr.Length == 0)
            {
                throw new ArgumentNullException("unmanagedIDs", "The passed list is empty");
            }
            else
            {
                if (idArr.Length == 1)
                {
                    if (idArr[0].Trim() == string.Empty)
                    {
                        throw new ArgumentNullException("id", "The passed ID is empty.");
                    }
                    else
                    {
                        System.Text.StringBuilder url = new System.Text.StringBuilder();
                        url.Append("http://ichart.yahoo.com/table.csv?s=");
                        url.Append(Uri.EscapeDataString(MyHelper.CleanYqlParam(FinanceHelper.CleanIndexID(idArr[0]).ToUpper())));
                        url.Append("&a=");
                        url.Append(fromDate.Month - 1);
                        url.Append("&b=");
                        url.Append(fromDate.Day);
                        url.Append("&c=");
                        url.Append(fromDate.Year);
                        url.Append("&d=");
                        url.Append(toDate.Month - 1);
                        url.Append("&e=");
                        url.Append(toDate.Day);
                        url.Append("&f=");
                        url.Append(toDate.Year);
                        url.Append("&g=");
                        url.Append(FinanceHelper.GetHistQuotesInterval(interval));
                        url.Append("&ignore=.csv");
                        return url.ToString();
                    }
                }
                else
                {
                    System.Text.StringBuilder url = new System.Text.StringBuilder();
                    url.Append("url in (");
                    for (int i = 0; i <= idArr.Length - 1; i++)
                    {
                        url.Append('\'');
                        url.Append(this.DownloadURL(new string[] { MyHelper.CleanYqlParam(FinanceHelper.CleanIndexID(idArr[i]).ToUpper()) }, fromDate, toDate, interval));
                        url.Append('\'');
                        if (i < idArr.Length - 1)
                            url.Append(',');
                    }
                    url.Append(")");
                    return MyHelper.YqlUrl("*", "csv", url.ToString(), null, true);
                }
            }
        }


        public override object Clone()
        {
            return new HistQuotesDownloadSettings((string[])this.IDs.Clone(), this.FromDate, this.ToDate, this.Interval);
        }
    }


}
