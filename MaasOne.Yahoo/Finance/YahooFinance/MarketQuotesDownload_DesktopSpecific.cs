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


namespace MaasOne.Finance.YahooFinance
{
    public partial class MarketQuotesDownload 
    {

        /// <summary>
        /// Downloads all available sector market quotes.
        /// </summary>
        /// <param name="rankedBy">The property, the list is ranked by</param>
        /// <param name="rankDir">The direction of ranking</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<MarketQuotesResult> DownloadAllSectorQuotes(MarketQuoteProperty rankedBy = MarketQuoteProperty.Name, ListSortDirection rankDir = ListSortDirection.Ascending)
        {
            return this.Download(new MarketQuotesDownloadSettings() { RankedBy = rankedBy, RankDirection = rankDir });
        }
        /// <summary>
        /// Downloads all available industries of a special sector.
        /// </summary>
        /// <param name="sector">The sector of the industries</param>
        /// <param name="rankedBy">The property the list is ranked by</param>
        /// <param name="rankDir">The direction of ranking</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<MarketQuotesResult> DownloadIndustryQuotes(Sector sector, MarketQuoteProperty rankedBy = MarketQuoteProperty.Name, ListSortDirection rankDir = ListSortDirection.Ascending)
        {
            return this.Download(new MarketQuotesDownloadSettings() { Sector = sector, RankedBy = rankedBy, RankDirection = rankDir });
        }
        /// <summary>
        /// Downloads all available company quotes of a special industry.
        /// </summary>
        /// <param name="industyID">The ID of the industry</param>
        /// <param name="rankedBy">The property the list is ranked by</param>
        /// <param name="rankDir">The direction of ranking</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<MarketQuotesResult> DownloadCompanyQuotes(Industry industyID, MarketQuoteProperty rankedBy = MarketQuoteProperty.Name, System.ComponentModel.ListSortDirection rankDir = System.ComponentModel.ListSortDirection.Ascending)
        {
            return this.Download(new MarketQuotesDownloadSettings() { Industry = industyID, RankedBy = rankedBy, RankDirection = rankDir });
        }
        public Base.Response<MarketQuotesResult> Download(MarketQuotesDownloadSettings settings)
        {
            return base.Download(settings);
        }

    }
}
