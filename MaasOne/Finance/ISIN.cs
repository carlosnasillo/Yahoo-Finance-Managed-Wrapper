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


namespace MaasOne.Finance
{
    /// <summary>
    /// Class for managing an International Securities Identification Number.
    /// </summary>
    /// <remarks></remarks>
    public class ISIN
    {
        private char[] mChars = new char[11];
        private int mCheckDigit = -1;
        
        /// <summary>
        /// The complete ISIN
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Value
        {
            get { return MyHelper.CharEnumToString(mChars) + this.CheckDigit; }
        }

        /// <summary>
        /// The country specific part of the ISIN
        /// </summary>
        /// <value></value>
        /// <returns>The first two letters if the ISIN is valid</returns>
        /// <remarks></remarks>
        public string CountryCode
        {
            get { return mChars[0].ToString() + mChars[1].ToString(); }
        }

        /// <summary>
        /// The core part of the ISIN
        /// </summary>
        /// <value></value>
        /// <returns>The ISIN without CountryCode and CheckDigit</returns>
        /// <remarks></remarks>
        public string CoreCode
        {
            get { return mChars[2].ToString() + mChars[3].ToString() + mChars[4].ToString() + mChars[5].ToString() + mChars[6].ToString() + mChars[7].ToString() + mChars[8].ToString() + mChars[9].ToString() + mChars[10].ToString(); }
        }

        /// <summary>
        /// The calculated check digit of the ISIN
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Every "get" call a calculation</remarks>
        public int CheckDigit
        {
            get { return mCheckDigit; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="isinWithOrWithoutCheckDigit">A valid ISIN with or without check digit</param>
        /// <remarks>If check digit exists, it will be ignored, but in this case the string must be a digit to be a valid ISIN. The check digit will be calculated seperatly.</remarks>
        public ISIN(string isinWithOrWithoutCheckDigit)
        {
            if (!IsValidFormatted(isinWithOrWithoutCheckDigit))
            {
                throw new ArgumentException("The ISIN value is not valid formatted.", "isinWithOrWithoutCheckDigit");
            }
            else
            {
                for (int i = 0; i <= 10; i++)
                {
                    mChars[i] = char.ToUpper(isinWithOrWithoutCheckDigit[i]);
                }

                List<int> checkDigits = new List<int>();
                foreach (char c in mChars)
                {
                    int d = this.CharToISINDigit(c);
                    checkDigits.AddRange(this.ToSingleDigits(d));
                }
                if (checkDigits.Count > 0)
                {
                    int quersumme = 0;
                    int count = 1;
                    for (int i = checkDigits.Count - 1; i >= 0; i += -1)
                    {
                        count += 1;
                        if (count % 2 == 0)
                        {
                            checkDigits[i] *= 2;
                        }
                        int[] digits = this.ToSingleDigits(checkDigits[i]);
                        foreach (int d in digits)
                        {
                            quersumme += d;
                        }
                    }
                    mCheckDigit = (10 - (quersumme % 10)) % 10;
                }
                else
                {
                    mCheckDigit = -1;
                }
            }
        }

        /// <summary>
        /// Returns the complete ISIN value.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.Value;
        }

        private int[] ToSingleDigits(int i)
        {
            if (i < 10)
            {
                return new int[] { i };
            }
            else
            {
                int[] db = new int[2];
                db[0] = Convert.ToInt32(i.ToString()[0].ToString());
                db[1] = Convert.ToInt32(i.ToString()[1].ToString());
                return db;
            }
        }
        private int CharToISINDigit(char c)
        {
            if (char.IsDigit(c))
            {
                return Convert.ToInt32(c.ToString());
            }
            else
            {
                int digit = 0;
                for (ISINDigits i = ISINDigits.A; i <= ISINDigits.Z; i++)
                {
                    if (c.ToString().ToUpper() == i.ToString())
                    {
                        digit = (int)i;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
                return digit;
            }
        }


        /// <summary>
        /// Checks a string for valid ISIN format
        /// </summary>
        /// <param name="isin">An ISIN with or without check digit</param>
        /// <returns>True if ISIN is valid formatted</returns>
        /// <remarks>If check digit exists, it will be ignored, but in this case the string must be a digit to be a valid ISIN. The check digit will be calculated seperatly.</remarks>
        public static bool IsValidFormatted(string isin)
        {
            if (isin.Length == 11 | isin.Length == 12)
            {
                if (char.IsLetter(isin[0]) & char.IsLetter(isin[1]))
                {
                    for (int i = 2; i <= 10; i++)
                    {
                        if (!char.IsLetterOrDigit(isin[i]))
                            return false;
                    }
                    return !(isin.Length == 12 && !char.IsDigit(isin[11]));
                }
            }
            return false;
        }
    

        private enum ISINDigits
        {
            A = 10,
            B = 11,
            C = 12,
            D = 13,
            E = 14,
            F = 15,
            G = 16,
            H = 17,
            I = 18,
            J = 19,
            K = 20,
            L = 21,
            M = 22,
            N = 23,
            O = 24,
            P = 25,
            Q = 26,
            R = 27,
            S = 28,
            T = 29,
            U = 30,
            V = 31,
            W = 32,
            X = 33,
            Y = 34,
            Z = 35
        }

    }
}
