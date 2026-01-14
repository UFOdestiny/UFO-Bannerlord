using System;
using System.Reflection;

internal class ReflectUtils
{
    public static BindingFlags GetBindingFlags()
    {
        return BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
    }

    public static T ReflectField<T>(string key, object instance)
    {
        T result = default(T);
        FieldInfo field = instance.GetType().GetField(key, GetBindingFlags());
        if (null != field && field.GetValue(instance) != null)
        {
            return (T)field.GetValue(instance);
        }
        return result;
    }

    public static object ReflectFieldAndSetValue(string key, object value, object instance)
    {
        object result = null;
        FieldInfo field = instance.GetType().GetField(key, GetBindingFlags());
        if (null != field && field.GetValue(instance) != null)
        {
            field.SetValue(instance, value);
        }
        return result;
    }

    public static object ReflectPropertyAndSetValue(string key, object value, object instance)
    {
        object result = null;
        PropertyInfo property = instance.GetType().GetProperty(key, GetBindingFlags());
        if (null != property)
        {
            property.SetValue(instance, value);
        }
        return result;
    }

    public static T ReflectProperty<T>(string key, object instance)
    {
        T result = default(T);
        PropertyInfo property = instance.GetType().GetProperty(key, GetBindingFlags());
        if (null != property)
        {
            return (T)property.GetValue(instance);
        }
        return result;
    }

    public static void ReflectMethodAndInvoke(string mothodName, object instance, object[] paramObjects)
    {
        MethodInfo method = instance.GetType().GetMethod(mothodName, GetBindingFlags());
        if (null != method)
        {
            method.Invoke(instance, paramObjects);
        }
    }

    public static T ReflectGetStaticField<T, K>(string FieldName)
    {
        T result = default(T);
        FieldInfo[] fields = typeof(K).GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        FieldInfo[] array = fields;
        foreach (FieldInfo fieldInfo in array)
        {
            if (fieldInfo.Name == "_idStringBuilder")
            {
                result = (T)fieldInfo.GetValue(null);
            }
        }
        return result;
    }

    public static void ReflectSetStaticField<T, K>(string FieldName, object value)
    {
        FieldInfo[] fields = typeof(K).GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        FieldInfo[] array = fields;
        foreach (FieldInfo fieldInfo in array)
        {
            if (fieldInfo.Name == "_idStringBuilder")
            {
                fieldInfo.SetValue(null, value);
            }
        }
    }

    public static T DeepCopy<T>(T obj)
    {
        if (obj is string || obj.GetType().IsValueType)
        {
            return obj;
        }
        object obj2 = Activator.CreateInstance(obj.GetType());
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        FieldInfo[] array = fields;
        foreach (FieldInfo fieldInfo in array)
        {
            try
            {
                fieldInfo.SetValue(obj2, DeepCopy(fieldInfo.GetValue(obj)));
            }
            catch
            {
            }
        }
        return (T)obj2;
    }
}
