using MCM.Common;
using UFO.Localization;

namespace UFO.Extension
{
    public static class EnumExtensions
    {

        public static T GetValue<T>(this Dropdown<LocalizedDropdownValue<T>> dropdown) where T : System.Enum
        {
            return (T)(object)dropdown.SelectedIndex;
        }
    }
}
