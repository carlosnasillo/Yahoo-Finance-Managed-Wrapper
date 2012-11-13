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
	/// Standard Deviation Indicator
	/// </summary>
	/// <remarks></remarks>
	public class StD : ISingleValueIndicator
	{

		public virtual string Name {
			get { return "Standard Deviation"; }
		}

		public virtual bool IsRealative {
			get { return true; }
		}

		public double ScaleMaximum {
			get { return double.PositiveInfinity; }
		}

		public double ScaleMinimum {
			get { return 0; }
		}

		public int Period {
			get { return mMA.Period; }
			set { mMA.Period = value; }
		}
		public bool PopulationStandardDeviation { get; set; }


        
		private MA mMA = new MA();
		
                      
        /// <summary>
		/// Calculates values of Standard Deviation for historic quote values.
		/// </summary>
		/// <param name="values">An unsorted IEnumerable of HistQuoteData.</param>
		/// <returns>The sorted dictionaries. 1) StD values; 2) MA values; 3) Quote values.</returns>
		/// <remarks></remarks>
		public virtual Dictionary<System.DateTime, double>[] Calculate(IEnumerable<KeyValuePair<System.DateTime, double>> values)
		{
			Dictionary<System.DateTime, double> stdResult = new Dictionary<System.DateTime, double>();

			Dictionary<DateTime, double>[] baseResults = mMA.Calculate(values);
			Dictionary<System.DateTime, double> maResult = baseResults[0];
			List<KeyValuePair<System.DateTime, double>> histQuotes = new List<KeyValuePair<System.DateTime, double>>(baseResults[1]);

			if (histQuotes.Count > 0) {
				double tempResult = 0;
				stdResult.Add(histQuotes[0].Key, 0);
				for (int i = 1; i <= histQuotes.Count - 1; i++) {
					tempResult = 0;
					if (i >= mMA.Period - 1) {
						for (int n = i - mMA.Period + 1; n <= i; n++) {
							tempResult += Math.Pow((histQuotes[n].Value - maResult[histQuotes[i].Key]), 2);
						}
						tempResult /= (mMA.Period + Convert.ToInt32((this.PopulationStandardDeviation ? 0 : -1)));
					} else {
						for (int n = 0; n <= i; n++) {
							tempResult += Math.Pow((histQuotes[n].Value - maResult[histQuotes[i].Key]), 2);
						}
						tempResult /= (i + Convert.ToInt32((this.PopulationStandardDeviation ? 1 : 0)));
					}
					tempResult = Math.Sqrt(tempResult);
					stdResult.Add(histQuotes[i].Key, tempResult);
				}
			}

			return new Dictionary<System.DateTime, double>[] {
				stdResult,
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
