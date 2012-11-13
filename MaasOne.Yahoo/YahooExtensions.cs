using System;
using System.Collections.Generic;
using System.Text;


namespace MaasOne
{  
    public partial class YahooExtensions
    {
        public static byte[] StringToAscii(string s)
        {
            return System.Text.Encoding.ASCII.GetBytes(s);
        }
    }
}
