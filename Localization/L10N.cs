using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UFO.Localization;
using UFO.Setting;

public static class L10N
{

    private static readonly Dictionary<string, string> Values = new Dictionary<string, string>(StringComparer.Ordinal);

    public static void LoadLanguage()
    {
        Values.Clear();
        var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        LocalizationResourceLoader.Overlay(Values, directory, "English.resx", required: true);
        string requestedLanguage = EnumExtensions.ToLanguage(SettingsManager.LanguageSetting.Value);
        if (!string.Equals(requestedLanguage, "English.resx", StringComparison.OrdinalIgnoreCase))
            LocalizationResourceLoader.Overlay(Values, directory, requestedLanguage, required: false);
    }

    public static string GetText(string key)
    {
        return Values.TryGetValue(key, out string value) ? value : key;
    }

    public static string GetTextFormat(string key, params object[] formatValues)
    {
        if (!Values.TryGetValue(key, out var value))
        {
            return key;
        }
        for (int i = 0; i < formatValues.Length; i++)
        {
            value = value.Replace($"{{{i}}}", formatValues[i].ToString());
        }
        return value;
    }
}
