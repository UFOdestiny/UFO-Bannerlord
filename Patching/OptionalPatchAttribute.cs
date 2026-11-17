using System;

namespace UFO.Patching;

/// <summary>A late-bound DLC postfix, also readable by the offline API audit.</summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
internal sealed class OptionalPatchAttribute : Attribute
{
    internal string TypeName { get; }
    internal string MethodName { get; }
    internal Type FirstArgumentType { get; }

    public OptionalPatchAttribute(string typeName, string methodName)
    {
        TypeName = typeName;
        MethodName = methodName;
    }

    public OptionalPatchAttribute(string typeName, string methodName, Type firstArgumentType)
        : this(typeName, methodName) => FirstArgumentType = firstArgumentType;
}
