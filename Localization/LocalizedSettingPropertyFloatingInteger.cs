using System;
using MCM.Abstractions;

namespace UFO.Localization
{
    public sealed class LocalizedSettingPropertyFloatingInteger : LocalizedSettingProperty, IPropertyDefinitionWithMinMax, IPropertyDefinitionWithFormat
    {
        public string ValueFormat { get; } = "0.00";

        public decimal MinValue { get; }

        public decimal MaxValue { get; }

        public LocalizedSettingPropertyFloatingInteger(string settingName, float minValue, float maxValue)
            : base(settingName)
        {
            MinValue = Convert.ToDecimal(minValue);
            MaxValue = Convert.ToDecimal(maxValue);
        }
    }
}