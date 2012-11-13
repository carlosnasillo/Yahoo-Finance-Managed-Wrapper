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




    /// <summary>
    /// Provides the available sizes for chart images.
    /// </summary>
    /// <remarks></remarks>
    public enum ChartImageSize
    {
        /// <summary>
        /// Large images have a size of 800 x 355 px.
        /// </summary>
        /// <remarks></remarks>
        Large,
        /// <summary>
        /// Middle images have a size of 512 x 288 px.
        /// </summary>
        /// <remarks></remarks>
        Middle,
        /// <summary>
        /// Small images have a size of 60 x 16 px.
        /// </summary>
        /// <remarks></remarks>
        Small
    }

    /// <summary>
    /// Provides the available scales for chart images.
    /// </summary>
    /// <remarks>
    /// Arithmetic: If the scale provides values from 0 to 100: Value 25 is at 1/4 of 'world'/visible values; 50 is at 1/2; 75 is at 3/4. ///\\\ Logarithmic: If the scale provides values from 0 to 100: Value 10 is at 50/100 of 'world'/visible values; 25 is at 69/100; 50 is at 85/100. The sense of this scale is to relate the absolute changings of a chart. If a stock has a price of $10 and jumps to $20. It's a rise of $10 or 100%. If a stock price is $100 and is increasing of $10 the rise is just 10%.
    /// </remarks>
    public enum ChartScale
    {
        /// <summary>
        /// Arithmetic scale has the same proportion in y-axis to the esed values.
        /// </summary>
        /// <remarks></remarks>
        Arithmetic,
        /// <summary>
        /// Logarithmic scale has for same value differences shorter going differences in y-axis lines (based on logarithmic calculation). 
        /// </summary>
        /// <remarks></remarks>
        Logarithmic
    }

    /// <summary>
    /// Provides the time spans for showed data base.
    /// </summary>
    /// <remarks></remarks>
    public enum ChartTimeSpan
    {
        /// <summary>
        /// 1 Day
        /// </summary>
        /// <remarks></remarks>
        c1D,
        /// <summary>
        /// 5 Days
        /// </summary>
        /// <remarks></remarks>
        c5D,
        /// <summary>
        /// 3 Months
        /// </summary>
        /// <remarks></remarks>
        c3M,
        /// <summary>
        /// 6 Months
        /// </summary>
        /// <remarks></remarks>
        c6M,
        /// <summary>
        /// 1 Year
        /// </summary>
        /// <remarks></remarks>
        c1Y,
        /// <summary>
        /// 2 Years
        /// </summary>
        /// <remarks></remarks>
        c2Y,
        /// <summary>
        /// 5 Years
        /// </summary>
        /// <remarks></remarks>
        c5Y,
        /// <summary>
        /// Maximum
        /// </summary>
        /// <remarks></remarks>
        cMax
    }

    /// <summary>
    /// Provides the chart type of the chart image.
    /// </summary>
    /// <remarks></remarks>
    public enum ChartType
    {
        /// <summary>
        /// Line
        /// </summary>
        /// <remarks></remarks>
        Line,
        /// <summary>
        /// er
        /// </summary>
        /// <remarks></remarks>
        Bar,
        /// <summary>
        /// Candle
        /// </summary>
        /// <remarks></remarks>
        Candle
    }

    /// <summary>
    /// Provides the financial type of the security in general like "Stock" or "Index".
    /// </summary>
    /// <remarks></remarks>
    public enum SecurityType
    {
        /// <summary>
        /// All
        /// </summary>
        /// <remarks></remarks>
        Any,
        /// <summary>
        /// Stock
        /// </summary>
        /// <remarks></remarks>
        Stock,
        /// <summary>
        /// Fund
        /// </summary>
        /// <remarks></remarks>
        Fund,
        /// <summary>
        /// Exchange Traded Fund
        /// </summary>
        /// <remarks></remarks>
        ETF,
        /// <summary>
        /// Index
        /// </summary>
        /// <remarks></remarks>
        Index,
        /// <summary>
        /// Future
        /// </summary>
        /// <remarks></remarks>
        Future,
        /// <summary>
        /// Warrant
        /// </summary>
        /// <remarks></remarks>
        Warrant,
        /// <summary>
        /// Currency
        /// </summary>
        /// <remarks></remarks>
        Currency
    }

    /// <summary>
    /// Provides the markets for limitation at ID search.
    /// </summary>
    /// <remarks></remarks>
    public enum FinancialMarket
    {
        AllMarkets,
        France,
        Germany,
        Spain,
        UK,
        UsAndCanada
    }

    /// <summary>
    /// Provides the available sortable properties at ID search.
    /// </summary>
    /// <remarks></remarks>
    public enum SecurityRankProperty
    {
        /// <summary>
        /// No special property
        /// </summary>
        /// <remarks></remarks>
        NoRanking,
        /// <summary>
        /// ID
        /// </summary>
        /// <remarks></remarks>
        ID,
        /// <summary>
        /// Name
        /// </summary>
        /// <remarks></remarks>
        Name,
        /// <summary>
        /// Category
        /// </summary>
        /// <remarks></remarks>
        Type,
        /// <summary>
        /// Exchange
        /// </summary>
        /// <remarks></remarks>
        Exchange
    }

    /// <summary>
    /// Provides the available intervals between received HistQuoteData items.
    /// </summary>
    /// <remarks>Daily Historical Quotes provide you with the daily open, high, low, close, and volume for each trading day in the chosen date range. Weekly Historical Quotes retrieve the open trade from the first trading day for the week, the high and low price quotes of the week, and the closing price on the last trading day of the week. The weekly volume is the average daily volume for all trading days in the reported week. Monthly Historical Quotes report the open trade from the first trading day of the month, the high and low price quotes for the month, and the closing price on the last trading day of the month. The monthly volume is the average daily volume for all trading days in the reported month.</remarks>
    public enum HistQuotesInterval
    {
        /// <summary>
        /// Daily
        /// </summary>
        /// <remarks></remarks>
        Daily,
        /// <summary>
        /// Weekly
        /// </summary>
        /// <remarks></remarks>
        Weekly,
        /// <summary>
        /// Monthly
        /// </summary>
        /// <remarks></remarks>
        Monthly
    }

    /// <summary>
    /// Provides every available property of market quote data.
    /// </summary>
    /// <remarks></remarks>
    public enum MarketQuoteProperty
    {
        /// <summary>
        /// Dividend Yield Percent
        /// </summary>
        /// <remarks></remarks>
        DividendYieldPercent,
        /// <summary>
        /// Long Term Dept To Equity
        /// </summary>
        /// <remarks></remarks>
        LongTermDeptToEquity,
        /// <summary>
        /// Market Capitalization In Million
        /// </summary>
        /// <remarks></remarks>
        MarketCapitalizationInMillion,
        /// <summary>
        /// Name
        /// </summary>
        /// <remarks></remarks>
        Name,
        /// <summary>
        /// Net Profit Margin in Percent
        /// </summary>
        /// <remarks></remarks>
        NetProfitMarginPercent,
        /// <summary>
        /// One Day Price Change Percent
        /// </summary>
        /// <remarks></remarks>
        OneDayPriceChangePercent,
        /// <summary>
        /// Price Earnings Ratio
        /// </summary>
        /// <remarks></remarks>
        PriceEarningsRatio,
        /// <summary>
        /// Price To Book Value
        /// </summary>
        /// <remarks></remarks>
        PriceToBookValue,
        /// <summary>
        /// Price To Free Cash Flow
        /// </summary>
        /// <remarks></remarks>
        PriceToFreeCashFlow,
        /// <summary>
        /// Return On Equity Percent
        /// </summary>
        /// <remarks></remarks>
        ReturnOnEquityPercent
    }

    /// <summary>
    /// The time span for the value ese of a calculated moving average. A bigger time span results in a more straightened line with less reaction to short term changings.
    /// </summary>
    /// <remarks></remarks>
    public enum MovingAverageInterval
    {
        /// <summary>
        /// 5
        /// </summary>
        /// <remarks></remarks>
        m5,
        /// <summary>
        /// 10
        /// </summary>
        /// <remarks></remarks>
        m10,
        /// <summary>
        /// 20
        /// </summary>
        /// <remarks></remarks>
        m20,
        /// <summary>
        /// 50
        /// </summary>
        /// <remarks></remarks>
        m50,
        /// <summary>
        /// 100
        /// </summary>
        /// <remarks></remarks>
        m100,
        /// <summary>
        /// 200
        /// </summary>
        /// <remarks></remarks>
        m200
    }

    /// <summary>
    /// Provides the two financial option type 'Call' and 'Put'.
    /// </summary>
    /// <remarks></remarks>
    public enum QuoteOptionType
    {
        /// <summary>
        /// Call
        /// </summary>
        /// <remarks></remarks>
        Call,
        /// <summary>
        /// Put
        /// </summary>
        /// <remarks></remarks>
        Put
    }

    /// <summary>
    /// Provides every available property of quote data.
    /// </summary>
    /// <remarks></remarks>
    public enum QuoteProperty
    {
        /// <summary>
        /// AfterHoursChangeRealtime
        /// </summary>
        /// <remarks></remarks>
        AfterHoursChangeRealtime,

        /// <summary>
        /// Annualized Gain
        /// </summary>
        /// <remarks></remarks>
        AnnualizedGain,

        /// <summary>
        /// Ask Size
        /// </summary>
        /// <remarks></remarks>
        Ask,

        /// <summary>
        /// Ask (Realtime)
        /// </summary>
        /// <remarks></remarks>
        AskRealtime,

        /// <summary>
        /// Ask Size
        /// </summary>
        /// <remarks></remarks>
        AskSize,

        /// <summary>
        /// Average Daily Volume
        /// </summary>
        /// <remarks></remarks>
        AverageDailyVolume,

        /// <summary>
        /// Bid Size
        /// </summary>
        /// <remarks></remarks>
        Bid,

        /// <summary>
        /// Bid (Realtime)
        /// </summary>
        /// <remarks></remarks>
        BidRealtime,

        /// <summary>
        /// Bid Size
        /// </summary>
        /// <remarks></remarks>
        BidSize,

        /// <summary>
        /// Book Value
        /// </summary>
        /// <remarks></remarks>
        BookValuePerShare,

        /// <summary>
        /// Change
        /// </summary>
        /// <remarks></remarks>
        Change,

        /// <summary>
        /// Change Percent
        /// </summary>
        /// <remarks></remarks>
        Change_ChangeInPercent,

        /// <summary>
        /// Change From 50 Days Moving Average
        /// </summary>
        /// <remarks></remarks>
        ChangeFromFiftydayMovingAverage,

        /// <summary>
        /// Change From 200 Days Moving Average
        /// </summary>
        /// <remarks></remarks>
        ChangeFromTwoHundreddayMovingAverage,

        /// <summary>
        /// Change From 1 Year High
        /// </summary>
        /// <remarks></remarks>
        ChangeFromYearHigh,

        /// <summary>
        /// Change From 1 Year Low
        /// </summary>
        /// <remarks></remarks>
        ChangeFromYearLow,

        /// <summary>
        /// Change In Percent
        /// </summary>
        /// <remarks></remarks>
        ChangeInPercent,

        /// <summary>
        /// Change Percent (Realtime)
        /// </summary>
        /// <remarks></remarks>
        ChangeInPercentRealtime,

        /// <summary>
        /// Days Value Change (Realtime)
        /// </summary>
        /// <remarks></remarks>
        ChangeRealtime,

        /// <summary>
        /// Commission
        /// </summary>
        /// <remarks></remarks>
        Commission,

        /// <summary>
        /// Currency
        /// </summary>
        /// <remarks></remarks>
        Currency,

        /// <summary>
        /// Days High
        /// </summary>
        /// <remarks></remarks>
        DaysHigh,

        /// <summary>
        /// Days Low
        /// </summary>
        /// <remarks></remarks>
        DaysLow,

        /// <summary>
        /// Days Range (Realtime)
        /// </summary>
        /// <remarks></remarks>
        DaysRange,

        /// <summary>
        /// Days Range (Realtime)
        /// </summary>
        /// <remarks></remarks>
        DaysRangeRealtime,

        /// <summary>
        /// Days Value Change
        /// </summary>
        /// <remarks></remarks>
        DaysValueChange,

        /// <summary>
        /// Days Value Change (Realtime)
        /// </summary>
        /// <remarks></remarks>
        DaysValueChangeRealtime,

        /// <summary>
        /// Dividend Pay Date
        /// </summary>
        /// <remarks></remarks>
        DividendPayDate,

        /// <summary>
        /// Dividend Share
        /// </summary>
        /// <remarks></remarks>
        TrailingAnnualDividendYield,

        /// <summary>
        /// Dividend Yield
        /// </summary>
        /// <remarks></remarks>
        TrailingAnnualDividendYieldInPercent,

        /// <summary>
        /// Earnings Share
        /// </summary>
        /// <remarks></remarks>
        DilutedEPS,

        /// <summary>
        /// EBITDA
        /// </summary>
        /// <remarks></remarks>
        EBITDA,

        /// <summary>
        /// Price EPS Estimate Current Year
        /// </summary>
        /// <remarks></remarks>
        EPSEstimateCurrentYear,

        /// <summary>
        /// EPS Estimate Next Quarter
        /// </summary>
        /// <remarks></remarks>
        EPSEstimateNextQuarter,

        /// <summary>
        /// Price EPS Estimate Next Year
        /// </summary>
        /// <remarks></remarks>
        EPSEstimateNextYear,

        /// <summary>
        /// Ex Dividend Date
        /// </summary>
        /// <remarks></remarks>
        ExDividendDate,

        /// <summary>
        /// 50 Days Moving Average
        /// </summary>
        /// <remarks></remarks>
        FiftydayMovingAverage,

        /// <summary>
        /// Float Shares
        /// </summary>
        /// <remarks></remarks>
        SharesFloat,

        /// <summary>
        /// High Limit
        /// </summary>
        /// <remarks></remarks>
        HighLimit,

        /// <summary>
        /// Holdings Gain
        /// </summary>
        /// <remarks></remarks>
        HoldingsGain,

        /// <summary>
        /// Holdings Gain Percent
        /// </summary>
        /// <remarks></remarks>
        HoldingsGainPercent,

        /// <summary>
        /// Holdings Gain Percent (Realtime)
        /// </summary>
        /// <remarks></remarks>
        HoldingsGainPercentRealtime,

        /// <summary>
        /// Holdings Gain (Realtime)
        /// </summary>
        /// <remarks></remarks>
        HoldingsGainRealtime,

        /// <summary>
        /// Holdings Value (Realtime)
        /// </summary>
        /// <remarks></remarks>
        HoldingsValue,

        /// <summary>
        /// Holdings Value (Realtime)
        /// </summary>
        /// <remarks></remarks>
        HoldingsValueRealtime,

        /// <summary>
        /// Last Trade Date
        /// </summary>
        /// <remarks></remarks>
        LastTradeDate,

        /// <summary>
        /// Last Trade Price Only
        /// </summary>
        /// <remarks></remarks>
        LastTradePriceOnly,

        /// <summary>
        /// Last Trade With Time (Realtime)
        /// </summary>
        /// <remarks></remarks>
        LastTradeRealtimeWithTime,

        LastTradeSize,

        /// <summary>
        /// Last Trade Time
        /// </summary>
        /// <remarks></remarks>
        LastTradeTime,

        /// <summary>
        /// Last Trade With Time
        /// </summary>
        /// <remarks></remarks>
        LastTradeWithTime,

        /// <summary>
        /// Low Limit
        /// </summary>
        /// <remarks></remarks>
        LowLimit,

        /// <summary>
        /// Market Capitalization
        /// </summary>
        /// <remarks></remarks>
        MarketCapitalization,

        /// <summary>
        /// Market Capitalization (Realtime)
        /// </summary>
        /// <remarks></remarks>
        MarketCapRealtime,

        /// <summary>
        /// More Info
        /// </summary>
        /// <remarks></remarks>
        MoreInfo,

        /// <summary>
        /// Name
        /// </summary>
        /// <remarks></remarks>
        Name,

        /// <summary>
        /// Notes
        /// </summary>
        /// <remarks></remarks>
        Notes,

        /// <summary>
        /// 1 Year Target Price
        /// </summary>
        /// <remarks></remarks>
        OneyrTargetPrice,

        /// <summary>
        /// Open
        /// </summary>
        /// <remarks></remarks>
        Open,

        /// <summary>
        /// Order Book (Realtime)
        /// </summary>
        /// <remarks></remarks>
        OrderBookRealtime,

        /// <summary>
        /// PEG Ratio
        /// </summary>
        /// <remarks></remarks>
        PEGRatio,

        /// <summary>
        /// PE Ratio (Realtime)
        /// </summary>
        /// <remarks></remarks>
        PERatio,

        /// <summary>
        /// PE Ratio (Realtime)
        /// </summary>
        /// <remarks></remarks>
        PERatioRealtime,

        /// <summary>
        /// Percent Change From 50 Days Moving Average
        /// </summary>
        /// <remarks></remarks>
        PercentChangeFromFiftydayMovingAverage,

        /// <summary>
        /// Percent Change From 200 Days Moving Average
        /// </summary>
        /// <remarks></remarks>
        PercentChangeFromTwoHundreddayMovingAverage,

        /// <summary>
        /// Percent Change From 1 Year High
        /// </summary>
        /// <remarks></remarks>
        ChangeInPercentFromYearHigh,

        /// <summary>
        /// Percent Change From 1 Year Low
        /// </summary>
        /// <remarks></remarks>
        PercentChangeFromYearLow,

        /// <summary>
        /// Previous Close
        /// </summary>
        /// <remarks></remarks>
        PreviousClose,

        /// <summary>
        /// Price Book
        /// </summary>
        /// <remarks></remarks>
        PriceBook,

        /// <summary>
        /// Price EPS Estimate Current Year
        /// </summary>
        /// <remarks></remarks>
        PriceEPSEstimateCurrentYear,

        /// <summary>
        /// Price EPS Estimate Next Year
        /// </summary>
        /// <remarks></remarks>
        PriceEPSEstimateNextYear,

        /// <summary>
        /// Price Paid
        /// </summary>
        /// <remarks></remarks>
        PricePaid,

        /// <summary>
        /// Price Sales
        /// </summary>
        /// <remarks></remarks>
        PriceSales,

        /// <summary>
        /// Revenue (ttm)
        /// </summary>
        /// <remarks></remarks>
        Revenue,

        /// <summary>
        /// Shares Owned
        /// </summary>
        /// <remarks></remarks>
        SharesOwned,

        /// <summary>
        /// Shares Outstanding
        /// </summary>
        /// <remarks></remarks>
        SharesOutstanding,

        /// <summary>
        /// Short Ratio
        /// </summary>
        /// <remarks></remarks>
        ShortRatio,

        /// <summary>
        /// Stock Exchange
        /// </summary>
        /// <remarks></remarks>
        StockExchange,

        /// <summary>
        /// Symbol
        /// </summary>
        /// <remarks></remarks>
        Symbol,

        /// <summary>
        /// Ticker Trend
        /// </summary>
        /// <remarks></remarks>
        TickerTrend,

        /// <summary>
        /// Trade Date
        /// </summary>
        /// <remarks></remarks>
        TradeDate,

        /// <summary>
        /// Trade Links
        /// </summary>
        /// <remarks></remarks>
        TradeLinks,

        /// <summary>
        /// Additional Html for Trade Links
        /// </summary>
        /// <remarks></remarks>
        TradeLinksAdditional,

        /// <summary>
        /// 200 Days Moving Average
        /// </summary>
        /// <remarks></remarks>
        TwoHundreddayMovingAverage,

        /// <summary>
        /// Volume td
        /// </summary>
        /// <remarks></remarks>
        Volume,

        /// <summary>
        /// 1 Year High
        /// </summary>
        /// <remarks></remarks>
        YearHigh,

        /// <summary>
        /// 1 Year Low
        /// </summary>
        /// <remarks></remarks>
        YearLow,

        /// <summary>
        /// 1 Year Range
        /// </summary>
        /// <remarks></remarks>
        YearRange
    }

    /// <summary>
    /// Provides the main US economic sectors.
    /// </summary>
    /// <remarks></remarks>
    public enum Sector
    {
        /// <summary>
        /// Basic Materials
        /// </summary>
        /// <remarks></remarks>
        Basic_Materials = 1,
        /// <summary>
        /// Conglomerates
        /// </summary>
        /// <remarks></remarks>
        Conglomerates = 2,
        /// <summary>
        /// Consumer Goods
        /// </summary>
        /// <remarks></remarks>
        Consumer_Goods = 3,
        /// <summary>
        /// Financial
        /// </summary>
        /// <remarks></remarks>
        Financial = 4,
        /// <summary>
        /// Healthcare
        /// </summary>
        /// <remarks></remarks>
        Healthcare = 5,
        /// <summary>
        /// Industrial Goods
        /// </summary>
        /// <remarks></remarks>
        Industrial_Goods = 6,
        /// <summary>
        /// Services
        /// </summary>
        /// <remarks></remarks>
        Services = 7,
        /// <summary>
        /// Technology
        /// </summary>
        /// <remarks></remarks>
        Technology = 8,
        /// <summary>
        /// Utilities
        /// </summary>
        /// <remarks></remarks>
        Utilities = 9
    }

    /// <summary>
    /// Provides the main US industries.
    /// </summary>
    /// <remarks></remarks>
    public enum Industry
    {
        Chemicals_Major_Diversified = 110,
        Synthetics = 111,
        Agricultural_Chemicals = 112,
        Specialty_Chemicals = 113,
        Major_Integrated_Oil_And_Gas = 120,
        Independent_Oil_And_Gas = 121,
        Oil_And_Gas_Refining_And_Marketing = 122,
        Oil_And_Gas_Drilling_And_Exploration = 123,
        Oil_And_Gas_Equipment_And_Services = 124,
        Oil_And_Gas_Pipelines = 125,
        Steel_And_Iron = 130,
        Copper = 131,
        Aluminum = 132,
        Industrial_Metals_And_Minerals = 133,
        Gold = 134,
        Silver = 135,
        Nonmetallic_Mineral_Mining = 136,
        Conglomerates = 210,
        Appliances = 310,
        Home_Furnishings_And_Fixtures = 311,
        Housewares_And_Accessories = 312,
        Business_Equipment = 313,
        Electronic_Equipment = 314,
        Toys_And_Games = 315,
        Sporting_Goods = 316,
        Recreational_Goods_Other = 317,
        Photographic_Equipment_And_Supplies = 318,
        Textile_Apparel_Clothing = 320,
        Textile_Apparel_Footwear_And_Accessories = 321,
        Rubber_And_Plastics = 322,
        Personal_Products = 323,
        Paper_And_Paper_Products = 324,
        Packaging_And_Containers = 325,
        Cleaning_Products = 326,
        Office_Supplies = 327,
        Auto_Manufacturers_Major = 330,
        Trucks_And_Other_Vehicles = 331,
        Recreational_Vehicles = 332,
        Auto_Parts = 333,
        Food_Major_Diversified = 340,
        Farm_Products = 341,
        Processed_And_Packaged_Goods = 342,
        Meat_Products = 343,
        Dairy_Products = 344,
        Confectioners = 345,
        Beverages_Brewers = 346,
        Beverages_Wineries_And_Distillers = 347,
        Beverages_Soft_Drinks = 348,
        Cigarettes = 350,
        Tobacco_Products_Other = 351,
        Money_Center_Banks = 410,
        Regional_Northeast_Banks = 411,
        Regional_Mid_Atlantic_Banks = 412,
        Regional_Southeast_Banks = 413,
        Regional_Midwest_Banks = 414,
        Regional_Southwest_Banks = 415,
        Regional_Pacific_Banks = 416,
        Foreign_Money_Center_Banks = 417,
        Foreign_Regional_Banks = 418,
        Savings_And_Loans = 419,
        Investment_Brokerage_National = 420,
        Investment_Brokerage_Regional = 421,
        Asset_Management = 422,
        Diversified_Investments = 423,
        Credit_Services = 424,
        Closed_End_Fund_Debt = 425,
        Closed_End_Fund_Equity = 426,
        Closed_End_Fund_Foreign = 427,
        Life_Insurance = 430,
        Accident_And_Health_Insurance = 431,
        Property_And_Casualty_Insurance = 432,
        Surety_And_Title_Insurance = 433,
        Insurance_Brokers = 434,
        REIT_Diversified = 440,
        REIT_Office = 441,
        REIT_Healthcare_Facilities = 442,
        REIT_Hotel_Motel = 443,
        REIT_Industrial = 444,
        REIT_Residential = 445,
        REIT_Retail = 446,
        Mortgage_Investment = 447,
        Property_Management = 448,
        Real_Estate_Development = 449,
        Drug_Manufacturers_Major = 510,
        Drug_Manufacturers_Other = 511,
        Drugs_Generic = 512,
        Drug_Delivery = 513,
        Drug_Related_Products = 514,
        Biotechnology = 515,
        Diagnostic_Substances = 516,
        Medical_Instruments_And_Supplies = 520,
        Medical_Appliances_And_Equipment = 521,
        Health_Care_Plans = 522,
        Long_Term_Care_Facilities = 523,
        Hospitals = 524,
        Medical_Laboratories_And_Research = 525,
        Home_Health_Care = 526,
        Medical_Practitioners = 527,
        Specialized_Health_Services = 528,
        Aerospace_Defense_Major_Diversified = 610,
        Aerospace_Defense_Products_And_Services = 611,
        Farm_And_Construction_Machinery = 620,
        Industrial_Equipment_And_Components = 621,
        Diversified_Machinery = 622,
        Pollution_And_Treatment_Controls = 623,
        Machine_Tools_And_Accessories = 624,
        Small_Tools_And_Accessories = 625,
        Metal_Fabrication = 626,
        Industrial_Electrical_Equipment = 627,
        Textile_Industrial = 628,
        Residential_Construction = 630,
        Manufactured_Housing = 631,
        Lumber_Wood_Production = 632,
        Cement = 633,
        General_Building_Materials = 634,
        Heavy_Construction = 635,
        General_Contractors = 636,
        Waste_Management = 637,
        Lodging = 710,
        Resorts_And_Casinos = 711,
        Restaurants = 712,
        Specialty_Eateries = 713,
        Gaming_Activities = 714,
        Sporting_Activities = 715,
        General_Entertainment = 716,
        Advertising_Agencies = 720,
        Marketing_Services = 721,
        Entertainment_Diversified = 722,
        Broadcasting_TV = 723,
        Broadcasting_Radio = 724,
        CATV_Systems = 725,
        Movie_Production_Theaters = 726,
        Publishing_Newspapers = 727,
        Publishing_Periodicals = 728,
        Publishing_Books = 729,
        Apparel_Stores = 730,
        Department_Stores = 731,
        Discount_Variety_Stores = 732,
        Drug_Stores = 733,
        Grocery_Stores = 734,
        Electronics_Stores = 735,
        Home_Improvement_Stores = 736,
        Home_Furnishing_Stores = 737,
        Auto_Parts_Stores = 738,
        Catalog_And_Mail_Order_Houses = 739,
        Sporting_Goods_Stores = 740,
        Toy_And_Hobby_Stores = 741,
        Jewelry_Stores = 742,
        Music_And_Video_Stores = 743,
        Auto_Dealerships = 744,
        Specialty_Retail_Other = 745,
        Auto_Parts_Wholesale = 750,
        Building_Materials_Wholesale = 751,
        Industrial_Equipment_Wholesale = 752,
        Electronics_Wholesale = 753,
        Medical_Equipment_Wholesale = 754,
        Computers_Wholesale = 755,
        Drugs_Wholesale = 756,
        Food_Wholesale = 757,
        Basic_Materials_Wholesale = 758,
        Wholesale_Other = 759,
        Business_Services = 760,
        Rental_And_Leasing_Services = 761,
        Personal_Services = 762,
        Consumer_Services = 763,
        Staffing_And_Outsourcing_Services = 764,
        Security_And_Protection_Services = 765,
        Education_And_Training_Services = 766,
        Technical_Services = 767,
        Research_Services = 768,
        Management_Services = 769,
        Major_Airlines = 770,
        Regional_Airlines = 771,
        Air_Services_Other = 772,
        Air_Delivery_And_Freight_Services = 773,
        Trucking = 774,
        Shipping = 775,
        Railroads = 776,
        Diversified_Computer_Systems = 810,
        Personal_Computers = 811,
        Computer_Based_Systems = 812,
        Data_Storage_Devices = 813,
        Networking_And_Communication_Devices = 814,
        Computer_Peripherals = 815,
        Multimedia_And_Graphics_Software = 820,
        Application_Software = 821,
        Technical_And_System_Software = 822,
        Security_Software_And_Services = 823,
        Information_Technology_Services = 824,
        Healthcare_Information_Services = 825,
        Business_Software_And_Services = 826,
        Information_And_Delivery_Services = 827,
        Semiconductor_Broad_Line = 830,
        Semiconductor_Memory_Chips = 831,
        Semiconductor_Specialized = 832,
        Semiconductor_Integrated_Circuits = 833,
        Semiconductor_Equipment_And_Materials = 834,
        Printed_Circuit_Boards = 835,
        Diversified_Electronics = 836,
        Scientific_And_Technical_Instruments = 837,
        Wireless_Communications = 840,
        Communication_Equipment = 841,
        Processing_Systems_And_Products = 842,
        Long_Distance_Carriers = 843,
        Telecom_Services_Domestic = 844,
        Telecom_Services_Foreign = 845,
        Diversified_Communication_Services = 846,
        Internet_Service_Providers = 850,
        Internet_Information_Providers = 851,
        Internet_Software_And_Services = 852,
        Foreign_Utilities = 910,
        Electric_Utilities = 911,
        Gas_Utilities = 912,
        Diversified_Utilities = 913,
        Water_Utilities = 914

    }

    /// <summary>
    /// Provides different technical indicators for chart images.
    /// </summary>
    /// <remarks></remarks>
    public enum TechnicalIndicator
    {
        /// <summary>
        /// Stochastic
        /// </summary>
        /// <remarks></remarks>
        Fast_Stoch,
        /// <summary>
        /// Moving-Average-Convergence-Divergence
        /// </summary>
        /// <remarks></remarks>
        MACD,
        /// <summary>
        /// Money Flow Index
        /// </summary>
        /// <remarks></remarks>
        MFI,
        /// <summary>
        /// Rate of Change
        /// </summary>
        /// <remarks></remarks>
        ROC,
        /// <summary>
        /// Relative Strength Index
        /// </summary>
        /// <remarks></remarks>
        RSI,
        /// <summary>
        /// Slow Stochastic
        /// </summary>
        /// <remarks></remarks>
        Slow_Stoch,
        /// <summary>
        /// Volume
        /// </summary>
        /// <remarks></remarks>
        Vol,
        /// <summary>
        /// Volume with Moving Average
        /// </summary>
        /// <remarks></remarks>
        Vol_MA,
        /// <summary>
        /// Williams Percent Range
        /// </summary>
        /// <remarks></remarks>
        W_R,
        /// <summary>
        /// Bollinger ends
        /// </summary>
        /// <remarks></remarks>
        Bollinger_Bands,
        /// <summary>
        /// Parabolic Stop And Reverse
        /// </summary>
        /// <remarks></remarks>
        Parabolic_SAR,
        /// <summary>
        /// Splits
        /// </summary>
        /// <remarks></remarks>
        Splits,
        /// <summary>
        /// Volume (inside chart)
        /// </summary>
        /// <remarks></remarks>
        Volume
    }
}
