using MCM.Common;
using System;
using TaleWorlds.Core;
using UFO.Localization;
using UFO.Setting;

namespace UFO.Extension
{
    public static class EnumExtensions
    {
        public static bool TryGetAgentState(this Setting.KnockoutOrKilled state, out AgentState result)
        {
            switch (state)
            {
                case Setting.KnockoutOrKilled.Default:
                    result = AgentState.None;
                    return false;
                case Setting.KnockoutOrKilled.Knockout:
                    result = AgentState.Unconscious;
                    return true;
                case Setting.KnockoutOrKilled.Killed:
                    result = AgentState.Killed;
                    return true;
                default:
                    throw new ArgumentOutOfRangeException("state", state, null);
            }
        }

        public static T GetValue<T>(this Dropdown<LocalizedDropdownValue<T>> dropdown) where T : System.Enum
        {
            return (T)(object)dropdown.SelectedIndex;
        }
    }


}