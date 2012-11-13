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
    public partial class QuotesDownload
    {

        public Base.Response<QuotesResult> Download(IEnumerable<string> unmanagedIDs) {
            return this.Download(unmanagedIDs, new QuoteProperty[] { QuoteProperty.Symbol, QuoteProperty.LastTradePriceOnly, QuoteProperty.DaysHigh, QuoteProperty.DaysLow, QuoteProperty.Volume, QuoteProperty.LastTradeDate, QuoteProperty.LastTradeTime });
        }
        /// <summary>
        /// Downloads quotes data.
        /// </summary>
        /// <param name="managedID">The managed ID</param>
        /// <param name="properties">The properties of each quote data. If parameter is null/Nothing, Symbol and LastTradePrizeOnly will set as property. In this case, with YQL server you will get every available property.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<QuotesResult> Download(IID managedID, IEnumerable<QuoteProperty> properties)
        {
            if (managedID == null)
                throw new ArgumentNullException("id", "The passed id is null.");
            return this.Download(managedID.ID, properties);
        }
        /// <summary>
        /// Downloads quotes data.
        /// </summary>
        /// <param name="unmanagedID">The unmanaged ID</param>
        /// <param name="properties">The properties of each quote data. If parameter is null/Nothing, Symbol and LastTradePrizeOnly will set as property. In this case, with YQL server you will get every available property.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<QuotesResult> Download(string unmanagedID, IEnumerable<QuoteProperty> properties)
        {
            if (unmanagedID == string.Empty)
                throw new ArgumentNullException("unmanagedID", "The passed id is empty.");
            return this.Download(new string[] { unmanagedID }, properties);
        }
        /// <summary>
        /// Downloads quotes data.
        /// </summary>
        /// <param name="managedIDs">The list of managed IDs</param>
        /// <param name="properties">The properties of each quote data. If parameter is null/Nothing, Symbol and LastTradePrizeOnly will set as property. In this case, with YQL server you will get every available property.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<QuotesResult> Download(IEnumerable<IID> managedIDs, IEnumerable<QuoteProperty> properties)
        {
            if (managedIDs == null)
                throw new ArgumentNullException("managedIDs", "The passed list is null.");
            return this.Download(FinanceHelper.IIDsToStrings(managedIDs), properties);
        }
        /// <summary>
        /// Downloads quotes data.
        /// </summary>
        /// <param name="unmanagedIDs">The list of unmanaged IDs</param>
        /// <param name="properties">The properties of each quote data. If parameter is null/Nothing, Symbol and LastTradePrizeOnly will set as property. In this case, with YQL server you will get every available property.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<QuotesResult> Download(IEnumerable<string> unmanagedIDs, IEnumerable<QuoteProperty> properties)
        {
            if (unmanagedIDs == null)
                throw new ArgumentNullException("unmanagedIDs", "The passed list is null.");
            return this.Download(new QuotesDownloadSettings() { IDs = MyHelper.EnumToArray(unmanagedIDs), Properties = MyHelper.EnumToArray(properties) });
        }
        public Base.Response<QuotesResult> Download(QuotesDownloadSettings settings)
        {
            return base.Download(settings);
        }

    }
}
