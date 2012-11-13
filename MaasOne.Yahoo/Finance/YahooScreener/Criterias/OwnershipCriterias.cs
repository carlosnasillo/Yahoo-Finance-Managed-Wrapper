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
	/// Criteria class for outstanding shares
	/// </summary>
	/// <remarks></remarks>
	public class SharesOutstandingCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Shares Outstanding Criteria"; }
		}

		public override string CriteriaName {
			get { return "Shares Outstanding"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Ownership; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.SharesOutstanding}; }
		}

		public SharesOutstandingCriteria() : base("1")
		{
		}
	}

	/// <summary>
	/// Criteria class for floating shares
	/// </summary>
	/// <remarks></remarks>
	public class SharesFloatingCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Shares Floating Criteria"; }
		}

		public override string CriteriaName {
			get { return "Shares Floating"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Ownership; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization,QuoteProperty.SharesFloat}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public SharesFloatingCriteria() : base("2")
		{
		}
	}

	/// <summary>
	/// Criteria class for short ratio
	/// </summary>
	/// <remarks></remarks>
	public class ShortRatioCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Short Ratio Criteria"; }
		}

		public override string CriteriaName {
			get { return "Short Ratio"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Ownership; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization,QuoteProperty.ShortRatio}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public ShortRatioCriteria() : base("3")
		{
		}
	}

	/// <summary>
	/// Criteria class for short shares for prior month
	/// </summary>
	/// <remarks></remarks>
	public class SharesShortPriorMonthCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Shares Short (prior month) Criteria"; }
		}

		public override string CriteriaName {
			get { return "Shares Short (prior month)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Ownership; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.SharesShortPriorMonth}; }
		}

		public SharesShortPriorMonthCriteria() : base("8g")
		{
		}
	}

	/// <summary>
	/// Criteria class for short shares
	/// </summary>
	/// <remarks></remarks>
	public class SharesShortCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Shares Short Criteria"; }
		}

		public override string CriteriaName {
			get { return "Shares Short"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Ownership; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.SharesShort}; }
		}

		public SharesShortCriteria() : base("8m")
		{
		}
	}

	/// <summary>
	/// Criteria class for shares held by insiders
	/// </summary>
	/// <remarks></remarks>
	public class HeldByInsidersCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Held By Insiders Criteria"; }
		}

		public override string CriteriaName {
			get { return "Held By Insiders"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Ownership; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.HeldByInsiders}; }
		}

		public HeldByInsidersCriteria() : base("9d")
		{
		}
	}

	/// <summary>
	/// Criteria class for shares held by institutions
	/// </summary>
	/// <remarks></remarks>
	public class HeldByInstitutionsCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Held By Institutions Criteria"; }
		}

		public override string CriteriaName {
			get { return "Held By Institutions"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Ownership; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.HeldByInstitutions}; }
		}

		public HeldByInstitutionsCriteria() : base("9n")
		{
		}
	}




}
