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


namespace MaasOne.Finance.YahooFinance.Support
{
    /// <summary>
    /// Stores informations of a financial product. Implements IID.
    /// </summary>
    /// <remarks></remarks>
    public class YID : IID, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string mID = string.Empty;
        private string mName = string.Empty;
        private string mIndustry = string.Empty;
        private StockExchange mStockExchange = null;
        private ISIN mISIN = null;

        private SecurityType mType = SecurityType.Any;
        /// <summary>
        /// The full ID with suffix 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual string ID
        {
            get { return mID; }
        }
        /// <summary>
        /// The base ID without suffix
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string BaseID
        {
            get
            {
                int index = mID.LastIndexOf('.');
                if (index == -1)
                {
                    return mID;
                }
                else
                {
                    return mID.Substring(0, index);
                }
            }
        }
        /// <summary>
        /// The suffix of the ID
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Suffix
        {
            get
            {
                int index = mID.LastIndexOf('.');
                if (index == -1)
                {
                    return string.Empty;
                }
                else
                {
                    return mID.Substring(index);
                }
            }
        }
        /// <summary>
        /// The name of the security
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name
        {
            get { return mName; }
            set { mName = value; this.OnPropertyChanged("Name"); }
        }
        /// <summary>
        /// The name of the industry
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Industry
        {
            get { return mIndustry; }
            set { mIndustry = value; this.OnPropertyChanged("Industry"); }
        }
        /// <summary>
        /// Informations about the stock exchange where the stock is traded
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Don't accept Null/Nothing.</remarks>
        public StockExchange StockExchange
        {
            get { return mStockExchange; }
            set { mStockExchange = value; this.OnPropertyChanged("StockExchange"); }
        }
        /// <summary>
        /// The International Securities Identification Number
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ISIN ISIN
        {
            get { return mISIN; }
            set { mISIN = value; this.OnPropertyChanged("ISIN"); }
        }
        /// <summary>
        /// The type of the security
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual SecurityType Type
        {
            get { return mType; }
            set { mType = value; this.OnPropertyChanged("Type"); }
        }

        protected virtual void SetID(string id, bool setStockExchangeBySuffix = true)
        {
            if (id == null || id.Trim() == string.Empty) throw new ArgumentNullException("id", "The ID is null.");
            mID = id.Trim().ToUpper();
            this.OnPropertyChanged("ID");
            if (setStockExchangeBySuffix)
            {
                mStockExchange = null;
                StockExchange se = WorldMarket.GetStockExchangeBySuffix(id);
                if (se != null) { this.StockExchange = se; }
            }
        }

        protected YID()
        {
        }
        /// <summary>
        /// Creates a new instance by an ID
        /// </summary>
        /// <param name="id"></param>
        /// <remarks></remarks>
        public YID(string id)
        {
            this.SetID(FinanceHelper.CleanIndexID(id));
        }
        /// <summary>
        /// Creates a new instance from an IDSearchResult
        /// </summary>
        /// <param name="searchResult"></param>
        /// <remarks></remarks>
        public YID(IDSearchData searchResult)
        {
            if (searchResult != null)
            {
                this.SetID(searchResult.ID, false);
                this.Name = searchResult.Name;
                this.Industry = searchResult.Industry;
                this.Type = searchResult.Type;

                string exc = searchResult.Exchange.Replace("N/A", "").Trim();
                if (exc != string.Empty)
                {
                    StockExchange se = WorldMarket.GetStockExchangeByID(exc);
                    if (se == null)
                        se = WorldMarket.GetStockExchangeByName(exc);
                    if (se != null)
                    {
                        this.StockExchange = se;
                        
                    }
                    else
                    {
                        se = WorldMarket.GetStockExchangeBySuffix(this.ID);
                        if (se != null)
                        {
                            this.StockExchange = se;
                            
                        }
                        else
                        {
                            CountryInfo cnt = WorldMarket.GetDefaultCountry(Country.US);
                            TradingTimeInfo tti = new TradingTimeInfo(0, new DayOfWeek[] {
							DayOfWeek.Monday,
							DayOfWeek.Tuesday,
							DayOfWeek.Wednesday,
							DayOfWeek.Thursday,
							DayOfWeek.Friday
						}, null, new DateTime(), new TimeSpan(23, 59, 59), -5);
                            this.StockExchange = new StockExchange(searchResult.Exchange, this.Suffix, searchResult.Exchange, cnt, tti);
                        }
                    }
                }

                if (searchResult.ISIN != null && searchResult.ISIN.Value.Replace("N/A", "").Trim() != string.Empty)
                {
                    try
                    {
                        this.ISIN = new ISIN(searchResult.ISIN.Value);
                    }
                    catch (ArgumentException ex)
                    {
                        this.ISIN = null;
                    }
                }

            }
            else
            {
                throw new ArgumentException("The passed result is null", "searchResult");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Returns the URL of the Yahoo! RSS news feed.
        /// </summary>
        /// <param name="culture">The culture of the feed.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string RssNewsURL(Culture culture = null)
        {
            Culture cult = culture;
            if (cult == null)
                cult = Culture.DefaultCultures.UnitedStates_English;
            return "http://feeds.finance.yahoo.com/rss/2.0/headline?s=" + MyHelper.CleanYqlParam(mID) + YahooHelper.CultureToParameters(cult);
        }
        /// <summary>
        /// Returns the URL of the Yahoo! RSS Blog feed.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string RssFinancialBlogsURL()
        {
            return "http://finance.yahoo.com/rss/blog?s=" + MyHelper.CleanYqlParam(mID);
        }

        /// <summary>
        /// Returns the full ID of the stock.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return mID;
        }

    }
}
