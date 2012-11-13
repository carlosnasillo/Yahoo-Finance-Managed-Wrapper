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
	/// Criteria class for market capitalization
	/// </summary>
	/// <remarks></remarks>
	public class MarketCapitalizationCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Market Capitalization Criteria"; }
		}

		public override string CriteriaName {
			get { return "Market Capitalization"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Valuation; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
            get { return new StockScreenerProperty[] { StockScreenerProperty.ReturnOnEquity, StockScreenerProperty.ReturnOnAssets, StockScreenerProperty.ForwardPriceToEarningsRatio }; }
		}

		public MarketCapitalizationCriteria() : base("c")
		{
		}
	}


	/// <summary>
	/// Criteria class for price/sales ratio
	/// </summary>
	/// <remarks></remarks>
	public class PriceSalesRatioCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Price/Sales Criteria"; }
		}

		public override string CriteriaName {
			get { return "Price/Sales"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Valuation; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
            get { return new QuoteProperty[] { QuoteProperty.Symbol, QuoteProperty.Name, QuoteProperty.LastTradePriceOnly, QuoteProperty.LastTradeTime, QuoteProperty.MarketCapitalization, QuoteProperty.PriceSales }; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public PriceSalesRatioCriteria() : base("v")
		{
		}
	}

	/// <summary>
	/// Criteria class for price/equity ratio
	/// </summary>
	/// <remarks></remarks>
	public class PriceEquityRatioCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Price/Equity Criteria"; }
		}

		public override string CriteriaName {
			get { return "Price/Equity"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Valuation; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
            get { return new QuoteProperty[] { QuoteProperty.Symbol, QuoteProperty.Name, QuoteProperty.LastTradePriceOnly, QuoteProperty.LastTradeTime, QuoteProperty.MarketCapitalization }; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.PriceEarningsRatio}; }
		}

		public PriceEquityRatioCriteria() : base("e")
		{
		}
	}

	/// <summary>
	/// Criteria class for forward price/equity ratio
	/// </summary>
	/// <remarks></remarks>
	public class ForwardPriceEquityRatioCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Forward Price/Equity Criteria"; }
		}

		public override string CriteriaName {
			get { return "Forward Price/Equity"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Valuation; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
            get { return new QuoteProperty[] { QuoteProperty.Symbol, QuoteProperty.Name, QuoteProperty.LastTradePriceOnly, QuoteProperty.LastTradeTime, QuoteProperty.MarketCapitalization }; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public ForwardPriceEquityRatioCriteria() : base("9t")
		{
		}
	}


	/// <summary>
	/// Criteria class for price/earnings/growth in earnings ratio
	/// </summary>
	/// <remarks></remarks>
	public class PEGRatioCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "P/E/G Criteria"; }
		}

		public override string CriteriaName {
			get { return "P/E/G"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Valuation; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
            get { return new QuoteProperty[] { QuoteProperty.Symbol, QuoteProperty.Name, QuoteProperty.LastTradePriceOnly, QuoteProperty.LastTradeTime, QuoteProperty.MarketCapitalization, QuoteProperty.PEGRatio }; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public PEGRatioCriteria() : base("u")
		{
		}
	}

	/// <summary>
	/// Criteria class for enterprise value
	/// </summary>
	/// <remarks></remarks>
	public class EntityValueCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Entity Value Criteria"; }
		}

		public override string CriteriaName {
			get { return "Entity Value"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Valuation; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
            get { return new QuoteProperty[] { QuoteProperty.Symbol, QuoteProperty.Name, QuoteProperty.LastTradePriceOnly, QuoteProperty.LastTradeTime, QuoteProperty.MarketCapitalization }; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.EntityValue}; }
		}

		public EntityValueCriteria() : base("9p")
		{
		}
	}

	/// <summary>
	/// Criteria class for enterprise value/revenue ratio
	/// </summary>
	/// <remarks></remarks>
	public class EntityValueRevenueRatio_ttmCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Entity Value/Revenue (ttm) Criteria"; }
		}

		public override string CriteriaName {
			get { return "Entity Value/Revenue (ttm)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Valuation; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
            get { return new QuoteProperty[] { QuoteProperty.Symbol, QuoteProperty.Name, QuoteProperty.LastTradePriceOnly, QuoteProperty.LastTradeTime, QuoteProperty.MarketCapitalization }; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.EntityValueToRevenueRatio}; }
		}

		public EntityValueRevenueRatio_ttmCriteria() : base("9q")
		{
		}
	}

	/// <summary>
	/// Criteria class for enterprise value/operating cash flow ratio
	/// </summary>
	/// <remarks></remarks>
	public class EntityValueOperatingCashFlowRatioCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Entity Value/Operating Cash Flow Criteria"; }
		}

		public override string CriteriaName {
			get { return "Entity Value/Operating Cash Flow"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Valuation; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
            get { return new QuoteProperty[] { QuoteProperty.Symbol, QuoteProperty.Name, QuoteProperty.LastTradePriceOnly, QuoteProperty.LastTradeTime, QuoteProperty.MarketCapitalization }; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.EntityValueToOperatingCashFlowRatio}; }
		}

		public EntityValueOperatingCashFlowRatioCriteria() : base("9r")
		{
		}
	}

	/// <summary>
	/// Criteria class for enterprise value/free cash flow ratio
	/// </summary>
	/// <remarks></remarks>
	public class EntityValueFreeCashFlowRatioCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Entity Value/Free Cash Flow Criteria"; }
		}

		public override string CriteriaName {
			get { return "Entity Value/Free Cash Flow"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Valuation; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
            get { return new QuoteProperty[] { QuoteProperty.Symbol, QuoteProperty.Name, QuoteProperty.LastTradePriceOnly, QuoteProperty.LastTradeTime, QuoteProperty.MarketCapitalization }; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.EntityValueToFreeCashFlowRatio}; }
		}

		public EntityValueFreeCashFlowRatioCriteria() : base("9s")
		{
		}
	}


}
