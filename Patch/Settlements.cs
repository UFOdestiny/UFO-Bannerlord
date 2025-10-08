using HarmonyLib;
using JetBrains.Annotations;
using SandBox.Tournaments.MissionLogics;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using UFO.Extension;
using UFO.Setting;
namespace UFO.Patch;


[HarmonyPatch(typeof(DefaultBuildingConstructionModel), "CalculateDailyConstructionPower")]
public static class ConstructionPowerMultiplier
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateDailyConstructionPower(ref Town town, ref bool includeDescriptions, ref ExplainedNumber __result)
    {
        try
        {
            if (town.IsPlayerTown() && SettingsManager.ConstructionPowerMultiplier.IsChanged)
            {
                __result.AddMultiplier(SettingsManager.ConstructionPowerMultiplier.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(ConstructionPowerMultiplier));
        }
    }
}



[HarmonyPatch(typeof(Town), "FoodChange", MethodType.Getter)]
public static class DailyFoodBonus
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void FoodChange(ref Town __instance, ref float __result)
    {
        try
        {
            if (__instance.IsPlayerTown() && SettingsManager.DailyFoodBonus.IsChanged)
            {
                __result += SettingsManager.DailyFoodBonus.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(DailyFoodBonus));
        }
    }
}



//[HarmonyPatch(typeof(Town), "GarrisonChange", MethodType.Getter)]
//public static class DailyGarrisonBonus
//{
//    [UsedImplicitly]
//    [HarmonyPostfix]
//    public static void GarrisonChange(ref Town __instance, ref int __result)
//    {
//        try
//        {
//            if (__instance.IsPlayerTown() && SettingsManager.DailyGarrisonBonus.IsChanged)
//            {
//                __result += SettingsManager.DailyGarrisonBonus.Value;
//            }
//        }
//        catch (Exception e)
//        {
//            SubModule.LogError(e, typeof(DailyGarrisonBonus));
//        }
//    }
//}



[HarmonyPatch(typeof(Village), "HearthChange", MethodType.Getter)]
public static class DailyHearthsBonus
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void HearthChange(ref Village __instance, ref float __result)
    {
        try
        {
            if (__instance.IsPlayerVillage() && SettingsManager.DailyHearthsBonus.IsChanged)
            {
                __result += SettingsManager.DailyHearthsBonus.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(DailyHearthsBonus));
        }
    }
}


[HarmonyPatch(typeof(Town), "LoyaltyChange", MethodType.Getter)]
public static class DailyLoyaltyBonus
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void LoyaltyChange(ref Town __instance, ref float __result)
    {
        try
        {
            if (__instance.IsPlayerTown() && SettingsManager.DailyLoyaltyBonus.IsChanged)
            {
                __result += SettingsManager.DailyLoyaltyBonus.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(DailyLoyaltyBonus));
        }
    }
}


[HarmonyPatch(typeof(Town), "MilitiaChange", MethodType.Getter)]
public static class DailyMilitiaBonusTown
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void MilitiaChange(ref Town __instance, ref float __result)
    {
        try
        {
            if (__instance.IsPlayerTown() && SettingsManager.DailyMilitiaBonus.IsChanged)
            {
                __result += SettingsManager.DailyMilitiaBonus.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(DailyMilitiaBonusTown));
        }
    }
}



[HarmonyPatch(typeof(Village), "MilitiaChange", MethodType.Getter)]
public static class DailyMilitiaBonusVillage
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void MilitiaChange(ref Village __instance, ref float __result)
    {
        try
        {
            if (__instance.IsPlayerVillage() && SettingsManager.DailyMilitiaBonus.IsChanged)
            {
                __result += SettingsManager.DailyMilitiaBonus.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(DailyMilitiaBonusVillage));
        }
    }
}


[HarmonyPatch(typeof(Town), "ProsperityChange", MethodType.Getter)]
public static class DailyProsperityBonus
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void ProsperityChange(ref Town __instance, ref float __result)
    {
        try
        {
            if (__instance.IsPlayerTown() && SettingsManager.DailyProsperityBonus.IsChanged)
            {
                __result += SettingsManager.DailyProsperityBonus.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(DailyProsperityBonus));
        }
    }
}



[HarmonyPatch(typeof(Town), "SecurityChange", MethodType.Getter)]
public static class DailySecurityBonus
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void SecurityChange(ref Town __instance, ref float __result)
    {
        try
        {
            if (__instance.IsPlayerTown() && SettingsManager.DailySecurityBonus.IsChanged)
            {
                __result += SettingsManager.DailySecurityBonus.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(DailySecurityBonus));
        }
    }
}



