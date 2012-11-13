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


namespace MaasOne.Finance.YahooFinance.Support
{
    /// <summary>
    /// Class for downloading exchange data
    /// </summary>
    /// <remarks></remarks>
    public partial class ExchangeRateDownload
    {


        /// <summary>
        /// Raises if an asynchronous download of exchange rates completes
        /// </summary>
        /// <param name="sender">The event raising object</param>
        /// <param name="ea">The event args of the asynchronous download</param>
        /// <remarks></remarks>
        public event AsyncRateDownloadCompletedEventHandler AsyncRateDownloadCompleted;
        public delegate void AsyncRateDownloadCompletedEventHandler(object sender, ExchangeRateDownloadCompletedEventArgs ea);


        private QuotesDownload mQuotesBaseDownload = new QuotesDownload();
        private HistQuotesDownload mHistQuotesDownload = new HistQuotesDownload();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public ExchangeRateDownload()
        {
            mQuotesBaseDownload.AsyncDownloadCompleted += DownloadQuotesAsync_Completed;
            mHistQuotesDownload.AsyncDownloadCompleted += DownloadHistQuotesAsync_Completed;
        }


        public bool CancelAsyncAll()
        {
            return mQuotesBaseDownload.CancelAsyncAll() & mHistQuotesDownload.CancelAsyncAll();
        }


        /// <summary>
        /// Starts an asynchronous download of actual currency exchange rates
        /// </summary>
        /// <param name="currencies">List of all currency-pairs</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadAsync(YCurrencyID[] currencies, object userArgs = null)
        {
            if (currencies == null)
            {
                throw new ArgumentNullException("currencies", "The passed currencies have no values.");
            }
            else
            {
                AsyncDownloadArgs args = new AsyncDownloadArgs(userArgs, currencies);
                mQuotesBaseDownload.DownloadAsync(FinanceHelper.IIDsToStrings(args.Currencies), args);
            }
        }
        /// <summary>
        ///  Starts an asynchronous download of historic currency exchange rates
        /// </summary>
        /// <param name="currencies">List of all currency-pairs</param>
        /// <param name="tradeDate">The trade date of exchange rates</param>
        /// <param name="userArgs">Individual user argument</param>
        /// <remarks></remarks>
        public void DownloadAsync(YCurrencyID[] currencies, DateTime tradeDate, object userArgs = null)
        {
            if (currencies == null)
            {
                throw new ArgumentNullException("currencies", "The passed currencies have no values.");
            }
            else
            {
                AsyncDownloadArgs args = new AsyncDownloadArgs(userArgs, currencies);
                mHistQuotesDownload.DownloadAsync(FinanceHelper.IIDsToStrings(args.Currencies), tradeDate, tradeDate, HistQuotesInterval.Daily, args);
            }
        }



        private void DownloadQuotesAsync_Completed(Base.DownloadClient<QuotesResult> sender, Base.DownloadCompletedEventArgs<QuotesResult> e)
        {
            AsyncDownloadArgs dlArgs = (AsyncDownloadArgs)e.UserArgs;
            if (AsyncRateDownloadCompleted != null)
            {
                AsyncRateDownloadCompleted(this, new ExchangeRateDownloadCompletedEventArgs(dlArgs.UserArgs, this.ToResponse(e.Response, dlArgs.Currencies)));
            }
        }
        private void DownloadHistQuotesAsync_Completed(Base.DownloadClient<HistQuotesResult> sender, Base.DownloadCompletedEventArgs<HistQuotesResult> e)
        {
            AsyncDownloadArgs dlArgs = (AsyncDownloadArgs)e.UserArgs;
            if (AsyncRateDownloadCompleted != null)
            {
                AsyncRateDownloadCompleted(this, new ExchangeRateDownloadCompletedEventArgs(dlArgs.UserArgs, this.ToResponse(e.Response, dlArgs.Currencies)));
            }
        }

