using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UFO.Localization;
using UFO.Setting;


public static class L10N
{
    public static class Keys
    {
        public const string Global = "Global";

        public const string ModName = "UFO's Cheat";

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
