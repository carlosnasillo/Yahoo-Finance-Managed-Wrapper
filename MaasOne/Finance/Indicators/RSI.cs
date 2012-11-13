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


    public class RSI : ISingleValueIndicator
    {

        public string Name { get { return "Relative Strength Index"; } }
        public bool IsRealative { get { return true; } }
        public double ScaleMaximum { get { return 100; } }
        public double ScaleMinimum { get { return 0; } }
        public int Period { get; set; }

        public RSI()
        {
            this.Period = 14;
        }


        private EMA mEMA = new EMA();
        public virtual Dictionary<System.DateTime, double>[] Calculate(IEnumerable<KeyValuePair<System.DateTime, double>> values)
        {
            Dictionary<System.DateTime, double> rsiResult = new Dictionary<System.DateTime, double>();

            List<KeyValuePair<System.DateTime, double>> quoteValues = new List<KeyValuePair<System.DateTime, double>>(values);
            quoteValues.Sort(new QuotesSorter());

            if (quoteValues.Count > 0)
            {
                rsiResult.Add(quoteValues[0].Key, 50);
                double up = 0;
                double down = 0;
                int upCount = 0;
                int downCount = 0;
                double aveUp = 0;
                double aveDown = 0;
                List<KeyValuePair<System.DateTime, double>> avgUp = new List<KeyValuePair<System.DateTime, double>>();
                List<KeyValuePair<System.DateTime, double>> avgDown = new List<KeyValuePair<System.DateTime, double>>();

                double rs = 0;
                for (int i = 1; i <= quoteValues.Count - 1; i++)
                {
                    int periodLength = Math.Min(i, this.Period);

                    up = 0;
                    down = 0;
                    upCount = 0;
                    downCount = 0;
                    aveUp = 0;
                    aveDown = 0;
                    rs = 0;
                    for (int s = i - periodLength + 1; s <= i; s++)
                    {
                        if (quoteValues[s].Value > quoteValues[s - 1].Value)
                        {
                            up += quoteValues[s].Value - quoteValues[s - 1].Value;
                            upCount += 1;
                        }
                        else if (quoteValues[s].Value < quoteValues[s - 1].Value)
                        {
                            down += quoteValues[s - 1].Value - quoteValues[s].Value;
                            downCount += 1;
                        }
                    }

                    if (upCount > 0)
                        aveUp = up / upCount;
                    if (downCount > 0)
                        aveDown = down / downCount;

                    if (aveDown != 0)
                        rs = aveUp / aveDown;
                    rsiResult.Add(quoteValues[i].Key, 100 - (100 / (1 + aveUp / aveDown)));
                }
            }


            return new Dictionary<System.DateTime, double>[] { rsiResult };
        }

        public override string ToString()
        {
            return this.Name + " " + this.Period;
        }

    }

}
