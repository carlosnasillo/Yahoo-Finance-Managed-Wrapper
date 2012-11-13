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
using MaasOne.Xml;
using System.Xml.Linq;


namespace MaasOne.Finance.YahooFinance.Support
{
    /// <summary>
    /// Stores default world market informations
    /// </summary>
    /// <remarks></remarks>
    public class WorldMarket
    {
        private static CurrencyInfo[] mCurrencies;
        private static CountryInfo[] mCountries;

        private static List<StockExchange> mStockExchanges = new List<StockExchange>();
        /// <summary>
        /// All default currencies.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CurrencyInfo[] DefaultCurrencies
        {
            get { return mCurrencies; }
        }
        /// <summary>
        /// All default countries.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CountryInfo[] DefaultCountries
        {
            get { return mCountries; }
        }

        /// <summary>
        /// The default stock exchanges. Is a reference for getting informations by setting the id of StockExchange or YahooID.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<StockExchange> DefaultStockExchanges
        {
            get { return mStockExchanges; }
        }

        /// <summary>
        /// A list of all available indices
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static YIndexID[] AllFinanceIndices
        {
            get
            {
                List<Support.YIndexID> lst = new List<Support.YIndexID>();
                foreach (CountryInfo cnt in DefaultCountries)
                {
                    lst.AddRange(cnt.Indices);
                }
                return lst.ToArray();
            }
        }


        static WorldMarket()
        {
            mCurrencies = GetDefaultCurrencies();
            mCountries = GetDefaultCountries();
            mStockExchanges.AddRange(GetDefaultStockExchanges());
            FillCountriesWithIndices();
        }
        

