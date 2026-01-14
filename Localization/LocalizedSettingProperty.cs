using MCM.Abstractions;
using System;

namespace UFO.Localization
{
    public abstract class LocalizedSettingProperty : Attribute, IPropertyDefinitionBase
    {
        public string DisplayName { get; }

        public int Order { get; } = -1;

        public bool RequireRestart { get; set; } = false;

        public string HintText { get; }

        public LocalizedSettingProperty(string settingName)
        {
            try
            {
                DisplayName = L10N.GetText(settingName + "_Name");
                HintText = L10N.GetText(settingName + "_Desc");
            }
            catch
            {
                DisplayName = settingName;
                HintText = "ERROR: Could not load description from localization resource.";
            }
        }
    }
}