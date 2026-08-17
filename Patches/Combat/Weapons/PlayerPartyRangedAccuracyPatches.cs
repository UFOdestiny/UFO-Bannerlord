using HarmonyLib;
using JetBrains.Annotations;
using SandBox.GameComponents;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ComponentInterfaces;
using UFO.Extension;
using UFO.Setting;

namespace UFO.Patch.Combat;

internal static class PlayerPartyRangedAccuracy
{
    internal static bool AppliesTo(Agent agent, WeaponComponentData weapon)
    {
        return agent?.Origin != null
            && agent.Origin.TryGetParty(out var party)
            && party == MobileParty.MainParty?.Party
            && weapon != null
            && (weapon.WeaponClass == WeaponClass.Bow || weapon.WeaponClass == WeaponClass.Crossbow);
    }

    internal static float Factor(float percentage) => percentage / 100f;
}

[HarmonyPatch(typeof(SandboxAgentStatCalculateModel), "GetWeaponInaccuracy")]
public static class PlayerPartyRangedWeaponInaccuracy
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void GetWeaponInaccuracy(Agent agent, MissionWeapon weapon, int weaponSkill, ref float __result)
    {
        try
        {
            if (PlayerPartyRangedAccuracy.AppliesTo(agent, weapon.CurrentUsageItem)
                && SettingsManager.PlayerPartyRangedInaccuracyPercentage.IsChanged)
            {
                __result *= PlayerPartyRangedAccuracy.Factor(SettingsManager.PlayerPartyRangedInaccuracyPercentage.Value);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PlayerPartyRangedWeaponInaccuracy));
        }
    }
}

[HarmonyPatch(typeof(AgentStatCalculateModel), "SetAllWeaponInaccuracy")]
public static class PlayerPartyRangedWeaponProperties
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void SetAllWeaponInaccuracy(Agent agent, ref AgentDrivenProperties agentDrivenProperties, int equippedIndex, WeaponComponentData equippedWeaponComponent)
    {
        try
        {
            if (!PlayerPartyRangedAccuracy.AppliesTo(agent, equippedWeaponComponent))
                return;

            agentDrivenProperties.WeaponMaxMovementAccuracyPenalty = Apply(agentDrivenProperties.WeaponMaxMovementAccuracyPenalty, SettingsManager.PlayerPartyRangedMovementPenaltyPercentage);
            agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty = Apply(agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty, SettingsManager.PlayerPartyRangedUnsteadyPenaltyPercentage);
            agentDrivenProperties.WeaponBestAccuracyWaitTime = Apply(agentDrivenProperties.WeaponBestAccuracyWaitTime, SettingsManager.PlayerPartyRangedBestAccuracyWaitPercentage);
            agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = Apply(agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians, SettingsManager.PlayerPartyRangedRotationPenaltyPercentage);
            agentDrivenProperties.WeaponExternalAccelerationAccuracyPenalty = Apply(agentDrivenProperties.WeaponExternalAccelerationAccuracyPenalty, SettingsManager.PlayerPartyRangedAccelerationPenaltyPercentage);

            if (!agent.IsPlayer())
            {
                agentDrivenProperties.AiShooterError = Apply(agentDrivenProperties.AiShooterError, SettingsManager.PlayerPartyRangedAiShooterErrorPercentage);
                agentDrivenProperties.AiRangerLeadErrorMin = Apply(agentDrivenProperties.AiRangerLeadErrorMin, SettingsManager.PlayerPartyRangedAiLeadErrorPercentage);
                agentDrivenProperties.AiRangerLeadErrorMax = Apply(agentDrivenProperties.AiRangerLeadErrorMax, SettingsManager.PlayerPartyRangedAiLeadErrorPercentage);
                agentDrivenProperties.AiRangerHorizontalErrorMultiplier = Apply(agentDrivenProperties.AiRangerHorizontalErrorMultiplier, SettingsManager.PlayerPartyRangedAiHorizontalErrorPercentage);
                agentDrivenProperties.AiRangerVerticalErrorMultiplier = Apply(agentDrivenProperties.AiRangerVerticalErrorMultiplier, SettingsManager.PlayerPartyRangedAiVerticalErrorPercentage);
                agentDrivenProperties.AiWaitBeforeShootFactor = Apply(agentDrivenProperties.AiWaitBeforeShootFactor, SettingsManager.PlayerPartyRangedAiShootIntervalPercentage);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PlayerPartyRangedWeaponProperties));
        }
    }

    private static float Apply(float value, SettingsManager.CheatValue<float> setting)
    {
        return setting.IsChanged ? value * PlayerPartyRangedAccuracy.Factor(setting.Value) : value;
    }
}
