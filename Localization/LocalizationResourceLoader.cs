using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace UFO.Localization;

internal static class LocalizationResourceLoader
{
    internal static void Overlay(Dictionary<string, string> values, string directory, string fileName, bool required)
    {
        var path = Path.Combine(directory, fileName);
        if (!File.Exists(path))
        {
            if (required)
                throw new FileNotFoundException("The fallback localization resource was not found.", path);
            return;
        }

        var document = XDocument.Load(path);
        foreach (var item in document.Root?.Descendants("data") ?? Enumerable.Empty<XElement>())
        {
            var key = item.Attribute("name")?.Value;
            var value = item.Element("value")?.Value;
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrEmpty(value))
                values[key] = value;
        }
    }
}
