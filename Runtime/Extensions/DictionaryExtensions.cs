using System.Collections.Generic;

namespace GameLokal.Toolkit
{
    public static class DictionaryExtensions
    {
        public static Dictionary<string, object> TryAddKeyValuePair(this Dictionary<string, object> dict, string key, object value)
        {
            if (dict == null) dict = new Dictionary<string, object>();

            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }

            return dict;
        }     
    }
}