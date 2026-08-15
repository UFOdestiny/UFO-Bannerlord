using HarmonyLib;
using JetBrains.Annotations;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using UFO.Extension;
using UFO.Infrastructure;
using UFO.Setting;

namespace UFO.Patch;

[HarmonyPatch(typeof(WeaponDesignVM), "GetResultPropertyList")]
internal class CraftedWeaponModifierBonus
{
    public static bool Prefix(CraftingSecondaryUsageItemVM usageItem, ref MBBindingList<WeaponDesignResultPropertyItemVM> __result, ref WeaponDesignVM __instance)
    {
        MBBindingList<WeaponDesignResultPropertyItemVM> mBBindingList = new MBBindingList<WeaponDesignResultPropertyItemVM>();
        if (usageItem == null)
        {
            __result = mBBindingList;
            return false;
        }
        int usageIndex = usageItem.UsageIndex;
        WeaponDesignAccess.SetUsageIndex(__instance, usageIndex);
        WeaponDesignAccess.RefreshStats(__instance);
        ICraftingCampaignBehavior craftingCampaignBehavior = WeaponDesignAccess.GetCraftingBehavior(__instance);
        ItemModifier currentItemModifier = craftingCampaignBehavior.GetCurrentItemModifier();
        foreach (CraftingListPropertyItem primaryProperty in __instance.PrimaryPropertyList)
        {
            float changeAmount = 0f;
            bool showFloatingPoint = primaryProperty.Type == CraftingTemplate.CraftingStatTypes.Weight;
            if (currentItemModifier != null)
            {
                float num = primaryProperty.PropertyValue;
                if (primaryProperty.Type == CraftingTemplate.CraftingStatTypes.SwingDamage)
                {
                    num = currentItemModifier.ModifyDamage((int)primaryProperty.PropertyValue);
                    num += (float)SettingsManager.CraftedWeaponSwingDamageBonus.Value;
                }
                else if (primaryProperty.Type == CraftingTemplate.CraftingStatTypes.SwingSpeed)
                {
                    num = currentItemModifier.ModifySpeed((int)primaryProperty.PropertyValue);
                    num += (float)SettingsManager.CraftedWeaponSwingSpeedBonus.Value;
                }
                else if (primaryProperty.Type == CraftingTemplate.CraftingStatTypes.ThrustDamage)
                {
                    num = currentItemModifier.ModifyDamage((int)primaryProperty.PropertyValue);
                    num += (float)SettingsManager.CraftedWeaponThrustDamageBonus.Value;
                }
                else if (primaryProperty.Type == CraftingTemplate.CraftingStatTypes.ThrustSpeed)
                {
                    num = currentItemModifier.ModifySpeed((int)primaryProperty.PropertyValue);
                    num += (float)SettingsManager.CraftedWeaponThrustSpeedBonus.Value;
                }
                else if (primaryProperty.Type == CraftingTemplate.CraftingStatTypes.Handling)
                {
                    num = currentItemModifier.ModifySpeed((int)primaryProperty.PropertyValue);
                    num += (float)SettingsManager.CraftedWeaponHandlingBonus.Value;
                }
                if (num != primaryProperty.PropertyValue)
                {
                    changeAmount = num - primaryProperty.PropertyValue;
                }
            }
            if (__instance.IsInOrderMode)
            {
                mBBindingList.Add(new WeaponDesignResultPropertyItemVM(primaryProperty.Description, primaryProperty.PropertyValue, primaryProperty.TargetValue, changeAmount, showFloatingPoint, primaryProperty.IsExceedingBeneficial));
            }
            else
            {
                mBBindingList.Add(new WeaponDesignResultPropertyItemVM(primaryProperty.Description, primaryProperty.PropertyValue, changeAmount, showFloatingPoint));
            }
        }
        __result = mBBindingList;
        return false;
    }
}


