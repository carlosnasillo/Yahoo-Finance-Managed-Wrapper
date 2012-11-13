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
using System.Collections.Generic;
using System.Text;


namespace MaasOne
{
    public class Culture : System.Globalization.CultureInfo
    {

        private Language mLanguage = Language.en;

        private Country mCountry = Country.US;
        public Language Language
        {
            get { return mLanguage; }
        }
        public Country Country
        {
            get { return mCountry; }
        }

        public Culture(Language lang, Country cnt)
            : base(lang.ToString().Replace("no", "nn").Replace("tzh", "zh") + "-" + cnt.ToString().Replace("CT", "ES").Replace("UK", "GB"))
        {
            mLanguage = lang;
            mCountry = cnt;
        }

        public override object Clone()
        {
            return new Culture(this.Language, this.Country);
        }
        public Culture CloneStrict()
        {
            return (Culture)this.Clone();
        }
        public override string ToString()
        {
            return this.DisplayName;
        }





        public abstract class DefaultCultures
        {

            private static Culture[] mItems = new Culture[45];
            internal DefaultCultures()
            {
            }

            //New Culture(Language.en, Country.ID), _
            //New Culture(Language.tl, Country.PH), _
            static DefaultCultures()
            {
                mItems[0] = new Culture(Language.es, Country.AR);
                mItems[1] = new Culture(Language.de, Country.AT);
                mItems[2] = new Culture(Language.en, Country.AU);
                mItems[3] = new Culture(Language.pt, Country.BR);
                mItems[4] = new Culture(Language.en, Country.CA);
                mItems[5] = new Culture(Language.fr, Country.CA);
                mItems[6] = new Culture(Language.ca, Country.CT);
                mItems[7] = new Culture(Language.es, Country.CL);
                mItems[8] = new Culture(Language.es, Country.CO);
                mItems[9] = new Culture(Language.cs, Country.CZ);
                mItems[10] = new Culture(Language.da, Country.DK);
                mItems[11] = new Culture(Language.fi, Country.FI);
                mItems[12] = new Culture(Language.fr, Country.FR);
                mItems[13] = new Culture(Language.de, Country.DE);
                mItems[14] = new Culture(Language.tzh, Country.HK);
                mItems[15] = new Culture(Language.hu, Country.HU);
                mItems[16] = new Culture(Language.id, Country.ID);
                mItems[17] = new Culture(Language.en, Country.IN);
                mItems[18] = new Culture(Language.he, Country.IL);
                mItems[19] = new Culture(Language.it, Country.IT);
                mItems[20] = new Culture(Language.ja, Country.JP);
                mItems[21] = new Culture(Language.ko, Country.KR);
                mItems[22] = new Culture(Language.en, Country.MY);
                mItems[23] = new Culture(Language.ms, Country.MY);
                mItems[24] = new Culture(Language.es, Country.MX);
                mItems[25] = new Culture(Language.nl, Country.NL);
                mItems[26] = new Culture(Language.no, Country.NO);
                mItems[27] = new Culture(Language.en, Country.NZ);
                mItems[28] = new Culture(Language.es, Country.PE);
                mItems[29] = new Culture(Language.en, Country.PH);
                mItems[30] = new Culture(Language.ro, Country.RO);
                mItems[31] = new Culture(Language.ru, Country.RU);
                mItems[32] = new Culture(Language.en, Country.SG);
                mItems[33] = new Culture(Language.es, Country.ES);
                mItems[34] = new Culture(Language.fr, Country.CH);
                mItems[35] = new Culture(Language.de, Country.CH);
                mItems[36] = new Culture(Language.it, Country.CH);
                mItems[37] = new Culture(Language.th, Country.TH);
                mItems[38] = new Culture(Language.tr, Country.TR);
                mItems[39] = new Culture(Language.tzh, Country.TW);
                mItems[40] = new Culture(Language.en, Country.UK);
                mItems[41] = new Culture(Language.en, Country.US);
                mItems[42] = new Culture(Language.es, Country.US);
                mItems[43] = new Culture(Language.es, Country.VE);
                mItems[44] = new Culture(Language.vi, Country.VN);
            }

            public static Culture[] Items
            {
                get { return mItems; }
            }

            public static Culture Argentina
            {
                get { return mItems[0]; }
            }
            public static Culture Austria
            {
                get { return mItems[1]; }
            }
            public static Culture Australia
            {
                get { return mItems[2]; }
            }
            public static Culture Brazil
            {
                get { return mItems[3]; }
            }
            public static Culture Canada_English
            {
                get { return mItems[4]; }
            }
            public static Culture Canada_French
            {
                get { return mItems[5]; }
            }
            public static Culture Catalan
            {
                get { return mItems[6]; }
            }
            public static Culture Chile
            {
                get { return mItems[7]; }
            }
            public static Culture Columbia
            {
                get { return mItems[8]; }
            }
            public static Culture CzechRepublic
            {
                get { return mItems[9]; }
            }
            public static Culture Denmark
            {
                get { return mItems[10]; }
            }
            public static Culture Finland
            {
                get { return mItems[11]; }
            }
            public static Culture France
            {
                get { return mItems[12]; }
            }
            public static Culture Germany
            {
                get { return mItems[13]; }
            }
            public static Culture HongKong
            {
                get { return mItems[14]; }
            }
            public static Culture Hungary
            {
                get { return mItems[15]; }
            }
            public static Culture Indonesia
            {
                get { return mItems[16]; }
            }
            public static Culture India
            {
                get { return mItems[17]; }
            }
            public static Culture Israel
            {
                get { return mItems[18]; }
            }
            public static Culture Italy
            {
                get { return mItems[19]; }
            }
            public static Culture Japan
            {
                get { return mItems[20]; }
            }
            public static Culture Korea
            {
                get { return mItems[21]; }
            }
            public static Culture Malaysia_English
            {
                get { return mItems[22]; }
            }
            public static Culture Malaysia_Malaysian
            {
                get { return mItems[23]; }
            }
            public static Culture Mexico
            {
                get { return mItems[24]; }
            }
            public static Culture Netherlands
            {
                get { return mItems[25]; }
            }
            public static Culture Norway
            {
                get { return mItems[26]; }
            }
            public static Culture NewZealand
            {
                get { return mItems[27]; }
            }
            public static Culture Peru
            {
                get { return mItems[28]; }
            }
            public static Culture Philippines_English
            {
                get { return mItems[29]; }
            }
            public static Culture Romania
            {
                get { return mItems[30]; }
            }
            public static Culture Russia
            {
                get { return mItems[31]; }
            }
            public static Culture Singapore
            {
                get { return mItems[32]; }
            }
            public static Culture Spain
            {
                get { return mItems[33]; }
            }
            public static Culture Switzerland_French
            {
                get { return mItems[34]; }
            }
            public static Culture Switzerland_German
            {
                get { return mItems[35]; }
            }
            public static Culture Switzerland_Italian
            {
                get { return mItems[36]; }
            }
            public static Culture Thailand
            {
                get { return mItems[37]; }
            }
            public static Culture Turkey
            {
                get { return mItems[38]; }
            }
            public static Culture Taiwan
            {
                get { return mItems[39]; }
            }
            public static Culture UnitedKingdom
            {
                get { return mItems[40]; }
            }
            public static Culture UnitedStates_English
            {
                get { return mItems[41]; }
            }
            public static Culture UnitedStates_Spanish
            {
                get { return mItems[42]; }
            }
            public static Culture Venezuela
            {
                get { return mItems[43]; }
            }
            public static Culture Vietnam
            {
                get { return mItems[44]; }
            }

        }
    }
}
