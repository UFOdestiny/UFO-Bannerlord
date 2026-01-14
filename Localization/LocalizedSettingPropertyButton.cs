using MCM.Abstractions;

namespace UFO.Localization
{
    public class LocalizedSettingPropertyButton : LocalizedSettingProperty, IPropertyDefinitionButton, IPropertyDefinitionBase
    {
        public string Content { get; }

        public LocalizedSettingPropertyButton(string settingName, string buttonContentKey)
            : base(settingName)
        {
            Content = L10N.GetText(buttonContentKey);
        }
    }
}