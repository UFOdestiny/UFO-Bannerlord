using HarmonyLib;
using JetBrains.Annotations;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using UFO.Extension;
using UFO.Setting;
using static TaleWorlds.CampaignSystem.ComponentInterfaces.CombatXpModel;

namespace UFO.Patch;

[HarmonyPatch(typeof(DefaultCharacterDevelopmentModel), nameof(DefaultCharacterDevelopmentModel.CalculateLearningRate))]
internal static class HeroLearningRateMultipliers
{
    private static void Postfix(IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes, ref ExplainedNumber __result)
    {
        if (Campaign.Current?.MainParty == null) return;
        if (ReferenceEquals(characterAttributes, Hero.MainHero.CharacterAttributes))
        {
            if (SettingsManager.LearningRateMultiplier.IsChanged)
                __result.AddMultiplier(SettingsManager.LearningRateMultiplier.Value);
            return;
        }
        if (!SettingsManager.CompanionLearningRateMultiplier.IsChanged) return;
        foreach (var companion in Clan.PlayerClan.Companions)
        {
            if (!ReferenceEquals(characterAttributes, companion.CharacterAttributes)) continue;
            __result.AddMultiplier(SettingsManager.CompanionLearningRateMultiplier.Value);
            break;
        }
    }
}

[HarmonyPatch(typeof(HeroDeveloper), "AddSkillXp")]
public static class ClanExperienceMultiplier
{
    [UsedImplicitly]
    [HarmonyPrefix]
    public static void AddSkillXp(SkillObject skill, ref float rawXp, bool isAffectedByFocusFactor, bool shouldNotify, ref HeroDeveloper __instance)
    {
        if (__instance.Hero.IsPlayerClan() && !__instance.Hero.IsPlayer() && !__instance.Hero.IsPlayerCompanion() && SettingsManager.ClanExperienceMultiplier.IsChanged)
        {
            rawXp *= SettingsManager.ClanExperienceMultiplier.Value;
        }
    }
}

[HarmonyPatch(typeof(HeroDeveloper), "AddSkillXp")]
public static class CompanionExperienceMultiplier
{
    [UsedImplicitly]
    [HarmonyPrefix]
    public static void AddSkillXp(SkillObject skill, ref float rawXp, bool isAffectedByFocusFactor, bool shouldNotify, ref HeroDeveloper __instance)
    {
        if (__instance.Hero.IsPlayerCompanion() && SettingsManager.CompanionExperienceMultiplier.IsChanged)
        {
            rawXp *= SettingsManager.CompanionExperienceMultiplier.Value;
        }
    }
}

[HarmonyPatch(typeof(HeroDeveloper), "AddSkillXp")]
public static class ExperienceMultiplier
{
    [UsedImplicitly]
    [HarmonyPrefix]
    public static void AddSkillXp(SkillObject skill, ref float rawXp, bool isAffectedByFocusFactor, bool shouldNotify, ref HeroDeveloper __instance)
    {
        if (__instance.Hero.IsPlayer() && SettingsManager.ExperienceMultiplier.IsChanged)
        {
            rawXp *= SettingsManager.ExperienceMultiplier.Value;
        }
    }
}

[HarmonyPatch(typeof(HeroDeveloper), "GetRequiredFocusPointsToAddFocus")]
public static class FreeFocusPointAssignment
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetRequiredFocusPointsToAddFocus(SkillObject skill, ref int __result)
    {
        try
        {
            if (SettingsManager.FreeFocusPointAssignment.IsChanged)
            {
                __result = 0;
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(FreeFocusPointAssignment));
        }
    }
}

[HarmonyPatch(typeof(DefaultCharacterDevelopmentModel), "CalculateLearningLimit")]
public static class LearningLimitMultiplier
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void CalculateLearningLimit(IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes, int focusValue, SkillObject skill, bool includeDescriptions,
        ref ExplainedNumber __result)
    {
        try
        {
            if (SettingsManager.LearningLimitMultiplier.IsChanged)
            {
                __result.AddMultiplier(SettingsManager.LearningLimitMultiplier.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(LearningLimitMultiplier));
        }
    }
}

[HarmonyPatch(typeof(DefaultCombatXpModel))]
[HarmonyPatch("GetXpFromHit")]
[HarmonyPatch(new Type[] {
        typeof(CharacterObject),
        typeof(CharacterObject),
        typeof(CharacterObject),
        typeof(PartyBase),
        typeof(int),
        typeof(bool),
        typeof(MissionTypeEnum)
    })]
public static class TroopExperienceMultiplier
{
    [HarmonyPostfix]
    public static void Postfix(
        CharacterObject attackerTroop,
        CharacterObject captain,
        CharacterObject attackedTroop,
        PartyBase attackerParty,
        int damage,
        bool isFatal,
        MissionTypeEnum missionType,
        ref ExplainedNumber __result)
    {
        try
        {
            if (attackerParty == null) return;

            if (attackerParty.IsPlayerParty() &&
                !attackerTroop.IsPlayer() &&
                SettingsManager.TroopExperienceMultiplier.IsChanged)
            {
                __result.AddFactor(SettingsManager.TroopExperienceMultiplier.Value - 1f);
            }
        }
        catch (Exception ex)
        {
            SubModule.LogError(ex, typeof(TroopExperienceMultiplier));
        }
    }
}
