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
    public partial class ExchangeRateCalculator
    {

        /// <summary>
        /// The used proxy informations
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.Net.IWebProxy Proxy
        {
            get { return mDownloader.Proxy; }
            set { mDownloader.Proxy = value; }
        }

        /// <summary>
        /// Updates exchange informations
        /// </summary>
        /// <param name="baseCurrency">The currency all other currencies are depending</param>
        /// <param name="currencies">The dependent currencies</param>
        /// <remarks></remarks>
        public void Update(CurrencyInfo baseCurrency, IEnumerable<CurrencyInfo> currencies)
        {
            mDownloader.CancelAsyncAll();
            mDonwloadCounter += 1;
            mExchangeItems = new ExchangeRateData[] { };
            ExchangeRateResponse resp = mDownloader.Download(this.GetCurrencyList(baseCurrency, currencies));
            if (resp.Connection.State == Base.ConnectionState.Success)
                mExchangeItems = resp.Result.Items;
        }

        /// <summary>
        /// Converts as value of a currency to the ratio value of another currency
        /// </summary>
        /// <param name="value"></param>
        /// <param name="currencyOfValue"></param>
        /// <param name="returnCurrency"></param>
        /// <returns></returns>
        /// <remarks>Returns 0, if the dependency value of one of both currencies is not in the list</remarks>
        public double ConvertCurrency(double value, CurrencyInfo currencyOfValue, CurrencyInfo returnCurrency)
        {
            try
            {
                if (currencyOfValue.ID != returnCurrency.ID)
                {
                    double fromRatio = 0;
                    double toRatio = 0;
                    foreach (ExchangeRateData eiFrom in mExchangeItems)
                    {
                        if (eiFrom.CurrencyRelation.DepCurrency.ID == currencyOfValue.ID)
                        {
                            fromRatio = eiFrom.DependencyValue;
                            foreach (ExchangeRateData eiTo in mExchangeItems)
                            {
                                if (eiTo.CurrencyRelation.DepCurrency.ID == returnCurrency.ID)
                                {
                                    toRatio = eiTo.DependencyValue;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    if (fromRatio != 0 & toRatio != 0)
                        return (value / fromRatio * toRatio);
                }
                else
                {
                    return value;
                }
            }
            catch (Exception ex)
            {
            }
            return 0;
        }

    }
}
