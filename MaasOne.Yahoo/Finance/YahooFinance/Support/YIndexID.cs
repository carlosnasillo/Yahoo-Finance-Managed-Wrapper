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
    /// Stores information of an stock index. Implements IID.
    /// </summary>
    /// <remarks></remarks>
    public class YIndexID : YID
    {


        private bool mDownloadComponents = false;
        /// <summary>
        /// The Yahoo index ID
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ID
        {
            get
            {
                if (mDownloadComponents)
                {
                    return "@" + base.ID;
                }
                else
                {
                    return base.ID;
                }
            }
        }
        /// <summary>
        /// The overloaded type of the financial product 
        /// </summary>
        /// <value></value>
        /// <returns>Returns only [Index]. The type of an stock index is of course always [Index].</returns>
        /// <remarks></remarks>
        public override SecurityType Type
        {
            get { return SecurityType.Index; }
            set { base.Type = SecurityType.Index; }
        }
        /// <summary>
        /// Indicates whether the downloader will query all stocks of an index or not
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool DownloadComponents
        {
            get { return mDownloadComponents; }
            set { mDownloadComponents = value; this.OnPropertyChanged("DownloadComponents"); }
        }

        protected YIndexID()
        {
        }
        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="id">The unmanaged ID</param>
        /// <remarks></remarks>
        public YIndexID(string id)
            : base(id)
        {
            base.Type = SecurityType.Index;
            mDownloadComponents = id.StartsWith("@");
        }
        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="id">The unmanaged ID</param>
        /// <param name="downloadComponents">True, if you want to download all components of the index</param>
        /// <remarks></remarks>
        public YIndexID(string id, bool downloadComponents)
            : this(id)
        {
            mDownloadComponents = downloadComponents;
        }
        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="searchResult">The downloaded search result with the values</param>
        /// <remarks></remarks>
        public YIndexID(IDSearchData searchResult)
            : base(searchResult)
        {
            if (!(searchResult.Type == SecurityType.Index))
            {
                throw new ArgumentException("The passed result is not an index", "result");
            }
        }

        /// <summary>
        /// Returns the full ID of the stock index.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.ID;
        }

        /// <summary>
        /// Proves if a search result represents an index
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsValidSearchResult(IDSearchData result)
        {
            return result.Type == SecurityType.Index;
        }
    }
}
