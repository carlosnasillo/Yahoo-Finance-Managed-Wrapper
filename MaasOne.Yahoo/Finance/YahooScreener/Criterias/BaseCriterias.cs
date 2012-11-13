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
	/// Criteria base class
	/// </summary>
	/// <remarks></remarks>
	public abstract class CriteriaDefinition
	{
		public abstract string DisplayName { get; }
		public abstract string CriteriaName { get; }
		internal abstract string CriteriaParameter();
		internal abstract bool IsValid { get; }
	}

	/// <summary>
	/// Criteria base class for Stock Screener
	/// </summary>
	/// <remarks></remarks>
	public abstract class StockCriteriaDefinition : CriteriaDefinition
	{

		/// <summary>
		/// The Stock Screener criteria group
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public abstract StockScreenerCriteriaGroup CriteriaGroup { get; }
		/// <summary>
		/// The quote properties that the specific criteria class represents/provides
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public abstract QuoteProperty[] ProvidedQuoteProperties { get; }
		/// <summary>
		/// The Stock Screener properties that the specific criteria class represents/provides
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public abstract StockScreenerProperty[] ProvidedScreenerProperties { get; }
		internal string CriteriaTag { get; set; }

		internal override bool IsValid {
			get { return this.CriteriaTag != string.Empty; }
		}

		protected StockCriteriaDefinition(string paramType)
		{
			this.CriteriaTag = paramType;
		}
	}



	public interface IPercentageAvailability
	{
		/// <summary>
		/// Indicates if the compared values are in percent or absolute
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		bool PercentValues { get; set; }
	}

	public interface IGainLoss
	{
		StockPriceChangeDirection GainOrLoss { get; set; }
	}

	public interface IExtremeParameter
	{
		StockExtremeParameter ExtremeParameter { get; set; }
	}

	public interface ILessGreater
	{
		LessGreater LessGreater { get; set; }
	}

	public interface IUpDown
	{
		UpDown UpDown { get; set; }
	}

	public interface IMovingAverage
	{
		MovingAverageType MovingAverage { get; set; }
	}

	public interface IRelativeTimeSpan
	{
		StockTradingTimeSpan RelativeTimeSpanInMinutes { get; set; }
	}

	public interface IRelativeTimePoint
	{
		StockTradingRelativeTimePoint TimeSpanRelativeTo { get; set; }
	}

	public interface IAbsoluteTimePoint
	{
		StockTradingAbsoluteTimePoint ValueRelativeTo { get; set; }
	}

	/// <summary>
	/// Criteria base class for Stock Screener and digit values
	/// </summary>
	/// <remarks></remarks>
	public abstract class StockDigitCriteriaDefinition : StockCriteriaDefinition
	{


		private System.Globalization.CultureInfo mConverterCulture = new System.Globalization.CultureInfo("en-US");
		/// <summary>
		/// The maximum value of the value range.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Nullable<double> MinimumValue { get; set; }
		/// <summary>
		/// The minimum value of the value range
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		/// <remarks></remarks>
		public Nullable<double> MaximumValue { get; set; }

		internal int OptionalParamValue { get; set; }
		internal override bool IsValid {
			get { return base.IsValid && (this.MaximumValue.HasValue | this.MinimumValue.HasValue); }
		}

		protected StockDigitCriteriaDefinition(string paramType) : base(paramType)
		{
            this.OptionalParamValue = -1;
			this.MinimumValue = 0;
		}

		internal override string CriteriaParameter()
		{
			if (this.IsValid) {
				return "&" + base.CriteriaTag + "=" + ((this.OptionalParamValue != -1) ? (this.OptionalParamValue.ToString() + "_") : "").ToString() + this.GetParamDigitValue(this.MinimumValue) + "_" + this.GetParamDigitValue(this.MaximumValue) + "_e_3";
			} else {
				throw new NotSupportedException("The parameters are invalid.");
			}
		}

		private string GetParamDigitValue(Nullable<double> paramValue)
		{
			if (paramValue.HasValue) {
				return paramValue.Value.ToString("R",mConverterCulture);
			} else {
				return "u";
			}
		}

	}



	/// <summary>
	/// Criteria base class for Stock Screener and string values
	/// </summary>
	/// <remarks></remarks>
	public abstract class StockStringCriteriaDefinition : StockCriteriaDefinition
	{

		internal string Value { get; set; }
		internal override bool IsValid {
			get { return base.IsValid && this.Value != string.Empty; }
		}

		protected StockStringCriteriaDefinition(string paramType) : base(paramType)
		{
		}

		internal override string CriteriaParameter()
		{
			if (this.IsValid) {
				return "&" + base.CriteriaTag + "=" + this.Value + "_e_3";
			} else {
				throw new NotSupportedException("The parameters are invalid.");
			}
		}

	}

}