        /// <summary>
        /// Loads a list of default currencies from market.xml
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CurrencyInfo[] GetDefaultCurrencies()
        {
            List<CurrencyInfo> currencies = new List<CurrencyInfo>();
            XDocument xmlDoc = MyHelper.ParseXmlDocument(Properties.Resources.market);            
            XElement[] curs = XPath.GetElements("//Resources/Currencies/Currency", xmlDoc);
            foreach (XElement curNode in curs)
            {
                currencies.Add(new CurrencyInfo(MyHelper.GetXmlAttributeValue(curNode, "ID"), MyHelper.GetXmlAttributeValue(curNode, "Name")));
            }
            return currencies.ToArray();
        }
        /// <summary>
        /// Loads as list of default countries from market.xml
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static CountryInfo[] GetDefaultCountries()
        {
            List<CountryInfo> countries = new List<CountryInfo>();

            XDocument xmlDoc = MyHelper.ParseXmlDocument(Properties.Resources.market);
            XElement[] cntNodes = XPath.GetElements("//Resources/Countries/Country", xmlDoc);
            System.Globalization.CultureInfo convCulture = new System.Globalization.CultureInfo("en-US");

            foreach (XElement cntNode in cntNodes)
            {
                for (Country cnt = 0; cnt <= Country.VN; cnt++)
                {
                    if (cnt.ToString() == MyHelper.GetXmlAttributeValue(cntNode, "ID"))
                    {
                        CurrencyInfo cntCur = null;
                        string curID = MyHelper.GetXmlAttributeValue(cntNode, "Currency");
                        foreach (CurrencyInfo cur in DefaultCurrencies)
                        {
                            if (cur.ID.ToString() == curID)
                            {
                                cntCur = cur;
                                break;
                            }
                        }

                        XElement dstNodes = XPath.GetElement("DaylightSavingTimes", cntNode);
                        List<DaylightSavingTime> dstList = new List<DaylightSavingTime>();
                        foreach (XElement dstNode in dstNodes.Elements())
                        {
                            if (dstNode.Name.LocalName == "DST")
                            {
                                DateTime dstStart = Convert.ToDateTime(MyHelper.GetXmlAttributeValue(dstNode, "Start"), convCulture);
                                DateTime dstEnd = Convert.ToDateTime(MyHelper.GetXmlAttributeValue(dstNode, "End"), convCulture);
                                dstList.Add(new DaylightSavingTime(dstStart, dstEnd));
                            }
                        }
                        countries.Add(new CountryInfo(cnt, MyHelper.GetXmlAttributeValue(cntNode, "Name"), cntCur, dstList.ToArray()));
                        break;
                    }
                }
            }
            return countries.ToArray();
        }
        /// <summary>
        /// Loads a list of default stock exchanges from market.xml
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static StockExchange[] GetDefaultStockExchanges()
        {
            List<StockExchange> lst = new List<StockExchange>();

            XDocument xmlDoc = MyHelper.ParseXmlDocument(Properties.Resources.market);
            XElement[] exchanges = XPath.GetElements("//Resources/StockExchanges/StockExchange", xmlDoc);

            foreach (XElement exchangeNode in exchanges)
            {
                string seID = MyHelper.GetXmlAttributeValue(exchangeNode, "ID");
                string seSuffix = MyHelper.GetXmlAttributeValue(exchangeNode, "Suffix");
                string seName = MyHelper.GetXmlAttributeValue(exchangeNode, "Name");

                CountryInfo seCountry = null;
                string ctrID = MyHelper.GetXmlAttributeValue(exchangeNode, "Country");
                foreach (CountryInfo ctr in DefaultCountries)
                {
                    if (ctr.ID.ToString() == ctrID)
                    {
                        seCountry = ctr;
                        break;
                    }
                }

                //TradingTimeInfo
                int seDelayMinutes = Convert.ToInt32(MyHelper.GetXmlAttributeValue(exchangeNode, "DelayMinutes"));
                int seRelativeToUTC = Convert.ToInt32(MyHelper.GetXmlAttributeValue(exchangeNode, "UtcOffsetStandardTime"));
                DateTime seOpeningTimeLocal = Convert.ToDateTime(MyHelper.GetXmlAttributeValue(exchangeNode, "OpeningTimeLocal"));
                DateTime seClosingTimeLocal = Convert.ToDateTime(MyHelper.GetXmlAttributeValue(exchangeNode, "ClosingTimeLocal"));
                TimeSpan seTradingSpan = seClosingTimeLocal - seOpeningTimeLocal;

                List<DayOfWeek> seTradingDaysList = new List<DayOfWeek>();
                string trdDays = MyHelper.GetXmlAttributeValue(exchangeNode, "TradingDays");
                foreach (string day in trdDays.Split(','))
                {
                    switch (day)
                    {
                        case "Mo":
                            seTradingDaysList.Add(DayOfWeek.Monday);
                            break;
                        case "Tu":
                            seTradingDaysList.Add(DayOfWeek.Tuesday);
                            break;
                        case "We":
                            seTradingDaysList.Add(DayOfWeek.Wednesday);
                            break;
                        case "Th":
                            seTradingDaysList.Add(DayOfWeek.Thursday);
                            break;
                        case "Fr":
                            seTradingDaysList.Add(DayOfWeek.Friday);
                            break;
                        case "Sa":
                            seTradingDaysList.Add(DayOfWeek.Saturday);
                            break;
                        case "Su":
                            seTradingDaysList.Add(DayOfWeek.Sunday);
                            break;
                    }
                }

                DaylightSavingTime[] seDaylightSavingTimes = null;
                if (seCountry != null)
                    seDaylightSavingTimes = seCountry.DaylightSavingTimes;

                TradingTimeInfo seTradingTimeInfo = new TradingTimeInfo(seDelayMinutes, seTradingDaysList.ToArray(), null, seOpeningTimeLocal, seTradingSpan, seRelativeToUTC, seDaylightSavingTimes);

                StockExchange se = new StockExchange(seID, seSuffix, seName, seCountry, seTradingTimeInfo);
                string s = MyHelper.GetXmlAttributeValue(exchangeNode, "Tags");
                if (s != string.Empty) se.Tags.AddRange(s.Split(','));
                lst.Add(se);
            }

            return lst.ToArray();
        }
        /// <summary>
        /// Loads default market information from market.xml
        /// </summary>
        /// <remarks></remarks>

