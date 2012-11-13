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


namespace MaasOne.Finance.YahooFinance.Support
{
    /// <summary>
    /// Class for managing stock exchange information. 
    /// </summary>
    /// <remarks></remarks>
    public class StockExchange 
    {
       
        private string mID = string.Empty;
        private string mName = string.Empty;
        private string mSuffix = string.Empty;

        private CountryInfo mCountry = null;

        private TradingTimeInfo mTradingTime = null;
        /// <summary>
        /// The ID of the exchange
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>If the ID is in WorldMarket.DefaultStockExchanges, properties will be setted automatically</remarks>
        public string ID
        {
            get { return mID; }
        }

        /// <summary>
        /// The ending string for stock IDs
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>If the suffix is in DefaultStockExchanges, properties will get automatically</remarks>
        public string Suffix
        {
            get { return mSuffix; }
        }

        /// <summary>
        /// The name of the exchange
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name
        {
            get { return mName; }
        }


        public CountryInfo Country
        {
            get { return mCountry; }
        }


        public TradingTimeInfo TradingTime
        {
            get { return mTradingTime; }
        }

        private readonly List<string> mTags = new List<string>();
        internal List<string> Tags
        {
            get { return mTags; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="suffix"></param>
        /// <remarks></remarks>
        public StockExchange(string id, string suffix, string name, CountryInfo country, TradingTimeInfo tradeTime)
        {
            if (id != string.Empty)
            {
                mID = id;
            }
            else
            {
                throw new ArgumentNullException("id", "The ID is empty.");
            }

            mSuffix = suffix;
            mName = name;
            if (country != null)
            {
                mCountry = country;
            }
            else
            {
                throw new ArgumentNullException("country", "The country is null.");
            }

            if (tradeTime != null)
            {
                mTradingTime = tradeTime;
            }
            else
            {
                throw new ArgumentNullException("tradeTime", "The trade time is null.");
            }
        }
        internal StockExchange(StockExchange se)
        {
            if (se == null)
            {
                throw new ArgumentNullException("se", "Original StockExchange is null.");
            }
            else
            {
                if (se != null)
                {
                    mID = se.ID;
                    mCountry = se.Country;
                    mSuffix = se.Suffix;
                    mName = se.Name;
                    TradingTimeInfo tt = se.TradingTime;
                    mTradingTime = new TradingTimeInfo(tt.DelayMinutes, tt.TradingDays, tt.Holidays, tt.LocalOpeningTime, tt.TradingSpan, tt.UtcOffsetStandardTime, tt.DaylightSavingTimes);
                }
            }
        }            

        /// <summary>
        /// Returns the name of the stock exchange
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return mName;
        }

    }


    public class TradingTimeInfo
    {
        private int mDelayMinutes = 0;
        private List<DayOfWeek> mTradingDays = new List<DayOfWeek>();
        private List<System.DateTime> mHolidays = new List<System.DateTime>();
        private DateTime mLocalOpeningTime = new DateTime();
        private TimeSpan mTradingSpan = new TimeSpan(24, 0, 0);
        private int mUtcOffsetStandardTime = 0;

        private DaylightSavingTime[] mDaylightSavingTimes = new DaylightSavingTime[-1 + 1];
        /// <summary>
        /// The data response delay to realtime of yahoo servers
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int DelayMinutes
        {
            get { return mDelayMinutes; }
        }
        /// <summary>
        /// The days when trading is active
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<DayOfWeek> TradingDays
        {
            get { return mTradingDays; }
        }
        /// <summary>
        /// Days without active trading time.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<System.DateTime> Holidays
        {
            get { return mHolidays; }
        }
        /// <summary>
        /// The time when trading starts
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>By setting a value, the date is not important, only hour and minute</remarks>
        public DateTime LocalOpeningTime
        {
            get { return mLocalOpeningTime; }
        }
        /// <summary>
        /// The timespan of active trading for each trading day
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public TimeSpan TradingSpan
        {
            get { return mTradingSpan; }
        }
        /// <summary>
        /// The time when trading ends
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>By setting a value, the date is not important, only hour and minute. If time value is smaler than opening, trading ends on the next day. 24 hours trading is maximum</remarks>
        public DateTime LocalClosingTime
        {
            get { return mLocalOpeningTime.Add(mTradingSpan); }
        }
        /// <summary>
        /// The number of hours relative to UTC of the exchange's timezone
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int UtcOffsetStandardTime
        {
            get { return mUtcOffsetStandardTime; }
        }
        public DaylightSavingTime[] DaylightSavingTimes
        {
            get { return mDaylightSavingTimes; }
        }

