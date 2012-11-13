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
    /// Stores informations about a country.
    /// </summary>
    /// <remarks></remarks>
    public class CountryInfo
    {
        private Country mID;
        private string mName = string.Empty;
        private CurrencyInfo mCurrency = null;
        private DaylightSavingTime[] mDaylightSavingTimes = new DaylightSavingTime[-1 + 1];

        private List<Support.YIndexID> mIndices = new List<Support.YIndexID>();
        /// <summary>
        /// The country ID.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Country ID
        {
            get { return mID; }
        }
        /// <summary>
        /// The currency of this country
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CurrencyInfo Currency
        {
            get { return mCurrency; }
        }
        /// <summary>
        /// The name of the country
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name
        {
            get { return mName; }
        }
        /// <summary>
        /// The list of Daylight Saving Times of the country for each year
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DaylightSavingTime[] DaylightSavingTimes
        {
            get { return mDaylightSavingTimes; }
        }
        /// <summary>
        ///The indices of the country
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<Support.YIndexID> Indices
        {
            get { return mIndices; }
        }

        public CountryInfo(Country id, string name, CurrencyInfo cur)
        {
            mID = id;
            mName = name;
            mCurrency = cur;
        }
        public CountryInfo(Country id, string name, CurrencyInfo cur, DaylightSavingTime[] dstArray)
            : this(id, name, cur)
        {
            if (dstArray != null)
                mDaylightSavingTimes = dstArray;
        }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
