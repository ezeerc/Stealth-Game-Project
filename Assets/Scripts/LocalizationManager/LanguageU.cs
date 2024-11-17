using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LanguageU
{
    public static Dictionary<LocalizationLanguage, Dictionary<string, string>> LoadTranslation(DataLocalization[] data)
    {

        var tempDict = new Dictionary<LocalizationLanguage, Dictionary<string, string>>();

        for (int i = 0; i < data.Length; i++)
        {
            var tempData = new Dictionary<string, string>();

            foreach (var item in data[i].languageData)
            {
                var f = item.text.Split(';');

                foreach (var d in f)
                {
                    var c = d.Replace('{', ' ').
                              Replace('}', ' ').
                              Replace('"', ' ').
                              Split(':');

                    if (c.Length == 2 && !tempData.ContainsKey(c[0]))
                        tempData.Add(c[0].Trim(), c[1].Trim()); // .Trim() nos saca los espacios al principio y al final
                }
            }

            tempDict.Add(data[i].language, tempData);
        }

        return tempDict;
    }
}
