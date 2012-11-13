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
using MaasOne.Xml;
using System.Text;
using System.Xml.Linq;


namespace MaasOne.Finance.YahooFinance
{
    /// <summary>
    /// Provides methods for downloading quote options.
    /// </summary>
    /// <remarks></remarks>
    public partial class QuoteOptionsDownload : Base.DownloadClient<QuoteOptionsResult>
    {

        public QuoteOptionsDownloadSettings Settings { get { return (QuoteOptionsDownloadSettings)base.Settings; } set { base.SetSettings(value); } }


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public QuoteOptionsDownload()
        {
            this.Settings = new QuoteOptionsDownloadSettings();
        }

        public void DownloadAsync(IID managedID, object userArgs = null)
        {
            if (managedID == null)
            {
                throw new ArgumentNullException("managedID", "The passed ID is null.");
            }
            else
            {
                this.DownloadAsync(managedID.ID);
            }
        }
        public void DownloadAsync(IEnumerable<IID> managedIDs, object userArgs = null)
        {
            if (managedIDs == null)
            {
                throw new ArgumentNullException("managedIDs", "The passed list is null.");
            }
            else
            {
                this.DownloadAsync(FinanceHelper.IIDsToStrings(managedIDs));
            }
        }
        public void DownloadAsync(IID managedID, System.DateTime expirationDate, object userArgs = null)
        {
            if (managedID == null)
            {
                throw new ArgumentNullException("managedID", "The passed ID is null.");
            }
            else
            {
                this.DownloadAsync(managedID.ID, expirationDate);
            }
        }
        public void DownloadAsync(IEnumerable<IID> managedIDs, System.DateTime expirationDate, object userArgs = null)
        {
            if (managedIDs == null)
            {
                throw new ArgumentNullException("managedIDs", "The passed list is null.");
            }
            else
            {
                this.DownloadAsync(FinanceHelper.IIDsToStrings(managedIDs), expirationDate);
            }
        }
        public void DownloadAsync(IID managedID, IEnumerable<System.DateTime> expirationDates, object userArgs = null)
        {
            if (managedID == null)
            {
                throw new ArgumentNullException("managedID", "The passed ID is null.");
            }
            else
            {
                this.DownloadAsync(managedID.ID, expirationDates);
            }
        }

        public void DownloadAsync(string unmanagedID, object userArgs = null)
        {
            if (unmanagedID == string.Empty)
            {
                throw new ArgumentNullException("unmanagedID", "The passed ID is empty.");
            }
            else
            {
                this.DownloadAsync(new QuoteOptionsDownloadSettings() { IDs = new string[] { unmanagedID } }, userArgs);
            }
        }
        public void DownloadAsync(IEnumerable<string> unmanagedIDs, object userArgs = null)
        {
            if (unmanagedIDs == null)
            {
                throw new ArgumentNullException("unmanagedIDs", "The passed list is null.");
            }
            else
            {
                this.DownloadAsync(new QuoteOptionsDownloadSettings() { IDs = MyHelper.EnumToArray(unmanagedIDs) }, userArgs);
            }
        }
        public void DownloadAsync(string unmanagedID, System.DateTime expirationDate, object userArgs = null)
        {
            if (unmanagedID == string.Empty)
            {
                throw new ArgumentNullException("unmanagedID", "The passed ID is empty.");
            }
            else
            {
                this.DownloadAsync(new QuoteOptionsDownloadSettings() { IDs = new string[] { unmanagedID }, ExpirationDates = new DateTime[] { expirationDate } }, userArgs);
            }
        }
        public void DownloadAsync(IEnumerable<string> unmanagedIDs, System.DateTime expirationDate, object userArgs = null)
        {
            if (unmanagedIDs == null)
            {
                throw new ArgumentNullException("unmanagedIDs", "The passed list is null.");
            }
            else
            {
                this.DownloadAsync(new QuoteOptionsDownloadSettings() { IDs = MyHelper.EnumToArray(unmanagedIDs), ExpirationDates = new DateTime[] { expirationDate } }, userArgs);
            }
        }
        public void DownloadAsync(string unmanagedID, IEnumerable<System.DateTime> expirationDates, object userArgs = null)
        {
            if (unmanagedID == string.Empty)
            {
                throw new ArgumentNullException("unmanagedID", "The passed ID is empty.");
            }
            else
            {
                this.DownloadAsync(new QuoteOptionsDownloadSettings() { IDs = new string[] { unmanagedID }, ExpirationDates = MyHelper.EnumToArray(expirationDates) }, userArgs);
            }
        }

