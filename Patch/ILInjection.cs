using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Pages;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using UFO;
using UFO.Extension;
using UFO.Patch;
using UFO.Patch.Combat;
using UFO.Setting;
namespace UFO.Patch;

public static class ILInjection
{
    public static bool SearchInjection(MBList<CodeInstruction> instructions)
    {
        foreach (CodeInstruction instruction in instructions)
        {
            if (instruction.operand != null && instruction.ToString().IndexOf("ILInjection") > 0)
            {
                return true;
            }
        }
        return false;
    }

    public static bool SearchInjection(MBList<CodeInstruction> instructions, string method)
    {
        foreach (CodeInstruction instruction in instructions)
        {
            if (instruction.operand != null && instruction.ToString().IndexOf("ILInjection") > 0 && instruction.operand.ToString().IndexOf(method) > 0)
            {
                return true;
            }
        }
        return false;
    }

    public static void EnableHotkeysEncyclopedia(EncyclopediaPageVM __instance)
    {
        EnableHotkeysAddEncyclopediaTroops.Handler(__instance);
        EnableHotkeysChangePlayerCharacter.Handler(__instance);
        EnableHotkeysKillCharacter.Handler(__instance);
        EnableHotkeysTransferSettlement.Handler(__instance);
    }

    public static void EnableHotkeysGameManagerBase()
    {
        EnableHotkeysAddItems.Handler();
        EnableHotkeysCharacterAttributes.Handler();
        EnableHotkeysCharacterPoints.Handler();
        EnableHotkeysInfluence.Handler();
        EnableHotkeysMoney.Handler();
        EnableHotkeysTroopCount.Handler();
        EnableHotkeysTroopExperience.Handler();
        HealthRegeneration.Handler();
        PartyHealthRegeneration.Handler();
    }

    public static float TroopWagesPercentage(MobileParty mobileParty)
    {
        try
        {
            if (mobileParty != null && mobileParty.IsPlayerParty() && SettingsManager.TroopWagesPercentage.IsChanged)
            {
                return (1f - SettingsManager.TroopWagesPercentage.Value / 100f) * -1f;
            }
            return 0f;
        }
        catch (Exception)
        {
            return 0f;
        }
    }

    public static float CalculatePercentageFactor(ExplainedNumber exp, float Percentage)
    {
        float num = exp.ResultNumber * (Percentage / 100f);
        float num2 = ReflectUtils.ReflectField<float>("_sumOfFactors", exp);
        float num3 = (num * 100f - exp.BaseNumber * 100f) / exp.BaseNumber;
        return num3 / 100f - num2;
    }

    public static ExplainedNumber TroopWagesPercentageExplained(MobileParty mobileParty, ExplainedNumber exp)
    {
        if (exp.ResultNumber == 0f)
        {
            return exp;
        }
        try
        {
            if (mobileParty != null && mobileParty.IsPlayerParty() && SettingsManager.TroopWagesPercentage.IsChanged)
            {
                exp.AddFactor(CalculatePercentageFactor(exp, SettingsManager.TroopWagesPercentage.Value), new TextObject("BCheatsBonus"));
            }
            if (mobileParty.IsGarrison && mobileParty.IsPlayerParty() && SettingsManager.GarrisonWagesPercentage.IsChanged)
            {
                exp.AddFactor(CalculatePercentageFactor(exp, SettingsManager.GarrisonWagesPercentage.Value), new TextObject("BCheatsBonus"));
            }
            return exp;
        }
        catch (Exception)
        {
            return exp;
        }
    }

    public static int GPLFT(Clan clan)
    {
        try
        {
            if (clan.IsPlayerClan() && SettingsManager.ExtraClanPartyLimit.IsChanged)
            {
                return SettingsManager.ExtraClanPartyLimit.Value;
            }
            return 0;
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(ExtraClanPartyLimit));
            return 0;
        }
    }

    public static ExplainedNumber GPMSL(PartyBase party, ExplainedNumber __result)
    {
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
            return __result;
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(ExtraClanPartySize));
            return __result;
        }
    }
}
