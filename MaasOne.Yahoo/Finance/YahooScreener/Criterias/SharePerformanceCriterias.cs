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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using MaasOne.Xml;
using MaasOne.Finance.YahooFinance;


namespace MaasOne.Finance.YahooScreener.Criterias
{

	/// <summary>
	/// Criteria definition for current price (LastTradePriceOnly)
	/// </summary>
	/// <remarks></remarks>
	public class CurrentPriceCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Current Price Criteria"; }
		}

		public override string CriteriaName {
			get { return "Price"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.SharePerformance; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public CurrentPriceCriteria() : base("b")
		{
		}

	}

	/// <summary>
	/// Criteria definition for price change gainer or losers
	/// </summary>
	/// <remarks></remarks>
	public class PriceGainerLosersCriteria : StockDigitCriteriaDefinition, IPercentageAvailability, IGainLoss, IAbsoluteTimePoint
	{

		public override string DisplayName {
			get { return "Price Gainer/Losers"; }
		}

		public override string CriteriaName {
			get { return "Price"; }
		}

		private StockTradingAbsoluteTimePoint mRelativeDate = StockTradingAbsoluteTimePoint.TodaysOpen;

		private StockPriceChangeDirection mGainOrLoss = StockPriceChangeDirection.Gain;
		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.SharePerformance; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get {
				if (this.ValueRelativeTo == StockTradingAbsoluteTimePoint.TodaysOpen) {
					return new QuoteProperty[] {
						QuoteProperty.Symbol,
						QuoteProperty.Name,
						QuoteProperty.LastTradePriceOnly,
						QuoteProperty.LastTradeTime,
						QuoteProperty.MarketCapitalization,
						QuoteProperty.Change,
						QuoteProperty.ChangeInPercent,
						QuoteProperty.Open
					};
				} else {
                    return new QuoteProperty[] {
						QuoteProperty.Symbol,
						QuoteProperty.Name,
						QuoteProperty.LastTradePriceOnly,
						QuoteProperty.LastTradeTime,
						QuoteProperty.MarketCapitalization,
						QuoteProperty.Change,
						QuoteProperty.ChangeInPercent,
						QuoteProperty.PreviousClose
					};

				}
			}
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public StockTradingAbsoluteTimePoint ValueRelativeTo {
			get { return mRelativeDate; }
			set {
				mRelativeDate = value;
				this.SetOptionalParam();
			}
		}
		public StockPriceChangeDirection GainOrLoss {
			get { return mGainOrLoss; }
			set {
				mGainOrLoss = value;
				this.SetOptionalParam();
			}
		}
		public bool PercentValues {
			get { return Convert.ToBoolean((base.CriteriaTag == "g" ? true : false)); }
			set {
				if (value) {
					base.CriteriaTag = "g";
				} else {
					base.CriteriaTag = "f";
				}
			}
		}

		private void SetOptionalParam()
		{
			if (mGainOrLoss == StockPriceChangeDirection.Gain) {
				if (mRelativeDate == StockTradingAbsoluteTimePoint.TodaysOpen) {
					base.OptionalParamValue = 0;
				} else {
					base.OptionalParamValue = 1;
				}
			} else {
				if (mRelativeDate == StockTradingAbsoluteTimePoint.TodaysOpen) {
					base.OptionalParamValue = 2;
				} else {
					base.OptionalParamValue = 3;
				}
			}
		}

		public PriceGainerLosersCriteria() : base("f")
		{
			this.SetOptionalParam();
		}

	}

	/// <summary>
	/// Criteria definition for price development in a specific time period
	/// </summary>
	/// <remarks></remarks>
	public class PriceMomentumCriteria : StockDigitCriteriaDefinition, IPercentageAvailability, IGainLoss, IRelativeTimePoint, IRelativeTimeSpan
	{

		public override string DisplayName {
			get { return "Price Momentum Criteria"; }
		}

		public override string CriteriaName {
			get { return "Price"; }
		}

		private StockTradingRelativeTimePoint mRelativeDate = StockTradingRelativeTimePoint.BeforeLastTradeTimePoint;
		private StockPriceChangeDirection mGainOrLoss = StockPriceChangeDirection.Gain;

		private StockTradingTimeSpan mTimeSpanInMinutes = StockTradingTimeSpan._1;
		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.SharePerformance; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public StockTradingRelativeTimePoint TimeSpanRelativeTo {
			get { return mRelativeDate; }
			set {
				mRelativeDate = value;
				this.SetOptionalParam();
			}
		}
		public StockPriceChangeDirection GainOrLoss {
			get { return mGainOrLoss; }
			set {
				mGainOrLoss = value;
				this.SetOptionalParam();
			}
		}
		public StockTradingTimeSpan RelativeTimeSpanInMinutes {
			get { return mTimeSpanInMinutes; }
			set {
				mTimeSpanInMinutes = value;
				this.SetOptionalParam();
			}
		}
		public bool PercentValues {
			get { return Convert.ToBoolean((base.CriteriaTag == "i" ? true : false)); }
			set {
				if (value) {
					base.CriteriaTag = "i";
				} else {
					base.CriteriaTag = "h";
				}
			}
		}

		private void SetOptionalParam()
		{
			base.OptionalParamValue = (Convert.ToInt32(mGainOrLoss) * 12) + (Convert.ToInt32(mRelativeDate) * 6) + Convert.ToInt32(mTimeSpanInMinutes);
		}

		public PriceMomentumCriteria() : base("h")
		{
			this.SetOptionalParam();
		}

	}

	/// <summary>
	/// Criteria definition for extreme price value and current price
	/// </summary>
	/// <remarks></remarks>
	public class ExtremePriceCriteria : StockDigitCriteriaDefinition, IPercentageAvailability, IExtremeParameter, ILessGreater
	{

		public override string DisplayName {
			get { return "Extreme Price Criteria"; }
		}

		public override string CriteriaName {
			get { return "Price"; }
		}

		private StockExtremeParameter mExtremeParameter = StockExtremeParameter.TodaysHigh;

		private LessGreater mLessGreater = LessGreater.Less;
		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.SharePerformance; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get {
				switch (this.ExtremeParameter) {
					case StockExtremeParameter.TodaysHigh:
						return  new QuoteProperty[] {
							QuoteProperty.Symbol,
							QuoteProperty.Name,
							QuoteProperty.LastTradePriceOnly,
							QuoteProperty.LastTradeTime,
							QuoteProperty.MarketCapitalization,
							QuoteProperty.DaysHigh

						};
					case StockExtremeParameter.TodaysLow:
						return  new QuoteProperty[] {
							QuoteProperty.Symbol,
							QuoteProperty.Name,
							QuoteProperty.LastTradePriceOnly,
							QuoteProperty.LastTradeTime,
							QuoteProperty.MarketCapitalization,
							QuoteProperty.DaysLow

						};
					case StockExtremeParameter.YearsHigh:
						return  new QuoteProperty[] {
							QuoteProperty.Symbol,
							QuoteProperty.Name,
							QuoteProperty.LastTradePriceOnly,
							QuoteProperty.LastTradeTime,
							QuoteProperty.MarketCapitalization,
							QuoteProperty.YearHigh,
							QuoteProperty.ChangeInPercentFromYearHigh,
							QuoteProperty.ChangeFromYearHigh

						};
					case StockExtremeParameter.YearsLow:
						return  new QuoteProperty[] {
							QuoteProperty.Symbol,
							QuoteProperty.Name,
							QuoteProperty.LastTradePriceOnly,
							QuoteProperty.LastTradeTime,
							QuoteProperty.MarketCapitalization,
							QuoteProperty.YearLow,
							QuoteProperty.PercentChangeFromYearLow,
							QuoteProperty.ChangeFromYearLow
						};
					default:

						return new QuoteProperty[] {};
				}
			}
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		internal override bool IsValid {
			get { return base.IsValid & !((mLessGreater == LessGreater.Greater & mExtremeParameter == StockExtremeParameter.TodaysHigh) | (mLessGreater == LessGreater.Less & mExtremeParameter == StockExtremeParameter.TodaysLow)); }
		}
		public bool PercentValues {
			get { return Convert.ToBoolean((base.CriteriaTag == "k" ? true : false)); }
			set {
				if (value) {
					base.CriteriaTag = "k";
				} else {
					base.CriteriaTag = "j";
				}
			}
		}

		/// <summary>
		/// With Today's High/Low only following combination is allowed: Less/TodaysHigh or Greater/TodaysLow. Other combinations wouldn't make sense.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public StockExtremeParameter ExtremeParameter {
			get { return mExtremeParameter; }
			set {
				mExtremeParameter = value;
				this.SetOptionalParam();
			}
		}
		/// <summary>
		/// With Today's High/Low only following combination is allowed: Less/TodaysHigh or Greater/TodaysLow. Other combinations wouldn't make sense.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public LessGreater LessGreater {
			get { return mLessGreater; }
			set {
				mLessGreater = value;
				this.SetOptionalParam();
			}
		}

		private void SetOptionalParam()
		{
			switch (mExtremeParameter) {
				case StockExtremeParameter.TodaysHigh:
					base.OptionalParamValue = 0;
					break;
				case StockExtremeParameter.TodaysLow:
					base.OptionalParamValue = 1;
					break;
				case StockExtremeParameter.YearsHigh:
					if (mLessGreater == LessGreater.Less) {
						base.OptionalParamValue = 2;
					} else {
						base.OptionalParamValue = 4;
					}
					break;
				case StockExtremeParameter.YearsLow:
					if (mLessGreater == LessGreater.Less) {
						base.OptionalParamValue = 5;
					} else {
						base.OptionalParamValue = 3;
					}
					break;
			}
		}

		public ExtremePriceCriteria() : base("j")
		{
			this.SetOptionalParam();
		}

	}

	/// <summary>
	/// Criteria definition for trading gaps
	/// </summary>
	/// <remarks></remarks>
	public class GapVsPreviousClose : StockDigitCriteriaDefinition, IPercentageAvailability, IUpDown
	{

		public override string DisplayName {
			get { return "Gap vs. Previous Close"; }
		}

		public override string CriteriaName {
			get { return "Gap"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.SharePerformance; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get {
				if (this.PercentValues) {
					return  new StockScreenerProperty[] {
						StockScreenerProperty.ReturnOnEquity,
						StockScreenerProperty.ReturnOnAssets,
						StockScreenerProperty.ForwardPriceToEarningsRatio,
						StockScreenerProperty.GapInPercent
					};
				} else {
                    return new StockScreenerProperty[] {
						StockScreenerProperty.ReturnOnEquity,
						StockScreenerProperty.ReturnOnAssets,
						StockScreenerProperty.ForwardPriceToEarningsRatio,
						StockScreenerProperty.Gap
					};
				}
			}
		}

		public bool PercentValues {
			get { return Convert.ToBoolean((base.CriteriaTag == "m" ? true : false)); }
			set {
				if (value) {
					base.CriteriaTag = "m";
				} else {
					base.CriteriaTag = "l";
				}
			}
		}

		public UpDown UpDown {
			get { return (UpDown)base.OptionalParamValue; }
			set { base.OptionalParamValue = Convert.ToInt32(value); }
		}

		public GapVsPreviousClose() : base("l")
		{
			this.UpDown = UpDown.Up;
		}
	}

	/// <summary>
	/// Criteria class for current price vs. Moving Average value in percent
	/// </summary>
	/// <remarks>Describes how much is the difference between Moving Average value (base) and last trade price (criteria value in percent)</remarks>
	public class PriceToMovingAverageRatioCriteria : StockDigitCriteriaDefinition, IUpDown, IMovingAverage
	{

		public override string DisplayName {
			get { return "Price/Moving Average Ratio Criteria"; }
		}

		public override string CriteriaName {
			get { return "Price/MovingAverage"; }
		}

		private UpDown mUpDown = UpDown.Up;

		private MovingAverageType mMovingAverage = MovingAverageType.FiftyDays;
		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.SharePerformance; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get {
				if (this.MovingAverage == MovingAverageType.FiftyDays) {
					return  new QuoteProperty[] {
						QuoteProperty.Symbol,
						QuoteProperty.Name,
						QuoteProperty.LastTradePriceOnly,
						QuoteProperty.LastTradeTime,
						QuoteProperty.MarketCapitalization,
						QuoteProperty.FiftydayMovingAverage,
						QuoteProperty.ChangeFromFiftydayMovingAverage,
						QuoteProperty.PercentChangeFromFiftydayMovingAverage
					};
				} else {
                    return new QuoteProperty[] {
						QuoteProperty.Symbol,
						QuoteProperty.Name,
						QuoteProperty.LastTradePriceOnly,
						QuoteProperty.LastTradeTime,
						QuoteProperty.MarketCapitalization,
						QuoteProperty.TwoHundreddayMovingAverage,
						QuoteProperty.ChangeFromTwoHundreddayMovingAverage,
						QuoteProperty.PercentChangeFromTwoHundreddayMovingAverage
					};
				}

			}
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public UpDown UpDown {
			get { return mUpDown; }
			set {
				mUpDown = value;
				this.SetOptionalParam();
			}
		}
		public MovingAverageType MovingAverage {

			get { return mMovingAverage; }
			set {
				mMovingAverage = value;
				this.SetOptionalParam();
			}
		}

		private void SetOptionalParam()
		{
			if (this.UpDown == UpDown.Up) {
				if (this.MovingAverage == MovingAverageType.FiftyDays) {
					base.OptionalParamValue = 0;
				} else {
					base.OptionalParamValue = 1;
				}
			} else {
				if (this.MovingAverage == MovingAverageType.FiftyDays) {
					base.OptionalParamValue = 2;
				} else {
					base.OptionalParamValue = 3;
				}
			}
		}

		public PriceToMovingAverageRatioCriteria() : base("o")
		{
			this.SetOptionalParam();
		}


	}

}
