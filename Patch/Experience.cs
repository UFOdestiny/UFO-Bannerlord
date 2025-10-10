using HarmonyLib;
using JetBrains.Annotations;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using UFO.Extension;
using UFO.Setting;
using static TaleWorlds.CampaignSystem.ComponentInterfaces.CombatXpModel;

namespace UFO.Patch;

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



//[HarmonyPatch(typeof(DefaultCharacterDevelopmentModel), "CalculateLearningRate", new Type[]
//{
//    typeof(Hero),
//    typeof(SkillObject)
//})]
//public static class CompanionLearningRateMultiplier
//{
//    [UsedImplicitly]
//    [HarmonyPostfix]
//    public static void CalculateLearningRate(IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes,
//        int focusValue,
//        int skillValue,
//        SkillObject skill,
//        bool includeDescriptions,
//        ref ExplainedNumber __result)
//    {
//        try
//        {
//            if (characterAttributes.GetPropertyValue())
//            {

//            }
//            //if (!characterAttributes.IsPlayer() && hero.IsPlayerCompanion() && SettingsManager.CompanionLearningRateMultiplier.IsChanged)
//            //{
//            //    __result *= SettingsManager.CompanionLearningRateMultiplier.Value;
//            //}
//        }
//        catch (Exception e)
//        {
//            SubModule.LogError(e, typeof(CompanionLearningRateMultiplier));
//        }
//    }
//}


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

//[HarmonyPatch(typeof(DefaultCharacterDevelopmentModel), "CalculateLearningRate")]
//public static class LearningRateMultiplier
//{
//    [UsedImplicitly]
//    [HarmonyPostfix]
//    public static void CalculateLearningRate(IReadOnlyPropertyOwner<CharacterAttribute> characterAttributes, int focusValue, SkillObject skill, bool includeDescriptions,
//        ref ExplainedNumber __result)
//    {
//        try
//        {
//            if (characterAttributes.IsPlayer() && SettingsManager.LearningRateMultiplier.IsChanged)
//            {
//                __result *= SettingsManager.LearningRateMultiplier.Value;
//            }
//        }
//        catch (Exception e)
//        {
//            SubModule.LogError(e, typeof(LearningRateMultiplier));
//        }
//    }
//}



[HarmonyPatch(typeof(DefaultCombatXpModel), "GetXpFromHit")]
public static class TroopExperienceMultiplier
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void Postfix(CharacterObject attackerTroop,
        CharacterObject captain, CharacterObject attackedTroop,
        PartyBase party, int damage, bool isFatal,
        MissionTypeEnum missionType,
        ref ExplainedNumber __result)
    {
        try
        {
            if (party.IsPlayerParty() && !attackerTroop.IsPlayer() && SettingsManager.TroopExperienceMultiplier.IsChanged)
            {
                __result = new ExplainedNumber((int)Math.Round(__result.ResultNumber * SettingsManager.TroopExperienceMultiplier.Value));
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(TroopExperienceMultiplier));
        }
    }
}
