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


    public abstract class HistQuotesBaseDownloadSettings<T> : Base.SettingsBase where T : HistQuotesBaseResult
    {
        public virtual string ID { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime ToDate { get; set; }
    }



    public abstract class HistQuotesBaseResult
    {
        private HistQuotesDataChain mItems = null;
        public HistQuotesDataChain Items { get { return mItems; } }

        public HistQuotesBaseResult(HistQuotesDataChain items)
        {
            mItems = items;
        }
    }


    public class HistQuotesDataChain : List<HistQuotesData>, ISettableID
    {

        private string mID = string.Empty;
        /// <summary>
        /// The ID of the historic quotes owning stock, index, etc.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ID
        {
            get { return mID; }
        }
        public void SetID(string id)
        {
            mID = id;
        }

        public HistQuotesDataChain()
            : base()
        {
        }
        public HistQuotesDataChain(string id)
            : base()
        {
            mID = id;
        }
        public HistQuotesDataChain(IEnumerable<HistQuotesData> items)
            : base()
        {
            foreach (HistQuotesData i in items)
            {
                base.Add(i);
            }
            base.Sort(new HistQuotesSorter());
            if (base.Count > 0)
            {
                for (int i = 0; i <= base.Count - 1; i++)
                {
                    if (i > 0)
                    {
                        base[i].PreviousClose = base[i - 1].Close;
                    }
                }
            }
        }
        public HistQuotesDataChain(string id, IEnumerable<HistQuotesData> items)
            : this(items)
        {
            mID = id;
        }

        private class HistQuotesSorter : IComparer<HistQuotesData>
        {
            public int Compare(HistQuotesData x, HistQuotesData y)
            {
                return DateTime.Compare(x.TradingDate, y.TradingDate);
            }
        }

    }


    /// <summary>
    /// Stores informations about one historic trading period (day, week or month).
    /// </summary>
    /// <remarks></remarks>
    public class HistQuotesData
    {
        /// <summary>
        /// The startdate of the period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.DateTime TradingDate { get; set; }
        /// <summary>
        /// The first value in trading period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Open { get; set; }
        /// <summary>
        /// The highest value in trading period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double High { get; set; }
        /// <summary>
        /// The lowest value in trading period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Low { get; set; }
        /// <summary>
        /// The last value in trading period.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Close { get; set; }
        /// <summary>
        /// The last value in trading period in relation to share splits.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double CloseAdjusted { get; set; }
        /// <summary>
        /// The traded volume.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public long Volume { get; set; }
        /// <summary>
        /// The close value of the previous HistQuoteData in chain.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double PreviousClose { get; set; }
    }




}
