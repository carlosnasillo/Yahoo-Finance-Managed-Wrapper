using System;
using System.Collections.Generic;
using System.Text;

namespace MaasOne
{
    public abstract class LocalizationManager
    {
        protected static event EventHandler OnInit;

        public static List<string> CsvSources = new List<string>();
        private static Dictionary<string, int> mLanguageIndices = new Dictionary<string, int>();
        private static Dictionary<string, string[]> mSource = new Dictionary<string, string[]>();
        public static string GetValue(string id) { return GetValue(id, string.Empty); }
        public static string GetValue(string id, System.Globalization.CultureInfo culture)
        {
            string cult = culture != null ? culture.Name.ToString() : string.Empty;
            return GetValue(id, cult);
        }
        public static string GetValue(string id, string culture)
        {
            int langInd = (culture != string.Empty && mLanguageIndices.ContainsKey(culture)) ? mLanguageIndices[culture] : 0;
            if (mSource.ContainsKey(id))
            {
                string[] arr = mSource[id];
                if (arr[langInd] != string.Empty)
                {
                    return arr[langInd];
                }
                else if (arr.Length > 0)
                {
                    return arr[0];
                }
                else
                {
                    return id;
                }
            }
            else
            {
                return id;
            }
        }
        static LocalizationManager()
        {
            if (OnInit != null) OnInit(null,new EventArgs());
            Init();
        }
        public static void Init()
        {
            mSource.Clear();
            foreach (string csv in CsvSources)
            {
                AddRessource(csv);
            }
        }

        private static void AddRessource(string csv)
        {
            string[] lines = csv.Split('\n');
            if (lines.Length > 1)
            {
                string[] cultures = lines[0].TrimEnd().Split(';');
                Dictionary<string, int> localLangIndices = new Dictionary<string, int>();
                for (int i = 1; i < cultures.Length; i++)
                {
                    if (!mLanguageIndices.ContainsKey(cultures[i]))
                    {
                        mLanguageIndices.Add(cultures[i], mLanguageIndices.Count);
                    }
                }
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(';');
                    if (parts.Length == cultures.Length)
                    {
                        string[] lstValues = new string[mLanguageIndices.Count];
                        for (int n = 1; n < parts.Length; n++)
                        {
                            lstValues[mLanguageIndices[cultures[n]]] = parts[n].Trim();
                        }
                        mSource.Add(parts[0], lstValues);
                    }
                }

            }

        }
    }
}
