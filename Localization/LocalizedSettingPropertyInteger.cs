using MCM.Abstractions;

namespace UFO.Localization
{
    public sealed class LocalizedSettingPropertyInteger : LocalizedSettingProperty, IPropertyDefinitionWithMinMax, IPropertyDefinitionWithFormat
    {
        public string ValueFormat { get; } = "0";

        public decimal MinValue { get; }

        public decimal MaxValue { get; }

        public LocalizedSettingPropertyInteger(string settingName, int minValue, int maxValue)
            : base(settingName)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}