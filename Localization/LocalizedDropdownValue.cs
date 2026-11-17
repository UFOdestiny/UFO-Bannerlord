using MCM.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UFO.Localization
{
    public struct LocalizedDropdownValue<T> : IEquatable<LocalizedDropdownValue<T>> where T : Enum
    {
        public T Value { get; set; }

        public string DisplayName
        {
            get
            {
                string key = "Enum_" + typeof(T).Name + "_" + Enum.GetName(typeof(T), Value);
                return L10N.GetText(key);
            }
        }

        public LocalizedDropdownValue(T value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public bool Equals(LocalizedDropdownValue<T> other) => EqualityComparer<T>.Default.Equals(Value, other.Value);

        public override bool Equals(object obj) => obj is LocalizedDropdownValue<T> other && Equals(other);

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }

        public static Dropdown<LocalizedDropdownValue<T>> GenerateDropdown(T selected)
        {
            LocalizedDropdownValue<T>[] array = (from T value in Enum.GetValues(typeof(T))
                                                 select new LocalizedDropdownValue<T>(value)).ToArray();
            int selectedIndex = Array.IndexOf(array, array.First((LocalizedDropdownValue<T> x) => x.Value.Equals(selected)));
            return new Dropdown<LocalizedDropdownValue<T>>(array, selectedIndex);
        }
    }
}
