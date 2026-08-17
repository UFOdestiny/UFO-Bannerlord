using HarmonyLib;
using JetBrains.Annotations;
using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ComponentInterfaces;
using UFO.Extension;
using UFO.Setting;

namespace UFO.Patch.Combat;

internal static class PlayerPartyMelee
{
    internal static bool AppliesTo(Agent agent, WeaponComponentData weapon)
    {
        return agent?.Origin != null
            && agent.Origin.TryGetParty(out var party)
            && party == MobileParty.MainParty?.Party
            && weapon?.IsMeleeWeapon == true;
    }

    internal static float Apply(float value, SettingsManager.CheatValue<float> setting)
    {
        return setting.IsChanged ? value * setting.Value : value;
    }
}

[HarmonyPatch(typeof(AgentStatCalculateModel), "SetAllWeaponInaccuracy")]
public static class PlayerPartyMeleeProperties
{
    [UsedImplicitly]
    [HarmonyPostfix]
    public static void SetAllWeaponInaccuracy(Agent agent, ref AgentDrivenProperties agentDrivenProperties, int equippedIndex, WeaponComponentData equippedWeaponComponent)
    {
        try
        {
            if (!PlayerPartyMelee.AppliesTo(agent, equippedWeaponComponent))
                return;

            agentDrivenProperties.SwingSpeedMultiplier = PlayerPartyMelee.Apply(agentDrivenProperties.SwingSpeedMultiplier, SettingsManager.PlayerPartyMeleeSwingSpeedMultiplier);
            agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier = PlayerPartyMelee.Apply(agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier, SettingsManager.PlayerPartyMeleeThrustReadySpeedMultiplier);
            agentDrivenProperties.HandlingMultiplier = PlayerPartyMelee.Apply(agentDrivenProperties.HandlingMultiplier, SettingsManager.PlayerPartyMeleeHandlingMultiplier);
            agentDrivenProperties.OffhandWeaponDefendSpeedMultiplier = PlayerPartyMelee.Apply(agentDrivenProperties.OffhandWeaponDefendSpeedMultiplier, SettingsManager.PlayerPartyMeleeOffhandDefenseSpeedMultiplier);
            agentDrivenProperties.ShieldBashStunDurationMultiplier = PlayerPartyMelee.Apply(agentDrivenProperties.ShieldBashStunDurationMultiplier, SettingsManager.PlayerPartyMeleeShieldBashStunMultiplier);
            agentDrivenProperties.KickStunDurationMultiplier = PlayerPartyMelee.Apply(agentDrivenProperties.KickStunDurationMultiplier, SettingsManager.PlayerPartyMeleeKickStunMultiplier);

            if (!agent.IsPlayer())
            {
                agentDrivenProperties.AIBlockOnDecideAbility = PlayerPartyMelee.Apply(agentDrivenProperties.AIBlockOnDecideAbility, SettingsManager.PlayerPartyMeleeAiBlockDecisionMultiplier);
                agentDrivenProperties.AIParryOnDecideAbility = PlayerPartyMelee.Apply(agentDrivenProperties.AIParryOnDecideAbility, SettingsManager.PlayerPartyMeleeAiParryDecisionMultiplier);
                agentDrivenProperties.AiDefendWithShieldDecisionChanceValue = PlayerPartyMelee.Apply(agentDrivenProperties.AiDefendWithShieldDecisionChanceValue, SettingsManager.PlayerPartyMeleeAiShieldDefenseMultiplier);
            }
        }
        catch (Exception e)
        {
            SubModule.LogError(e, typeof(PlayerPartyMeleeProperties));
        }
    }
}
