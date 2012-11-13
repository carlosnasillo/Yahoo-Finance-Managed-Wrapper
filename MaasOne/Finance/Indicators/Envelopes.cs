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
	/// Envelopes Indicator
	/// </summary>
	/// <remarks></remarks>
	public class Env : MA
	{

		public override string Name {
			get { return "Envelopes"; }
		}

		private bool mIsBufferFixed = false;
		public bool IsBufferFixed {
			get { return mIsBufferFixed; }
			set { mIsBufferFixed = value; }
		}
		private double mPercentBuffer = 0.05;
		public double PercentBuffer {
			get { return mPercentBuffer; }
			set { mPercentBuffer = value; }
		}
		private double mFixedBuffer = 1;
		public double FixedBuffer {
			get { return mFixedBuffer; }
			set { mFixedBuffer = value; }
		}


		public override Dictionary<System.DateTime, double>[] Calculate(IEnumerable<KeyValuePair<System.DateTime, double>> values)
		{
			Dictionary<System.DateTime, double> envResultUpper = new Dictionary<System.DateTime, double>();
			Dictionary<System.DateTime, double> envResultLower = new Dictionary<System.DateTime, double>();

            Dictionary<DateTime, double>[] baseResults = base.Calculate(values);
			Dictionary<System.DateTime, double> maResult = baseResults[0];
			List<KeyValuePair<System.DateTime, double>> histQuotes = new List<KeyValuePair<System.DateTime, double>>(baseResults[1]);

			if (this.IsBufferFixed) {
				foreach (KeyValuePair<DateTime, double> maRes in maResult) {
					envResultUpper.Add(maRes.Key, maRes.Value + this.FixedBuffer);
					envResultLower.Add(maRes.Key, maRes.Value - this.FixedBuffer);
				}
			} else {
				foreach (KeyValuePair<DateTime, double> maRes in maResult) {
					envResultUpper.Add(maRes.Key, maRes.Value * (1 + this.PercentBuffer));
					envResultLower.Add(maRes.Key, maRes.Value * (1 - this.PercentBuffer));
				}
			}

			return new Dictionary<System.DateTime, double>[] {
				envResultUpper,
				envResultLower,
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
