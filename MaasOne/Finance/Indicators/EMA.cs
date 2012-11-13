// ******************************************************************************
// ** 
// **  MaasOne WebServices
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



namespace MaasOne.Finance.Indicators
{

	/// <summary>
	/// Exponential Moving Average Indicator. Inherits from Moving Average(MA).
	/// </summary>
	/// <remarks></remarks>
	public class EMA : MA
	{

		public override string Name {
			get { return "Exponential Moving Average"; }
		}

		/// <summary>
		/// Calculates values of Exponential Moving Average for historic quote values.
		/// </summary>
		/// <param name="values">An unsorted IEnumerable of HistQuoteData.</param>
		/// <returns>The sorted dictionaries. 1) EMA values; 2) MA values; 3) Quote values.</returns>
		/// <remarks></remarks>
		public override Dictionary<System.DateTime, double>[] Calculate(IEnumerable<KeyValuePair<System.DateTime, double>> values)
		{
			Dictionary<DateTime, double> emaResult = new Dictionary<DateTime, double>();
			Dictionary<DateTime,double>[] baseResults = base.Calculate(values);
			Dictionary<DateTime, double> maResult = baseResults[0];

			List<KeyValuePair<DateTime, double>> histQuoteCloses = new List<KeyValuePair<DateTime, double>>(baseResults[1]);

			double exponent = 0;
			DateTime d = default(DateTime);
			if (histQuoteCloses.Count > 1) {
				emaResult.Add(histQuoteCloses[0].Key, histQuoteCloses[0].Value);
				for (int i = 1; i < histQuoteCloses.Count; i++) {
					exponent = 2.0 / (Math.Min(this.Period, i + 1) + 1);
					d = histQuoteCloses[i].Key;
                    double value = (exponent * histQuoteCloses[i].Value) + ((1 - exponent) * emaResult[histQuoteCloses[i - 1].Key]);
					emaResult.Add(d, value);
				}
			}

			return new Dictionary<DateTime, double>[] {
				emaResult,
				maResult,
				baseResults[1]
			};
		}

		public override string ToString()
		{
			return this.Name + " " + this.Period;
		}
	}

}
