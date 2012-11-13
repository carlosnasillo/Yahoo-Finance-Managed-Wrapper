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
    public partial class MarketDownload 
    {

        /// <summary>
        /// Downloads all available sectors and its industry IDs
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public SectorResponse DownloadAllSectors()
        {
            return this.DownloadSectors(null);
        }
        /// <summary>
        /// Downloads sectors and its industry IDs with passed names.
        /// </summary>
        /// <param name="sectors">The names of the sectors</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public SectorResponse DownloadSectors(IEnumerable<Sector> sectors)
        {
            return (SectorResponse)base.Download(new MarketDownloadSettings() { Sectors = MyHelper.EnumToArray(sectors)});
        }
        /// <summary>
        /// Downloads industries and its company IDs with passed IndustryData.
        /// </summary>
        /// <param name="industries">The industries</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public IndustryResponse DownloadIndustries(IEnumerable<IndustryData> industries)
        {
            List<Industry> lst = new List<Industry>();
            if (industries != null)
            {
                foreach (IndustryData ind in industries)
                {
                    lst.Add(ind.ID);
                }
            }
            if (lst.Count == 0)
                throw new ArgumentNullException("industries", "The passed list is empty.");
            return (IndustryResponse)base.Download(new MarketDownloadSettings() { Industries = lst.ToArray() });
        }
        /// <summary>
        /// Downloads industries and its company IDs with passed IDs.
        /// </summary>
        /// <param name="industryIDs">The IDs of the industries</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public IndustryResponse DownloadIndustries(IEnumerable<Industry> industryIDs)
        {
            Industry[] ids = MyHelper.EnumToArray(industryIDs);
            if (ids.Length == 0)
                throw new ArgumentNullException("industryIDs", "The passed list is empty.");
            return (IndustryResponse)base.Download(new MarketDownloadSettings() { Industries = ids });
        }
           
    }
}
