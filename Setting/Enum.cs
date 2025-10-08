namespace UFO.Setting;

public enum KnockoutOrKilled
{
    Default,
    Knockout,
    Killed
}

public enum AutoChoosePerk_Type
{
    Clan,
    Player,
    All,
    No
}

public enum Setting_Language
{
    English,
    Chinese,
    Other
}

public static class EnumExtensions
{
    public static string ToLanguage(Setting_Language s)
    {
        switch (s)
        {
            case Setting_Language.English:
                return "English.resx";
            case Setting_Language.Chinese:
                return "Chinese.resx";
            case Setting_Language.Other:
                return "Other.resx";
            default:
                return "English.resx";
        }
    }
}