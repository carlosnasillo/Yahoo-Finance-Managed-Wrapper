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
	/// Criteria class for operating margin
	/// </summary>
	/// <remarks></remarks>
	public class OperatingMarginCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Operating Margin Criteria"; }
		}

		public override string CriteriaName {
			get { return "Operating Margin"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Margins; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.OperatingMargin}; }
		}

		public OperatingMarginCriteria() : base("8a")
		{
		}
	}

	/// <summary>
	/// Criteria class for profit margin (trailing twelve month)
	/// </summary>
	/// <remarks></remarks>
	public class ProfitMargin_ttmCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Profit Margin (ttm) Criteria"; }
		}

		public override string CriteriaName {
			get { return "Profit Margin (ttm)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Margins; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.ProfitMargin_ttm}; }
		}

		public ProfitMargin_ttmCriteria() : base("8r")
		{
		}
	}

	/// <summary>
	/// Criteria class for EBITDA margin (earnings before interest, taxes, depreciation and amortization) (trailing twelve month)
	/// </summary>
	/// <remarks></remarks>
	public class EBITDAMargin_ttmCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "EBITDA Margin (ttm) Criteria"; }
		}

		public override string CriteriaName {
			get { return "EBITDA Margin (ttm)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Margins; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.EBITDAMargin_ttm}; }
		}

		public EBITDAMargin_ttmCriteria() : base("9f")
		{
		}
	}

	/// <summary>
	/// Criteria class for gross margin (trailing twelve month)
	/// </summary>
	/// <remarks></remarks>
	public class GrossMargin_ttmCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Gross Margin (ttm) Criteria"; }
		}

		public override string CriteriaName {
			get { return "Gross Margin"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Margins; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.GrossMargin_ttm}; }
		}

		public GrossMargin_ttmCriteria() : base("9k")
		{
		}
	}

}
