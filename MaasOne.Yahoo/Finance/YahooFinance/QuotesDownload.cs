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


namespace MaasOne.Finance.YahooFinance
{
    /// <summary>
    /// Provides methods for downloading quotes data.
    /// </summary>
    /// <remarks></remarks>
    public partial class QuotesDownload : Base.DownloadClient<QuotesResult>
    {

        public QuotesDownloadSettings Settings { get { return (QuotesDownloadSettings)base.Settings; } set { base.SetSettings(value); } }


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public QuotesDownload()
        {
            this.Settings = new QuotesDownloadSettings();
        }

        public void DownloadAsync(IEnumerable<string> unmanagedIDs, object userArgs)
        {
            this.DownloadAsync(unmanagedIDs, new QuoteProperty[] { QuoteProperty.Symbol, QuoteProperty.LastTradePriceOnly, QuoteProperty.DaysHigh, QuoteProperty.DaysLow, QuoteProperty.Volume, QuoteProperty.LastTradeDate, QuoteProperty.LastTradeTime }, userArgs);
        }

        /// <summary>
        /// Starts an asynchronous download of quotes data.
        /// </summary>
        /// <param name="managedID">The managed ID</param>
        /// <param name="properties">The properties of each quote data. If parameter is null/Nothing, Symbol and LastTradePrizeOnly will set as property. In this case, with YQL server you will get every available property.</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadAsync(IID managedID, IEnumerable<QuoteProperty> properties, object userArgs)
        {
            if (managedID == null)
                throw new ArgumentNullException("managedID", "The passed ID is null.");
            this.DownloadAsync(managedID.ID, properties, userArgs);
        }
        /// <summary>
        /// Starts an asynchronous download of quotes data.
        /// </summary>
        /// <param name="unmanagedID">The unmanaged ID</param>
        /// <param name="properties">The properties of each quote data. If parameter is null/Nothing, Symbol and LastTradePrizeOnly will set as property. In this case, with YQL server you will get every available property.</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadAsync(string unmanagedID, IEnumerable<QuoteProperty> properties, object userArgs)
        {
            if (unmanagedID == string.Empty)
                throw new ArgumentNullException("unmanagedID", "The passed ID is empty.");
            this.DownloadAsync(new string[] { unmanagedID }, properties, userArgs);
        }
        /// <summary>
        /// Starts an asynchronous download of quotes data.
        /// </summary>
        /// <param name="managedIDs">The list of managed IDs</param>
        /// <param name="properties">The properties of each quote data. If parameter is null/Nothing, Symbol and LastTradePrizeOnly will set as property. In this case, with YQL server you will get every available property.</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadAsync(IEnumerable<IID> managedIDs, IEnumerable<QuoteProperty> properties, object userArgs)
        {
            if (managedIDs == null)
                throw new ArgumentNullException("managedIDs", "The passed list is null.");
            this.DownloadAsync(FinanceHelper.IIDsToStrings(managedIDs), properties, userArgs);
        }
        /// <summary>
        /// Starts an asynchronous download of quotes data.
        /// </summary>
        /// <param name="unmanagedIDs">The list of unmanaged IDs</param>
        /// <param name="properties">The properties of each quote data. If parameter is null/Nothing, Symbol and LastTradePrizeOnly will set as property. In this case, with YQL server you will get every available property.</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadAsync(IEnumerable<string> unmanagedIDs, IEnumerable<QuoteProperty> properties, object userArgs)
        {
            this.DownloadAsync(new QuotesDownloadSettings() { IDs = MyHelper.EnumToArray(unmanagedIDs), Properties = MyHelper.EnumToArray(properties) }, userArgs);
        }


        /// <summary>
        /// Starts an asynchronous download of quotes data.
        /// </summary>
        /// <param name="settings">Individual Download Settings.</param>
        /// <param name="userArgs">Individual user argument.</param>
        public void DownloadAsync(QuotesDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }

