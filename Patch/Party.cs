using HarmonyLib;
using JetBrains.Annotations;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Localization;
using UFO.Extension;
using UFO.Setting;

namespace UFO.Patch;


[HarmonyPatch(typeof(DefaultPartyMoraleModel), "GetEffectivePartyMorale")]
public static class ExtraPartyMorale
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetEffectivePartyMorale(ref MobileParty mobileParty, ref bool includeDescription, ref ExplainedNumber __result)
    {
        try
        {
            if (mobileParty.IsPlayerParty() && SettingsManager.ExtraPartyMorale.IsChanged)
            {
                __result.Add(SettingsManager.ExtraPartyMorale.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(ExtraPartyMorale));
        }
    }
}


[HarmonyPatch(typeof(DefaultPartySizeLimitModel), "GetPartyPrisonerSizeLimit")]
public static class ExtraPartyPrisonerSize
{
    public static void Postfix(ref PartyBase party, ref bool includeDescriptions, ref ExplainedNumber __result)
    {
        try
        {
            if (party.IsPlayerParty() && SettingsManager.ExtraPartyPrisonerSize.IsChanged)
            {
                __result.Add(SettingsManager.ExtraPartyPrisonerSize.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(ExtraPartyPrisonerSize));
        }
    }
}



[HarmonyPatch(typeof(DefaultMobilePartyFoodConsumptionModel), "CalculateDailyFoodConsumptionf")]
public static class FoodConsumptionPercentage
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateDailyFoodConsumptionf(MobileParty party, ExplainedNumber baseConsumption, ref ExplainedNumber __result)
    {
        try
        {
            if (party.IsPlayerParty() && SettingsManager.FoodConsumptionPercentage.IsChanged)
            {
                __result.AddPercentage(SettingsManager.FoodConsumptionPercentage.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(FoodConsumptionPercentage));
        }
    }
}



[HarmonyPatch(typeof(DefaultCompanionHiringPriceCalculationModel), "GetCompanionHiringPrice")]
public static class FreeCompanionHiring
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetCompanionHiringPrice(Hero companion, ref int __result)
    {
        try
        {
            if (SettingsManager.FreeCompanionHiring.IsChanged)
            {
                __result = 0;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(FreeCompanionHiring));
        }
    }
}


[HarmonyPatch(typeof(DefaultPartyTroopUpgradeModel), "GetGoldCostForUpgrade")]
public static class FreeTroopUpgrades
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetGoldCostForUpgrade(ref PartyBase party, ref CharacterObject characterObject, ref CharacterObject upgradeTarget, ref ExplainedNumber __result)
    {
        try
        {
            if (party.IsPlayerParty() && SettingsManager.FreeTroopUpgrades.IsChanged)
            {
                __result = new ExplainedNumber(0);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(FreeTroopUpgrades));
        }
    }
}



[HarmonyPatch(typeof(PlayerCaptivityCampaignBehavior), "CheckCaptivityChange")]
public static class InstantEscape
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CheckCaptivityChange(float dt)
    {
        try
        {
            if (SettingsManager.InstantEscape.IsChanged)
            {
                PlayerCaptivity.EndCaptivity();
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(InstantEscape));
        }
    }
}



[HarmonyPatch(typeof(DefaultPrisonerRecruitmentCalculationModel), "CalculateRecruitableNumber")]
public static class InstantPrisonerRecruitment
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateRecruitableNumber(ref PartyBase party, ref CharacterObject character, ref int __result)
    {
        try
        {
            if (party.IsPlayerParty() && !character.IsHero() && SettingsManager.InstantPrisonerRecruitment.IsChanged)
            {
                __result = party.PrisonRoster.GetTroopCount(character);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(InstantPrisonerRecruitment));
        }
    }
}


[HarmonyPatch(typeof(EndCaptivityAction), "ApplyByEscape")]
public static class NoPrisonerEscape
{
    [UsedImplicitly]
    [HarmonyPrefix]
    public static bool ApplyByEscape(Hero character, Hero facilitator, bool showNotification)
    {
        try
        {
            if (character.IsPrisoner && character.PartyBelongedToAsPrisoner != null && character.PartyBelongedToAsPrisoner.MapFaction == Hero.MainHero.MapFaction && SettingsManager.NoPrisonerEscape.IsChanged)
            {
                return false;
            }
            return true;
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NoPrisonerEscape));
            return true;
        }
    }
}


[HarmonyPatch(typeof(DefaultPartyHealingModel), "GetDailyHealingHpForHeroes")]
public static class PartyHealingMultiplierHeroes
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetDailyHealingHpForHeroes(PartyBase party, bool isPrisoners, bool includeDescriptions,
        ref ExplainedNumber __result)
    {
        try
        {
            if (party.IsPlayerParty() && SettingsManager.PartyHealingMultiplier.IsChanged)
            {
                __result.AddMultiplier(SettingsManager.PartyHealingMultiplier.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PartyHealingMultiplierHeroes));
        }
    }
}


[HarmonyPatch(typeof(DefaultPartyHealingModel), "GetDailyHealingForRegulars")]
public static class PartyHealingMultiplierTroops
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetDailyHealingForRegulars(PartyBase party, bool isPrisoners, bool includeDescriptions,
        ref ExplainedNumber __result)
    {
        try
        {
            if (party.IsPlayerParty() && SettingsManager.PartyHealingMultiplier.IsChanged)
            {
                __result.AddMultiplier(SettingsManager.PartyHealingMultiplier.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PartyHealingMultiplierTroops));
        }
    }
}



[HarmonyPatch(typeof(DefaultPartyWageModel), "GetTotalWage")]
public static class TroopWagesPercentage
{
    public static void Postfix(MobileParty mobileParty, TroopRoster troopRoster, bool includeDescriptions,
        ref ExplainedNumber __result)
    {
        if (__result.ResultNumber == 0f || __result.BaseNumber == 0f)
        {
            return;
        }
        try
        {
            if (mobileParty != null && mobileParty.IsPlayerParty() && SettingsManager.TroopWagesPercentage.IsChanged)
            {
                __result.AddFactor(ILInjection.CalculatePercentageFactor(__result, SettingsManager.TroopWagesPercentage.Value), new TextObject("BCheatsBonus"));
            }
            if (mobileParty.IsGarrison && mobileParty.IsPlayerParty() && SettingsManager.GarrisonWagesPercentage.IsChanged)
            {
                __result.AddFactor(ILInjection.CalculatePercentageFactor(__result, SettingsManager.GarrisonWagesPercentage.Value), new TextObject("BCheatsBonus"));
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(TroopWagesPercentage));
        }
    }
}