using MCM.Abstractions;

namespace UFO.Localization
{
    public sealed class LocalizedSettingPropertyPercent : LocalizedSettingProperty, IPropertyDefinitionWithMinMax, IPropertyDefinitionWithFormat
    {
        public string ValueFormat { get; } = "0.00\\%";

        public decimal MinValue { get; } = default(decimal);

        public decimal MaxValue { get; } = 100m;

        public LocalizedSettingPropertyPercent(string settingName)
            : base(settingName)
        {
        }
    }
}