        private ExchangeRateResponse ToResponse(Base.Response<QuotesResult> resp, YCurrencyID[] currencies)
        {
            List<ExchangeRateData> lst = new List<ExchangeRateData>();
            if (resp.Result != null)
            {
                foreach (QuotesData q in resp.Result.Items)
                {
                    YCurrencyID cur = FinanceHelper.YCurrencyIDFromString(q.ID);
                    if (cur != null)
                        lst.Add(new ExchangeRateData(cur.BaseCurrency, cur.DepCurrency, q));
                }
            }
            return new ExchangeRateResponse(resp.Connection, new ExchangeRateResult(lst.ToArray(), currencies));
        }
        private ExchangeRateResponse ToResponse(Base.Response<HistQuotesResult> resp, YCurrencyID[] currencies)
        {
            List<ExchangeRateData> lst = new List<ExchangeRateData>();
            if (resp.Result != null)
            {
                foreach (HistQuotesDataChain hqc in resp.Result.Chains)
                {
                    if (hqc.Count > 0)
                    {
                        YCurrencyID cur = FinanceHelper.YCurrencyIDFromString(hqc.ID);
                        if (cur != null)
                        {
                            QuotesData q = new QuotesData();
                            q.SetID(hqc.ID);
                            HistQuotesData hqd = (HistQuotesData)hqc[0];
                            q.Change = hqd.Close - hqd.PreviousClose;
                            q.LastTradePriceOnly = hqd.Close;
                            q.LastTradeDate = hqd.TradingDate;
                            q.Volume = hqd.Volume;
                            lst.Add(new ExchangeRateData(cur.BaseCurrency, cur.DepCurrency, q));
                        }
                    }
                }
            }
            return new ExchangeRateResponse(resp.Connection, new ExchangeRateResult(lst.ToArray(), currencies));
        }

        private class AsyncDownloadArgs : Base.DownloadEventArgs
        {
            public YCurrencyID[] Currencies;
            public AsyncDownloadArgs(object userArgs, YCurrencyID[] curs)
                : base(userArgs)
            {
                this.Currencies = curs;
            }
        }



    }



    public class ExchangeRateDownloadCompletedEventArgs : Base.DownloadCompletedEventArgs<ExchangeRateResult>
    {

        public ExchangeRateDownloadCompletedEventArgs(object userArgs, ExchangeRateResponse response)
            : base(userArgs, response, null)
        {
        }

    }

    public class ExchangeRateResponse : Base.Response<ExchangeRateResult>
    {

        public ExchangeRateResponse(Base.ConnectionInfo connInfo, ExchangeRateResult result)
            : base(connInfo, result)
        {
        }


    }



    public class ExchangeRateResult
    {
        private YCurrencyID[] mCurrencies;
        public YCurrencyID[] Currencies
        {
            get { return mCurrencies; }
        }

        private ExchangeRateData[] mItems = null;
        public ExchangeRateData[] Items
        {
            get { return mItems; }
        }

        internal ExchangeRateResult(ExchangeRateData[] items, YCurrencyID[] curs)
        {
            mItems = items;
        }

    }

    /// <summary>
    /// Stores exchange rate informations of two currencies. Implements IID.
    /// </summary>
    /// <remarks></remarks>
    public class ExchangeRateData : QuotesBaseData
    {


        private YCurrencyID mCurrencyRelation = new YCurrencyID();
        /// <summary>
        /// The both currencies representing the relation
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public YCurrencyID CurrencyRelation
        {
            get { return mCurrencyRelation; }
            set
            {
                if (value != null)
                {
                    mCurrencyRelation = value;
                }
                else
                {
                    mCurrencyRelation = new YCurrencyID();
                }
            }
        }
        /// <summary>
        /// The dependent value of one unit of base currency (LastTradePriceOnly)
        /// </summary>
        /// <value></value>
        /// <returns>Value of LastTradePriceOnly of quote values</returns>
        /// <remarks></remarks>
        public double DependencyValue
        {
            get { return base.LastTradePriceOnly; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public ExchangeRateData()
        {
        }
        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="baseCur">The currency with the ratio value of 1</param>
        /// <param name="cur">The currency of the dependent value</param>
        /// <remarks></remarks>
        public ExchangeRateData(CurrencyInfo baseCur, CurrencyInfo cur)
        {
            mCurrencyRelation.BaseCurrency = baseCur;
            mCurrencyRelation.DepCurrency = cur;
            base.SetID(mCurrencyRelation.ID);
        }
        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="baseCur">The currency with the ratio value of 1</param>
        /// <param name="cur">The currency of the dependent value</param>
        /// <param name="quotes">The quote values of the relation</param>
        /// <remarks></remarks>
        public ExchangeRateData(CurrencyInfo baseCur, CurrencyInfo cur, QuotesBaseData q)
        {
            mCurrencyRelation.BaseCurrency = baseCur;
            mCurrencyRelation.DepCurrency = cur;
            this.Change = q.Change;
            this.LastTradePriceOnly = q.LastTradePriceOnly;
            this.LastTradeDate = q.LastTradeDate;
            this.Volume = q.Volume;
        }

        public override string ToString()
        {
            if (mCurrencyRelation.DepCurrency.Description != string.Empty)
            {
                return mCurrencyRelation.DepCurrency.Description + " (" + mCurrencyRelation.DepCurrency.ID + ")";
            }
            else
            {
                return base.ToString();
            }
        }

    }





}
