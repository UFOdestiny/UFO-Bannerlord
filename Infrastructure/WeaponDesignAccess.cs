using System;
using System.Reflection;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign;
using TaleWorlds.Core;

namespace UFO.Infrastructure;

internal static class WeaponDesignAccess
{
    private const BindingFlags InstancePrivate = BindingFlags.Instance | BindingFlags.NonPublic;

    private static readonly MethodInfo SetSecondaryUsageIndex = RequireMethod("TrySetSecondaryUsageIndex", typeof(int));
    private static readonly MethodInfo RefreshWeaponStats = RequireMethod("RefreshStats");
    private static readonly FieldInfo CraftingBehavior = RequireField("_craftingBehavior", typeof(ICraftingCampaignBehavior));
    private static readonly PropertyInfo Handling = RequireWeaponProperty("Handling");
    private static readonly PropertyInfo SwingDamage = RequireWeaponProperty("SwingDamage");
    private static readonly PropertyInfo SwingSpeed = RequireWeaponProperty("SwingSpeed");
    private static readonly PropertyInfo ThrustDamage = RequireWeaponProperty("ThrustDamage");
    private static readonly PropertyInfo ThrustSpeed = RequireWeaponProperty("ThrustSpeed");

    internal static void SetUsageIndex(WeaponDesignVM weaponDesign, int usageIndex) =>
        SetSecondaryUsageIndex.Invoke(weaponDesign, new object[] { usageIndex });

    internal static void RefreshStats(WeaponDesignVM weaponDesign) =>
        RefreshWeaponStats.Invoke(weaponDesign, Array.Empty<object>());

    internal static ICraftingCampaignBehavior GetCraftingBehavior(WeaponDesignVM weaponDesign) =>
        (ICraftingCampaignBehavior)CraftingBehavior.GetValue(weaponDesign);

    internal static void ApplyStatBonuses(WeaponComponentData weapon, int handling, int swingDamage, int swingSpeed, int thrustDamage, int thrustSpeed)
    {
        Handling.SetValue(weapon, weapon.Handling + handling);
        SwingDamage.SetValue(weapon, weapon.SwingDamage + swingDamage);
        SwingSpeed.SetValue(weapon, weapon.SwingSpeed + swingSpeed);
        ThrustDamage.SetValue(weapon, weapon.ThrustDamage + thrustDamage);
        ThrustSpeed.SetValue(weapon, weapon.ThrustSpeed + thrustSpeed);
    }

    private static MethodInfo RequireMethod(string name, params Type[] parameterTypes) =>
        typeof(WeaponDesignVM).GetMethod(name, InstancePrivate, null, parameterTypes, null)
        ?? throw new MissingMethodException(typeof(WeaponDesignVM).FullName, name);

    private static FieldInfo RequireField(string name, Type fieldType)
    {
        var field = typeof(WeaponDesignVM).GetField(name, InstancePrivate);
        if (field is null || field.FieldType != fieldType)
            throw new MissingFieldException(typeof(WeaponDesignVM).FullName, name);
        return field;
    }

    private static PropertyInfo RequireWeaponProperty(string name)
    {
        var property = typeof(WeaponComponentData).GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
        if (property?.PropertyType != typeof(int) || property.SetMethod is null)
            throw new MissingMemberException(typeof(WeaponComponentData).FullName, name);
        return property;
    }
}
