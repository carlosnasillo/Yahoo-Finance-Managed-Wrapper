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
    /// Class for downloading and calculating exchange rates
    /// </summary>
    /// <remarks></remarks>
    public partial class ExchangeRateCalculator
    {
        /// <summary>
        /// Raises when started asynchronous download completes
        /// </summary>
        /// <param name="sender"></param>
        /// <remarks></remarks>
        public event AsyncUpdateCompletedEventHandler AsyncUpdateCompleted;
        public delegate void AsyncUpdateCompletedEventHandler(ExchangeRateCalculator sender, ExchangeRateCalculatorCompletedEventArgs ea);

        private ExchangeRateData[] mExchangeItems = new ExchangeRateData[-1 + 1];
        private ExchangeRateDownload mDownloader = new ExchangeRateDownload();

        private long mDonwloadCounter = 0;
        /// <summary>
        /// The downloaded or setted exchange rate informations
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>By setting the items the base currency of each ExchangeRateData must be the same</remarks>
        public ExchangeRateData[] ExchangeItems
        {
            get { return mExchangeItems; }
            set
            {
                if (value != null && value.Length > 0)
                {
                    if (value[0].CurrencyRelation.BaseCurrency != null)
                    {
                        CurrencyInfo bc = value[0].CurrencyRelation.BaseCurrency;
                        bool hasSameBC = true;
                        foreach (ExchangeRateData item in value)
                        {
                            if (item.CurrencyRelation.BaseCurrency == null || item.CurrencyRelation.BaseCurrency.ID != bc.ID)
                            {
                                hasSameBC = false;
                                break; // TODO: might not be correct. Was : Exit For
                            }
                        }
                        if (hasSameBC)
                            mExchangeItems = value;
                    }
                }
            }
        }


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks></remarks>
        public ExchangeRateCalculator()
        {
            mDownloader.AsyncRateDownloadCompleted += AsyncDownload_Completed;
        }

        /// <summary>
        /// Starts asynchronous update of exchange rates
        /// </summary>
        /// <param name="baseCurrency">The currency all other currencies are depending</param>
        /// <param name="currencies">The dependent currencies</param>
        /// <remarks></remarks>
        public void UpdateAsync(CurrencyInfo baseCurrency, IEnumerable<CurrencyInfo> currencies, object userArgs = null)
        {
            mDownloader.CancelAsyncAll();
            mDonwloadCounter += 1;
            mExchangeItems = new ExchangeRateData[] { };
            mDownloader.DownloadAsync(this.GetCurrencyList(baseCurrency, currencies), new AsyncDownloadArgs(userArgs, mDonwloadCounter));
        }
        /// <summary>
        /// Starts asynchronous update of exchange rates
        /// </summary>
        /// <param name="baseCurrency">The currency all other currencies are depending</param>
        /// <param name="currencies">The dependent currencies</param>
        /// <remarks></remarks>
        public void UpdateAsync(CurrencyInfo baseCurrency, IEnumerable<CurrencyInfo> currencies, DateTime tradeDate, object userArgs = null)
        {
            mDownloader.CancelAsyncAll();
            mDonwloadCounter += 1;
            mExchangeItems = new ExchangeRateData[] { };
            mDownloader.DownloadAsync(this.GetCurrencyList(baseCurrency, currencies), tradeDate, new AsyncDownloadArgs(userArgs, mDonwloadCounter));
        }


        private void AsyncDownload_Completed(object sender, Base.DownloadCompletedEventArgs<ExchangeRateResult> e)
        {
            AsyncDownloadArgs dlArgs = (AsyncDownloadArgs)e.UserArgs;
            if (e.Response.Connection.State == Base.ConnectionState.Success & dlArgs.Counter == mDonwloadCounter)
                mExchangeItems = e.Response.Result.Items;
            if (AsyncUpdateCompleted != null)
            {
                AsyncUpdateCompleted(this, new ExchangeRateCalculatorCompletedEventArgs(dlArgs.UserArgs, e.Response.Connection.State == Base.ConnectionState.Success));
            }
        }


        private YCurrencyID[] GetCurrencyList(CurrencyInfo baseCurrency, IEnumerable<CurrencyInfo> currencies)
        {
            List<YCurrencyID> lst = new List<YCurrencyID>();
            if (currencies != null)
            {
                foreach (CurrencyInfo cur in currencies)
                {
                    lst.Add(new YCurrencyID(baseCurrency, cur));
                }
            }
            return lst.ToArray();
        }

        /// <summary>
        /// Converts as value of a currenciy to the ratio value of another currency; both ExchangeRataData have same base currency
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="currencyOfValue">The base currency</param>
        /// <param name="returnCurrency">The dependent currency</param>
        /// <returns></returns>
        /// <remarks>Returns 0, if the base currency of both is not equal</remarks>
        public static double ConvertCurrency(double value, ExchangeRateData currencyOfValue, ExchangeRateData returnCurrency)
        {
            try
            {
                if (currencyOfValue.CurrencyRelation.BaseCurrency.ID == returnCurrency.CurrencyRelation.BaseCurrency.ID)
                {
                    if (currencyOfValue.CurrencyRelation.DepCurrency.ID != returnCurrency.CurrencyRelation.DepCurrency.ID)
                    {
                        double fromRatio = currencyOfValue.DependencyValue;
                        double toRatio = returnCurrency.DependencyValue;
                        if (fromRatio != 0 & toRatio != 0)
                        {
                            return (value / fromRatio * toRatio);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return value;
                    }
                }
                else
                {
                    throw new ArgumentException("The exchange rates have not the same base currency.");
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public static double ConvertCurrency(double value, ExchangeRateData exchangeRate, bool reverse = false)
        {
            if (exchangeRate != null && exchangeRate.LastTradePriceOnly != 0)
            {
                if (!reverse)
                {
                    return value * exchangeRate.LastTradePriceOnly;
                }
                else
                {
                    return value / exchangeRate.LastTradePriceOnly;
                }
            }
            else
            {
                throw new ArgumentException("The exchange rate is invalid", "exchangeRate");
            }
        }


        private class AsyncDownloadArgs : Base.DownloadEventArgs
        {
            public long Counter = 0;
            public AsyncDownloadArgs(object userArgs, long cnt)
                : base(userArgs)
            {
                this.Counter = cnt;
            }
        }
    }

    /// <summary>
    /// Stores the downloaded size in bytes
    /// </summary>
    /// <remarks></remarks>
    public class ExchangeRateCalculatorCompletedEventArgs : Base.DownloadEventArgs
    {
        private bool mSuccess;
        public bool Success
        {
            get { return mSuccess; }
        }
        internal ExchangeRateCalculatorCompletedEventArgs(object userArgs, bool success)
            : base(userArgs)
        {
            mSuccess = success;
        }
    }
}
