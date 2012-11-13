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
    public partial class HistQuotesDownload 
    {

        /// <summary>
        /// Downloads historic quotes data.
        /// </summary>
        /// <param name="managedID">The managed ID</param>
        /// <param name="fromDate">The startdate of the reviewed period</param>
        /// <param name="todate">The enddate of the reviewed period</param>
        /// <param name="interval">The trading period interval</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<HistQuotesResult> Download(IID managedID, System.DateTime fromDate, System.DateTime todate, HistQuotesInterval interval)
        {
            if (managedID == null)
                throw new ArgumentNullException("managedID", "The passed ID is null.");
            return this.Download(managedID.ID, fromDate, todate, interval);
        }
        /// <summary>
        /// Downloads historic quotes data.
        /// </summary>
        /// <param name="unmanagedID">The unmanaged ID</param>
        /// <param name="fromDate">The startdate of the reviewed period</param>
        /// <param name="todate">The enddate of the reviewed period</param>
        /// <param name="interval">The trading period interval</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<HistQuotesResult> Download(string unmanagedID, System.DateTime fromDate, System.DateTime todate, HistQuotesInterval interval)
        {
            if (unmanagedID.Trim() == string.Empty)
                throw new ArgumentNullException("unmanagedID", "The passed ID is empty.");
            return this.Download(new string[] { unmanagedID }, fromDate, todate, interval);
        }
        /// <summary>
        /// Downloads historic quotes data.
        /// </summary>
        /// <param name="managedIDs">The managed ID</param>
        /// <param name="fromDate">The startdate of the reviewed period</param>
        /// <param name="todate">The enddate of the reviewed period</param>
        /// <param name="interval">The trading period interval</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<HistQuotesResult> Download(IEnumerable<IID> managedIDs, System.DateTime fromDate, System.DateTime toDate, HistQuotesInterval interval)
        {
            if (managedIDs == null)
                throw new ArgumentNullException("managedIDs", "The passed list is null.");
            return this.Download(FinanceHelper.IIDsToStrings(managedIDs), fromDate, toDate, interval);
        }
        /// <summary>
        /// Downloads historic quotes data.
        /// </summary>
        /// <param name="unmanagedIDs">The unmanaged ID</param>
        /// <param name="fromDate">The startdate of the reviewed period</param>
        /// <param name="todate">The enddate of the reviewed period</param>
        /// <param name="interval">The trading period interval</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<HistQuotesResult> Download(IEnumerable<string> unmanagedIDs, System.DateTime fromDate, System.DateTime toDate, HistQuotesInterval interval)
        {
            if (unmanagedIDs == null)
                throw new ArgumentNullException("unmanagedID", "The passed ID is empty.");
            this.CheckDates(fromDate, toDate);
            string[] ids = FinanceHelper.CleanIDfromAT(unmanagedIDs);
            return this.Download(new HistQuotesDownloadSettings(ids, fromDate, toDate, interval));
        }

        public Base.Response<HistQuotesResult> Download(HistQuotesDownloadSettings settings)
        {

            return base.Download(settings);
        }
    }
}
