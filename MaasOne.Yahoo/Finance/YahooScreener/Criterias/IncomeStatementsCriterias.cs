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
	/// Criteria class for earnings per share (trailing twelve month)
	/// </summary>
	/// <remarks></remarks>
	public class EPS_ttmCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "EPS (ttm) Criteria"; }
		}

		public override string CriteriaName {
			get { return "EPS (ttm)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.IncomeStatements; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization,QuoteProperty.DilutedEPS}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public EPS_ttmCriteria() : base("w")
		{
		}
	}

	/// <summary>
	/// Criteria class for earnings per share (most recent quarter)
	/// </summary>
	/// <remarks></remarks>
	public class EPS_mrqCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "EPS (mrq) Criteria"; }
		}

		public override string CriteriaName {
			get { return "EPS (mrq)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.IncomeStatements; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.EPS_mrq}; }
		}

		public EPS_mrqCriteria() : base("8i")
		{
		}
	}

	/// <summary>
	/// Criteria class for sales (trailing twelve month)
	/// </summary>
	/// <remarks></remarks>
	public class Sales_ttmCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Sales (ttm) Criteria"; }
		}

		public override string CriteriaName {
			get { return "Sales (ttm)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.IncomeStatements; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.Sales_ttm}; }
		}

		public Sales_ttmCriteria() : base("0")
		{
		}
	}

	/// <summary>
	/// Criteria class for EBITDA (earnings before interest, taxes, depreciation and amortization)
	/// </summary>
	/// <remarks></remarks>
	public class EBITDACriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "EBITDA Criteria"; }
		}

		public override string CriteriaName {
			get { return "EBITDA"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.IncomeStatements; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization,QuoteProperty.EBITDA}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public EBITDACriteria() : base("t")
		{
		}
	}

	/// <summary>
	/// Criteria class for gross profit
	/// </summary>
	/// <remarks></remarks>
	public class GrossProfitCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Gross Profit Criteria"; }
		}

		public override string CriteriaName {
			get { return "Gross Profit"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.IncomeStatements; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.GrossProfit}; }
		}

		public GrossProfitCriteria() : base("8n")
		{
		}
	}

	/// <summary>
	/// Criteria class for net income
	/// </summary>
	/// <remarks></remarks>
	public class NetIncomeCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Net Income Criteria"; }
		}

		public override string CriteriaName {
			get { return "Net Income"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.IncomeStatements; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.NetIncome}; }
		}

		public NetIncomeCriteria() : base("8p")
		{
		}
	}

	/// <summary>
	/// Criteria class for operating income
	/// </summary>
	/// <remarks></remarks>
	public class OperatingIncomeCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Operating Income Criteria"; }
		}

		public override string CriteriaName {
			get { return "Operating Income"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.IncomeStatements; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.OperatingIncome}; }
		}

		public OperatingIncomeCriteria() : base("9j")
		{
		}
	}


}
