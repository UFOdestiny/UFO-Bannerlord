using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

public static class L10N
{
    public static class Keys
    {
        public const string Global = "Global";

        public const string ModName = "ModName";

        public const string CombatPlayerGroupName = "Combat_Player";

        public const string CombatPartyGroupName = "Combat_Party";

        public const string CombatAlliesGroupName = "Combat_Allies";

        public const string CombatEnemiesGroupName = "Combat_Enemies";

        public const string CombatMiscGroupName = "Combat_Misc";

        public const string GeneralGroupName = "General";

        public const string MapGroupName = "Map";

        public const string InventoryGroupName = "Inventory";

        public const string PartyGroupName = "Party";

        public const string ClanGroupName = "Clan";

        public const string KingdomGroupName = "Kingdom";

        public const string ExperienceGroupName = "Experience";

        public const string SiegesGroupName = "Sieges";

        public const string ArmyGroupName = "Army";

        public const string SmithingGroupName = "Smithing";

        public const string SettlementsGroupName = "Settlements";

        public const string CharactersGroupName = "Characters";

        public const string WorkshopsGroupName = "Workshops";
    }

    private static Dictionary<string, string> Values;

    static L10N()
    {
        Values = new Dictionary<string, string>();
        string uri = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "L10N.resx");
        XDocument xDocument = XDocument.Load(uri);
        XElement xElement = xDocument.Element("root");
        IEnumerable<XElement> enumerable = xElement.Descendants("data");
        foreach (XElement item in enumerable)
        {
            string value = item.Attribute("name").Value;
            string value2 = item.Element("value").Value;
            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(value2))
            {
                Values.Add(value, value2);
            }
        }
    }

    public static string GetText(string key)
    {
        string value;
        return Values.TryGetValue(key, out value) ? value : key;
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
