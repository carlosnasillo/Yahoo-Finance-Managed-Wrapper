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
using MaasOne.Finance.YahooScreener;
using MaasOne.Finance.YahooScreener.Criterias;
using MaasOne.Finance.YahooFinance;



namespace MaasOne.Finance.YahooScreener.Criterias
{

	/// <summary>
	/// Criteria class for price/book value ratio
	/// </summary>
	/// <remarks></remarks>
	public class PriceBookRatioCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Price/Book Ratio Criteria"; }
		}

		public override string CriteriaName {
			get { return "Price/Book"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.BalanceSheet; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization,QuoteProperty.PriceBook}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public PriceBookRatioCriteria() : base("8f")
		{
		}
	}

	/// <summary>
	/// Criteria class for cash per share
	/// </summary>
	/// <remarks></remarks>
	public class CashPerShareCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Cash/Share Criteria"; }
		}

		public override string CriteriaName {
			get { return "Cash/Share"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.BalanceSheet; }
		}

		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.CashPerShare}; }
		}

		public CashPerShareCriteria() : base("8j")
		{
		}
	}

	/// <summary>
	/// Criteria class for total cash
	/// </summary>
	/// <remarks></remarks>
	public class TotalCashCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Total Cash Criteria"; }
		}

		public override string CriteriaName {
			get { return "Total Cash"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.BalanceSheet; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.TotalCash}; }
		}

		public TotalCashCriteria() : base("8l")
		{
		}
	}

	/// <summary>
	/// Criteria class for book value
	/// </summary>
	/// <remarks></remarks>
	public class BookValueCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Book Value Criteria"; }
		}

		public override string CriteriaName {
			get { return "Book Value"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.BalanceSheet; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization,QuoteProperty.BookValuePerShare}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		public BookValueCriteria() : base("6")
		{
		}
	}

	/// <summary>
	/// Criteria class for total debt
	/// </summary>
	/// <remarks></remarks>
	public class TotalDebtCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Total Debt Criteria"; }
		}

		public override string CriteriaName {
			get { return "Total Debt"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.BalanceSheet; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.TotalDebt}; }
		}

		public TotalDebtCriteria() : base("9e")
		{
		}
	}

	/// <summary>
	/// Criteria class for total debt/equity ratio
	/// </summary>
	/// <remarks></remarks>
	public class TotalDebtToEquityCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Total Debt/Equity Criteria"; }
		}

		public override string CriteriaName {
			get { return "Total Debt/Equity"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.BalanceSheet; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.TotalDebtToEquityRatio}; }
		}

		public TotalDebtToEquityCriteria() : base("9g")
		{
		}
	}

	/// <summary>
	/// Criteria class for current ratio
	/// </summary>
	/// <remarks></remarks>
	public class CurrentRatioCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Current Ratio Criteria"; }
		}

		public override string CriteriaName {
			get { return "Current Ratio"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.BalanceSheet; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.CurrentRatio}; }
		}

		public CurrentRatioCriteria() : base("9h")
		{
		}
	}

	/// <summary>
	/// Criteria class for long term debt/equity ratio
	/// </summary>
	/// <remarks></remarks>
	public class LongTermDebtToEquityCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Long Term Debt/Equity Criteria"; }
		}

		public override string CriteriaName {
			get { return "Long Term Debt/Equity"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.BalanceSheet; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.LongTermDebtToEquityRatio}; }
		}

		public LongTermDebtToEquityCriteria() : base("9i")
		{
		}
	}

	/// <summary>
	/// Criteria class for quick ratio
	/// </summary>
	/// <remarks></remarks>
	public class QuickRatioCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Gross Margin (ttm) Criteria"; }
		}

		public override string CriteriaName {
			get { return "Gross Margin (ttm)"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.BalanceSheet; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.QuickRatio}; }
		}

		public QuickRatioCriteria() : base("9l")
		{
		}
	}




}
