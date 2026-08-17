using MCM.Abstractions;

namespace UFO.Localization;

public sealed class LocalizedSettingPropertyMultiplierPercent : LocalizedSettingProperty, IPropertyDefinitionWithMinMax, IPropertyDefinitionWithFormat
{
    public string ValueFormat { get; } = "0\\%";

    public decimal MinValue { get; } = 0.1m;

    public decimal MaxValue { get; } = 3m;

    public LocalizedSettingPropertyMultiplierPercent(string settingName)
        : base(settingName)
    {
    }
}
