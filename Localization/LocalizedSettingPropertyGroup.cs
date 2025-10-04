using System;
using MCM.Abstractions;

namespace UFO.Localization
{
    public class LocalizedSettingPropertyGroup : Attribute, IPropertyGroupDefinition
    {
        public string GroupName { get; }

        public int GroupOrder { get; set; }

        public LocalizedSettingPropertyGroup(string groupName)
        {
            try
            {
                GroupName = L10N.GetText(groupName + "_GroupName");
            }
            catch
            {
                GroupName = groupName;
            }
        }
    }
}