        public static void FillCountriesWithIndices()
        {
            XDocument xmlDoc = MyHelper.ParseXmlDocument(Properties.Resources.market);

            XElement[] countryNodes = XPath.GetElements("//Resources/Countries/Country", xmlDoc);

            foreach (XElement countryNode in countryNodes)
            {
                CountryInfo ctr = null;

                string ctrIDStr = MyHelper.GetXmlAttributeValue(countryNode, "ID");
                foreach (CountryInfo defaultCtr in DefaultCountries)
                {
                    if (defaultCtr.ID.ToString() == ctrIDStr)
                    {
                        ctr = defaultCtr;
                        break;
                    }
                }

                if (ctr != null)
                {
                    ctr.Indices.Clear();
                    XElement indicesNode = XPath.GetElement("Indices", countryNode);
                    foreach (XElement indexNode in indicesNode.Elements())
                    {
                        if (indexNode.Name.LocalName == "Index")
                        {
                            string name = MyHelper.GetXmlAttributeValue(indexNode, "Name");
                            string id = MyHelper.GetXmlAttributeValue(indexNode, "ID");

                            string seStr = MyHelper.GetXmlAttributeValue(indexNode, "StockExchange");
                            StockExchange se = null;
                            foreach (StockExchange defaultExc in mStockExchanges)
                            {
                                if (defaultExc.ID == seStr)
                                {
                                    se = defaultExc;
                                    break;
                                }
                            }

                            ctr.Indices.Add(new YIndexID(id)
                            {
                                Name = name,
                                StockExchange = se
                            });
                        }
                    }
                }

            }

        }



        public static CurrencyInfo GetDefaultCurrencyByID(string id)
        {
            foreach (CurrencyInfo cur in DefaultCurrencies)
            {
                if (cur.ID == id)
                {
                    return cur;
                }
            }
            return null;
        }



        public static CountryInfo GetDefaultCountry(Country cnt)
        {
            foreach (CountryInfo defaultCnt in DefaultCountries)
            {
                if (defaultCnt.ID == cnt)
                {
                    return defaultCnt;
                }
            }
            return null;
        }

        /// <summary>
        ///  Tries to return a StockExchange by StockExchange ID string.
        /// </summary>
        /// <param name="id">The non-case sensitive ID of the stock exchange. E.g. "DJI" or "gEr"</param>
        /// <returns>The confirmed stock exchange or null.</returns>
        /// <remarks></remarks>
        public static StockExchange GetStockExchangeByID(string id)
        {
            if (WorldMarket.DefaultStockExchanges != null & id != string.Empty)
            {
                string n = id.ToUpper();
                foreach (StockExchange se in WorldMarket.DefaultStockExchanges)
                {
                    if (se.ID == id)
                    {
                        return se;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Tries to return a StockExchange by an YID String.
        /// </summary>
        /// <param name="suffix">The non-case sensitive ID or Suffix of ID. E.g. "BAS.DE" or ".DE"</param>
        /// <returns>The confirmed stock exchange or null.</returns>
        /// <remarks></remarks>
        public static StockExchange GetStockExchangeBySuffix(string suffix)
        {
            if (WorldMarket.DefaultStockExchanges != null & suffix != string.Empty)
            {
                int index = suffix.LastIndexOf('.');
                if (index >= 0 & suffix.Length >= index)
                {
                    string suffStr = suffix.Substring(index, suffix.Length - index).ToUpper();
                    foreach (StockExchange se in WorldMarket.DefaultStockExchanges)
                    {
                        if (se.Suffix == suffStr)
                            return se;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Tries to return a StockExchange by the StockExchange's name
        /// </summary>
        /// <param name="name">A non-case sensitive name or part of name of a stock exchange. The first name that contains the string will be returned.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static StockExchange GetStockExchangeByName(string name)
        {
            if (WorldMarket.DefaultStockExchanges != null & name != string.Empty)
            {
                string n = name.ToLower();
                foreach (StockExchange se in WorldMarket.DefaultStockExchanges)
                {
                    if (se.Name.ToLower().IndexOf(n) > -1)
                    {
                        return se;
                    }
                    else
                    {
                        foreach (string tag in se.Tags)
                        {
                            if (n == tag.ToLower())
                                return se;
                        }
                    }
                }
            }
            return null;
        }


    }
}
