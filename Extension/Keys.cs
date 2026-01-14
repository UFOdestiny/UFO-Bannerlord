using System.Collections.Generic;
using System.Linq;
using TaleWorlds.InputSystem;
namespace UFO.Extension
{
    public static class Keys
    {
        private static readonly List<InputKey> PressedKeys = new List<InputKey>();

        public static bool IsKeyPressed(params InputKey[] keys)
        {
            if (keys.All((InputKey key) => key.IsDown()))
            {
                if (keys.All((InputKey key) => PressedKeys.Contains(key)))
                {
                    return false;
                }
                foreach (InputKey item in keys)
                {
                    if (!PressedKeys.Contains(item))
                    {
                        PressedKeys.Add(item);
                    }
                }
            }
            else if (keys.Any((InputKey key) => key.IsDown()))
            {
                foreach (InputKey inputKey in keys)
                {
                    if (!inputKey.IsDown() && PressedKeys.Contains(inputKey))
                    {
                        PressedKeys.Remove(inputKey);
                    }
                }
            }
            else
            {
                foreach (InputKey item2 in keys)
                {
                    if (PressedKeys.Contains(item2))
                    {
                        PressedKeys.Remove(item2);
                    }
                }
            }
            return keys.All((InputKey key) => PressedKeys.Contains(key));
        }
    }
}