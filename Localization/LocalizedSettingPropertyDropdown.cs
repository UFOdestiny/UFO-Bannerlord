using MCM.Abstractions;

namespace UFO.Localization
{
    public class LocalizedSettingPropertyDropdown : LocalizedSettingProperty, IPropertyDefinitionDropdown, IPropertyDefinitionBase
    {
        public int SelectedIndex { get; }

        public LocalizedSettingPropertyDropdown(string settingName, int defaultIndex)
            : base(settingName)
        {
            SelectedIndex = defaultIndex;
        }

        public LocalizedSettingPropertyDropdown(string settingName, object defaultIndex)
            : base(settingName)
        {
            SelectedIndex = (int)defaultIndex;
        }
    }
}