[HarmonyPatch(typeof(DefaultDisguiseDetectionModel), "CalculateDisguiseDetectionProbability")]
public static class DisguiseAlwaysWorks
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateDisguiseDetectionProbability(Settlement settlement, ref float __result)
    {
        try
        {
            if (SettingsManager.DisguiseAlwaysWorks.IsChanged)
            {
                __result = 1f;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(DisguiseAlwaysWorks));
        }
    }
}


[HarmonyPatch(typeof(DefaultPartyWageModel), "GetTroopRecruitmentCost")]
public static class FreeTroopRecruitment
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetTroopRecruitmentCost(CharacterObject troop, Hero buyerHero, bool withoutItemCost, ref ExplainedNumber __result)
    {
        try
        {
            if (buyerHero.IsPlayer() && SettingsManager.FreeTroopRecruitment.IsChanged)
            {
                __result = new ExplainedNumber(1);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(FreeTroopRecruitment));
        }
    }
}


[HarmonyPatch(typeof(DefaultTradeItemPriceFactorModel), "GetPrice")]
public static class ItemTradingCostPercentage
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetPrice(EquipmentElement itemRosterElement, MobileParty clientParty, PartyBase merchant, bool isSelling, float inStoreValue, float supply, float demand, ref int __result)
    {
        try
        {
            if (clientParty.IsPlayerParty() && !isSelling && SettingsManager.ItemTradingCostPercentage.IsChanged)
            {
                float num = SettingsManager.ItemTradingCostPercentage.Value / 100f;
                int num2 = (int)Math.Round(num * (float)__result);
                __result = num2;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(ItemTradingCostPercentage));
        }
    }
}



[HarmonyPatch(typeof(Mission), "DoesMissionRequireCivilianEquipment", MethodType.Getter)]
public static class NeverRequireCivilianEquipment
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void DoesMissionRequireCivilianEquipment(ref Mission __instance, ref bool __result)
    {
        try
        {
            if (SettingsManager.NeverRequireCivilianEquipment.IsChanged)
            {
                __result = false;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NeverRequireCivilianEquipment));
        }
    }
}



[HarmonyPatch(typeof(DefaultBribeCalculationModel), "IsBribeNotNeededToEnterKeep")]
public static class NoBribeToEnterKeep
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void IsBribeNotNeededToEnterKeep(Settlement settlement, ref bool __result)
    {
        try
        {
            if (SettingsManager.NoBribeToEnterKeep.IsChanged)
            {
                __result = true;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NoBribeToEnterKeep));
        }
    }
}


[HarmonyPatch(typeof(RebellionsCampaignBehavior), "CheckRebellionEvent")]
public static class RebellionChancePercentage
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CheckRebellionEvent(ref Settlement settlement, ref bool __result)
    {
        try
        {
            if (settlement.IsPlayerSettlement() && SettingsManager.SettlementsNeverRebel.IsChanged)
            {
                __result = false;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(RebellionChancePercentage));
        }
    }
}



[HarmonyPatch(typeof(DefaultTradeItemPriceFactorModel), "GetPrice")]
public static class SellingPriceMultiplier
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetPrice(EquipmentElement itemRosterElement, MobileParty clientParty, PartyBase merchant, bool isSelling, float inStoreValue, float supply, float demand, ref int __result)
    {
        try
        {
            if (clientParty.IsPlayerParty() && isSelling && SettingsManager.SellingPriceMultiplier.IsChanged)
            {
                __result = (int)Math.Round((float)__result * SettingsManager.SellingPriceMultiplier.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(SellingPriceMultiplier));
        }
    }
}


[HarmonyPatch(typeof(TournamentBehavior), "GetMaximumBet")]
public static class TournamentMaximumBetMultiplier
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetMaximumBet(ref int __result)
    {
        try
        {
            if (SettingsManager.TournamentMaximumBetMultiplier.IsChanged)
            {
                int num = (int)Math.Round((float)__result * SettingsManager.TournamentMaximumBetMultiplier.Value);
                __result = num;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(TournamentMaximumBetMultiplier));
        }
    }
}


[HarmonyPatch(typeof(StartBattleAction), "ApplyStartRaid")]
public static class VillagesNeverRaided
{
    [UsedImplicitly]
    [HarmonyPrefix]
    public static bool ApplyStartRaid(ref MobileParty attackerParty, ref Settlement settlement)
    {
        if (settlement.IsPlayerSettlement() && SettingsManager.VillagesNeverRaided.IsChanged)
        {
            return false;
        }
        return true;
    }
}