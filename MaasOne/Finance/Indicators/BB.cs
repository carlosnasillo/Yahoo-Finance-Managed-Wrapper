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
	/// Bollinger Bands Indicator
	/// </summary>
	/// <remarks></remarks>
	public class BB : StD
	{

		public override string Name {
			get { return "Bollinger Bands"; }
		}

		public override bool IsRealative {
			get { return false; }
		}

		public override Dictionary<System.DateTime, double>[] Calculate(IEnumerable<KeyValuePair<System.DateTime, double>> values)
		{
			Dictionary<System.DateTime, double> bbResultUpper = new Dictionary<System.DateTime, double>();
			Dictionary<System.DateTime, double> bbResultLower = new Dictionary<System.DateTime, double>();

            Dictionary<DateTime, double>[] baseResults = base.Calculate(values);
			Dictionary<System.DateTime, double> stdResult = baseResults[0];
			Dictionary<System.DateTime, double> maResult = baseResults[1];
			List<KeyValuePair<System.DateTime, double>> histQuotes = new List<KeyValuePair<System.DateTime, double>>(baseResults[2]);

			if (histQuotes.Count > 0) {
				foreach (KeyValuePair<DateTime, double> hq in histQuotes) {
					bbResultUpper.Add(hq.Key, maResult[hq.Key] + stdResult[hq.Key]);
					bbResultLower.Add(hq.Key, maResult[hq.Key] - stdResult[hq.Key]);
				}
			}

			return new Dictionary<System.DateTime, double>[] {
				bbResultUpper,
				bbResultLower,
				stdResult,
				maResult,
				baseResults[2]
			};
		}


		public override string ToString()
		{
			return this.Name + " " + this.Period;
		}
	}

}