[HarmonyPatch(typeof(CampaignEvents), "OnNewItemCrafted", new Type[]
{
    typeof(ItemObject),
    typeof(ItemModifier),
    typeof(bool)
})]
internal class CraftedWeaponModifierSave
{
    public static bool Prefix(ref ItemObject itemObject, ref ItemModifier overriddenItemModifier, ref bool isCraftingOrderItem)
    {
        int handling = itemObject.Weapons[0].Handling + SettingsManager.CraftedWeaponHandlingBonus.Value;
        int swingDamage = itemObject.Weapons[0].SwingDamage + SettingsManager.CraftedWeaponSwingDamageBonus.Value;
        int swingSpeed = itemObject.Weapons[0].SwingSpeed + SettingsManager.CraftedWeaponSwingSpeedBonus.Value;
        int thrustDamage = itemObject.Weapons[0].ThrustDamage + SettingsManager.CraftedWeaponThrustDamageBonus.Value;
        int thrustSpeed = itemObject.Weapons[0].ThrustSpeed + SettingsManager.CraftedWeaponThrustSpeedBonus.Value;
        string stringId = itemObject.StringId;
        SavingWeaponProperties.WeaponPropertiesList.Add(new SavingWeaponProperties.WeaponProperties(null)
        {
            StringId = stringId,
            Handling = handling,
            SwingDamage = swingDamage,
            SwingSpeed = swingSpeed,
            ThrustDamage = thrustDamage,
            ThrustSpeed = thrustSpeed
        });
        for (int i = 0; i < itemObject.Weapons.Count; i++)
        {
            WeaponDesignAccess.ApplyStatBonuses(
                itemObject.Weapons[i],
                SettingsManager.CraftedWeaponHandlingBonus.Value,
                SettingsManager.CraftedWeaponSwingDamageBonus.Value,
                SettingsManager.CraftedWeaponSwingSpeedBonus.Value,
                SettingsManager.CraftedWeaponThrustDamageBonus.Value,
                SettingsManager.CraftedWeaponThrustSpeedBonus.Value);
        }
        return true;
    }
}

[HarmonyPatch(typeof(DefaultSmithingModel), "GetSmithingCostsForWeaponDesign")]
public static class SmithingCostPercentage
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetSmithingCostsForWeaponDesign(WeaponDesign weaponDesign, ref int[] __result)
    {
        try
        {
            if (SettingsManager.SmithingCostPercentage.IsChanged)
            {
                float num = SettingsManager.SmithingCostPercentage.Value / 100f;
                for (int i = 0; i < __result.Length; i++)
                {
                    int num2 = (int)Math.Round(num * (float)__result[i]);
                    __result[i] = num2;
                }
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(SmithingCostPercentage));
        }
    }
}



[HarmonyPatch(typeof(DefaultSmithingModel), "GetCraftingPartDifficulty")]
public static class SmithingDifficultyPercentage
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetCraftingPartDifficulty(CraftingPiece craftingPiece, ref int __result)
    {
        try
        {
            if (SettingsManager.SmithingDifficultyPercentage.IsChanged)
            {
                float num = SettingsManager.SmithingDifficultyPercentage.Value / 100f;
                int num2 = (int)Math.Round(num * (float)__result);
                __result = num2;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(SmithingDifficultyPercentage));
        }
    }
}


[HarmonyPatch(typeof(DefaultSmithingModel), "GetEnergyCostForRefining")]
public static class SmithingEnergyCostPercentageRefining
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetEnergyCostForRefining(ref Crafting.RefiningFormula refineFormula, Hero hero, ref int __result)
    {
        try
        {
            if (hero.PartyBelongedTo.IsPlayerParty() && SettingsManager.SmithingEnergyCostPercentage.IsChanged)
            {
                float num = SettingsManager.SmithingEnergyCostPercentage.Value / 100f;
                int num2 = (int)Math.Round(num * (float)__result);
                __result = num2;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(SmithingEnergyCostPercentageRefining));
        }
    }
}


[HarmonyPatch(typeof(DefaultSmithingModel), "GetEnergyCostForSmelting")]
public static class SmithingEnergyCostPercentageSmelting
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetEnergyCostForSmelting(ItemObject item, Hero hero, ref int __result)
    {
        try
        {
            if (hero.PartyBelongedTo.IsPlayerParty() && SettingsManager.SmithingEnergyCostPercentage.IsChanged)
            {
                float num = SettingsManager.SmithingEnergyCostPercentage.Value / 100f;
                int num2 = (int)Math.Round(num * (float)__result);
                __result = num2;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(SmithingEnergyCostPercentageSmelting));
        }
    }
}


[HarmonyPatch(typeof(DefaultSmithingModel), "GetEnergyCostForSmithing")]
public static class SmithingEnergyCostPercentageSmithing
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetEnergyCostForSmithing(ItemObject item, Hero hero, ref int __result)
    {
        try
        {
            if (hero.PartyBelongedTo.IsPlayerParty() && SettingsManager.SmithingEnergyCostPercentage.IsChanged)
            {
                float num = SettingsManager.SmithingEnergyCostPercentage.Value / 100f;
                int num2 = (int)Math.Round(num * (float)__result);
                __result = num2;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(SmithingEnergyCostPercentageSmithing));
        }
    }
}


[HarmonyPatch(typeof(CraftingCampaignBehavior), "IsOpened")]
public static class UnlockAllParts
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void IsOpened(CraftingPiece craftingPiece, ref bool __result)
    {
        try
        {
            if (SettingsManager.UnlockAllParts.IsChanged)
            {
                __result = true;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(UnlockAllParts));
        }
    }
}
