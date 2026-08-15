using MCM.Abstractions;

namespace UFO.Localization;

public sealed class LocalizedSettingPropertyText : LocalizedSettingProperty, IPropertyDefinitionText
{
    public LocalizedSettingPropertyText(string settingName) : base(settingName) { }
}
