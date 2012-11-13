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


namespace MaasOne.Finance.YahooScreener
{
    /// <summary>
    /// Additional quote properties
    /// </summary>
    /// <remarks></remarks>
    public enum StockScreenerProperty
    {
        Beta,
        CashPerShare,
        CurrentRatio,
        EarningsGrowthEstimate_Next5Years,
        EarningsGrowthEstimate_NextYear,
        EarningsGrowthEstimate_ThisYear,
        EarningsGrowth_Past5Years,
        EBITDAMargin_ttm,
        EntityValue,
        EntityValueToRevenueRatio,
        EntityValueToOperatingCashFlowRatio,
        EntityValueToFreeCashFlowRatio,
        EPS_NYCE,
        EPS_mrq,
        ForwardPriceToEarningsRatio,
        FreeCashFlow,
        Gap,
        GapInPercent,
        GrossMargin_ttm,
        GrossProfit,
        HeldByInsiders,
        HeldByInstitutions,
        LongTermDebtToEquityRatio,
        NetIncome,
        NumberOfEmployees,
        OperatingCashFlow,
        OperatingIncome,
        OperatingMargin,
        PriceEarningsRatio,
        ProfitMargin_ttm,
        QuickRatio,
        ReturnOnAssets,
        ReturnOnEquity,
        RevenueEstimate_NextQuarter,
        RevenueEstimate_ThisQuarter,
        RevenueEstimate_ThisYear,
        Sales_ttm,
        SalesGrowthEstimate_NextQuarter,
        SalesGrowthEstimate_NextYear,
        SalesGrowthEstimate_ThisQuarter,
        SalesGrowthEstimate_ThisYear,
        SharesOutstanding,
        SharesShort,
        SharesShortPriorMonth,
        TotalCash,
        TotalDebt,
        TotalDebtToEquityRatio
    }

    /// <summary>
    /// Criteria groups for Stock Screener
    /// </summary>
    /// <remarks></remarks>
    public enum StockScreenerCriteriaGroup
    {
        Descriptive,
        SharePerformance,
        TradingAndVolume,
        Valuation,
        AnalystEstimates,
        Ownership,
        Dividends,
        Margins,
        BalanceSheet,
        IncomeStatements,
        Profitability,
        Growth,
        CashFlow
    }



    /// <summary>
    /// Available stock exchanges for Stock Screener
    /// </summary>
    /// <remarks></remarks>
    public enum StockExchange
    {
        AMEX,
        NASDAQ,
        NYSE
    }

    /// <summary>
    /// General stock value development directions
    /// </summary>
    /// <remarks></remarks>
    public enum StockPriceChangeDirection
    {
        Gain = 0,
        Loss = 1
    }

    /// <summary>
    /// Time points for comparing stock values with latest value
    /// </summary>
    /// <remarks></remarks>
    public enum StockTradingAbsoluteTimePoint
    {
        TodaysOpen = 0,
        PreviousClose = 1
    }

    /// <summary>
    /// Relative time points for begin or end of used time span
    /// </summary>
    /// <remarks></remarks>
    public enum StockTradingRelativeTimePoint
    {
        BeforeLastTradeTimePoint = 0,
        AfterMarketOpens = 1
    }

    /// <summary>
    /// Time span in minutes for stock value comparing
    /// </summary>
    /// <remarks></remarks>
    public enum StockTradingTimeSpan
    {
        _1 = 0,
        _5 = 1,
        _15 = 2,
        _30 = 3,
        _60 = 4,
        _90 = 5
    }

    /// <summary>
    /// Stock value extreme trading parameters
    /// </summary>
    /// <remarks></remarks>
    public enum StockExtremeParameter
    {
        TodaysHigh,
        TodaysLow,
        YearsHigh,
        YearsLow
    }

    public enum LessGreater
    {
        Less,
        Greater
    }

    public enum UpDown
    {
        Up = 0,
        Down = 1
    }

    /// <summary>
    /// Available Moving Averages
    /// </summary>
    /// <remarks></remarks>
    public enum MovingAverageType
    {
        FiftyDays,
        TwoHundredDays
    }




    public enum BondProperty
    {
        Type,
        Issue,
        Price,
        CouponInPercent,
        Maturity,
        YtmInPercent,
        CurrentYieldInPercent,
        FitchRatings,
        Callable
    }

    public enum UsState
    {
        Any,
        AL,
        AK,
        AZ,
        AR,
        CA,
        CO,
        CT,
        DE,
        FL,
        GA,
        HI,
        ID,
        IL,
        IN,
        IA,
        KS,
        KY,
        LA,
        ME,
        MD,
        MA,
        MI,
        MN,
        MS,
        MO,
        MT,
        NE,
        NV,
        NH,
        NJ,
        NM,
        NY,
        NC,
        ND,
        OH,
        OK,
        OR,
        PA,
        RI,
        SC,
        SD,
        TN,
        TX,
        UT,
        VT,
        VA,
        WA,
        WV,
        WI,
        WY
    }

    public enum PriceType
    {
        Any,
        Premium,
        Par,
        Discount
    }

    public enum Rating
    {
        Any = 0,
        AAA = 1,
        AA = 2,
        A = 3,
        BBB = 4,
        BB = 5,
        B = 6,
        CCC = 7,
        CC = 8,
        D = 9,
        NR = 10
    }

    public enum BondType
    {
        Treasury,
        TreasuryZeroCoupon,
        Corporate,
        Municipal
    }

}
