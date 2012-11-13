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
    /// Class for saving informations about the fix dates for Daylight Saving Time in a specific year.
    /// </summary>
    /// <remarks></remarks>
    public class DaylightSavingTime
    {

        private int mYear;
        private DateTime mStartDate;

        private DateTime mEndDate;
        public int Year
        {
            get { return mYear; }
        }
        public DateTime StartDate
        {
            get { return mStartDate; }
        }
        public DateTime EndDate
        {
            get { return mEndDate; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="startDate">The start date of Daylight Saving Time</param>
        /// <param name="endDate">The end date of Daylight Saving Time</param>
        /// <remarks>In case of countries in southern hemisphere the start date is higher then the end date. In this case the object contains the end date of the period that starts one year before. The start date is the begin of the period that will go on into the next year.</remarks>
        public DaylightSavingTime(DateTime startDate, DateTime endDate)
        {
            if (startDate.Year != endDate.Year)
            {
                throw new ArgumentException("The year of [startDate] is not the same year like [endDate] parameter.", "startDate");
            }
            else
            {
                mYear = startDate.Year;
                mStartDate = startDate;
                mEndDate = endDate;
            }
        }

    }
}
