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
    public partial class ExchangeRateDownload
    {

        public System.Net.IWebProxy Proxy
        {
            get { return mQuotesBaseDownload.Proxy; }
            set
            {
                mQuotesBaseDownload.Proxy = value;
                mHistQuotesDownload.Proxy = value;
            }
        }

        /// <summary>
        /// Downloads a list of actual currency exchange rates
        /// </summary>
        /// <param name="currencies">List of all currency-pairs</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public ExchangeRateResponse Download(YCurrencyID[] currencies)
        {
            if (currencies == null)
                throw new ArgumentNullException("currencies", "The passed currencies have no values.");
            return this.ToResponse(mQuotesBaseDownload.Download(FinanceHelper.IIDsToStrings(currencies)), currencies);
        }
        /// <summary>
        /// Downloads a list of historical currency exchange rates
        /// </summary>
        /// <param name="currencies"></param>
        /// <param name="tradeDate"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public ExchangeRateResponse Download(YCurrencyID[] currencies, DateTime tradeDate)
        {
            if (currencies == null)
                throw new ArgumentNullException("currencies", "The passed currencies have no values.");
            return this.ToResponse(mHistQuotesDownload.Download(FinanceHelper.IIDsToStrings(currencies), tradeDate, tradeDate, HistQuotesInterval.Daily), currencies);
        }

    }
}
