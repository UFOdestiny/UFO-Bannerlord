using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using UFO.Extension;
using UFO.Setting;

namespace UFO.Patch;

[HarmonyPatch(typeof(Army), "DailyCohesionChange", MethodType.Getter)]
public static class ACLP
{
    public static void Postfix(ref Army __instance, ref float __result)
    {
        try
        {
            if (__instance.IsPlayerArmy() && SettingsManager.ArmyCohesionLossPercentage.IsChanged)
            {
                float num = SettingsManager.ArmyCohesionLossPercentage.Value / 100f;
                __result *= num;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(ACLP));
        }
    }
}

[HarmonyPatch(typeof(DefaultMobilePartyFoodConsumptionModel), "CalculateDailyFoodConsumptionf")]
public static class AFCP
{
    public static void Postfix(MobileParty party, ExplainedNumber baseConsumption, ref ExplainedNumber __result)
    {
        try
        {
            if (party.IsPlayerArmy() && SettingsManager.ArmyFoodConsumptionPercentage.IsChanged)
            {
                __result.AddPercentage(SettingsManager.ArmyFoodConsumptionPercentage.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(AFCP));
        }
    }
}


[HarmonyPatch(typeof(Army), "DailyCohesionChange", MethodType.Getter)]
public static class FACLP
{
    public static void Postfix(ref Army __instance, ref float __result)
    {
        try
        {
            if (__instance.IsOfPlayerKingdom() && !__instance.IsPlayerArmy() && SettingsManager.FactionArmyCohesionLossPercentage.IsChanged)
            {
                float num = SettingsManager.FactionArmyCohesionLossPercentage.Value / 100f;
                __result *= num;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(FACLP));
        }
    }
}
