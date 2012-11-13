// ******************************************************************************
// ** 
// **  MaasOne WebServices
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


namespace MaasOne.Finance
{


    public class QuotesBaseResult { 
    
        private QuotesBaseData[] mItems = null;
        public QuotesBaseData[] Items { get { return mItems; } }

        public QuotesBaseResult(QuotesBaseData[] items)
        {
            mItems = items;
        }

    }


    public abstract class QuotesBaseData : IID, ISettableID
    {

        private string mID = string.Empty;

        /// <summary>
        /// The ID of the QuoteBaseData
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual string ID
        {
            get { return mID; }
        }
        /// <summary>
        /// Sets a new ID value. Implementation from ISettableID.
        /// </summary>
        /// <param name="id">A valid Yahoo! ID</param>
        /// <remarks></remarks>
        public virtual void SetID(string id)
        {
            mID = id;
        }
        public virtual string Name { get; set; }
        /// <summary>
        /// The price value of the QuoteBaseData
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual double LastTradePriceOnly { get; set; }
        /// <summary>
        /// The change of the price in relation to close value of the previous day
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual double Change { get; set; }
        /// <summary>
        /// The trade volume of the day
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual long Volume { get; set; }
        /// <summary>
        /// The calculated close price of the last trading day
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>[LastTradePriceOnly] - [Change]</remarks>
        public virtual double PreviewClose
        {
            get { return this.LastTradePriceOnly - this.Change; }
        }
        /// <summary>
        /// The calculated price change in percent
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>[Change] / [PreviewClose]</remarks>
        public virtual double ChangeInPercent
        {
            get
            {
                if (this.PreviewClose != 0)
                {
                    return (this.Change / this.PreviewClose) * 100;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// The date value of the data
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual System.DateTime LastTradeDate { get; set; }
        /// <summary>
        /// The time value of the data
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual System.DateTime LastTradeTime { get; set; }

        public QuotesBaseData() {
            mID = string.Empty;
            this.Name = string.Empty;
            this.LastTradePriceOnly = 0;
            this.Change = 0;
            this.Volume = 0;
            this.LastTradeDate = new DateTime();
            this.LastTradeTime = new DateTime();
        }

    }
}
