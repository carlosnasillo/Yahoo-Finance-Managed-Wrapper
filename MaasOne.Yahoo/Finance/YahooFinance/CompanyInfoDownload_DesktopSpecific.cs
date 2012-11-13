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
    public partial class CompanyInfoDownload 
    {

        /// <summary>
        /// Downloads company informations with passed ID.
        /// </summary>
        /// <param name="managedID">The managed ID of the company</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<CompanyInfoResult> Download(IID managedID)
        {
            if (managedID == null)
                throw new ArgumentNullException("managedID", "The passed ID is null.");
            return this.Download(managedID.ID);
        }
        /// <summary>
        /// Downloads company informations with passed ID.
        /// </summary>
        /// <param name="unmanagedID">The unmanaged ID of the company</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<CompanyInfoResult> Download(string unmanagedID)
        {
            if (unmanagedID == string.Empty)
                throw new ArgumentNullException("unmanagedID", "The passed ID is empty.");
            return this.Download(new string[] { unmanagedID });
        }
        /// <summary>
        /// Downloads company informations with passed IDs.
        /// </summary>
        /// <param name="ids">The managed list of IDs of the companies</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<CompanyInfoResult> Download(IEnumerable<IID> ids)
        {
            if (ids == null)
                throw new ArgumentNullException("ids", "The passed list is null.");
            return this.Download(FinanceHelper.IIDsToStrings(ids));
        }
        /// <summary>
        /// Downloads company information with passed IDs.
        /// </summary>
        /// <param name="ids">The unmanaged list of IDs of the companies</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Base.Response<CompanyInfoResult> Download(IEnumerable<string> ids)
        {
            if (ids == null)
                throw new ArgumentNullException("ids", "The passed list is null.");
            return this.Download(new CompanyInfoDownloadSettings(MyHelper.EnumToArray(ids)));
        }
        public Base.Response<CompanyInfoResult> Download(CompanyInfoDownloadSettings settings)
        {
            return base.Download(settings);
        }

    }
}
