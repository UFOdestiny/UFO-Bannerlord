using HarmonyLib;
using JetBrains.Annotations;
using System;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using UFO;
using UFO.Extension;
using UFO.Setting;

namespace UFO.Patch;

[HarmonyPatch(typeof(DefaultWorkshopModel), "GetCostForPlayer")]
public static class WorkshopBuyingCostPercentage
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetCostForPlayer(ref Workshop workshop, ref int __result)
    {
        try
        {
            if (SettingsManager.WorkshopBuyingCostPercentage.IsChanged)
            {
                float num = SettingsManager.WorkshopBuyingCostPercentage.Value / 100f;
                __result = (int)((float)__result * num);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(WorkshopBuyingCostPercentage));
        }
    }
}


[HarmonyPatch(typeof(Workshop), "Expense", MethodType.Getter)]
public static class WorkshopDailyExpensePercentage
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void Expense(ref Workshop __instance, ref int __result)
    {
        try
        {
            if (SettingsManager.WorkshopDailyExpensePercentage.IsChanged && __instance.Owner.IsPlayer())
            {
                float num = SettingsManager.WorkshopDailyExpensePercentage.Value / 100f;
                __result = (int)((float)__result * num);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(WorkshopDailyExpensePercentage));
        }
    }
}


[HarmonyPatch(typeof(DefaultWorkshopModel), "GetCostForNotable")]
public static class WorkshopSellingCostMultiplier
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetSellingCost(ref Workshop workshop, ref int __result)
    {
        try
        {
            if (SettingsManager.WorkshopSellingCostMultiplier.IsChanged)
            {
                __result = (int)((float)__result * SettingsManager.WorkshopSellingCostMultiplier.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(WorkshopSellingCostMultiplier));
        }
    }
}
