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
                return "L10N_English.resx";
            case Setting_Language.Chinese:
                return "L10N_Chinese.resx";
            case Setting_Language.Other:
                return "L10N_Other.resx";
            default:
                return "L10N.resx";
        }
    }
}