        protected override QuotesResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            QuotesDownloadSettings set = (QuotesDownloadSettings)settings;
            return new QuotesResult(ImportExport.ToQuotesData(MyHelper.StreamToString(stream, set.TextEncoding), ',', set.Properties, new System.Globalization.CultureInfo("en-US")), set);
        }

    }

    /// <summary>
    /// Stores the result data
    /// </summary>
    public class QuotesResult : QuotesBaseResult
    {

        private QuotesDownloadSettings mSettings = null;
        public QuotesDownloadSettings Settings { get { return mSettings; } }

        public QuotesData[] Items { get { return (QuotesData[])base.Items; } }

        internal QuotesResult(QuotesData[] items, QuotesDownloadSettings settings)
            : base(items)
        {
            mSettings = settings;
        }
    }



    /// <summary>
    /// Stores informations of different quote values. Implements IID.
    /// </summary>
    /// <remarks></remarks>
    public class QuotesData : QuotesBaseData, ICloneable
    {

        private object[] mValues = new object[88];

        public object Values(QuoteProperty prp) { return this[prp]; }
        /// <summary>
        /// Gets or sets the value of a specfic property
        /// </summary>
        /// <param name="prp">Gets or sets the property you want to get or set</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public object this[QuoteProperty prp]
        {
            get { return mValues[(int)prp]; }
            set
            {
                if (value != null)
                {
                    double t = 0;
                    System.DateTime dt;
                    long l = 0;
                    switch (prp)
                    {
                        case QuoteProperty.Symbol:
                            base.SetID(value.ToString());
                            break;
                        case QuoteProperty.LastTradePriceOnly:
                            if (double.TryParse(value.ToString(), out t))
                                base.LastTradePriceOnly = t;
                            break;
                        case QuoteProperty.LastTradeDate:
                            if (System.DateTime.TryParse(value.ToString(), out dt))
                                base.LastTradeDate = dt;

                            break;
                        case QuoteProperty.LastTradeTime:
                            if (System.DateTime.TryParse(value.ToString(), out dt))
                                base.LastTradeTime = dt;
                            break;
                        case QuoteProperty.Change:
                            if (double.TryParse(value.ToString(), out t))
                                base.Change = t;
                            break;
                        case QuoteProperty.Volume:
                            if (long.TryParse(value.ToString(), out l))
                                base.Volume = l;
                            break;
                        case QuoteProperty.Name:
                            base.Name = value.ToString();
                            break;
                    }
                }
                mValues[(int)prp] = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the QuoteData
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Represents the value of QuoteProperty.Name</remarks>
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; mValues[(int)QuoteProperty.Name] = value; }
        }
        /// <summary>
        /// Sets a new ID value. Implementation from ISettableID.
        /// </summary>
        /// <param name="id">A valid Yahoo! ID</param>
        /// <remarks></remarks>
        public override void SetID(string id)
        {
            base.SetID(id);
            mValues[(int)QuoteProperty.Symbol] = id;
        }
        /// <summary>
        /// Gets or sets the latest price value of the QuoteData.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Represents the value of QuoteProperty.LastTradePriceOnly</remarks>
        public override double LastTradePriceOnly
        {
            get { return base.LastTradePriceOnly; }
            set { base.LastTradePriceOnly = value; mValues[(int)QuoteProperty.LastTradePriceOnly] = value; }
        }
        /// <summary>
        /// Gets or sets the date value of the last trade.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public override System.DateTime LastTradeDate
        {
            get { return base.LastTradeDate; }
            set
            { base.LastTradeDate = value; mValues[(int)QuoteProperty.LastTradeDate] = value; }
        }
        /// <summary>
        /// Gets or sets the time value of the last trade.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public override DateTime LastTradeTime
        {
            get { return base.LastTradeTime; }
            set { base.LastTradeTime = value; mValues[(int)QuoteProperty.LastTradeTime] = value; }
        }
        /// <summary>
        /// Gets or sets the change in percent.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public override double Change
        {
            get { return base.Change; }
            set { base.Change = value; mValues[(int)QuoteProperty.Change] = value; }
        }
        /// <summary>
        /// Gets or sets the trade volume of the day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public override long Volume
        {
            get { return base.Volume; }
            set { base.Volume = value; mValues[(int)QuoteProperty.Volume] = value; }
        }
        /// <summary>
        /// Gets or sets the opening price value of the day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Open
        {
            get { if (mValues[(int)QuoteProperty.Open] != null) { return (double)mValues[(int)QuoteProperty.Open]; } else { return 0; } }
            set { mValues[(int)QuoteProperty.Open] = value; }
        }
        /// <summary>
        /// Gets or sets the highest value of the day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double DaysHigh
        {
            get { if (mValues[(int)QuoteProperty.DaysHigh] != null) { return (double)mValues[(int)QuoteProperty.DaysHigh]; } else { return 0; } }
            set { mValues[(int)QuoteProperty.DaysHigh] = value; }
        }
        /// <summary>
        /// Gets or sets the lowest price value of the day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double DaysLow
        {
            get { if (mValues[(int)QuoteProperty.DaysLow] != null) { return (double)mValues[(int)QuoteProperty.DaysLow]; } else { return 0; } }
            set { mValues[(int)QuoteProperty.DaysLow] = value; }
        }
        public string Currency
        {
            get { return this[QuoteProperty.Currency] != null ? this[QuoteProperty.Currency].ToString() : string.Empty; }
            set { this[QuoteProperty.Currency] = value; }
        }

        public override double PreviewClose
        {
            get
            {
                if (this[QuoteProperty.PreviousClose] != null && this[QuoteProperty.PreviousClose] is double)
                {
                    return Convert.ToDouble(this[QuoteProperty.PreviousClose]);
                }
                else
                {
                    return base.PreviewClose;
                }
            }
        }
        public override double ChangeInPercent
        {
            get
            {
                if (this[QuoteProperty.ChangeInPercent] != null && this[QuoteProperty.ChangeInPercent] is double)
                {
                    return Convert.ToDouble(this[QuoteProperty.ChangeInPercent]);
                }
                else
                {
                    return base.ChangeInPercent;
                }
            }
        }

        public QuotesData() { }
        public QuotesData(string id) { this.SetID(id); }

        public virtual object Clone()
        {
            QuotesData cln = new QuotesData();
            foreach (QuoteProperty qp in Enum.GetValues(typeof(QuoteProperty)))
            {
                if (this[qp] is object[])
                {
                    object[] obj = (object[])this[qp];
                    object[] newObj = new object[obj.Length];
                    if (obj.Length > 0)
                    {
                        for (int i = 0; i <= obj.Length - 1; i++)
                        {
                            newObj[i] = obj[i];
                        }
                    }
                    cln[qp] = newObj;
                }
                else
                {
                    cln[qp] = this[qp];
                }
            }
            return cln;
        }


    }



    public class QuotesDownloadSettings : Base.SettingsBase, ITextEncodingSettings
    {
        /// <summary>
        /// The text encoding for CSV download.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.Text.Encoding TextEncoding { get; set; }

        public string[] IDs { get; set; }
        public QuoteProperty[] Properties { get; set; }

        public QuotesDownloadSettings()
        {
            this.TextEncoding = System.Text.Encoding.UTF8;
            this.IDs = new string[] { };
            this.Properties = new QuoteProperty[] { QuoteProperty.Symbol, QuoteProperty.Name, QuoteProperty.LastTradePriceOnly };
        }
        public QuotesDownloadSettings(string id)
        {
            this.TextEncoding = System.Text.Encoding.UTF8;
            this.IDs = new string[] { id };
            this.Properties = new QuoteProperty[] { QuoteProperty.Symbol, QuoteProperty.Name, QuoteProperty.LastTradePriceOnly };
        }
        public QuotesDownloadSettings(string id, QuoteProperty[] properties)
        {
            this.TextEncoding = System.Text.Encoding.UTF8;
            this.IDs = new string[] { id };
            this.Properties = properties;
        }

        protected override string GetUrl()
        {
            if (this.IDs == null || this.IDs.Length == 0)
            {
                throw new NotSupportedException("An empty id list will not be supported.");
            }
            else
            {
                System.Text.StringBuilder ids = new System.Text.StringBuilder();
                foreach (string s in this.IDs)
                {
                    ids.Append(MyHelper.CleanYqlParam(s));
                    ids.Append('+');
                }
                String url = "http://download.finance.yahoo.com/d/quotes.csv?s=" + Uri.EscapeDataString(ids.ToString()) + "&f=" + FinanceHelper.CsvQuotePropertyTags(this.Properties) + "&e=.csv";
                return url;
            }
        }

        public override object Clone()
        {
            QuotesDownloadSettings cln = new QuotesDownloadSettings();
            cln.IDs = (string[])this.IDs.Clone();
            cln.Properties = (QuoteProperty[])this.Properties.Clone();
            return cln;
        }
    }




}