        public void DownloadAsync(QuoteOptionsDownloadSettings settings, object userArgs)
        {
            base.DownloadAsync(settings, userArgs);
        }


        protected override QuoteOptionsResult ConvertResult(Base.ConnectionInfo connInfo, System.IO.Stream stream, Base.SettingsBase settings)
        {
            List<QuoteOptionsDataChain> options = new List<QuoteOptionsDataChain>();

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            XDocument doc = MyHelper.ParseXmlDocument(stream);
            XElement[] mainLst = XPath.GetElements("//optionsChain", doc);

            foreach (XElement chain in mainLst)
            {
                string idAtt = MyHelper.GetXmlAttributeValue(chain, "symbol");
                string expirationDateAtt = MyHelper.GetXmlAttributeValue(chain, "expiration");
                DateTime expirationDate = default(DateTime);
                if (!System.DateTime.TryParseExact(expirationDateAtt, "yyyy-MM-dd", culture, System.Globalization.DateTimeStyles.None, out expirationDate))
                {
                    System.DateTime.TryParseExact(expirationDateAtt, "yyyy-MM", culture, System.Globalization.DateTimeStyles.None, out expirationDate);
                }
                List<QuoteOptionsData> lst = new List<QuoteOptionsData>();
                foreach (XElement optionNode in chain.Elements())
                {
                    if (optionNode.Name.LocalName == "option")
                    {
                        QuoteOptionsData opt = ImportExport.ToQuoteOption(optionNode, culture);
                        if (opt != null)
                            lst.Add(opt);
                    }
                }
                options.Add(new QuoteOptionsDataChain(idAtt, expirationDate, lst));
            }
            return new QuoteOptionsResult(options.ToArray());
        }



    }


    /// <summary>
    /// Stores the result data
    /// </summary>
    public class QuoteOptionsResult
    {
        private QuoteOptionsDataChain[] mItems = null;
        public QuoteOptionsDataChain[] Items
        {
            get { return mItems; }
        }
        internal QuoteOptionsResult(QuoteOptionsDataChain[] items)
        {
            mItems = items;
        }
    }

    /// <summary>
    /// Stores informations of quote options.
    /// </summary>
    /// <remarks></remarks>
    public class QuoteOptionsData
    {

        private double[] mValues = new double[5];
        /// <summary>
        /// The basic parts of new option symbol are: Root symbol + Expiration Year(yy)+ Expiration Month(mm)+ Expiration Day(dd) + Call/Put Indicator (C or P) + Strike price
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Symbol { get; set; }
        /// <summary>
        /// Call/Put Indicator
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public QuoteOptionType Type { get; set; }
        /// <summary>
        ///  The stated price per share for which underlying stock can be purchased (in the case of a call) or sold (in the case of a put) by the option holder upon exercise of the option contract.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double StrikePrice { get; set; }
        /// <summary>
        /// The price of the last trade made for option contract.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double LastPrice { get; set; }
        /// <summary>
        /// The change in price for the day. This is the difference between the last trade and the previous day's closing price (Prev Close). The change is reported as "0" if the option hasn't traded today.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Change { get; set; }
        /// <summary>
        /// The Bid price is the price you get if you were to write (sell) an option.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Bid { get; set; }
        /// <summary>
        /// The Ask price is the price you have to pay to purchase an option.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Ask { get; set; }
        /// <summary>
        /// The volume indicates the number of option contracts that have traded for the current day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Volume { get; set; }
        /// <summary>
        /// The total number of derivative contracts traded that have not yet been liquidated either by an offsetting derivative transaction or by delivery.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int OpenInterest { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public QuoteOptionsData()
        {
        }
        internal QuoteOptionsData(string symb, QuoteOptionType typ, double strike, double last, double cng, double b, double a, int vol, int interest)
        {
            this.Symbol = symb;
            this.Type = typ;
            this.StrikePrice = strike;
            this.LastPrice = last;
            this.Change = cng;
            this.Bid = b;
            this.Ask = a;
            this.Volume = vol;
            this.OpenInterest = interest;
        }

