using System;
using System.Collections.Generic;
using System.Text;

namespace MaasOne
{
    internal class YahooLocalizationManager
    {
        public static bool AlreadyUpdated { get; set; }
        private static bool addedSource = false;

        public static string GetValue(string id) { return LocalizationManager.GetValue(id); }
        public static string GetValue(string id, System.Globalization.CultureInfo culture) { return LocalizationManager.GetValue(id, culture); }
        public static string GetValue(string id, string culture) { return LocalizationManager.GetValue(id, culture); }


        static YahooLocalizationManager()
        {
            if (!addedSource)
            {
                AlreadyUpdated = true;
                LocalizationManager.CsvSources.Add(Properties.Resources.localization);
                addedSource = true;
            }
            LocalizationManager.Init();
        }

    }
}