        public TradingTimeInfo(int delayMinutes, IEnumerable<DayOfWeek> tradingDays, IEnumerable<System.DateTime> holidays, DateTime localOpeningTime, TimeSpan tradingSpan, int utcOffset)
        {
            if (delayMinutes >= 0 & delayMinutes < 3600)
            {
                mDelayMinutes = delayMinutes;
            }
            else
            {
                throw new ArgumentException("The delay in minutes must be minimum 0 and maximum 3600.", "delayMinutes");
            }

            if (tradingDays != null)
            {
                mTradingDays.AddRange(tradingDays);
            }

            if (holidays != null)
            {
                mHolidays.AddRange(holidays);
            }

            mLocalOpeningTime = new DateTime().AddHours(localOpeningTime.Hour).AddMinutes(localOpeningTime.Minute);

            if (tradingSpan.TotalMinutes > 0 & tradingSpan.TotalMinutes <= 3600)
            {
                mTradingSpan = tradingSpan;
            }
            else if (tradingSpan.TotalMinutes == 0)
            {
                mTradingSpan = new TimeSpan(24, 0, 0);
            }
            else
            {
                throw new ArgumentException("The trading span must be within 24 hours", "tradingSpan");
            }

            if (utcOffset >= -12 & utcOffset <= 12)
            {
                mUtcOffsetStandardTime = utcOffset;
            }
            else
            {
                throw new ArgumentException("The UTC offset must be between -12 and +12", "utcOffset");
            }

        }
        public TradingTimeInfo(int delayMinutes, IEnumerable<DayOfWeek> tradingDays, IEnumerable<System.DateTime> holidays, DateTime localOpeningTime, TimeSpan tradingSpan, int utcOffset, DaylightSavingTime[] dst)
            : this(delayMinutes, tradingDays, holidays, localOpeningTime, tradingSpan, utcOffset)
        {
            if (dst != null)
            {
                mDaylightSavingTimes = dst;
            }
        }


        /// <summary>
        /// Returns if trading is active at a specific datetime in relation to stock exchange's current timezone.
        /// </summary>
        /// <param name="time">The DateTime of the stock exchange's local time zone.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsActiveExchangeLocal(DateTime time)
        {
            bool isHoliday = false;
            if (mHolidays != null && mHolidays.Count > 0)
            {
                foreach (System.DateTime h in mHolidays)
                {
                    if (h.Date == time.Date)
                    {
                        isHoliday = true;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }

            if (!isHoliday && mTradingDays != null && mTradingDays.Contains(time.DayOfWeek))
            {
                DateTime localStartTime = time.Date.AddHours(mLocalOpeningTime.Hour).AddMinutes(mLocalOpeningTime.Minute);
                DateTime localEndTime = localStartTime.Add(mTradingSpan);
                return (time > localStartTime & time < localEndTime);
            }
            else
            {
                return false;
            }
        }

        public DateTime ActualExchangeLocalTime()
        {
            DateTime time = System.DateTime.UtcNow;
            int dstOffset = 0;
            if (mDaylightSavingTimes.Length > 0)
            {
                foreach (DaylightSavingTime dst in mDaylightSavingTimes)
                {
                    if (dst.Year == time.Year)
                    {
                        if (dst.StartDate < dst.EndDate)
                        {
                            if (time > dst.StartDate & time < dst.EndDate)
                            {
                                dstOffset += 1;
                            }
                        }
                        else
                        {
                            if (time < dst.StartDate | time > dst.EndDate)
                            {
                                dstOffset += 1;
                            }
                        }
                    }
                }
            }
            return time.AddHours(mUtcOffsetStandardTime + dstOffset);
        }

        /// <summary>
        /// Returns if trading is active at a specific UTC DateTime.
        /// </summary>
        /// <param name="time">The UTC DateTime</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsActiveUTC(DateTime time)
        {
            int dstOffset = 0;
            if (mDaylightSavingTimes.Length > 0)
            {
                foreach (DaylightSavingTime dst in mDaylightSavingTimes)
                {
                    if (dst.Year == time.Year)
                    {
                        if (dst.StartDate < dst.EndDate)
                        {
                            if (time > dst.StartDate & time < dst.EndDate)
                            {
                                dstOffset += 1;
                            }
                        }
                        else
                        {
                            if (time < dst.StartDate | time > dst.EndDate)
                            {
                                dstOffset += 1;
                            }
                        }
                    }
                }
            }
            return this.IsActiveExchangeLocal(time.AddHours(mUtcOffsetStandardTime + dstOffset));
        }

        public bool IsActiveMachineLocal(DateTime time)
        {
            return this.IsActiveUTC(time.ToUniversalTime());
        }





    }
}
