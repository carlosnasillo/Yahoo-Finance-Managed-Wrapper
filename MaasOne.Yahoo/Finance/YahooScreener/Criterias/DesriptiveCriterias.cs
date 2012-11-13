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
	/// Criteria definition for filtering results for specific stock exchange
	/// </summary>
	/// <remarks></remarks>
	public class ExchangeCriteria : StockStringCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Exchange Criteria"; }
		}

		public override string CriteriaName {
			get { return "Stock Exchange"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Descriptive; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		internal override bool IsValid {
			get { return base.IsValid && (base.Value == "amex" | base.Value == "nasdaq" | base.Value == "nyse"); }
		}

		public Nullable<StockExchange> Exchange {
			get {
				if (this.IsValid) {
					switch (base.Value) {
						case "amex":
							return StockExchange.AMEX;
						case "nasdaq":
							return StockExchange.NASDAQ;
						case "nyse":
							return StockExchange.NYSE;
						default:
							return null;
					}
				} else {
					return null;
				}
			}
			set {
				if (value.HasValue) {
					base.Value = value.ToString().ToLower();
				} else {
					base.Value = string.Empty;
				}
			}
		}

		public ExchangeCriteria() : base("a")
		{
		}

	}

	/// <summary>
	/// Criteria definition for filtering results for specific sector
	/// </summary>
	/// <remarks></remarks>
	public class SectorCriteria : StockStringCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Sector Criteria"; }
		}

		public override string CriteriaName {
			get { return "Sector"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Descriptive; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		internal override bool IsValid {
			get {
				int i = 0;
				return base.IsValid && int.TryParse(base.Value, out i) && (i >= 1 & i <= 9);
			}
		}

		public Nullable<Sector> Sector {
			get {
				if (this.IsValid) {
					return (Sector)Convert.ToInt32(base.Value);
				} else {
					return null;
				}
			}
			set {
				if (value.HasValue) {
					base.Value = Convert.ToInt32(value).ToString();
				} else {
					base.Value = string.Empty;
				}
			}
		}

		public SectorCriteria() : base("w")
		{
		}
	}

	/// <summary>
	/// Criteria definition for filtering results for specific industry
	/// </summary>
	/// <remarks></remarks>
	public class IndustryCriteria : StockStringCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Industry Criteria"; }
		}

		public override string CriteriaName {
			get { return "Industry"; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Descriptive; }
		}
		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio}; }
		}

		internal override bool IsValid {
			get {
				int i = 0;
				return base.IsValid && int.TryParse(base.Value, out i) && (i >= 1 & i <= 9);
			}
		}

		public Nullable<Industry> Industry {
			get {
				if (this.IsValid) {
					return (Industry)Convert.ToInt32(base.Value);
				} else {
					return null;
				}
			}
			set {
				if (value.HasValue) {
					base.Value = Convert.ToInt32(value).ToString();
				} else {
					base.Value = string.Empty;
				}
			}
		}

		public IndustryCriteria() : base("d")
		{
		}

	}

	/// <summary>
	/// Criteria definition for number of employees
	/// </summary>
	/// <remarks></remarks>
	public class NumberOfEmployeesCriteria : StockDigitCriteriaDefinition
	{

		public override string DisplayName {
			get { return "Number of Employees Criteria"; }
		}

		public override string CriteriaName {
			get { return "Number of Employees"; }
		}

		public override QuoteProperty[] ProvidedQuoteProperties {
			get { return new  QuoteProperty[] {QuoteProperty.Symbol,QuoteProperty.Name,QuoteProperty.LastTradePriceOnly,QuoteProperty.LastTradeTime,QuoteProperty.MarketCapitalization}; }
		}
		public override StockScreenerProperty[] ProvidedScreenerProperties {
			get { return new  StockScreenerProperty[] {StockScreenerProperty.ReturnOnEquity,StockScreenerProperty.ReturnOnAssets,StockScreenerProperty.ForwardPriceToEarningsRatio,StockScreenerProperty.NumberOfEmployees}; }
		}

		public override StockScreenerCriteriaGroup CriteriaGroup {
			get { return StockScreenerCriteriaGroup.Descriptive; }
		}

		public NumberOfEmployeesCriteria() : base("9o")
		{
		}

	}

}
