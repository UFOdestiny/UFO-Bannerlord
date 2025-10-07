using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;
using UFO.Extension;
using UFO.Setting;

namespace UFO.Patch;


[HarmonyPatch(typeof(DefaultMarriageModel), "GetClanAfterMarriage")]
public static class KeepDaughter
{
    public static void Postfix(ref Clan __result, Hero firstHero, Hero secondHero)
    {
        if (firstHero.Clan?.Leader == firstHero || secondHero.Clan?.Leader == secondHero)
            return;

        if (firstHero.Clan != Hero.MainHero.Clan && secondHero.Clan != Hero.MainHero.Clan)
            return;

        if (firstHero.Clan == Hero.MainHero.Clan && secondHero.Clan == Hero.MainHero.Clan)
            return;

        Hero clanHero = firstHero.Clan == Hero.MainHero.Clan ? firstHero : secondHero;

        if (clanHero.IsFemale && SettingsManager.KeepDaughter.Value)
        {
            __result = clanHero.Clan;
        }

        return;
    }
}


[HarmonyPatch(typeof(DefaultClanTierModel), "GetPartyLimitForTier")]
public static class ExtraClanPartyLimit
{
    public static void Postfix(Clan clan, int clanTierToCheck, ref int __result)
    {
        try
        {
            if (clan.IsPlayerClan() && SettingsManager.ExtraClanPartyLimit.IsChanged)
            {
                __result += SettingsManager.ExtraClanPartyLimit.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(ExtraClanPartyLimit));
        }
    }
}


[HarmonyPatch(typeof(DefaultPartySizeLimitModel), "GetPartyMemberSizeLimit")]
public static class ExtraClanPartySize
{
    public static void Postfix(PartyBase party, bool includeDescriptions, ref ExplainedNumber __result)
    {
        if (__result.ResultNumber == 0f || __result.BaseNumber == 0f)
        {
            return;
        }
        try
        {
            if (party.IsPlayerClan() && !party.IsPlayerParty() && SettingsManager.ExtraClanPartySize.IsChanged)
            {
                __result.Add(SettingsManager.ExtraClanPartySize.Value, new TextObject("BCheatsBonus"));
            }
            else if (party.IsPlayerParty() && SettingsManager.ExtraPartyMemberSize.IsChanged)
            {
                __result.Add(SettingsManager.ExtraPartyMemberSize.Value, new TextObject("BCheatsBonus"));
            }
            else if (party.IsPlayerClan() && SettingsManager.ExtraClanPartySize.IsChanged)
            {
                __result.Add(SettingsManager.ExtraClanPartySize.Value, new TextObject("BCheatsBonus"));
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(ExtraClanPartySize));
        }
    }
}


[HarmonyPatch(typeof(DefaultClanTierModel), "GetCompanionLimit")]
public static class ExtraCompanionLimit
{
    public static void Postfix(Clan clan, ref int __result)
    {
        try
        {
            if (clan.IsPlayerClan() && SettingsManager.ExtraCompanionLimit.IsChanged)
            {
                __result += SettingsManager.ExtraCompanionLimit.Value;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(ExtraCompanionLimit));
        }
    }
}
