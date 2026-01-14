using MCM.Abstractions;

namespace UFO.Localization
{
    public sealed class LocalizedSettingPropertyBool : LocalizedSettingProperty, IPropertyDefinitionBool, IPropertyDefinitionBase
    {
        public LocalizedSettingPropertyBool(string settingName)
            : base(settingName)
        {
        }
    }
}