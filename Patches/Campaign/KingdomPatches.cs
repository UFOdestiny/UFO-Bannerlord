using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.GameComponents;
using UFO.Setting;
namespace UFO.Patch;

[HarmonyPatch(typeof(DefaultClanPoliticsModel), "GetInfluenceRequiredToOverrideKingdomDecision")]
public static class DecisionOverrideInfluenceCostPercentage
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetInfluenceRequiredToOverrideKingdomDecision(ref DecisionOutcome popularOption, ref DecisionOutcome overridingOption, ref KingdomDecision decision, ref int __result)
    {
        try
        {
            if (SettingsManager.DecisionOverrideInfluenceCostPercentage.IsChanged)
            {
                float num = SettingsManager.DecisionOverrideInfluenceCostPercentage.Value / 100f;
                float num2 = (float)__result * num;
                __result = (int)Math.Round(num2);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(DecisionOverrideInfluenceCostPercentage));
        }
    }
}



[HarmonyPatch(typeof(DecisionOutcome), "TotalSupportPoints", MethodType.Getter)]
public static class KingdomDecisionWeightMultiplier
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void Getter(ref DecisionOutcome __instance, ref float __result)
    {
        try
        {
            if (__instance.SupporterList.Any((Supporter x) => x.IsPlayer) && SettingsManager.KingdomDecisionWeightMultiplier.IsChanged)
            {
                __result *= SettingsManager.KingdomDecisionWeightMultiplier.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(KingdomDecisionWeightMultiplier));
        }
    }
}



[HarmonyPatch(typeof(ChangeCrimeRatingAction), "Apply")]
public static class NoCrimeRatingForCrimes
{
    [UsedImplicitly]
    [HarmonyPrefix]
    public static void Apply(ref IFaction faction, ref float deltaCrimeRating, ref bool showNotification)
    {
        try
        {
            if (SettingsManager.NoCrimeRatingForCrimes.IsChanged)
            {
                deltaCrimeRating = 0f;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NoCrimeRatingForCrimes));
        }
    }
}


[HarmonyPatch(typeof(DefaultDiplomacyModel), "GetRelationCostOfDisbandingArmy")]
public static class NoRelationshipLossOnDecisionArmyDisband
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetRelationCostOfDisbandingArmy(bool isLeaderParty, ref int __result)
    {
        try
        {
            if (SettingsManager.NoRelationshipLossOnDecision.IsChanged)
            {
                __result = 0;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NoRelationshipLossOnDecisionArmyDisband));
        }
    }
}



[HarmonyPatch(typeof(DefaultDiplomacyModel), "GetRelationCostOfExpellingClanFromKingdom")]
public static class NoRelationshipLossOnDecisionExpellingClan
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetRelationCostOfExpellingClanFromKingdom(ref int __result)
    {
        try
        {
            if (SettingsManager.NoRelationshipLossOnDecision.IsChanged)
            {
                __result = 0;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NoRelationshipLossOnDecisionExpellingClan));
        }
    }
}



[HarmonyPatch(typeof(KingdomElection), "GetRelationChangeWithSponsor")]
public static class NoRelationshipLossOnDecisionKingdomDecision
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetRelationChangeWithSponsor(Supporter.SupportWeights supportWeight, bool isOpposingSides, ref int __result)
    {
        try
        {
            if (SettingsManager.NoRelationshipLossOnDecision.IsChanged)
            {
                __result = Math.Max(__result, 0);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NoRelationshipLossOnDecisionKingdomDecision));
        }
    }
}
