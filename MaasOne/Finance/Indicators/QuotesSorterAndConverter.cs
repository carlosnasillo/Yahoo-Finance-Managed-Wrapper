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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;


namespace MaasOne.Finance.Indicators
{


    internal class QuotesSorter : IComparer<KeyValuePair<System.DateTime, double>>
    {

        public int Compare(KeyValuePair<System.DateTime, double> x, KeyValuePair<System.DateTime, double> y)
        {
            if (x.Key > y.Key)
            {
                return 1;
            }
            else if (x.Key < y.Key)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }


    internal class HistQuotesSorter : IComparer<HistQuotesData>
    {

        public int Compare(HistQuotesData x, HistQuotesData y)
        {
            if (x.TradingDate > y.TradingDate)
            {
                return 1;
            }
            else if (x.TradingDate < y.TradingDate)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }



    public abstract class QuotesConverter
    {

        public static Dictionary<System.DateTime, double> ConvertHistQuotesToSingleValues(IEnumerable<HistQuotesData> quotes)
        {
            Dictionary<System.DateTime, double> dict = new Dictionary<System.DateTime, double>();
            foreach (HistQuotesData hq in quotes)
            {
                dict.Add(hq.TradingDate, hq.Close);
            }
            return dict;
        }

        private QuotesConverter() { }
    }


}
