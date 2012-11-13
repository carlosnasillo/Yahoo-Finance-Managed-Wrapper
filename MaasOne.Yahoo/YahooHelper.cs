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
using System.Collections.Generic;
using System.Text;


namespace MaasOne
{
    internal abstract class YahooHelper
    {

        public static string CultureToParameters(Culture cult)
        {
            if (cult == null)
                throw new ArgumentNullException("cult", "Culture must have a value");
            return string.Format("&region={0}&lang={1}-{2}", cult.Country.ToString(), cult.Language.ToString(), cult.Country.ToString());
        }

        public static string ServerString(YahooServer server)
        {
            switch (server)
            {
                case YahooServer.Argentina:
                    return "ar.";
                case YahooServer.Australia:
                    return "au.";
                case YahooServer.Brazil:
                    return "br.";
                case YahooServer.Canada:
                    return "ca.";
                case YahooServer.France:
                    return "fr.";
                case YahooServer.Germany:
                    return "de.";
                case YahooServer.HongKong:
                    return "hk.";
                case YahooServer.India:
                    return "in.";
                case YahooServer.Italy:
                    return "it.";
                case YahooServer.Korea:
                    return "kr.";
                case YahooServer.Mexico:
                    return "mx.";
                case YahooServer.Singapore:
                    return "sg.";
                case YahooServer.Spain:
                    return "es.";
                case YahooServer.UK:
                    return "uk.";
                case YahooServer.NewZealand:
                    return "nz.";
                default:
                    return string.Empty;
            }
        }

        private YahooHelper()
        {
        }

    }
}
