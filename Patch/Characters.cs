using HarmonyLib;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using UFO;
using UFO.Setting;
using UFO.Extension;
using System.Linq;

namespace UFO.Patch;

[HarmonyPatch(typeof(DefaultPregnancyModel), "PregnancyDurationInDays", MethodType.Getter)]
public static class APD
{
    public static void Postfix(ref float __result)
    {
        try
        {
            if (SettingsManager.AdjustPregnancyDuration.IsChanged)
            {
                __result = SettingsManager.AdjustPregnancyDuration.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(APD));
        }
    }
}



[HarmonyPatch(typeof(DefaultMarriageModel), "IsCoupleSuitableForMarriage")]
public static class ASSM
{
    public static void Postfix(Hero firstHero, Hero secondHero, ref bool __result)
    {
        try
        {
            if (SettingsManager.AllowSameSexMarriage.IsChanged && (firstHero.IsPlayer() || secondHero.IsPlayer()))
            {
                __result = (firstHero.Clan?.Leader != firstHero || secondHero.Clan?.Leader != secondHero) && !DiscoverAncestors(firstHero, 3).Intersect(DiscoverAncestors(secondHero, 3)).Any() && firstHero.CanMarry() && secondHero.CanMarry();
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(ASSM));
        }
    }

    private static IEnumerable<Hero> DiscoverAncestors(Hero hero, int n)
    {
        if (hero == null)
        {
            yield break;
        }
        yield return hero;
        if (n <= 0)
        {
            yield break;
        }
        foreach (Hero item in DiscoverAncestors(hero.Mother, n - 1))
        {
            yield return item;
        }
        foreach (Hero item2 in DiscoverAncestors(hero.Father, n - 1))
        {
            yield return item2;
        }
    }
}



[HarmonyPatch(typeof(BarterManager), "IsOfferAcceptable", new Type[]
{
    typeof(BarterData),
    typeof(Hero),
    typeof(PartyBase)
})]
public static class BOAA
{
    public static void Postfix(BarterData args, Hero hero, PartyBase party, ref BarterManager __instance, ref bool __result)
    {
        try
        {
            if (SettingsManager.BarterOfferAlwaysAccepted.IsChanged)
            {
                __result = true;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(BOAA));
        }
    }
}



[HarmonyPatch(typeof(DefaultPersuasionModel), "GetChances")]
public static class CAS
{
    public static void Postfix(PersuasionOptionArgs optionArgs, ref float successChance, ref float critSuccessChance, ref float critFailChance, ref float failChance, float difficultyMultiplier)
    {
        try
        {
            if (SettingsManager.ConversationAlwaysSuccessful.IsChanged)
            {
                successChance = 1f;
                critSuccessChance = 1f;
                failChance = 0f;
                critFailChance = 0f;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(CAS));
        }
    }
}


[HarmonyPatch(typeof(BarterManager), "CanPlayerBarterWithHero")]
public static class NBC
{
    public static void Postfix(Hero hero, ref bool __result)
    {
        try
        {
            if (SettingsManager.NoBarterCooldown.IsChanged)
            {
                __result = true;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NBC));
        }
    }
}


[HarmonyPatch(typeof(KillCharacterAction), "ApplyByOldAge")]
public static class NDOOA
{
    public static bool Prefix(ref Hero victim, ref bool showNotification)
    {
        try
        {
            if (victim.IsPlayer() && SettingsManager.NeverDieOfOldAge.IsChanged)
            {
                return false;
            }
            return true;
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(NDOOA));
            return true;
        }
    }
}



[HarmonyPatch(typeof(DefaultRomanceModel), "GetAttractionValuePercentage")]
public static class PA
{
    public static void Postfix(Hero potentiallyInterestedCharacter, Hero heroOfInterest, ref int __result)
    {
        try
        {
            if (SettingsManager.PerfectAttraction.IsChanged && heroOfInterest.IsPlayer())
            {
                __result = 100;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PA));
        }
    }
}


[HarmonyPatch(typeof(DefaultPregnancyModel), "GetDailyChanceOfPregnancyForHero")]
public static class PCM
{
    public static void Postfix(Hero hero, ref float __result)
    {
        try
        {
            if (SettingsManager.PregnancyChanceMultiplier.IsChanged && (hero.IsPlayer() || hero.Spouse.IsPlayer()))
            {
                __result *= SettingsManager.PregnancyChanceMultiplier.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PCM));
        }
    }
}


[HarmonyPatch(typeof(Hero), "GetRelation")]
public static class PR
{
    public static void Postfix(Hero otherHero, ref Hero __instance, ref int __result)
    {
        try
        {
            if ((__instance.IsPlayer() || otherHero.IsPlayer()) && SettingsManager.PerfectRelationships.IsChanged)
            {
                __result = 100;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PR));
        }
    }
}


[HarmonyPatch(typeof(DefaultBattleRewardModel), "GetPlayerGainedRelationAmount")]
public static class RGABM
{
    public static void Postfix(MapEvent mapEvent, Hero hero, ref int __result)
    {
        if (SettingsManager.RelationGainAfterBattleMultiplier.IsChanged)
        {
            __result = (int)Math.Round((float)__result * SettingsManager.RelationGainAfterBattleMultiplier.Value);
        }
    }
}
