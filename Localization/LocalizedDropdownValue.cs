using System;
using System.Collections.Generic;
using System.Linq;
using MCM.Common;

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

        public bool Equals(T other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other);
        }

        public bool Equals(LocalizedDropdownValue<T> other)
        {
            if ((object)other == null)
            {
                return false;
            }
            if ((object)this == (object)other)
            {
                return true;
            }
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if ((object)this == obj)
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((LocalizedDropdownValue<T>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }

        public static bool operator ==(LocalizedDropdownValue<T> left, T right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LocalizedDropdownValue<T> left, T right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(LocalizedDropdownValue<T> left, LocalizedDropdownValue<T> right)
        {
            return object.Equals(left, right);
        }

        public static bool operator !=(LocalizedDropdownValue<T> left, LocalizedDropdownValue<T> right)
        {
            return !object.Equals(left, right);
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