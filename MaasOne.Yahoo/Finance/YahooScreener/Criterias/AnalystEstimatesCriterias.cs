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
	/// Criteria class for earnings per share for next quarter
	/// </summary>
	/// <remarks></remarks>
	public class EPS_NextQuarterCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "EPS (next quarter) Criteria"; }
		}

		public override string CriteriaName {
			get { return "EPS (next quarter)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.AnalystEstimates; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization,QuoteProperty.EPSEstimateNextQuarter}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public EPS_NextQuarterCriteria() : base("x")
		{
		}

	}

	/// <summary>
	/// Criteria class for earnings per share for this year
	/// </summary>
	/// <remarks></remarks>
	public class EPS_ThisYearCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "EPS (this year) Criteria"; }
		}

		public override string CriteriaName {
			get { return "EPS (this year)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.AnalystEstimates; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization,QuoteProperty.EPSEstimateCurrentYear}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public EPS_ThisYearCriteria() : base("y")
		{
		}
	}

	/// <summary>
	/// Criteria class for earnings per share for next year
	/// </summary>
	/// <remarks></remarks>
	public class EPS_NextYearCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "EPS (next year) Criteria"; }
		}

		public override string CriteriaName {
			get { return "EPS (next year)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.AnalystEstimates; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization,QuoteProperty.EPSEstimateNextYear}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public EPS_NextYearCriteria() : base("z")
		{
		}
	}

	/// <summary>
	/// Criteria class for earnings per share for NYCE
	/// </summary>
	/// <remarks></remarks>
	public class EPS_NYCECriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "EPS (NYCE) Criteria"; }
		}

		public override string CriteriaName {
			get { return "EPS (NYCE)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.AnalystEstimates; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.EPS_NYCE}; }
		}

		public EPS_NYCECriteria() : base("8e")
		{
		}
	}

	/// <summary>
	/// Criteria class for sales growth estimate for this quarter
	/// </summary>
	/// <remarks></remarks>
	public class SalesGrowthEstimateThisQuarterCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Sales Growth Estimate (this year) Criteria"; }
		}

		public override string CriteriaName {
			get { return "Sales Growth Estimate (this year)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.AnalystEstimates; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.SalesGrowthEstimate_ThisQuarter}; }
		}

		public SalesGrowthEstimateThisQuarterCriteria() : base("9v")
		{
		}
	}

	/// <summary>
	/// Criteria class for revenue estimate for this year
	/// </summary>
	/// <remarks></remarks>
	public class RevenueEstimateThisYearCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Revenue Estimate (this year) Criteria"; }
		}

		public override string CriteriaName {
			get { return "Revenue Estimate (this year)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.AnalystEstimates; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.RevenueEstimate_ThisYear}; }
		}

		public RevenueEstimateThisYearCriteria() : base("8c")
		{
		}
	}

	/// <summary>
	/// Criteria class for earnings growth estimate for this year
	/// </summary>
	/// <remarks></remarks>
	public class EarningsGrowthEstimateThisYearCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Earnings Growth Estimate (this year) Criteria"; }
		}

		public override string CriteriaName {
			get { return "Earnings Growth Estimate (this year)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.AnalystEstimates; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.EarningsGrowthEstimate_ThisYear}; }
		}

		public EarningsGrowthEstimateThisYearCriteria() : base("8h")
		{
		}
	}

	/// <summary>
	/// Criteria class for earnings growth estimate for next year
	/// </summary>
	/// <remarks></remarks>
	public class EarningsGrowthEstimateNextYearCriteria : StockDigitCriteriaDefinition
	{


		public override string DisplayName {
			get { return "Earnings Growth Estimate (next year) Criteria"; }
		}

		public override string CriteriaName {
			get { return "Earnings Growth Estimate (next year)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.AnalystEstimates; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.EarningsGrowthEstimate_NextYear}; }
		}

		public EarningsGrowthEstimateNextYearCriteria() : base("9b")
		{
		}

	}

	/// <summary>
	/// Criteria class for earnings growth estimate for next 5 years
	/// </summary>
	/// <remarks></remarks>
	public class EarningsGrowthEstimateNext5YearsCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Earnings Growth Estimate (next 5 years) Criteria"; }
		}

		public override string CriteriaName {
			get { return "Earnings Growth Estimate (next 5 years)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.AnalystEstimates; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.EarningsGrowthEstimate_Next5Years}; }
		}

		public EarningsGrowthEstimateNext5YearsCriteria() : base("9u")
		{
		}
	}

}
