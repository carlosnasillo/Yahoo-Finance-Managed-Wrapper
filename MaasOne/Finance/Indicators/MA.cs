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
	/// Moving Average Indicator
	/// </summary>
	/// <remarks></remarks>
	public class MA : ISingleValueIndicator
	{

		public virtual string Name {
			get { return "Moving Average"; }
		}

		public virtual bool IsRealative {
			get { return false; }
		}

		public double ScaleMaximum {
			get { return double.PositiveInfinity; }
		}

		public double ScaleMinimum {
			get { return double.NegativeInfinity; }
		}


		public int Period { get; set; }

        public MA() {
            this.Period = 12;
        }

		/// <summary>
		/// Calculate values of Moving Average for historic quote values.
		/// </summary>
		/// <param name="values">An unsorted IEnumerable of HistQuoteData.</param>
		/// <returns>The sorted dictionaries. 1) MA values; 2) Quote values.</returns>
		/// <remarks></remarks>
		public virtual Dictionary<System.DateTime, double>[] Calculate(IEnumerable<KeyValuePair<System.DateTime, double>> values)
		{
			Dictionary<System.DateTime, double> maResult = new Dictionary<System.DateTime, double>();

			List<KeyValuePair<System.DateTime, double>> quoteValues = new List<KeyValuePair<System.DateTime, double>>(values);
			quoteValues.Sort(new QuotesSorter());

			Dictionary<System.DateTime, double> valDict = new Dictionary<System.DateTime, double>();
			foreach (KeyValuePair<System.DateTime, double> item in quoteValues) {
				valDict.Add(item.Key, item.Value);
			}

			if (quoteValues.Count > 0) {
				double ave = 0;
				for (int i = 0; i <= quoteValues.Count - 1; i++) {
					ave = 0;
					if (i + 1 - this.Period >= 0) {
						for (int n = i + 1 - this.Period; n <= i; n++) {
							ave += quoteValues[n].Value;
						}
						ave = ave / this.Period;
					} else {
						for (int n = 0; n <= i; n++) {
							ave += quoteValues[n].Value;
						}
						ave = ave / (i + 1);
					}
					maResult.Add(quoteValues[i].Key, ave);
				}
			}

			return new Dictionary<System.DateTime, double>[] {
				maResult,
				valDict
			};
		}

		public override string ToString()
		{
			return this.Name + " " + this.Period;
		}

	}




}
