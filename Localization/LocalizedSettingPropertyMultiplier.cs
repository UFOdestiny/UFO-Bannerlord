using MCM.Abstractions;

namespace UFO.Localization;

public sealed class LocalizedSettingPropertyMultiplier : LocalizedSettingProperty, IPropertyDefinitionWithMinMax, IPropertyDefinitionWithFormat
{
    public string ValueFormat { get; } = "0.000";

    public decimal MinValue { get; } = 0m;

    public decimal MaxValue { get; } = 10m;

    public LocalizedSettingPropertyMultiplier(string settingName)
        : base(settingName)
    {
    }
}