        public override string ToString()
        {
            return this.Symbol;
        }

    }



    public class QuoteOptionsDataChain : ISettableID, IEnumerable<QuoteOptionsData>
    {

        private string mID = string.Empty;
        private System.DateTime mExpirationDate;

        private List<QuoteOptionsData> mItems = new List<QuoteOptionsData>();

        public string ID
        {
            get { return mID; }
        }
        public System.DateTime ExpirationDate
        {
            get { return mExpirationDate; }
        }
        public void SetID(string id)
        {
            mID = id;
        }

        public QuoteOptionsData this[int index]
        {
            get { return mItems[index]; }
            set { mItems[index] = value; }
        }

        public List<QuoteOptionsData> Items
        {
            get { return mItems; }
        }
        public int Count
        {
            get { return mItems.Count; }
        }

        public QuoteOptionsDataChain()
        {
        }
        public QuoteOptionsDataChain(string id)
        {
            mID = id;
        }
        public QuoteOptionsDataChain(IEnumerable<QuoteOptionsData> items)
        {
            mItems = new List<QuoteOptionsData>(items);
        }
        public QuoteOptionsDataChain(string id, System.DateTime expirationDate, IEnumerable<QuoteOptionsData> items)
            : this(items)
        {
            mID = id;
            mExpirationDate = expirationDate;
        }

        public System.Collections.Generic.IEnumerator<QuoteOptionsData> GetEnumerator()
        {
            return mItems.GetEnumerator();
        }
        public System.Collections.IEnumerator GetEnumerator1()
        {
            return mItems.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }

    }



    public class QuoteOptionsDownloadSettings : Base.SettingsBase
    {

        public string[] IDs { get; set; }
        public DateTime[] ExpirationDates { get; set; }

        public QuoteOptionsDownloadSettings()
        {
            this.IDs = new string[] { };
            this.ExpirationDates = new DateTime[] { };
        }

        protected override string GetUrl()
        {
            string[] ids = MyHelper.EnumToArray(this.IDs);

            if (ids.Length > 0)
            {
                System.Text.StringBuilder whereClause = new System.Text.StringBuilder();

                if (ids.Length == 1)
                {
                    whereClause.AppendFormat("symbol=\"{0}\"", ids[0]);
                }
                else
                {
                    whereClause.Append("symbol in (");
                    foreach (string id in ids)
                    {
                        if (id.Trim() != string.Empty)
                        {
                            whereClause.AppendFormat("\"{0}\",", MyHelper.CleanYqlParam(id));
                        }
                    }
                    whereClause.Remove(whereClause.Length - 1, 1);
                    whereClause.Append(')');
                }

                if (this.ExpirationDates != null)
                {
                    System.DateTime[] expirations = MyHelper.EnumToArray(this.ExpirationDates);
                    if (ids.Length > 1 & expirations.Length > 1)
                        throw new NotSupportedException("Multiple IDs and multiple Expiration Dates are not supported");
                    if (expirations.Length > 0)
                    {
                        if (expirations.Length == 1)
                        {
                            whereClause.AppendFormat(" AND expiration=\"{0}-{1:00}\"", expirations[0].Year, expirations[0].Month);
                        }
                        else
                        {
                            whereClause.Append(" AND expiration in (");
                            foreach (System.DateTime exp in expirations)
                            {
                                whereClause.AppendFormat("\"{0}-{1:00}\",", exp.Year, exp.Month);
                            }
                            whereClause.Remove(whereClause.Length - 1, 1);
                            whereClause.Append(')');
                        }
                    }
                }
                return MyHelper.YqlUrl("*", "yahoo.finance.options", whereClause.ToString(), null, false);
            }
            else
            {
                throw new NotSupportedException("An empty id list will not be supported.");
            }
        }


        public override object Clone()
        {
            return new QuoteOptionsDownloadSettings() { IDs = (string[])this.IDs.Clone(), ExpirationDates = (DateTime[])this.ExpirationDates.Clone() };
        }

    }


}
